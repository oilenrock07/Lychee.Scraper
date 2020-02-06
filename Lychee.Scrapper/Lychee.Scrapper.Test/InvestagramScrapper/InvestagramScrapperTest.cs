using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lychee.Infrastructure.Interfaces;
using Lychee.Infrastructure.Repositories;
using Lychee.Scrapper.Domain.Interfaces;
using Lychee.Scrapper.Domain.Models.Scrappers;
using Lychee.Scrapper.Domain.Services;
using Lychee.Scrapper.Repository.Interfaces;
using Lychee.Scrapper.Repository.Repositories;
using Moq;
using NUnit.Framework;
using PuppeteerSharp;
using Serilog;
using Serilog.Core;

namespace Lychee.Scrapper.Test.InvestagramScrapper
{
    [TestFixture]
    public class InvestagramScrapperTest
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
        public async Task GetStockData()
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
                InvokeScriptBackdoorScript
            };

            //Act
            var result = await _scrapper.Scrape();
        }

        private async Task InvokeScriptBackdoorScript(Page page, ResultCollection<ResultItemCollection> resultCollection, Dictionary<string, object> args)
        {
            var resultSelector = "#MainContent_HistoricalDataTableUpdatePanel";
            var test = "";
            await page.WaitForSelectorAsync(resultSelector);

            var tData = await page.EvaluateFunctionAsync(@"(async (test) => {
            test = await angular.element(document.body).injector().get('stockApiService').viewStock('PSE:MAC');
return myData;
})()", test);


        }
    }
}
