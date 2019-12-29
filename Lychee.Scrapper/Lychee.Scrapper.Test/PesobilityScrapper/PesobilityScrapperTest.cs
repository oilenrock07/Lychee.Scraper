using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Lychee.Scrapper.Domain.Enums;
using Lychee.Scrapper.Domain.Extensions;
using Lychee.Scrapper.Domain.Helpers;
using Lychee.Scrapper.Domain.Interfaces;
using Lychee.Scrapper.Domain.Models.Scrappers;
using Lychee.Scrapper.Domain.Services;
using Lychee.Scrapper.Repository.Entities;
using Lychee.Scrapper.Repository.Interfaces;
using Lychee.Scrapper.Repository.Repositories;
using Moq;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using PuppeteerSharp;
using Serilog;
using Serilog.Core;

namespace Lychee.Scrapper.Test.MyMoviesScrapper
{
    [TestFixture]
    public class PesobilityScrapperTest
    {
        private SmartScrapper _scrapper;
        private Mock<ISettingRepository> _settingRepository;
        private Mock<ILoggingService> _loggingService;
        private Mock<IWebQueryService> _webQueryService;
        private IScrappedSettingRepository _scrappedSettingRepository;

        private Logger _logger;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _settingRepository = new Mock<ISettingRepository>();
            _loggingService = new Mock<ILoggingService>();

            _webQueryService = new Mock<IWebQueryService>();
            _scrappedSettingRepository = new ScrappedSettingRepository();

            _settingRepository.Setup(x => x.GetSettingValue<string>("SmartScrapper.Chromium.DownloadPath")).Returns(@"C:\Cawi\DEV\Lychee\Lychee.Scrapper\CustomChromium");

            var loggingPath = ConfigurationManager.AppSettings["LoggingPath"];
            _logger = new LoggerConfiguration().WriteTo.File(loggingPath).CreateLogger();
            _scrapper = new SmartScrapper(_settingRepository.Object, _loggingService.Object, _webQueryService.Object);

            _loggingService.Setup(x => x.Logger).Returns(_logger);
        }

        [Test]
        public void RecreateTables()
        {
            var resultCollectionService = new ResultCollectionService(new ColumnDefinitionRepository(), new ScrappedDataRepository(), new SettingRepository());
            resultCollectionService.SaveScrappedData(null);
        }

        [Test]
        //[TestCase("https://www.pesobility.com/stock")]
        //[TestCase("https://www.pesobility.com/stock/class-a")]
        [TestCase("https://www.pesobility.com/stock/blue-chips")]
        public async Task ScrapeStockList(string url)
        {
            //Arrange
            _scrapper.IsHeadless = true;
            _scrapper.Url = url;
            _scrapper.CustomScrappingInstructions = new List<SmartScrapper.CustomScrapping>
            {
                ScrapeStockTable
            };

            //Act
            var result = await _scrapper.Scrape();
            var resultCollectionService = new ResultCollectionService(new ColumnDefinitionRepository(), new ScrappedDataRepository(), new SettingRepository());
            resultCollectionService.SaveScrappedData(result);
        }

        private async Task ScrapeStockTable(Page page, ResultCollection<ResultItemCollection> resultCollection)
        {
            var resultSelector = "#MAIN_BODY";
            await page.WaitForSelectorAsync(resultSelector);

            //try to get the table headers
            var mappings = _scrappedSettingRepository.GetItemSettings("TableDataSetting");

            var dataSelector = "#MAIN_BODY table > tbody > tr";
            var tData = await PuppeteerHelper.GetTableData(page, dataSelector, mappings);

            var identifier = mappings.FirstOrDefault(x => x.IsIdentifier)?.Key;
            foreach (JToken link in tData)
            {
                resultCollection.Add(new ResultItemCollection
                {
                    Key = identifier,
                    Items = mappings.Select(x => new ResultItem
                    {
                        Name = x.Key,
                        Value = link[x.Key].GetValue()
                    }).ToList()
                });
            }
        }
    }
}
