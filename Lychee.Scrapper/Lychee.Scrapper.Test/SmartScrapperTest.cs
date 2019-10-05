using Lychee.Scrapper.Domain;
using Lychee.Scrapper.Domain.Interfaces;
using Lychee.Scrapper.Domain.Models.Scrappers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lychee.Scrapper.Domain.Services;
using Lychee.Scrapper.Repository.Repositories;
using Moq;
using PuppeteerSharp;

namespace Lychee.Scrapper.Test
{
    [TestFixture]
    public class SmartScrapperTest
    {
        private SmartScrapper _scrapper;
        private Mock<ILoggingService> _loggingService;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _loggingService = new Mock<ILoggingService>();
            //var scrapperFactory = new ScrapperFactory("SMART", new SettingRepository(), new LoggingService());

            //_scrapper = scrapperFactory.GetScrapper() as SmartScrapper;
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
    }
}
