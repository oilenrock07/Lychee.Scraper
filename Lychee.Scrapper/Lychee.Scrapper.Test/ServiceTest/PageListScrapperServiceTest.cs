using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using Lychee.Scrapper.Domain.Interfaces;
using Lychee.Scrapper.Domain.Models.Scrappers;
using Lychee.Scrapper.Domain.Services;
using Lychee.Scrapper.Entities.Entities;
using Lychee.Scrapper.Repository.Interfaces;
using Moq;
using NUnit.Framework;
using Serilog;

namespace Lychee.Scrapper.Test.ServiceTest
{
    [TestFixture]
    public class PageListScrapperServiceTest : PageListScrapperService
    {
        private static readonly Mock<ISettingRepository> _settingRepository = new Mock<ISettingRepository>();
        private static readonly Mock<IResultCollectionService> _resultCollectionService = new Mock<IResultCollectionService>();
        private static readonly Mock<ILoggingService> _loggingService = new Mock<ILoggingService>();
        private static readonly Mock<IColumnDefinitionRepository> _columnDefinitionRepository = new Mock<IColumnDefinitionRepository>();
        private static readonly Mock<IWebQueryService> _webQueryService = new Mock<IWebQueryService>();
        private static readonly Mock<IPageListPaginationService> _pageListPaginationService = new Mock<IPageListPaginationService>();

        public PageListScrapperServiceTest() : base(_settingRepository.Object, null, _loggingService.Object,
            _resultCollectionService.Object, _webQueryService.Object, _pageListPaginationService.Object)
        {
        }

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var loggingPath = ConfigurationManager.AppSettings["LoggingPath"];
            var logger = new LoggerConfiguration().WriteTo.File(loggingPath).CreateLogger();

            _loggingService.Setup(x => x.Logger).Returns(logger);

            _settingRepository
                .Setup(x => x.GetSettingValue<string>("Scrapping.URL.QueryStringPageVariable"))
                .Returns("page");

            _settingRepository.Setup(x => x.GetSettingValue<bool>("Core.Logger.LogDownloadedPage")).Returns(false);
            _settingRepository.Setup(x => x.GetSettingValue<bool>("Scrapping.PageListScrapper.QueryStringMapRouted")).Returns(true);
            _settingRepository.Setup(x => x.GetSettingValue<string>("PageListScrapper.Pagination.QueryStringRouteMap")).Returns("Movie/Index/");
            _settingRepository.Setup(x => x.GetSettingValue<bool>("Core.Schema.IsMultipleRow")).Returns(false);
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
                .Setup(x => x.GetSettingValue<string>("PageListScrapper.Pagination.QueryStringRouteMap"))
                .Returns(routeMap);
            

            var url = GetNextUrl(2, currentUrl);

            Assert.That(url, Is.EqualTo(expectedUrl));
        }

        [Test]
        public void CanLoadOtherPagesAsynchronously()
        {
            //Arrange
            _columnDefinitionRepository
                .Setup(x => x.GetAllScrappedDataColumnDefinitions())
                .Returns(new Dictionary<(string, string), string>
                {
                    {(nameof(ScrappedData), "MovieName"), "String1"}
                });

            var scrapper = new PageListScrapper(_settingRepository.Object, _loggingService.Object, _webQueryService.Object)
            {
                Url = "http://mymovies.localhost/",
                ItemXPath = "#content .container .row:first-child .col-sm-6",
                Items = new List<ScrapeItemSetting> {new ScrapeItemSetting {Key = "MovieName", Selector = "h3"}},
                PaginationSettings = new PageListPagination
                {
                    PaginationSelector = "ul.pagination"
                }
            };

            
            var scrappedDataRepository = new ScrappedDataRepositoryMock();
            var resultCollectionService = new ResultCollectionService(_columnDefinitionRepository.Object, scrappedDataRepository, _settingRepository.Object);
            var service = new PageListScrapperService(_settingRepository.Object, scrapper, _loggingService.Object,
                resultCollectionService, _webQueryService.Object, _pageListPaginationService.Object);


            //Act
            var startTime = DateTime.Now;
            service.ScrapeOtherPages(58, scrapper);
            var totalExecutionTime = DateTime.Now.Subtract(startTime);

            //Assert
            Assert.That(scrappedDataRepository.Data.Count, Is.GreaterThan(0));
            Assert.That(scrappedDataRepository.Data.Count, Is.EqualTo(2808));
        }

        [Test]
        [Ignore("This is only to prove that async is faster than non-async. The difference is not much but since async is running first and non-async is getting all the cached request from async. Sometimes, nonasync is faster a few milliseconds")]
        public async Task ProveThatLoadingPageAsynchronouslyIsFasterThanLoading1By1()
        {
            //Arrange
            _columnDefinitionRepository
                .Setup(x => x.GetAllScrappedDataColumnDefinitions())
                .Returns(new Dictionary<(string, string), string>
                {
                    {(nameof(ScrappedData), "MovieName"), "String1"}
                });

            var scrapper = new PageListScrapper(_settingRepository.Object, _loggingService.Object, _webQueryService.Object)
            {
                Url = "http://mymovies.localhost/",
                ItemXPath = "#content .container .row:first-child .col-sm-6",
                Items = new List<ScrapeItemSetting> { new ScrapeItemSetting { Key = "MovieName", Selector = "h3" } },
                PaginationSettings = new PageListPagination
                {
                    PaginationSelector = "ul.pagination"
                }
            };

            var scrappedDataRepository = new ScrappedDataRepositoryMock();
            var resultCollectionService = new ResultCollectionService(_columnDefinitionRepository.Object, scrappedDataRepository, _settingRepository.Object);
            var service = new PageListScrapperService(_settingRepository.Object, scrapper, _loggingService.Object,
                resultCollectionService, _webQueryService.Object, _pageListPaginationService.Object);


            //Act
            //Async
            var startTime = DateTime.Now;
            service.ScrapeOtherPages(58, scrapper);
            var totalExecutionTime = DateTime.Now.Subtract(startTime);

            //Non Async
            var nonAsyncStartTime = DateTime.Now;
            var resultCollection = new ResultCollection<ResultItemCollection>();

            for (var i = 2; i <= 58; i++)
            {
                var newScrapper = new PageListScrapper(_settingRepository.Object, _loggingService.Object, _webQueryService.Object);
                scrapper.Clone(newScrapper);
                newScrapper.Url = GetNextUrl(i, "http://mymovies.localhost/");
                var result = await newScrapper.Scrape();
                resultCollection.AddRange(result);
            }

            var totalNonAsyncExecutionTime = DateTime.Now.Subtract(nonAsyncStartTime);

            //Asserts
            Assert.That(scrappedDataRepository.Data.Count, Is.GreaterThan(0));
            Assert.That(scrappedDataRepository.Data.Count, Is.EqualTo(2808));
            Assert.That(resultCollection.Count, Is.GreaterThan(0));
            Assert.That(resultCollection.Count, Is.EqualTo(2808));
            Assert.That(totalExecutionTime, Is.LessThan(totalNonAsyncExecutionTime));
        }
    }
}
