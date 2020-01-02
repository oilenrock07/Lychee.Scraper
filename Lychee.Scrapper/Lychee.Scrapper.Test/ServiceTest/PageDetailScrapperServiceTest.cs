using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
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
    public class PageDetailScrapperServiceTest
    {
        private Mock<ISettingRepository> _settingRepository;
        private Mock<ILoggingService> _loggingService;
        private Mock<IColumnDefinitionRepository> _columnDefinitionRepository;
        private Mock<IWebQueryService> _webQueryService;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _settingRepository = new Mock<ISettingRepository>();
            _loggingService = new Mock<ILoggingService>();
            _columnDefinitionRepository = new Mock<IColumnDefinitionRepository>();
            _webQueryService = new Mock<IWebQueryService>();

            var loggingPath = ConfigurationManager.AppSettings["LoggingPath"];
            var logger = new LoggerConfiguration().WriteTo.File(loggingPath).CreateLogger();

            _loggingService.Setup(x => x.Logger).Returns(logger);
        }

        [Test]
        public async Task CanLoadPageDetail()
        {
            //Arrange
            _columnDefinitionRepository
                .Setup(x => x.GetAllScrappedDataColumnDefinitions())
                .Returns(new Dictionary<(string, string), string>
                {
                    {(nameof(ScrappedData), "MovieName"), "String1"},
                    {(nameof(ScrappedData), "Description"), "String2"},
                });

            var scrapper = new PageDetailScrapper(_loggingService.Object, _settingRepository.Object, _webQueryService.Object)
            {
                Url = "http://mymovies.localhost/Movie/Detail/670",
                Items = new List<ScrapeItemSetting>
                {
                    new ScrapeItemSetting {Key = "MovieName", Selector = "#content .page-header h1", IsIdentifier = true},
                    new ScrapeItemSetting {Key = "Description", Selector = "#content .portfolio-details p" }
                },
            };

            
            var scrappedDataRepository = new ScrappedDataRepositoryMock();
            var resultCollectionService = new ResultCollectionService(_columnDefinitionRepository.Object, scrappedDataRepository, _settingRepository.Object);
            var service = new PageDetailScrapperService(_settingRepository.Object, scrapper, _loggingService.Object, resultCollectionService);


            //Act
            await service.Scrape();

            //Assert
            Assert.That(scrappedDataRepository.Data.Count, Is.GreaterThan(0));
            Assert.That(scrappedDataRepository.Data.Count, Is.EqualTo(1));
            Assert.That(scrappedDataRepository.Data.First().String2, Is.EqualTo("An offbeat romantic comedy about a woman who doesn't believe true love exists, and the young man who falls for her."));
        }

    }
}
