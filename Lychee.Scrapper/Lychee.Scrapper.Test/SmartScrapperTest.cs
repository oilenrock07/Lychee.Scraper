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
using PuppeteerSharp;

namespace Lychee.Scrapper.Test
{
    [TestFixture]
    public class SmartScrapperTest
    {
        private SmartScrapper _scrapper;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var scrapperFactory = new ScrapperFactory("SMART");

            _scrapper = scrapperFactory.GetScrapper() as SmartScrapper;
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
