using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Lychee.Scrapper.Domain.Interfaces;
using Lychee.Scrapper.Domain.Models.Scrappers;
using Lychee.Scrapper.Domain.Services;
using Lychee.Scrapper.Repository.Interfaces;
using Lychee.Scrapper.Repository.Repositories;
using Moq;
using NUnit.Framework;
using Serilog;
using Serilog.Core;

namespace Lychee.Scrapper.Test.Scrapper
{
    [TestFixture]
    public class PageListScrapperTest
    {

        private PageListScrapper _scrapper;
        private Mock<ISettingRepository> _settingRepository;
        private Mock<ILoggingService> _loggingService;
        private Logger _logger;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _settingRepository = new Mock<ISettingRepository>();
            _loggingService = new Mock<ILoggingService>();

            var loggingPath = ConfigurationManager.AppSettings["LoggingPath"];

            _logger = new LoggerConfiguration().WriteTo.File(loggingPath).CreateLogger();
            _scrapper = new PageListScrapper(_settingRepository.Object, _loggingService.Object);
        }

        [Test]
        public void CanClonePageListScrapper()
        {
            //Arrange
            _scrapper.ItemXPath = "div.productList";
            _scrapper.PaginationSettings = new PageListPagination
            {
                ShowLastPagination = true,
                PaginationSelector = ".pagination"
            };
            _scrapper.Items = new List<ItemSetting>
            {
                new ItemSetting { Key = "Name", Selector = ".productname"},
                new ItemSetting { Key = "Url", Selector = ".url"},
            };
            var newScrapper = new PageListScrapper(_settingRepository.Object, _loggingService.Object);

            
            //Act
            _scrapper.Clone(newScrapper);
            newScrapper.ItemXPath = "div .list";
            newScrapper.PaginationSettings.ShowLastPagination = false;
            newScrapper.Items[0].Selector = "#productName";

            //Asserts
            Assert.That(newScrapper.Items.Count, Is.EqualTo(2));
            Assert.That(newScrapper.ItemXPath, Is.EqualTo("div .list"));
            Assert.That(_scrapper.Items[0].Selector, Is.EqualTo(".productname"));
            Assert.That(newScrapper.Items[0].Selector, Is.EqualTo("#productName"));
            Assert.That(_scrapper.ItemXPath, Is.EqualTo("div.productList"));
            Assert.That(newScrapper.PaginationSettings.ShowLastPagination, Is.EqualTo(false));
            Assert.That(_scrapper.PaginationSettings.ShowLastPagination, Is.EqualTo(true));

        }
    }
}
