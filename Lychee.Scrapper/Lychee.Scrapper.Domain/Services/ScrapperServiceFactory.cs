using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lychee.Scrapper.Domain.Enums;
using Lychee.Scrapper.Domain.Interfaces;
using Lychee.Scrapper.Domain.Models.Scrappers;
using Lychee.Scrapper.Repository.Interfaces;
using Serilog;

namespace Lychee.Scrapper.Domain.Services
{
    public class ScrapperServiceFactory : IScrapperServiceFactory
    {

        private string _scrapper;
        private readonly ISettingRepository _settingRepository;
        private readonly ILoggingService _loggingService;
        private readonly IResultCollectionService _resultCollectionService;

        public ScrapperServiceFactory(string scrapper, ISettingRepository settingRepository, ILoggingService loggingService, IResultCollectionService resultCollectionService)
        {
            _scrapper = scrapper;
            _settingRepository = settingRepository;
            _loggingService = loggingService;
            _resultCollectionService = resultCollectionService;
        }

        public IScrapperService GetScrapper()
        {
            var loggingPath = ConfigurationManager.AppSettings["LoggingPath"];
            var logger = new LoggerConfiguration().WriteTo.File(loggingPath).CreateLogger();

            if (string.Equals(_scrapper, ScrapperType.PageList, StringComparison.InvariantCultureIgnoreCase))
            {
                var scrapper = new PageListScrapper(_settingRepository, _loggingService);
                return new PageListScrapperService(_settingRepository, scrapper, _loggingService, _resultCollectionService);
            }
                
            throw new NotImplementedException();

            //if (String.Equals(_scrapper, "PAGE_LIST", StringComparison.InvariantCultureIgnoreCase))
            //    return new PageDetailScrapper(logger);

            //return new SmartScrapper(logger);
        }
    }
}
