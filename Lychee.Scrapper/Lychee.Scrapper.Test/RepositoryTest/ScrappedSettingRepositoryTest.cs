using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lychee.Scrapper.Repository.Repositories;
using NUnit.Framework;

namespace Lychee.Scrapper.Test.RepositoryTest
{
    [TestFixture]
    public class ScrappedSettingRepositoryTest
    {
        [Test]
        [Ignore("This is not actually a test. Just for me to verify if working")]
        public void CanLoadScrapeSettingRepository()
        {
            //Arrange
            var repository = new ScrappedSettingRepository();

            //Act
            var result = repository.GetItemSettings("Test");

            //Asserts
            Assert.That(result, Is.Not.Null);
        }
    }
}
