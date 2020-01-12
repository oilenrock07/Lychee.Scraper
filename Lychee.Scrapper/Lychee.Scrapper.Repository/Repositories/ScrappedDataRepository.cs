using System.Collections.Generic;
using Lychee.Infrastructure;
using Lychee.Scrapper.Entities.Entities;
using Lychee.Scrapper.Repository.Interfaces;

namespace Lychee.Scrapper.Repository.Repositories
{
    public class ScrappedDataRepository : IScrappedDataRepository
    {
        public virtual void SaveScrappedData(List<ScrappedData> data)
        {
            using (var context = new ScrapperContext())
            {
                var repository = new Repository<ScrappedData>(context);
                foreach (var scrappedData in data)
                    repository.Add(scrappedData);

                repository.SaveChanges();
            }
        }
    }
}
