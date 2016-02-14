using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TextAnalysis;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using WebCrawler;

namespace TextAnalysisTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var input = File.ReadAllLines(@"c:\users\rasmus\documents\visual studio 2015\Projects\TextAnalysis\TextAnalysisTest\Input\TextFile1.txt");
            var textAnalyzer = new TextAnalyzer();
            var keyWords = textAnalyzer.FindKeyWords(input);
            foreach (var item in keyWords)
            {
                Console.WriteLine(item);
            }
        }

        [TestMethod]
        public void DownloadTextFromWebPage()
        {
            var webCrawler = new WebSiteCrawler();
            var text = webCrawler.GetRawTextFromSite(@"http://politiken.dk/oekonomi/2050/energi/ECE3050109/ekspert-olieselskaberne-har-fejlvurderet-og-taget-for-store-risici/");
            Console.WriteLine(text);
        }

        [TestMethod]
        public void GetLinksFromSite()
        {
            var webCrawler = new WebSiteCrawler();
            var links = webCrawler.GetAllLinksFromSite(@"http://politiken.dk");
            foreach (var link in links)
            {
                Console.WriteLine(link);
            }
        }
    }
}
