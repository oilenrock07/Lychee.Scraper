using System;
using System.Collections.Generic;
using System.Configuration;
using Lychee.Scrapper.Domain.Interfaces;
using Lychee.Scrapper.Domain.Models.Scrappers;
using Lychee.Scrapper.Domain.Services;
using Lychee.Scrapper.Repository.Entities;
using Lychee.Scrapper.Repository.Interfaces;
using Moq;
using NUnit.Framework;
using Serilog;

namespace Lychee.Scrapper.Test.ServiceTest
{
    [TestFixture]
    public class PageListScrapperServiceTest : PageListScrapperService
    {
        private static Mock<ISettingRepository> _settingRepository = new Mock<ISettingRepository>();
        private static Mock<IResultCollectionService> _resultCollectionService = new Mock<IResultCollectionService>();
        private static Mock<ILoggingService> _loggingService = new Mock<ILoggingService>();
        private static Mock<IColumnDefinitionRepository> _columnDefinitionRepository = new Mock<IColumnDefinitionRepository>();

        public PageListScrapperServiceTest() : base(_settingRepository.Object, null, _loggingService.Object,
            _resultCollectionService.Object)
        {
        }

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _settingRepository
                .Setup(x => x.GetSettingValue<string>("Scrapping.PageListScrapper.QueryStringPageVariable"))
                .Returns("page");
        }

        [Test]
        [TestCase(2, "https://www.mightyape.co.nz/games/ps4/best-sellers", "https://www.mightyape.co.nz/games/ps4/best-sellers?page=2")]
        [TestCase(2, "https://www.mightyape.co.nz/games/ps4/best-sellers?page=1", "https://www.mightyape.co.nz/games/ps4/best-sellers?page=2")]
        [TestCase(2, "https://www.mightyape.co.nz/games/ps4/best-sellers?search=witcher%203", "https://www.mightyape.co.nz/games/ps4/best-sellers?search=witcher+3&page=2")]
        [TestCase(2, "https://www.mightyape.co.nz/games/ps4/best-sellers?search=witcher%203&page=2", "https://www.mightyape.co.nz/games/ps4/best-sellers?search=witcher+3&page=2")]
        public void CanGetTheNextUrl(int page, string currentUrl, string expectedUrl)
        {
            _settingRepository
                .Setup(x => x.GetSettingValue<bool>("Scrapping.PageListScrapper.QueryStringMapRouted"))
                .Returns(false);

            var url = GetNextUrl(page, currentUrl);

            Assert.That(url, Is.EqualTo(expectedUrl));
        }

        /// <summary>
        /// This is when the url is formatted. Example http://mymovies.localhost/Movie/Index/2 (2) is the current page
        /// </summary>
        /// <param name="routeMap"></param>
        /// <param name="currentUrl"></param>
        /// <param name="expectedUrl"></param>
        [Test]
        [TestCase("/Movie/Index/", "http://mymovies.localhost", "http://mymovies.localhost/Movie/Index/2")]
        [TestCase("", "http://mymovies.localhost/Movie/Index/Category/Action", "http://mymovies.localhost/Movie/Index/Category/Action/2")]
        public void CanGetTheNextUrlWithUrlFormatting(string routeMap, string currentUrl, string expectedUrl)
        {
            _settingRepository
                .Setup(x => x.GetSettingValue<bool>("Scrapping.PageListScrapper.QueryStringMapRouted"))
                .Returns(true);
            _settingRepository
                .Setup(x => x.GetSettingValue<string>("Scrapping.URL.QueryStringPaginationRouteMap"))
                .Returns(routeMap);
            

            var url = GetNextUrl(2, currentUrl);

            Assert.That(url, Is.EqualTo(expectedUrl));
        }

        [Test]
        public void CanLoadOtherPagesAsynchronously()
        {
            //Arrange
            var loggingPath = ConfigurationManager.AppSettings["LoggingPath"];
            var logger = new LoggerConfiguration().WriteTo.File(loggingPath).CreateLogger();

            _loggingService.Setup(x => x.Logger).Returns(logger);
            _columnDefinitionRepository
                .Setup(x => x.GetAllScrappedDataColumnDefinitions())
                .Returns(new Dictionary<(string, string), string>
                {
                    {(nameof(ScrappedData), "MovieName"), "String1"}
                });

            var scrapper = new PageListScrapper(_settingRepository.Object, _loggingService.Object)
            {
                Url = "http://mymovies.localhost/",
                ItemXPath = "#content .container .row:first-child .col-sm-6",
                Items = new List<ItemSetting> {new ItemSetting {Key = "MovieName", Selector = "h3"}},
                PaginationSettings = new PageListPagination
                {
                    PaginationSelector = "ul.pagination"
                }
            };

            _settingRepository.Setup(x => x.GetSettingValue<bool>("Scrapping.ScrapperSetting.LogDownloadedPage")).Returns(false);
            _settingRepository.Setup(x => x.GetSettingValue<bool>("Scrapping.PageListScrapper.QueryStringMapRouted")).Returns(true);
            _settingRepository.Setup(x => x.GetSettingValue<string>("Scrapping.URL.QueryStringPaginationRouteMap")).Returns("Movie/Index/");
            _settingRepository.Setup(x => x.GetSettingValue<bool>("Scrapping.RelatedData.IsMultipleRow")).Returns(false);

            var scrappedDataRepository = new ScrappedDataRepositoryMock();
            var resultCollectionService = new ResultCollectionService(_columnDefinitionRepository.Object, scrappedDataRepository, _settingRepository.Object);
            var service = new PageListScrapperService(_settingRepository.Object, scrapper, _loggingService.Object, resultCollectionService);


            //Act
            var startTime = DateTime.Now;
            service.ScrapeOtherPages(58, scrapper);
            var totalExecutionTime = DateTime.Now.Subtract(startTime);

            //Assert
            Assert.That(scrappedDataRepository.Data.Count, Is.GreaterThan(0));
            Assert.That(scrappedDataRepository.Data.Count, Is.EqualTo(150));
        }
    }
}
