using Lychee.Scrapper.Domain.Interfaces;
using Lychee.Scrapper.Domain.Models.Scrappers;
using System;
using System.Configuration;
using Serilog;

namespace Lychee.Scrapper.Domain
{
    public class ScrapperFactory
    {
        private string _scrapper;

        public ScrapperFactory(string scrapper)
        {
            _scrapper = scrapper;
        }

        public IScrapper GetScrapper()
        {
            var loggingPath = ConfigurationManager.AppSettings["LoggingPath"];
            var logger = new LoggerConfiguration().WriteTo.File(loggingPath).CreateLogger();

            if (String.Equals(_scrapper, "PAGE_LIST", StringComparison.InvariantCultureIgnoreCase))
                return new PageListScrapper(logger);
            if (String.Equals(_scrapper, "PAGE_LIST", StringComparison.InvariantCultureIgnoreCase))
                return new PageDetailScrapper(logger);

            return new SmartScrapper(logger);
        }
    }
}
