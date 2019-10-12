using System.Linq;
using System.Threading.Tasks;
using Lychee.Scrapper.Domain.Interfaces;
using Lychee.Scrapper.Domain.Models.Scrappers;
using Lychee.Scrapper.Repository.Interfaces;

namespace Lychee.Scrapper.Domain.Services
{
    public class PageDetailScrapperService : IScrapperService
    {
        private readonly ISettingRepository _settingRepository;
        private readonly PageDetailScrapper _scrapper;
        private readonly ILoggingService _loggingService;
        private readonly IResultCollectionService _resultCollectionService;

        public PageDetailScrapperService(ISettingRepository settingRepository, PageDetailScrapper scrapper,
            ILoggingService loggingService,
            IResultCollectionService resultCollectionService)
        {
            _settingRepository = settingRepository;
            _scrapper = scrapper;
            _loggingService = loggingService;
            _resultCollectionService = resultCollectionService;
        }

        public async Task Scrape()
        {
            //Loads the initial page
            var scrappedData = await _scrapper.Scrape();

            //Save to db
            _resultCollectionService.SaveScrappedData(scrappedData);
        }
    }
}
