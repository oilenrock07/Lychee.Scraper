using System.Collections.Generic;
using Lychee.Scrapper.Repository.Entities;

namespace Lychee.Scrapper.Repository.Interfaces
{
    public interface IScrappedDataRepository
    {
        void SaveScrappedData(List<ScrappedData> data);
    }
}
