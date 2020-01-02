using System.Collections.Generic;
using System.Linq;
using Lychee.Scrapper.Domain.Models.Scrappers;
using Lychee.Scrapper.Domain.Services;
using Lychee.Scrapper.Entities.Entities;
using Lychee.Scrapper.Repository.Interfaces;
using Lychee.Scrapper.Test.RepositoryTest;
using Moq;
using NUnit.Framework;

namespace Lychee.Scrapper.Test.ServiceTest
{
    [TestFixture]
    public class ResultCollectionServiceTest
    {
        [Test]
        public void CanPopulateRelatedDataSingleRow()
        {
            //Arrange
            var columnDefinitionRepository = new Mock<IColumnDefinitionRepository>();
            var scrappedDataRepository = new ScrappedDataRepositoryMock();
            var settingDataRepository = new Mock<ISettingRepository>();

            columnDefinitionRepository
                .Setup(x => x.GetAllScrappedDataColumnDefinitions())
                .Returns(new Dictionary<(string, string), string>
                {
                    {(nameof(ScrappedData), "Name"), "String1"}
                });

            columnDefinitionRepository
                .Setup(x => x.GetAllRelatedDataColumnDefinitions())
                .Returns(new Dictionary<(string, string), string>
                {
                    {(nameof(RelatedData), "String1"), "Image"},
                    {(nameof(RelatedData), "String2"), "Image"},
                    {(nameof(RelatedData), "String3"), "Image"},
                    {(nameof(RelatedData), "String4"), "Image"},
                    {(nameof(RelatedData), "String5"), "Image"},
                    {(nameof(RelatedData), "String6"), "Image"},
                    {(nameof(RelatedData), "String7"), "Image"},
                    {(nameof(RelatedData), "String8"), "Image"},
                    {(nameof(RelatedData), "String9"), "Image"},
                    {(nameof(RelatedData), "String10"), "Image"},
                    {(nameof(RelatedData), "String11"), "Platform"},
                    {(nameof(RelatedData), "String12"), "Platform"},
                    {(nameof(RelatedData), "String13"), "Platform"},
                    {(nameof(RelatedData), "String14"), "Platform"},
                    {(nameof(RelatedData), "String15"), "Platform"}
                });

            settingDataRepository.Setup(x => x.GetSettingValue<bool>("Core.Schema.IsMultipleRow")).Returns(false);

            var service = new ResultCollectionService(columnDefinitionRepository.Object, scrappedDataRepository, settingDataRepository.Object);
            var data = new ResultCollection<ResultItemCollection>
            {
                new ResultItemCollection
                {
                    Key = "Witcher 3",
                    Items = new List<ResultItem>
                    {
                        new ResultItem {Name = "Name", Value = "Witcher 3"},
                        new ResultItem {Name = "Image", Value = "Witcher1.jpg", IsMultiple = true},
                        new ResultItem {Name = "Image", Value = "Witcher2.jpg", IsMultiple = true},
                        new ResultItem {Name = "Image", Value = "Witcher3.jpg", IsMultiple = true},
                        new ResultItem {Name = "Image", Value = "Witcher4.jpg", IsMultiple = true},
                        new ResultItem {Name = "Image", Value = "Witcher5.jpg", IsMultiple = true},
                        new ResultItem {Name = "Platform", Value = "PS4", IsMultiple = true},
                        new ResultItem {Name = "Platform", Value = "XBox One", IsMultiple = true},
                        new ResultItem {Name = "Platform", Value = "PC", IsMultiple = true},
                        new ResultItem {Name = "Platform", Value = "Nintendo Switch", IsMultiple = true}
                    }
                },
                new ResultItemCollection
                {
                    Key = "Mass Effect 2",
                    Items = new List<ResultItem>
                    {
                        new ResultItem {Name = "Name", Value = "Mass Effect 2"},
                        new ResultItem {Name = "Image", Value = "MassEffect2.jpg", IsMultiple = true},
                        new ResultItem {Name = "Platform", Value = "XBox", IsMultiple = true},
                        new ResultItem {Name = "Platform", Value = "PS3", IsMultiple = true},
                        new ResultItem {Name = "Platform", Value = "PC", IsMultiple = true},
                    }
                }
            };

            //Act
            service.SaveScrappedData(data);

            //Assert
            Assert.That(scrappedDataRepository.Data, Is.Not.Null);
            Assert.That(scrappedDataRepository.Data.Count, Is.EqualTo(2));
            Assert.That(scrappedDataRepository.Data.First().String1, Is.EqualTo("Witcher 3"));
            Assert.That(scrappedDataRepository.Data.Last().String1, Is.EqualTo("Mass Effect 2"));
            Assert.That(scrappedDataRepository.Data.First().RelatedData.Count, Is.EqualTo(1));
            Assert.That(scrappedDataRepository.Data.Last().RelatedData.Count, Is.EqualTo(1));
            Assert.That(scrappedDataRepository.Data.First().RelatedData.ElementAt(0).String1, Is.EqualTo("Witcher1.jpg"));
            Assert.That(scrappedDataRepository.Data.First().RelatedData.ElementAt(0).String2, Is.EqualTo("Witcher2.jpg"));
            Assert.That(scrappedDataRepository.Data.First().RelatedData.ElementAt(0).String3, Is.EqualTo("Witcher3.jpg"));
            Assert.That(scrappedDataRepository.Data.First().RelatedData.ElementAt(0).String4, Is.EqualTo("Witcher4.jpg"));
            Assert.That(scrappedDataRepository.Data.First().RelatedData.ElementAt(0).String5, Is.EqualTo("Witcher5.jpg"));
            Assert.That(scrappedDataRepository.Data.First().RelatedData.ElementAt(0).String6, Is.Empty.Or.Null);
            Assert.That(scrappedDataRepository.Data.First().RelatedData.ElementAt(0).String11, Is.EqualTo("PS4"));
            Assert.That(scrappedDataRepository.Data.First().RelatedData.ElementAt(0).String14, Is.EqualTo("Nintendo Switch"));

            Assert.That(scrappedDataRepository.Data.Last().RelatedData.ElementAt(0).String1, Is.EqualTo("MassEffect2.jpg"));
            Assert.That(scrappedDataRepository.Data.Last().RelatedData.ElementAt(0).String12, Is.EqualTo("PS3"));

        }
    }
}
