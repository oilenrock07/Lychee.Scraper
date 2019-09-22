using System.IO;
using HtmlAgilityPack;
using Lychee.Scrapper.Domain.Services;
using NUnit.Framework;

namespace Lychee.Scrapper.Test.ServiceTest
{
    [TestFixture]
    public class LoggingServiceTest
    {
        [TestCase("https://stackoverflow.com/questions/4650462/easiest-way-to-check-if-an-arbitrary-string-is-a-valid-filename")]
        [TestCase("https://www.facebook.com/pg/aaaequities/posts/")]
        public void CanSanitizeUrl(string url)
        {
            var validName = LoggingService.SanitizeFileName(url);
            Assert.That(true);
        }

        [Test]
        public void CanLogDownloadedHtmlDocument()
        {
            //Arrange
            var path = $"{Path.GetDirectoryName(TestContext.CurrentContext.TestDirectory)}/debug/MaxShopScrapper/";
            var doc = new HtmlDocument();
            doc.Load(path + "maxshop_pagelist.txt");

            var loggingService = new LoggingService();

            var url = "https://www.maxshop.com/shop/tops/fashion-tops";
            var sanitizedFileName = LoggingService.SanitizeFileName(url);

            //Act
            loggingService.LogHtmlDocument(doc.DocumentNode, path, url);

            //Assert
            File.Exists(Path.Combine(path, sanitizedFileName + ".txt"));
        }
    }
}

