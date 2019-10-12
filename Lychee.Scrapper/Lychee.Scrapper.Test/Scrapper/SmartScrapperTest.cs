using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lychee.Scrapper.Domain.Interfaces;
using Lychee.Scrapper.Domain.Models.Scrappers;
using Lychee.Scrapper.Repository.Interfaces;
using Moq;
using NUnit.Framework;
using PuppeteerSharp;
using Serilog;
using Serilog.Core;

namespace Lychee.Scrapper.Test.Scrapper
{
    [TestFixture]
    public class SmartScrapperTest
    {

        private SmartScrapper _scrapper;
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
            _scrapper = new SmartScrapper(_settingRepository.Object, _loggingService.Object, _webQueryService.Object);

            _loggingService.Setup(x => x.Logger).Returns(_logger);
        }

        [Test]
        public async Task CanLoadAjaxFromPage()
        {
            _scrapper.Url = "http://lychee.scrapper.localhost/";
            _scrapper.CustomScrappingInstructions += async page =>
            {
                await page.WaitForSelectorAsync("#Users div");
                var result = await page.MainFrame.GetContentAsync();
            };

            await _scrapper.Scrape();
        }

        [Test]
        public async Task CanScrapeContentFromModal()
        {
            //Arrange

            //Act
            _scrapper.IsHeadless = true;
            _scrapper.Url = "http://lychee.scrapper.localhost/Home/Data";
            _scrapper.CustomScrappingInstructions += ScrapeDataFromModal;
            var result = await _scrapper.Scrape();


            //Asserts
            Assert.That(result, Is.Not.Null);
        }

        private async Task ScrapeDataFromModal(Page page)
        {
            var buttonSelector = "button";
            await page.WaitForSelectorAsync(buttonSelector); //wait for the button to be displayed
            await page.ClickAsync(buttonSelector); //click the button to show the modal

            //wait for the modal to be fully shown
        }
    }
}
