using Lychee.Scrapper.Domain;
using Lychee.Scrapper.Domain.Models.Scrappers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lychee.Scrapper.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var scrapperType = ConfigurationManager.AppSettings["ScrapperType"];
            var scrapperFactory = new ScrapperFactory(scrapperType);

            var scrapper = scrapperFactory.GetScrapper();

            SmartLoad();

            System.Console.ReadLine();
        }

        static void LoadPage()
        {
            //var scrapper = new Domain.Services.BaseScrapperService();
            //scrapper.LoadPage("https://www.oneteaspoon.com.au/shop/woman/denim/shorts");
        }

        static async void SmartLoad()
        {
            var smartSetting = new SmartScrapper
            {
                Url = "http://www.moneycontrol.com/india/fnoquote/acc/ACC"
            };

            var scrapper = new SmartScrapper();
            scrapper.Scrape();
        }
    }
}
