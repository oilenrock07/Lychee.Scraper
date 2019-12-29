using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lychee.Scrapper.Domain.Interfaces;
using Lychee.Scrapper.Domain.Models.Scrappers;
using Lychee.Scrapper.Domain.Services;
using Lychee.Scrapper.Repository.Interfaces;
using Lychee.Scrapper.Repository.Repositories;
using Lychee.Scrapper.Test.MightyApeScrapper;
using Moq;
using NUnit.Framework;
using Serilog;
using Serilog.Core;

namespace Lychee.Scrapper.Test.ServiceTest
{
    [TestFixture]
    public class PageListScrapperServicePaginationTest
    {
        private Mock<ISettingRepository> _settingRepository;
        private Mock<PageListScrapper> _pageListScrapper;
        private Mock<ILoggingService> _loggingService;
        private Mock<IResultCollectionService> _resultCollectionService;
        private Mock<IWebQueryService> _webQueryService;

        private Logger Logger;
        

        private PageListScrapperService GetPageListScrapperService(PageListScrapper scrapper)
        {
            return new PageListScrapperService(_settingRepository.Object, scrapper, _loggingService.Object,
                _resultCollectionService.Object, _webQueryService.Object, null);
        }

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _settingRepository = new Mock<ISettingRepository>();
            _pageListScrapper = new Mock<PageListScrapper>();
            _loggingService = new Mock<ILoggingService>();
            _resultCollectionService = new Mock<IResultCollectionService>();
            _webQueryService = new Mock<IWebQueryService>();

            var loggingPath = ConfigurationManager.AppSettings["LoggingPath"];
            Logger = new LoggerConfiguration().WriteTo.File(loggingPath).CreateLogger();

            _loggingService.Setup(x => x.Logger).Returns(Logger);
        }

        [Test, TestCaseSource(typeof(PageListScrapperTestData), "TestCases")]
        public void CanDetermineIfFirstPageFromUrl(PageListScrapper scrapper, bool isFirstPage)
        {
            //Arrange
            _settingRepository.Setup(x => x.GetSettingValue<string>("PageListScrapper.URL.QueryStringPageVariable")).Returns("page");
            var pageListPaginationService = new PageListPaginationService(_settingRepository.Object, _loggingService.Object, scrapper);

            //Act
            var result = pageListPaginationService.IsFirstPage(null);

            //Asserts
            Assert.That(result, Is.EqualTo(isFirstPage));
        }

        [Test]
        public void CanDetermineIfFirstPageByLookingAtThePaginationDOM_ShouldPass()
        {
            //Arrange
            var scrapper = new PageListScrapper(new SettingRepository(), _loggingService.Object, _webQueryService.Object, MightyAppePageListScrapperTest.LoadHtmlFromText())
            {
                PaginationSettings = new PageListPagination {PaginationSelector = ".pagination li.active span"}
            };

            var pageListPaginationService = new PageListPaginationService(_settingRepository.Object, _loggingService.Object, scrapper);

            //Act
            var result = pageListPaginationService.IsFirstPage(scrapper.GetLoadedHtmlNode());

            //Asserts
            Assert.That(result, Is.EqualTo(true));
        }

        [Test]
        public void CanDetermineIfFirstPageByLookingAtThePaginationDOM_ShouldNOTPass_InvalidDOMPage()
        {
            //Arrange
            var scrapper = new PageListScrapper(new SettingRepository(), _loggingService.Object, _webQueryService.Object, MightyAppePageListScrapperTest.LoadHtmlFromText())
            {
                PaginationSettings = new PageListPagination { PaginationSelector = ".pagination li active span" }
            };

            var pageListPaginationService = new PageListPaginationService(_settingRepository.Object, _loggingService.Object, scrapper);

            //Act
            var result = pageListPaginationService.IsFirstPage(scrapper.GetLoadedHtmlNode());

            //Asserts
            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public void CanDetermineIfFirstPageByLookingAtThePaginationDOM_ShouldNOTPass_OnPage2()
        {
            ////Arrange
            //var scrapper = new PageListScrapper(null, new SettingRepository(), new LoggingService(), MightyAppePageListScrapperTest.LoadHtmlFromText())
            //{
            //    PaginationSettings = new PageListPagination { PaginationSelector = ".pagination li active span" }
            //};

            //var scrapperService = new PageListScrapperService(_settingRepository.Object, Logger, scrapper);
            
            ////Act
            //var result = scrapperService.IsFirstPage(htmlNode);

            ////Asserts
            //Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public void CanDetermineIfFirstPageByLookingAtThePaginationDOM_ShouldNOTPass_DoesNotHavePaginationDOM()
        {
            //Arrange
            //var scrapper = new PageListScrapper(null, new SettingRepository(), new LoggingService(), MightyAppePageListScrapperTest.LoadHtmlFromText())
            //{
            //    PaginationSettings = new PageListPagination { PaginationSelector = ".pagination li active span" }
            //};

            //var scrapperService = new PageListScrapperService(_settingRepository.Object, Logger, scrapper);

            ////Act
            //var result = scrapperService.IsFirstPage(scrapper.GetLoadedHtmlNode());

            ////Asserts
            //Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public void CanGetTheLastPageNumberIfTotalProductIsGiven()
        {
            //Arrange
            _settingRepository.Setup(x => x.GetSettingValue<bool>("PageListScrapper.Pagination.IsLastPageGiven")).Returns(false);
            _settingRepository.Setup(x => x.GetSettingValue<bool>("PageListScrapper.Pagination.IsTotalNumberOfProductsGiven")).Returns(true);
            _settingRepository.Setup(x => x.GetSettingValue<int>("PageListScrapper.Pagination.ProductsPerPage")).Returns(40);
            _settingRepository.Setup(x => x.GetSettingValue<string>("PageListScrapper.Pagination.TotalNumberOfProductsSelector")).Returns(".products .gallery-header .summary .results .total"); 


            var scrapper = new PageListScrapper(new SettingRepository(), _loggingService.Object, _webQueryService.Object, MightyAppePageListScrapperTest.LoadHtmlFromText())
            {
                PaginationSettings = new PageListPagination { PaginationSelector = ".pagination li active span" }
            };

            var pageListPaginationService = new PageListPaginationService(_settingRepository.Object, _loggingService.Object, scrapper);
            var node = scrapper.GetLoadedHtmlNode();

            //Act
            var lastPage = pageListPaginationService.GetLastPageNumber(node);

            //Assert
            Assert.That(lastPage, Is.EqualTo(13));
        }
    }

    internal class PageListScrapperTestData
    {
        public static IEnumerable TestCases
        {
            get
            {
                yield return new object[] { new PageListScrapper(new SettingRepository(), new LoggingService(null), new WebQueryService(null)) { Url = "https://www.mightyape.co.nz/games/ps4/best-sellers" }, false};
                yield return new object[] { new PageListScrapper(new SettingRepository(), new LoggingService(null), new WebQueryService(null)) { Url = "https://www.mightyape.co.nz/games/ps4/best-sellers?page=1" }, true };
                yield return new object[] { new PageListScrapper(new SettingRepository(), new LoggingService(null), new WebQueryService(null)) { Url = "https://www.mightyape.co.nz/games/ps4/best-sellers?page=2" }, false };
            }
        }
    }
}
