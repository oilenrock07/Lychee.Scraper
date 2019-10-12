using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Lychee.Scrapper.Domain.Helpers;
using Lychee.Scrapper.Domain.Interfaces;
using Lychee.Scrapper.Domain.Models.Scrappers;
using Lychee.Scrapper.Domain.Services;
using Lychee.Scrapper.Repository.Entities;
using Lychee.Scrapper.Repository.Interfaces;
using Lychee.Scrapper.Repository.Repositories;
using Moq;
using NUnit.Framework;
using Serilog;

namespace Lychee.Scrapper.Test.MyMoviesScrapper
{
    [TestFixture]
    public class MyMoviesScrapperTest
    {
        private PageListScrapper _scrapper;
        private Mock<ISettingRepository> _settingRepository;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var loggingPath = Path.Combine(ConfigurationManager.AppSettings["LoggingPath"], "MyMovies", "Log.txt");
            var logger = new LoggerConfiguration().WriteTo.File(loggingPath).CreateLogger();

            var webQueryService = new Mock<IWebQueryService>();
            _settingRepository = new Mock<ISettingRepository>();
            _scrapper = new PageListScrapper(_settingRepository.Object, new LoggingService(logger), webQueryService.Object);
        }

        [Test]
        public async Task ScrapeTest()
        {
            //Arrange
            _scrapper.Url = "http://mymovies.localhost/";
            _scrapper.ItemXPath = "#content .container .row:first-child .col-sm-6";
            _scrapper.Items = new List<ScrapeItemSetting>
            {
                new ScrapeItemSetting
                {
                    Key = "MovieName",
                    Selector = "h3"
                }
            };

            _settingRepository.Setup(x => x.GetSettingValue<bool>("Core.Logger.LogDownloadedPage")).Returns(true);

            //Act
            var products = await _scrapper.Scrape();

            //Assert
            Assert.That(products, Is.Not.Null);
            Assert.That(products, Is.All.Not.Null);
            Assert.That(products.All(x => x.Items.Exists(y => y.Name == "MovieName" && !string.IsNullOrEmpty(y.Value.ToString()))), Is.True);
        }
    }
}
