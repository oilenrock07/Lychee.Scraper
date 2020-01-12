using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Lychee.Scrapper.Domain.Extensions;
using Lychee.Scrapper.Domain.Helpers;
using Lychee.Scrapper.Domain.Interfaces;
using Lychee.Scrapper.Domain.Models.Scrappers;
using Lychee.Scrapper.Domain.Services;
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
        private IResultCollectionService _resultCollectionService;
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

            _resultCollectionService = new ResultCollectionService(new ColumnDefinitionRepository(), new ScrappedDataRepository(), new SettingRepository());
        }

        [Test]
        public void ConvertInvestagramDateToDateTime()
        {
            var strDate = "Jan 03, 2020";
            var date = Convert.ToDateTime(strDate);
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
            _resultCollectionService.SaveScrappedData(result);
        }

        [Test]
        public async Task ScrapeHistoricalDataAndTechnicalAnalysis()
        {
            //Arrange
            _scrapper.IsHeadless = true;
            _scrapper.Url = "https://www.investagrams.com/Stock/PSE:JFC";
            _scrapper.Parameters = new Dictionary<string, object>
            {
                {"StockCode", "JFC"}
            };

            _scrapper.CustomScrappingInstructions = new List<SmartScrapper.CustomScrapping>
            {
                ScrapeTechnicalAnalysisData,
                ScrapeHistoricalData
            };

            //Act
            var result = await _scrapper.Scrape();
            _resultCollectionService.SaveScrappedData(result);
        }

        [Test]
        public async Task ScrapeRealTimeStocks()
        {
            //Arrange
            _scrapper.IsHeadless = false;
            _scrapper.Url = "https://www.investagrams.com/Stock/RealTimeMonitoring";
            _scrapper.CustomScrappingInstructions = new List<SmartScrapper.CustomScrapping>
            {
                LogIn,
                ScrapeRealTimeMonitoringData
            };

            //Act
            var result = await _scrapper.Scrape();
        }

        private async Task LogIn(Page page, ResultCollection<ResultItemCollection> resultCollection, Dictionary<string, object> args)
        {
            var resultSelector = ".invest-login__input-holder";
            await page.WaitForSelectorAsync(resultSelector);

            await page.TypeAsync(".invest-login__input-holder form input[data-ng-model='LoginRequest.Username']", "cawicaancornelio@gmail.com");
            await page.TypeAsync("input[type='password']", "1234567890a!");
            await page.ClickAsync("button[data-ng-click='authenticateUser()']");
            await page.WaitForNavigationAsync();
        }

        private async Task ScrapeRealTimeMonitoringData(Page page, ResultCollection<ResultItemCollection> resultCollection, Dictionary<string, object> args)
        {
            var resultSelector = "#StockQuoteTable";
            await page.WaitForSelectorAsync(resultSelector);

            //try to get the table headers
            var mappings = _scrappedSettingRepository.GetItemSettings("RealTimeMonitoring");

            var dataSelector = "#StockQuoteTable > tbody > tr";
            var tData = await PuppeteerHelper.GetTableData(page, dataSelector, mappings);

            foreach (JToken link in tData)
            {
                resultCollection.Add(new ResultItemCollection
                {
                    Key = link["StockCode"].GetValue(),
                    Items = mappings.Select(x => new ResultItem
                    {
                        Name = x.Key,
                        Value = link[x.Key].GetValue()
                    }).ToList()
                });
            }
        }

        private async Task ScrapeStockTable(Page page, ResultCollection<ResultItemCollection> resultCollection, Dictionary<string, object> args = null)
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

        private async Task ScrapeHistoricalData(Page page, ResultCollection<ResultItemCollection> resultCollection, Dictionary<string, object> args = null)
        {
            var resultSelector = "#HistoricalDataTable";
            await page.WaitForSelectorAsync(resultSelector);

            //try to get the table headers
            var mappings = _scrappedSettingRepository.GetItemSettings("HistoricalData");

            var dataSelector = "#HistoricalDataTable > tbody > tr";
            var tData = await PuppeteerHelper.GetTableData(page, dataSelector, mappings);

            var identifier = args["StockCode"].ToString();
            var collection = resultCollection.GetItem(identifier);
            
            var date = "";
            foreach (var item in tData)
            {
                if (date != item["Date"]?.GetValue())
                    date = item["Date"].GetValue();
                
                collection.Items.AddRange(mappings.Select(x => new ResultItem
                {
                    Name = x.Key,
                    Value = item[x.Key].GetValue(),
                    Group = date,
                    IsMultiple = true
                }).ToList());
            }
        }

        private async Task ScrapeTechnicalAnalysisData(Page page, ResultCollection<ResultItemCollection> resultCollection, Dictionary<string, object> args = null)
        {
            var resultSelector = "#TechnicalAnalysisContent";
            await page.WaitForSelectorAsync(resultSelector);

            //try to get the table headers
            var mappings = _scrappedSettingRepository.GetItemSettings("TechnicalAnalysisData");
            var tData = await PuppeteerHelper.GetElements(page, mappings);
            var identifier = args["StockCode"].ToString();

            resultCollection.Add(new ResultItemCollection
            {
                Key = identifier,
                Items = mappings.Select(x => new ResultItem
                {
                    Name = x.Key,
                    Value = tData.FirstOrDefault(j => j[x.Key] != null)[x.Key].GetValue()
                }).ToList()
            });
        }
    }
}
