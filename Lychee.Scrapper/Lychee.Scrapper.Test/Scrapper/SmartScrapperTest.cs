using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Lychee.Scrapper.Domain.Enums;
using Lychee.Scrapper.Domain.Interfaces;
using Lychee.Scrapper.Domain.Models.Scrappers;
using Lychee.Scrapper.Repository.Interfaces;
using Moq;
using Newtonsoft.Json.Linq;
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

            _settingRepository.Setup(x => x.GetSettingValue<string>("SmartScrapper.Chromium.DownloadPath")).Returns(@"C:\Cawi\DEV\Lychee\Lychee.Scrapper\CustomChromium");

            var loggingPath = ConfigurationManager.AppSettings["LoggingPath"];
            _logger = new LoggerConfiguration().WriteTo.File(loggingPath).CreateLogger();
            _scrapper = new SmartScrapper(_settingRepository.Object, _loggingService.Object, _webQueryService.Object);

            _loggingService.Setup(x => x.Logger).Returns(_logger);
        }

        [Test]
        public async Task CanLoadAjaxFromPage()
        {
            _scrapper.Url = "http://lychee.scrapper.localhost/";
            _scrapper.CustomScrappingInstructions = new List<SmartScrapper.CustomScrapping>
            {
                async delegate(Page page, ResultCollection<ResultItemCollection> resultCollection)
                {
                    await page.WaitForSelectorAsync("#Users div");
                    var result = await page.MainFrame.GetContentAsync();
                }
            };

            await _scrapper.Scrape();
        }

        [Test]
        public async Task CanScrapeContentFromModal()
        {
            //Arrange
            _scrapper.IsHeadless = true;
            _scrapper.Url = "http://lychee.scrapper.localhost/Home/Data";
            _scrapper.CustomScrappingInstructions = new List<SmartScrapper.CustomScrapping>
            {
                ScrapeDataFromModal
            };

            //Act
            var result = await _scrapper.Scrape();


            //Asserts
            Assert.That(result, Is.Not.Null);
        }

        private async Task ScrapeDataFromModal(Page page, ResultCollection<ResultItemCollection> resultCollection)
        {
            var buttonSelector = ".btn-primary";
            await page.WaitForSelectorAsync(buttonSelector); //wait for the button to be displayed
            await page.ClickAsync(buttonSelector); //click the button to show the modal

            //wait for the modal to be fully shown
            var resultSelector = ".modal-body .table";
            await page.WaitForSelectorAsync(resultSelector);



            //try to get the table headers
            var tableMapping = new List<SmartScrapperTableObjectMapping>
            {
                new SmartScrapperTableObjectMapping {ColumnName = "Name", Accessor = TableObjectAccessor.InnerText, Index = 0},
                new SmartScrapperTableObjectMapping {ColumnName = "LoanAmount", Accessor = TableObjectAccessor.Value, Index = 1, ElementSelector = "input"},
                new SmartScrapperTableObjectMapping {ColumnName = "Image", Accessor = TableObjectAccessor.Attribute, Index = 2, ElementSelector = "img", ElementAttribute = "src"},
                new SmartScrapperTableObjectMapping {ColumnName = "ImageId", Accessor = TableObjectAccessor.Attribute, Index = 2, ElementSelector = "img", ElementAttribute = "data-id"},
                new SmartScrapperTableObjectMapping {ColumnName = "LocationValue", Accessor = TableObjectAccessor.Value, Index = 3, ElementSelector = "select"},
                new SmartScrapperTableObjectMapping {ColumnName = "LocationText", Accessor = TableObjectAccessor.SelectedDropDownText, Index = 3,  ElementSelector = "select"},
            };

            await page.AddScriptTagAsync("http://lychee.scrapper.localhost/Scripts/ScrapperFunctions.js");
            var dataSelector = "table > tbody > tr";
            var tData = await page.EvaluateFunctionAsync(@"(dataSelector, tableMapping) => {
                const data = Array.from(document.querySelectorAll(dataSelector));
                var items = [];
                data.forEach((item, index) => {
                    var obj = {};
                    tableMapping.forEach((i, idx) => {

                        if (i.accessor === 'InnerText') {
                            obj[i.columnName] = getInnerText(item.querySelectorAll('td').item(parseInt(i.index)));
                        }
                        else if (i.accessor === 'Value') {
                            obj[i.columnName] = getInputValue(item.querySelectorAll('td').item(parseInt(i.index)).querySelector(i.elementSelector));
                        }
                        else if (i.accessor === 'Attribute') {
                            obj[i.columnName] = getAttribute(item.querySelectorAll('td').item(parseInt(i.index)).querySelector(i.elementSelector), (i.elementAttribute));
                        }
                        else if (i.accessor === 'SelectedDropDownText') {
                            obj[i.columnName] = dropDownSelectedText(item.querySelectorAll('td').item(parseInt(i.index)).querySelector(i.elementSelector));
                        }
                    });
                    items.push(obj);
                });

                return items;
            }", dataSelector, tableMapping);


            foreach (JToken link in tData)
            {
                Debug.WriteLine(link["LocationText"].Value<string>());
            }

            //var content = page.MainFrame.GetContentAsync();
            //var document = new HtmlDocument();
            //document.LoadHtml(content.Result);

            //return document.DocumentNode;
        }
    }
}
