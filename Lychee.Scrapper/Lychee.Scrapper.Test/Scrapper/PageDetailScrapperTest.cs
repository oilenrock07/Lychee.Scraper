using System.Configuration;
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

            var webQueryService = new Mock<IWebQueryService>();
            var loggingPath = ConfigurationManager.AppSettings["LoggingPath"];
            var logger = new LoggerConfiguration().WriteTo.File(loggingPath).CreateLogger();

            _loggingService.Setup(x => x.Logger).Returns(logger);

            _scrapper = new PageDetailScrapper(_loggingService.Object, _settingRepository.Object, webQueryService.Object);
        }
    }
}
