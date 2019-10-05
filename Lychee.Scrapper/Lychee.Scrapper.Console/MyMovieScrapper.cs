using Lychee.Scrapper.Domain;
using Lychee.Scrapper.Domain.Models.Scrappers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lychee.Scrapper.Domain.Services;
using Lychee.Scrapper.Repository.Repositories;

namespace Lychee.Scrapper.Console
{
    class MyMovieScrapper
    {
        static void Main(string[] args)
        {
            //var loggingPath = ConfigurationManager.AppSettings["LoggingPath"];
            //var logger = new LoggerConfiguration().WriteTo.File(loggingPath).CreateLogger();

            //var scrapperType = ConfigurationManager.AppSettings["ScrapperType"];
            //var settingRepository = new SettingRepository();
            //var loggingService = new LoggingService(null);

            

            //var scrapper = scrapperFactory.GetScrapper();
            

            //System.Console.ReadLine();
        }

        static void LoadPage()
        {
            //var scrapper = new Domain.Services.BaseScrapperService();
            //scrapper.LoadPage("https://www.oneteaspoon.com.au/shop/woman/denim/shorts");
        }

        static async void SmartLoad()
        {
            //var smartSetting = new SmartScrapper
            //{
            //    Url = "http://www.moneycontrol.com/india/fnoquote/acc/ACC"
            //};

            //var scrapper = new SmartScrapper();
            //scrapper.Scrape();
        }
    }
}
