using Lychee.Scrapper.Domain.Models.Scrappers;

namespace Lychee.Scrapper.Domain.Interfaces
{
    public interface IPageListScrapperService
    {
        void ScrapeOtherPages(int lastPage, PageListScrapper firstPageScrapper);
    }
}
