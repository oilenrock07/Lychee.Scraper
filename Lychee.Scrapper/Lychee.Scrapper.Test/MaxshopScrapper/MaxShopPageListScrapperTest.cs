using Lychee.Scrapper.Domain;
using Lychee.Scrapper.Domain.Models.Scrappers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Moq;
using Serilog;

namespace Lychee.Scrapper.Test.MaxshopScrapper
{
    [TestFixture]
    public class MaxShopPageListScrapperTest
    {
        private PageListScrapper _scrapper;
        private HtmlNode _htmlNode;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            //URL: https://www.maxshop.com/shop/tops/fashion-tops
            _htmlNode = LoadHtmlFromText();
            var loggingPath = Path.Combine(ConfigurationManager.AppSettings["LoggingPath"], "Maxshop", "Log.txt");
            var logger = new LoggerConfiguration().WriteTo.File(loggingPath).CreateLogger();
            _scrapper = new PageListScrapper(logger, _htmlNode);
        }

        private HtmlNode LoadHtmlFromText()
        {
            var doc = new HtmlDocument();
            doc.Load($"{Path.GetDirectoryName(TestContext.CurrentContext.TestDirectory)}/debug/MaxShopScrapper/maxshop_pagelist.txt");

            return doc.DocumentNode;
        }

        [Test]
        public void CanLoadFromTextFile()
        {
            Assert.That(_htmlNode, Is.Not.Null);
            Assert.That(_htmlNode.OuterHtml, Is.Not.Empty);
        }

        [Test]
        public async Task ScrapeTest()
        {
            //Arrange
            _scrapper.ItemXPath = ".js-productContent article";
            _scrapper.Items = new List<ItemSetting>
            {
                new ItemSetting
                {
                    Key = "Url",
                    AttributeName = "href",
                    Selector = "a.js-imagehover"
                },
                new ItemSetting
                {
                    Key = "ProductName",
                    Selector = "div:nth-child(2) a",
                    AttributeName = "title",
                    IsIdentifier = true
                },
                new ItemSetting
                {
                    Key = "Price",
                    Selector = "div.price"
                },
                new ItemSetting
                {
                    Key = "Image",
                    AttributeName = "src",
                    ValueRequired = true,
                    Selector = "img",
                    MultipleValue = true
                },
                new ItemSetting
                {
                    Key = "ColourSwatch",
                    AttributeName = "title",
                    MultipleValue = true,
                    Selector = ".swatchContainer a"
                }
            };

            //Act
            var products = await _scrapper.Scrape();


            //Assert
            Assert.That(products, Is.Not.Null);
            Assert.That(products, Is.All.Not.Null);
            Assert.That(products.All(x => x.Items.Exists(y => y.Name == "ProductName" && !string.IsNullOrEmpty(y.Value.ToString()))), Is.True);
            Assert.That(products.All(x => x.Items.Exists(y => y.Name == "Price" && !string.IsNullOrEmpty(y.Value.ToString()))), Is.True);
            Assert.That(products.All(x => x.Items.Exists(y => y.Name == "Url" && !string.IsNullOrEmpty(y.Value.ToString()))), Is.True);
        }
    }
}
