using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lychee.Scrapper.Domain.Interfaces;
using Lychee.Scrapper.Domain.Models.Scrappers;
using Lychee.Scrapper.Repository.Interfaces;

namespace Lychee.Scrapper.Domain.Services
{
    public class PageListSmartScrapperService : IPageListScrapperService
    {
        private readonly ISettingRepository _settingRepository;
        private readonly PageListSmartScrapper _scrapper;
        private readonly ILoggingService _loggingService;
        private readonly IResultCollectionService _resultCollectionService;
        private readonly IWebQueryService _webQueryService;
        private readonly IPageListPaginationService _pageListPaginationService;

        public PageListSmartScrapperService(ISettingRepository settingRepository, PageListSmartScrapper scrapper,
            ILoggingService loggingService,
            IResultCollectionService resultCollectionService, IWebQueryService webQueryService, IPageListPaginationService pageListPaginationService)
        {
            _settingRepository = settingRepository;
            _scrapper = scrapper;
            _loggingService = loggingService;
            _resultCollectionService = resultCollectionService;
            _webQueryService = webQueryService;
            _pageListPaginationService = pageListPaginationService;
        }

        public async Task ScrapePageList()
        {
            _scrapper.IsFirstPage = true;
            var scrappedData = await _scrapper.Scrape();

            //Save to db
            _resultCollectionService.SaveScrappedData(scrappedData);

            var htmlNode = _scrapper.GetLoadedHtmlNode();
            var isFirstPage = _pageListPaginationService.IsFirstPage(htmlNode);

            if (isFirstPage)
            {
                var lastPage = _pageListPaginationService.GetLastPageNumber(htmlNode);
                if (lastPage > 1)
                {
                    ScrapeOtherPages(lastPage, _scrapper);
                }
            }
        }

        public void ScrapeOtherPages(int lastPage, IPageListScrapper firstPageScrapper)
        {
            throw new NotImplementedException();
        }
    }
}
