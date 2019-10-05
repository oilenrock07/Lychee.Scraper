using Lychee.Scrapper.Domain.Models.Scrappers;

namespace Lychee.Scrapper.Domain.Interfaces
{
    public interface IScrapperService
    {
        void Scrape();
        void ScrapeOtherPages(int lastPage, PageListScrapper firstPageScrapper);
    }
}
