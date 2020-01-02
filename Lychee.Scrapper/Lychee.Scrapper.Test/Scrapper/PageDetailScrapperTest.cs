using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using Lychee.Scrapper.Domain.Interfaces;
using Lychee.Scrapper.Domain.Models.Scrappers;
using Lychee.Scrapper.Repository.Interfaces;
using Moq;
using NUnit.Framework;
using Serilog;
using Serilog.Core;

namespace Lychee.Scrapper.Test.Scrapper
{
    [TestFixture]
    public class PageDetailScrapperTest
    {

        private PageDetailScrapper _scrapper;
        private Mock<ISettingRepository> _settingRepository;
        private Mock<ILoggingService> _loggingService;
        private Logger _logger;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _settingRepository = new Mock<ISettingRepository>();
            _loggingService = new Mock<ILoggingService>();

            var webQueryService = new Mock<IWebQueryService>();
            var loggingPath = ConfigurationManager.AppSettings["LoggingPath"];
            var logger = new LoggerConfiguration().WriteTo.File(loggingPath).CreateLogger();

            _loggingService.Setup(x => x.Logger).Returns(logger);

            _scrapper = new PageDetailScrapper(_loggingService.Object, _settingRepository.Object, webQueryService.Object);
        }

        //[Test]
        //public async Task CanScrapeDataFromTextBoxThatIsPopulatedOnPageLoad()
        //{
        //    //Arrange
        //    _scrapper.Url = "http://lychee.scrapper.localhost/Home/Data";
        //    _scrapper.Items = new List<ScrapeItemSetting>
        //    {
        //        new ScrapeItemSetting { Key = "TextBox", Selector = "#txtOnLoad", AttributeName = "value"}
        //    };

        //    //Act
        //    var result = await _scrapper.Scrape();

        //    //Asserts
        //    Assert.That(result, Is.Not.Null);
        //}
    }
}
