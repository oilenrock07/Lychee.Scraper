using System.Linq;
using Lychee.Scrapper.Repository;
using Lychee.Scrapper.Repository.Entities;
using Lychee.Scrapper.Repository.Repositories;
using NUnit.Framework;

namespace Lychee.Scrapper.Test.RepositoryTest
{
    public class AccessContextTest
    {
        [Test]
        public void CanConnectToAccess()
        {
            var count = 0;
            using (var context = new ScrapperContext())
            {
                count = context.ScrappedData.Count();
            }
        }

        [Test]
        public void CanAddNewScrapperData()
        {
            using (var context = new ScrapperContext())
            {
                var repository = new Repository<ScrappedData>(context);
                
                repository.Add(new ScrappedData
                {
                    Identifier = "12345",
                    String1 = "Witcher 3"
                });
            }
        }

        [Test]
        public void CanUpdateScrapperData()
        {
            ScrappedData data;
            using (var context = new ScrapperContext())
            {
                var repository = new Repository<ScrappedData>(context);
                data = repository.FirstOrDefault(x => x.Identifier == "12345");
            }

            data.String1 = "Witcher 4";
            using (var context = new ScrapperContext())
            {
                var repository = new Repository<ScrappedData>(context);

                repository.Update(data);
            }
        }

        [Test]
        public void CanSearchScrapperData()
        {
            ScrappedData data;
            using (var context = new ScrapperContext())
            {
                var repository = new Repository<ScrappedData>(context);
                data = repository.FirstOrDefault(x => x.Identifier == "12345");
            }

            Assert.That(data, Is.Not.Null);
            Assert.That(data.Identifier, Is.Not.Empty);
        }

        [Test]
        public void CanRemoveScrapperData()
        {
            ScrappedData data;
            using (var context = new ScrapperContext())
            {
                var repository = new Repository<ScrappedData>(context);
                data = repository.FirstOrDefault(x => x.Identifier == "12345");
                repository.Delete(data);
            }

            data = null;
            using (var context = new ScrapperContext())
            {
                var repository = new Repository<ScrappedData>(context);
                data = repository.FirstOrDefault(x => x.Identifier == "12345");
            }

            Assert.That(data, Is.Null);
        }
    }
}
