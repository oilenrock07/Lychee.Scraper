using System.Collections.Generic;
using System.Linq;
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
        private readonly ISettingRepository _settingRepository;
        public ResultCollectionService(IColumnDefinitionRepository columnDefinitionRepository, 
            IScrappedDataRepository scrappedDataRepository, ISettingRepository settingRepository)
        {
            _columnDefinitionRepository = columnDefinitionRepository;
            _scrappedDataRepository = scrappedDataRepository;
            _settingRepository = settingRepository;
        }

        public void SaveScrappedData(ResultCollection<ResultItemCollection> data)
        {
            var scrappedDataColumnDefinitions = _columnDefinitionRepository.GetAllScrappedDataColumnDefinitions();
            var relatedDataColumnDefinitions = _columnDefinitionRepository.GetAllRelatedDataColumnDefinitions();

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

                        var insertMultipleRow = _settingRepository.GetSettingValue<bool>("Core.Schema.IsMultipleRow");
                        if (insertMultipleRow)
                        {
                            var relatedData = new RelatedData
                            {
                                Description = item.Name,
                                String1 = item.Value.ToString()
                            };

                            scrappedData.RelatedData.Add(relatedData);
                        }
                        else
                        {
                            if (!scrappedData.RelatedData.Any())
                                scrappedData.RelatedData.Add(new RelatedData());

                            var relatedData = scrappedData.RelatedData.First();
                            RelatedDataHelper.SetValue(relatedData, item.Name, relatedDataColumnDefinitions, item.Value);
                        }
                    }
                    else
                    {
                        if (scrappedDataColumnDefinitions.TryGetValue((nameof(ScrappedData), item.Name), out var value))
                            ScrappedDataHelper.SetValue(scrappedData, value, item.Value);
                    }
                }

                list.Add(scrappedData);
            }

            _scrappedDataRepository.SaveScrappedData(list);
        }
    }
}
