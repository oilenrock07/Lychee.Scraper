using System.Collections.Generic;
using Lychee.Scrapper.Entities.Entities;
using Lychee.Scrapper.Repository.Interfaces;

namespace Lychee.Scrapper.Repository.Repositories
{
    public class ScrappedDataRepository : IScrappedDataRepository
    {
        public void SaveScrappedData(List<ScrappedData> data)
        {
            using (var context = new ScrapperContext())
            {
                var repository = new Repository<ScrappedData>(context, true);
                foreach (var scrappedData in data)
                    repository.Add(scrappedData);

                repository.SaveChanges();
            }
        }
    }
}
