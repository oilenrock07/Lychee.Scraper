using System.Collections.Generic;
using Lychee.Scrapper.Repository.Entities;
using Lychee.Scrapper.Repository.Interfaces;

namespace Lychee.Scrapper.Test
{

    public class ScrappedDataRepositoryMock : IScrappedDataRepository
    {
        public List<ScrappedData> Data { get; set; }

        public ScrappedDataRepositoryMock()
        {
            Data = new List<ScrappedData>(10000);
        }

        public void SaveScrappedData(List<ScrappedData> data)
        {
            Data.AddRange(data);
        }
    }
}
