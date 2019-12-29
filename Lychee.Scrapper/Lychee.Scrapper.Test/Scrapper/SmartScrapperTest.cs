using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Lychee.Scrapper.Domain.Enums;
using Lychee.Scrapper.Domain.Extensions;
using Lychee.Scrapper.Domain.Helpers;
using Lychee.Scrapper.Domain.Interfaces;
using Lychee.Scrapper.Domain.Models.Scrappers;
using Lychee.Scrapper.Repository.Entities;
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
        private Mock<IScrappedSettingRepository> _scrappedSettingRepository;

        private Logger _logger;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _settingRepository = new Mock<ISettingRepository>();
            _loggingService = new Mock<ILoggingService>();

            _webQueryService = new Mock<IWebQueryService>();
            _scrappedSettingRepository = new Mock<IScrappedSettingRepository>();

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

            _scrappedSettingRepository.Setup(x => x.GetItemSettings("TableDataSetting")).Returns(
                new List<ScrapeItemSetting>
                {
                    new ScrapeItemSetting {Key = "Name", Accessor = ScrappingAccessor.Html, Selector = "td:eq(0)"},
                    new ScrapeItemSetting {Key = "LoanAmount", Accessor = ScrappingAccessor.Value, Selector = "td:eq(1) input"},
                    new ScrapeItemSetting {Key = "Image", Accessor = ScrappingAccessor.Attribute, Selector = "td:eq(2) img", AttributeName = "src"},
                    new ScrapeItemSetting {Key = "ImageId", Accessor = ScrappingAccessor.Attribute, Selector = "td:eq(2) img", AttributeName = "data-id"},
                    new ScrapeItemSetting {Key = "LocationValue", Accessor = ScrappingAccessor.Value, Selector = "td:eq(3) select"},
                    new ScrapeItemSetting {Key = "LocationText", Accessor = ScrappingAccessor.Text, Selector = "td:eq(3) select option:selected"},
                    new ScrapeItemSetting {Key = "Enabled", Accessor = ScrappingAccessor.Attribute, Selector = "td:eq(4) input:checkbox", AttributeName = "checked"},
                });

            //Act
            var result = await _scrapper.Scrape();


            //Asserts
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task CanLoadTextboxValueGeneratedFromJavascipt()
        {
            //Arrange
            _scrapper.IsHeadless = true;
            _scrapper.Url = "http://lychee.scrapper.localhost/Home/Data";
            _scrapper.CustomScrappingInstructions = new List<SmartScrapper.CustomScrapping>
            {
                ScrapeTextboxDataGeneratedByJavascript
            };

            //Act
            var result = await _scrapper.Scrape();

            //Asserts
            Assert.That(result, Is.Not.Null);
            Assert.That(result.GetByKey("TextBox").First().Value, Is.Not.EqualTo("test"));
            Assert.That(result.GetByKey("TextBox").First().Value, Is.EqualTo("Content Loaded On Page Load"));

        }

        private async Task ScrapeTextboxDataGeneratedByJavascript(Page page, ResultCollection<ResultItemCollection> resultCollection)
        {
            var value = await PuppeteerHelper.GetTextboxValue(page, "#txtOnLoad");
            resultCollection.AddItem("TextBox", new List<ResultItem> { new ResultItem { Name = "TextBox", Value = value}});
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
            var mapping = _scrappedSettingRepository.Object.GetItemSettings("TableDataSetting");
            
            var dataSelector = ".modal-content table > tbody > tr";
            var tData = await page.EvaluateFunctionAsync(@"(dataSelector, tableMapping) => {
                const data = Array.from(document.querySelectorAll(dataSelector));
                var items = [];
                data.forEach((item, index) => {
                    var obj = {};
                    tableMapping.forEach((i, idx) => {

                        if (i.accessor === 'Html') {
                            obj[i.key] = getHtml($(item), i.selector);
                        }
                        else if (i.accessor === 'Value') {
                            obj[i.key] = getValue($(item), i.selector);
                        }
                        else if (i.accessor === 'Attribute') {
                            obj[i.key] = getAttribute($(item), i.selector, i.attributeName);
                        }
                        else if (i.accessor === 'Text') {
                            obj[i.key] = getText($(item), i.selector);
                        }
                    });
                    items.push(obj);
                });

                return items;
            }", dataSelector, mapping);


            foreach (JToken link in tData)
            {
                Debug.WriteLine(link["Name"].GetValue());
                Debug.WriteLine(link["LoanAmount"].GetValue());
                Debug.WriteLine(link["Image"].GetValue());
                Debug.WriteLine(link["ImageId"].GetValue());
                Debug.WriteLine(link["LocationValue"].GetValue());
                Debug.WriteLine(link["LocationText"].GetValue());
                Debug.WriteLine(link["Enabled"].GetValue());
            }

            //var content = page.MainFrame.GetContentAsync();
            //var document = new HtmlDocument();
            //document.LoadHtml(content.Result);

            //return document.DocumentNode;
        }
    }
}
