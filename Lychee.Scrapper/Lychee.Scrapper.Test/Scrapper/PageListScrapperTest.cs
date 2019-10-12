using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Lychee.Scrapper.Domain.Interfaces;
using Lychee.Scrapper.Domain.Models.Scrappers;
using Lychee.Scrapper.Domain.Models.Web;
using Lychee.Scrapper.Domain.Services;
using Lychee.Scrapper.Repository.Entities;
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
        private Mock<IWebQueryService> _webQueryService;

        private Logger _logger;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _settingRepository = new Mock<ISettingRepository>();
            _loggingService = new Mock<ILoggingService>();

            _webQueryService = new Mock<IWebQueryService>();
            var loggingPath = ConfigurationManager.AppSettings["LoggingPath"];

            _logger = new LoggerConfiguration().WriteTo.File(loggingPath).CreateLogger();
            _scrapper = new PageListScrapper(_settingRepository.Object, _loggingService.Object, _webQueryService.Object);

            _loggingService.Setup(x => x.Logger).Returns(_logger);
        }

        [Test]
        public void CanDeepClonePageListScrapper()
        {
            //Arrange
            _scrapper.ItemXPath = "div.productList";
            _scrapper.PaginationSettings = new PageListPagination
            {
                ShowLastPagination = true,
                PaginationSelector = ".pagination"
            };
            _scrapper.Items = new List<ScrapeItemSetting>
            {
                new ScrapeItemSetting { Key = "Name", Selector = ".productname"},
                new ScrapeItemSetting { Key = "Url", Selector = ".url"},
            };
            var newScrapper = new PageListScrapper(_settingRepository.Object, _loggingService.Object, _webQueryService.Object);

            
            //Act
            _scrapper.DeepClone(newScrapper);
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
            _scrapper.Items = new List<ScrapeItemSetting>
            {
                new ScrapeItemSetting { Key = "Name", Selector = ".productname"},
                new ScrapeItemSetting { Key = "Url", Selector = ".url"},
            };
            var newScrapper = new PageListScrapper(_settingRepository.Object, _loggingService.Object, _webQueryService.Object);


            //Act
            _scrapper.Clone(newScrapper);
            newScrapper.ItemXPath = "div .list";
            newScrapper.PaginationSettings.ShowLastPagination = false;
            newScrapper.Items[0].Selector = "#productName";

            //Asserts
            Assert.That(newScrapper.Items.Count, Is.EqualTo(2));
            Assert.That(newScrapper.ItemXPath, Is.EqualTo("div .list"));
            Assert.That(_scrapper.Items[0].Selector, Is.EqualTo("#productName"));
            Assert.That(newScrapper.Items[0].Selector, Is.EqualTo("#productName"));
            Assert.That(_scrapper.ItemXPath, Is.EqualTo("div.productList"));
            Assert.That(newScrapper.PaginationSettings.ShowLastPagination, Is.EqualTo(false));
            Assert.That(_scrapper.PaginationSettings.ShowLastPagination, Is.EqualTo(false));

        }

        [Test]
        [Ignore("MySql Service needs to be running or else this will fail. This is also highly dependent on the cookie")]
        public void CanLoadPageWithCredentialsRequired()
        {
            //Arrange
            _scrapper.ItemXPath = ".body-content table:first-child > tbody > tr";
            _scrapper.Url = "http://payroll.localhost/EmployeeController/Index";
            _scrapper.Items = new List<ScrapeItemSetting>
            {
                new ScrapeItemSetting { Key = "Id", Selector = "td:first-child"}
            };

            var webQuery = new WebQuery(_scrapper.Url);
            webQuery.AddCookie(".AspNet.ApplicationCookie", "FnilTTTvqPOprOsZV-BoyctK7Av1WSPTwWRxjpM3uLF2zgvbwhjB1tZjJMXG4QFN6TSVBJS0J1NFr0U1P7D21Yj77pw7D4iyob_n60sGMPUELH_C63FlLxT4VlAn68ejs5rc3U0SMPcLHQrlPW-L12wX_RBqVckS7YLBqE9S6UVHLGTfqbymFxVMaismDJcd3ewTC-BOuo4wjB1Bns9EUbshUXhNUNmSFJtgw8soGHwsSuPPe3t4IBRHXxc2qeIyVNoB4m1rYXuFge6XtodBoMhGiDq_laCB5spoh7XI5UZC4hKDMfF4ms0r0wZSS-EXMmzy-vACIxR8XB8u9MH9If2M0l99B_AwfXZqg7PLdMq_ZoALOsTRyPGrXnvDRcWqIm6gmSVuZC-TEG8kqPcXgp18Gm6sUr-J4LAseLJan6UO2e6_2gsuGg8hZNBoggwE");
            webQuery.AddCookie("__RequestVerificationToken", "f503FWEz0K8zF21K5z5YD5YvS_5uF2QQ142IDC9toQtI--njNpXG0efNZcYamnz-A5zr0Hu9weET7lDWRt5PxW26hIgvBdUXDO-9EN3P59A1");

            //Act
            var result = _scrapper.AdvancedLoadPage();

            //Asserts
            Assert.That(result, Is.Not.Null);
        }
    }
}
