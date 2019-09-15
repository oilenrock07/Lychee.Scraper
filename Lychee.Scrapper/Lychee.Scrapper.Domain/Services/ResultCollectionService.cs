using System.Collections.Generic;
using Lychee.Scrapper.Domain.Helpers;
using Lychee.Scrapper.Domain.Interfaces;
using Lychee.Scrapper.Domain.Models.Scrappers;
using Lychee.Scrapper.Repository.Entities;
using Lychee.Scrapper.Repository.Interfaces;

namespace Lychee.Scrapper.Domain.Services
{
    public class ResultCollectionService : IResultCollectionService
    {
        private readonly IColumnDefinitionRepository _columnDefinitionRepository;
        private readonly IScrappedDataRepository _scrappedDataRepository;

        public ResultCollectionService(IColumnDefinitionRepository columnDefinitionRepository, IScrappedDataRepository scrappedDataRepository)
        {
            _columnDefinitionRepository = columnDefinitionRepository;
            _scrappedDataRepository = scrappedDataRepository;
        }

        public void SaveScrappedData(ResultCollection<ResultItemCollection> data)
        {
            var columnDefinitions = _columnDefinitionRepository.GetAllColumnDefinitions();
            var list = new List<ScrappedData>();
            foreach (var product in data)
            {
                var scrappedData = new ScrappedData();
                foreach (var item in product.Items)
                {
                    if (item.IsMultiple)
                    {
                        if (scrappedData.RelatedData == null)
                            scrappedData.RelatedData = new List<RelatedData>();

                        var relatedData = new RelatedData
                        {
                            Description = item.Name,
                            String1 = item.Value.ToString()
                        };

                        //if (columnDefinitions.TryGetValue((nameof(RelatedData), item.Name), out var value))
                        //    RelatedDataHelper.SetValue(relatedData, value, item.Value);

                        scrappedData.RelatedData.Add(relatedData);
                    }
                    else
                    {
                        if (columnDefinitions.TryGetValue((nameof(ScrappedData), item.Name), out var value))
                            ScrappedDataHelper.SetValue(scrappedData, value, item.Value);
                    }
                }

                list.Add(scrappedData);
            }

            _scrappedDataRepository.SaveScrappedData(list);
        }
    }
}
