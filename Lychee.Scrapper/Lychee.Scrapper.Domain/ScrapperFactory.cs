using Lychee.Scrapper.Domain.Interfaces;
using Lychee.Scrapper.Domain.Models.Scrappers;
using System;
using System.Configuration;
using Lychee.Scrapper.Repository.Interfaces;
using Serilog;

namespace Lychee.Scrapper.Domain
{
    public class ScrapperFactory
    {
        private string _scrapper;
        private readonly ISettingRepository _settingRepository;
        private readonly ILoggingService _loggingService;

        public ScrapperFactory(string scrapper, ISettingRepository settingRepository, ILoggingService loggingService)
        {
            _scrapper = scrapper;
            _settingRepository = settingRepository;
            _loggingService = loggingService;
        }

        public IScrapper GetScrapper()
        {
            var loggingPath = ConfigurationManager.AppSettings["LoggingPath"];
            var logger = new LoggerConfiguration().WriteTo.File(loggingPath).CreateLogger();

            if (String.Equals(_scrapper, "PAGE_LIST", StringComparison.InvariantCultureIgnoreCase))
                return new PageListScrapper(logger, _settingRepository, _loggingService);
            if (String.Equals(_scrapper, "PAGE_LIST", StringComparison.InvariantCultureIgnoreCase))
                return new PageDetailScrapper(logger);

            return new SmartScrapper(logger);
        }
    }
}
