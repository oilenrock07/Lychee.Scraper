using System.Collections.Generic;
using Lychee.Scrapper.Domain.Interfaces;
using Lychee.Scrapper.Entities.Entities;
using Lychee.Scrapper.Repository.Interfaces;

namespace Lychee.Scrapper.Domain.Models.Scrappers
{
    public class PageListSmartScrapper : SmartScrapper, IPageListScrapper
    {
        public string ItemXPath { get; set; }
        public bool LoadMoreOnSamePage { get; set; }
        public bool WaitForJavascriptToLoad { get; set; }
        public PageListPagination PaginationSettings { get; set; }

        public PageListSmartScrapper(ISettingRepository settingRepository, ILoggingService loggingService,
            IWebQueryService webQueryService) : base(settingRepository, loggingService, webQueryService)
        {
        }

        public void Clone(IPageListScrapper scrapper)
        {
            scrapper.ItemXPath = ItemXPath;
            scrapper.LoadMoreOnSamePage = LoadMoreOnSamePage;
            scrapper.WaitForJavascriptToLoad = WaitForJavascriptToLoad;
            scrapper.Url = Url;

            //Since this is not really that important to cloning. We could just reference to the existing object from the memory
            scrapper.Items = Items;
            scrapper.PaginationSettings = PaginationSettings;
        }

        public void DeepClone(PageListScrapper scrapper)
        {
            Clone(scrapper);
            scrapper.Items = new List<ScrapeItemSetting>();

            if (Items != null)
            {
                foreach (var itemSetting in Items)
                {
                    scrapper.Items.Add(new ScrapeItemSetting
                    {
                        Key = itemSetting.Key,
                        AttributeName = itemSetting.AttributeName,
                        DefaultValue = itemSetting.DefaultValue,
                        IsIdentifier = itemSetting.IsIdentifier,
                        MultipleValue = itemSetting.MultipleValue,
                        Selector = itemSetting.Selector,
                        ValueRequired = itemSetting.ValueRequired
                    });
                }
            }

            if (PaginationSettings != null)
            {
                scrapper.PaginationSettings = new PageListPagination
                {
                    CanHaveIndefinitePagination = PaginationSettings.CanHaveIndefinitePagination,
                    NextPageXPath = PaginationSettings.NextPageXPath,
                    PaginationSelector = PaginationSettings.PaginationSelector,
                    ShowLastPagination = PaginationSettings.ShowLastPagination
                };
            }
        }
    }
}
