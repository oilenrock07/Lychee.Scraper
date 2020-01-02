using System.Collections.Generic;
using Lychee.Scrapper.Entities.Entities;

namespace Lychee.Scrapper.Repository.Interfaces
{
    public interface IScrappedDataRepository
    {
        void SaveScrappedData(List<ScrappedData> data);
    }
}
