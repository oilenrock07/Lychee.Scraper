using System.Collections.Generic;
using Lychee.Scrapper.Domain.Models.Scrappers;
using Lychee.Scrapper.Entities.Entities;

namespace Lychee.Scrapper.Domain.Interfaces
{
    public interface IResultCollectionService
    {
        void SaveScrappedData(ResultCollection<ResultItemCollection> data);
        List<ScrappedData> MapToScrappedData(ResultCollection<ResultItemCollection> data);
    }
}
