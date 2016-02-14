using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebCrawler
{
    public class WebSiteCrawler
    {
        public string GetRawTextFromSite(string webSite)
        {
            HtmlDocument doc = GetHtmlPage(webSite);

            StringBuilder sb = new StringBuilder();
            IEnumerable<HtmlNode> nodes = doc.DocumentNode.Descendants().Where(n =>
               n.NodeType == HtmlNodeType.Text &&
               n.ParentNode.Name != "script" &&
               n.ParentNode.Name != "style");

            foreach (HtmlNode node in nodes)
            {
                var sanitized = Regex.Replace(node.InnerText.Replace("\n", "").Replace("\t", ""), @"\s+", " ");
                if (sanitized.Length > 0)
                {
                    sb.Append(sanitized);
                }
            }

            return sb.ToString();
        }

        public string[] GetAllLinksFromSite(string baseSite, int maxSites = 100)
        {
            HashSet<string> hasLink = new HashSet<string>();
            List<string> result = new List<string>();
            Stack<string> sitesToVisit = new Stack<string>();

            sitesToVisit.Push(baseSite);
            result.Add(baseSite);
            hasLink.Add(baseSite);

            while (sitesToVisit.Any())
            {
                // New site to visit
                var site = sitesToVisit.Pop();

                var links = GetLinksFromSite(site);

                foreach (var link in links)
                {
                    if (!hasLink.Contains(link))
                    {
                        hasLink.Add(link);
                        result.Add(link);
                        sitesToVisit.Push(link);

                        if (result.Count == maxSites)
                        {
                            return result.ToArray();
                        }
                    }
                }
            }

            return result.ToArray();
        }

        public string[] GetLinksFromSite(string webSite)
        {
            Uri baseUri = new Uri(webSite);

            var links = new List<string>();
            HtmlDocument doc = GetHtmlPage(webSite);

            if (doc == null)
            {
                return links.ToArray();
            }

            foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//a[@href]"))
            {
                string linkValue = link.GetAttributeValue("href", string.Empty);

                Uri linkUri = null;
                if (Uri.IsWellFormedUriString(linkValue, UriKind.Absolute))
                {
                    linkUri = new Uri(linkValue);
                }
                else if (Uri.IsWellFormedUriString(linkValue, UriKind.Relative))
                {
                    linkUri = new Uri(baseUri, linkValue);
                }

                if (linkUri != null && linkUri.DnsSafeHost == baseUri.DnsSafeHost)
                {
                    links.Add(linkUri.AbsoluteUri);
                }

            }

            return links.ToArray();
        }

        private static HtmlDocument GetHtmlPage(string webSite)
        {
            try
            {
                WebClient client = new WebClient();

                client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");

                Stream data = client.OpenRead(webSite);
                StreamReader reader = new StreamReader(data);
                string s = reader.ReadToEnd();

                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(s);
                return doc;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
