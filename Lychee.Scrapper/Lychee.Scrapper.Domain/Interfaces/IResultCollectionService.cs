using Lychee.Scrapper.Domain.Models.Scrappers;

namespace Lychee.Scrapper.Domain.Interfaces
{
    public interface IResultCollectionService
    {
        void SaveScrappedData(ResultCollection<ResultItemCollection> data);
    }
}
