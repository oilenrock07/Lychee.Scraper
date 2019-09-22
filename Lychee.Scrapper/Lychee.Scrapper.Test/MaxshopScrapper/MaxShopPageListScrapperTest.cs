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
using Lychee.Scrapper.Domain.Helpers;
using Lychee.Scrapper.Domain.Services;
using Lychee.Scrapper.Repository.Entities;
using Lychee.Scrapper.Repository.Interfaces;
using Lychee.Scrapper.Repository.Repositories;
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
            _scrapper = new PageListScrapper(logger, new SettingRepository(), new LoggingService(), _htmlNode);
        }

        private HtmlNode LoadHtmlFromText()
        {
            var doc = new HtmlDocument();
            doc.Load($"{Path.GetDirectoryName(TestContext.CurrentContext.TestDirectory)}/debug/MaxShopScrapper/maxshop_pagelist.txt");

            return doc.DocumentNode;
        }

        [Test]
        public void CanMapAndSaveResultCollectionToDb()
        {
            var products = new ResultCollection<ResultItemCollection>
            {
                new ResultItemCollection
                {
                    Items = new List<ResultItem>
                    {
                        new ResultItem
                        {
                            Name = "ProductName",
                            Value = "Safeguard"
                        },
                        new ResultItem
                        {
                            Name = "Url",
                            Value = "http://safeguard.com"
                        },
                        new ResultItem
                        {
                            Name = "Price",
                            Value = "35"
                        },
                        new ResultItem
                        {
                            Name = "Image",
                            Value = "Safeguard Image 1",
                            IsMultiple = true
                        },
                        new ResultItem
                        {
                            Name = "Image",
                            Value = "Safeguard Image 2",
                            IsMultiple = true
                        }
                    },
                    Key = "Safeguard"
                },
                new ResultItemCollection
                {
                    Items = new List<ResultItem>
                    {
                        new ResultItem
                        {
                            Name = "ProductName",
                            Value = "Vaseline"
                        },
                        new ResultItem
                        {
                            Name = "Url",
                            Value = "http://vaseline.com"
                        },
                        new ResultItem
                        {
                            Name = "Price",
                            Value = "105"
                        },
                        new ResultItem
                        {
                            Name = "Image",
                            Value = "Vaseline Image 1",
                            IsMultiple = true
                        },
                        new ResultItem
                        {
                            Name = "Image",
                            Value = "Vaseline Image 2",
                            IsMultiple = true
                        },
                        new ResultItem
                        {
                            Name = "ColourSwatch",
                            Value = "Yellow",
                            IsMultiple = true
                        }
                    },
                    Key = "Vaseline"
                }
            };
            var resultCollectionService = new ResultCollectionService(new ColumnDefinitionRepository(), new ScrappedDataRepository(), new SettingRepository());
            resultCollectionService.SaveScrappedData(products);
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
