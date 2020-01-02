using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Lychee.Scrapper.Domain.Helpers;
using Lychee.Scrapper.Domain.Interfaces;
using Lychee.Scrapper.Domain.Models.Scrappers;
using Lychee.Scrapper.Domain.Services;
using Lychee.Scrapper.Entities.Entities;
using Lychee.Scrapper.Repository.Repositories;
using Moq;
using NUnit.Framework;
using Serilog;

namespace Lychee.Scrapper.Test.MightyApeScrapper
{
    [TestFixture]
    public class MightyAppePageListScrapperTest
    {
        private PageListScrapper _scrapper;
        private HtmlNode _htmlNode;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            //URL: https://www.mightyape.co.nz/games/ps4/best-sellers
            _htmlNode = LoadHtmlFromText();

            var webQueryService = new Mock<IWebQueryService>();
            var loggingPath = Path.Combine(ConfigurationManager.AppSettings["LoggingPath"], "MightyApe", "Log.txt");
            var logger = new LoggerConfiguration().WriteTo.File(loggingPath).CreateLogger();
            _scrapper = new PageListScrapper(new SettingRepository(), new LoggingService(logger), webQueryService.Object, _htmlNode);
        }

        [Test]
        public void LoadHtml()
        {
            var url = "https://www.mightyape.co.nz/games/ps4/best-sellers";
            var web = new HtmlWeb();
            var doc = web.Load(url);
        }

        public static HtmlNode LoadHtmlFromText()
        {
            var doc = new HtmlDocument();
            doc.Load($"{Path.GetDirectoryName(TestContext.CurrentContext.TestDirectory)}/debug/MightyApeScrapper/mightyape_pagelist.txt");

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
            _scrapper.ItemXPath = "div.product-list div.product";
            _scrapper.Items = new List<ScrapeItemSetting>
            {
                new ScrapeItemSetting
                {
                    Key = "Url",
                    AttributeName = "href",
                    Selector = "div.details div.title a"
                },
                new ScrapeItemSetting
                {
                    Key = "ProductName",
                    Selector = "div.details div.title a",
                    IsIdentifier = true
                },
                new ScrapeItemSetting
                {
                    Key = "Price",
                    Selector = "div.product-price span.price"
                },
                new ScrapeItemSetting
                {
                    Key = "Image",
                    AttributeName = "src",
                    ValueRequired = true,
                    Selector = "div.image img"
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

        [Test]
        public async Task CanDetermineTheLastPage()
        {
            //Arrange
            _scrapper.ItemXPath = "div.product-list div.product";
            _scrapper.PaginationSettings = new PageListPagination
            {
                CanHaveIndefinitePagination = true,
                PaginationSelector = "ul.pagination"
            };
            _scrapper.Items = new List<ScrapeItemSetting>
            {
                new ScrapeItemSetting
                {
                    Key = "ProductName",
                    Selector = "div.details div.title a",
                    IsIdentifier = true
                }
            };

            //Act
            var products = await _scrapper.Scrape();

            //Assert
        }

        [Test]
        public void ShouldWriteCsvColumns()
        {
            //Arrange
            var result = new ResultCollection<ResultItemCollection>();
            result.AddItem("Witcher 3", new List<ResultItem>
            {
                new ResultItem { Name = "Product Name", Value = "Witcher 3"},
                new ResultItem { Name = "Url", Value = "http://witcher.com"}
            });
            result.AddItem("NBA 2k20", new List<ResultItem>
            {
                new ResultItem { Name = "Product Name", Value = "NBA 2k20"},
                new ResultItem { Name = "Url", Value = "http://nba2k20.com"}
            });
            var fileName = $"{Path.GetDirectoryName(TestContext.CurrentContext.TestDirectory)}/debug/MightyApeScrapper/export.csv";

            //Act and Assert 
            Assert.DoesNotThrow(() => CSVHelper.WriteObject(fileName, result));
            Assert.That(File.Exists(fileName));
        }

        [Test]
        public void ShouldAddDataToExistingCsvFile()
        {
            //Arrange
            var result = new ResultCollection<ResultItemCollection>();
            result.AddItem("Witcher 3", new List<ResultItem>
            {
                new ResultItem { Name = "Product Name", Value = "Witcher 3"},
                new ResultItem { Name = "Url", Value = "http://witcher.com"}
            });
            result.AddItem("NBA 2k20", new List<ResultItem>
            {
                new ResultItem { Name = "Product Name", Value = "NBA 2k20"},
                new ResultItem { Name = "Url", Value = "http://nba2k20.com"}
            });
            var fileName = $"{Path.GetDirectoryName(TestContext.CurrentContext.TestDirectory)}/debug/MightyApeScrapper/export_add_row.csv";

            //Act
            //Create the first CSV File
            CSVHelper.WriteObject(fileName, result);


            Assert.That(File.Exists(fileName));
        }

        [Test]
        public void ShouldWriteCsvWithMultipleColumns()
        {
            //Arrange
            var result = new ResultCollection<ResultItemCollection>();
            result.AddItem("Witcher 3", new List<ResultItem>
            {
                new ResultItem { Name = "Product Name", Value = "Witcher 3"},
                new ResultItem { Name = "Url", Value = "http://witcher.com"},
                new ResultItem { Name = "Image", Value = "image1"}
            });
            result.AddItem("NBA 2k20", new List<ResultItem>
            {
                new ResultItem { Name = "Product Name", Value = "NBA 2k20"},
                new ResultItem { Name = "Url", Value = "http://nba2k20.com"},
                new ResultItem { Name = "Image", Value = "imageA"},
                new ResultItem { Name = "Image", Value = "imageB"}
            });
            var fileName = $"{Path.GetDirectoryName(TestContext.CurrentContext.TestDirectory)}/debug/MightyApeScrapper/export_with_multiple_columns.csv";

            //Act and Assert 
            Assert.DoesNotThrow(() => CSVHelper.WriteObject(fileName, result));
            Assert.That(File.Exists(fileName));
        }
    }
}
