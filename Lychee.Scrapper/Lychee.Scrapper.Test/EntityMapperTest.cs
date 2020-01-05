using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lychee.Scrapper.Domain.Helpers;
using Lychee.Scrapper.Entities.Entities;
using NUnit.Framework;

namespace Lychee.Scrapper.Test
{
    [TestFixture]
    public class EntityMapperTest
    {
        private List<ScrappedData> _list;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _list = new List<ScrappedData>
            {
                new ScrappedData
                {
                    String1 = "JFC",
                    String2 = "Jollibee Corporation",
                    String3 = "Sample Description",
                    Identifier = "JFC",
                    RelatedData = new List<RelatedData>
                    {
                        new RelatedData
                        {
                            String1 = "Jan 03, 2020",
                            String2 = "table success",
                            String3 = "214.80"
                        },
                        new RelatedData
                        {
                            String1 = "Jan 02, 2020",
                            String2 = "table-success",
                            String3 = "214.80"
                        }
                    }
                },
                new ScrappedData
                {
                    String1 = "MWC",
                    String2 = "Manila Water",
                    String3 = "MWC Desc",
                    Identifier = "MWC",
                    RelatedData = new List<RelatedData>
                    {
                        new RelatedData
                        {
                            String1 = "Jan 03, 2020",
                            String2 = "table-success",
                            String3 = "9.00"
                        },
                        new RelatedData
                        {
                            String1 = "Jan 02, 2020",
                            String2 = "table-success",
                            String3 = "8.15"
                        }
                    }
                }
            };
        }

        [Test]
        public void MapScrappedDataTest()
        {
            //Arrange
            var columnDefinitions = new Dictionary<(string, string), string>
            {
                {(nameof(ScrappedData), "StockCode"), "String1"},
                {(nameof(ScrappedData), "Name"), "String2"},
                {(nameof(ScrappedData), "Description"), "String3"}
            };

            //Act
            var result = ScrappedDataHelper.MapToObject<Stock>(_list, columnDefinitions);

            //Asserts
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.First().Name, Is.EqualTo("Jollibee Corporation"));
            Assert.That(result.Last().StockCode, Is.EqualTo("MWC"));
        }

        [Test]
        public void MapScrappedDataToDictionaryTest()
        {
            //Arrange
            var columnDefinitions = new Dictionary<(string, string), string>
            {
                {(nameof(ScrappedData), "StockCode"), "String1"},
                {(nameof(ScrappedData), "Name"), "String2"},
                {(nameof(ScrappedData), "Description"), "String3"}
            };

            //Act
            var result = ScrappedDataHelper.MapToDictionary<Stock>(_list, columnDefinitions);

            //Asserts
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.First().Key, Is.EqualTo("JFC"));
            Assert.That(result.Last().Key, Is.EqualTo("MWC"));
            Assert.That(result.First().Value.Count, Is.EqualTo(3)); //StockCode, Name, Description
            Assert.That(result.Last().Value.Count, Is.EqualTo(3)); //StockCode, Name, Description
            Assert.That(result.First().Value.ContainsKey("StockCode"), Is.True);
            Assert.That(result.First().Value["Name"], Is.EqualTo("Jollibee Corporation"));
        }

        [Test]
        public void MapRelatedDataTest()
        {
            //Arrange
            var columnDefinitions = new Dictionary<(string, string), string>
            {
                {(nameof(RelatedData), "String1"), "Date"},
                {(nameof(RelatedData), "String2"), "Trend"},
                {(nameof(RelatedData), "String3"), "Last"}
            };

            //Act
            var result = RelatedDataHelper.MapToObject<StockHistory>(_list, columnDefinitions, nameof(StockHistory.StockCode));

            //Asserts
            Assert.That(result.Count, Is.EqualTo(4));
            Assert.That(result.First().Date, Is.EqualTo("Jan 03, 2020"));
            Assert.That(result.Last().Last, Is.EqualTo("8.15"));
        }

        [Test]
        public void MapRelatedDataToDictionaryTest()
        {
            //Arrange
            var columnDefinitions = new Dictionary<(string, string), string>
            {
                {(nameof(RelatedData), "String1"), "Date"},
                {(nameof(RelatedData), "String2"), "Trend"},
                {(nameof(RelatedData), "String3"), "Last"}
            };

            //Act
            var result = RelatedDataHelper.MapToDictionary<Stock>(_list, columnDefinitions, "Date");

            //Asserts
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.First().Key, Is.EqualTo("JFC"));
            Assert.That(result.First().Value.Count, Is.EqualTo(6));
        }
    }

    public class StockHistory
    {
        public string StockCode { get; set; }
        public string Date { get; set; }
        public string Trend { get; set; }
        public string Last { get; set; }
    }

    public class Stock
    {
        public string StockCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
