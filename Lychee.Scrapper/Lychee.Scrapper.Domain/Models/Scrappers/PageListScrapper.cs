using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using Lychee.Scrapper.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lychee.Scrapper.Repository.Interfaces;
using Serilog;
using Serilog.Core;

namespace Lychee.Scrapper.Domain.Models.Scrappers
{
    public class PageListScrapper : BaseScrapper, IScrapper
    {
        private readonly ISettingRepository _settingRepository;
        private readonly ILoggingService _logService;

        private HtmlNode _htmlNode;

        public PageListScrapper(ISettingRepository settingRepository,
            ILoggingService logService,
            HtmlNode node = null)
        {
            _settingRepository = settingRepository;
            _logService = logService;
            _htmlNode = node;
        }

        public virtual PageListPagination PaginationSettings { get; set; }

        /// <summary>
        /// Selector of the item we want to scrape.
        /// The repeating DIVs that contain the products
        /// Example from Solutionists page list, this is the article
        /// </summary>
        public virtual string ItemXPath { get; set; }

        /// <summary>
        /// Some Product List page does not have pagination and only have Load More button
        /// on the same page. Example is Carousell
        /// </summary>
        public virtual bool LoadMoreOnSamePage { get; set; }

        /// <summary>
        /// Some pages are using ajax to load it's content.
        /// Set the property to wait for the content to load
        /// </summary>
        public virtual bool WaitForJavascriptToLoad { get; set; }

        public override async Task<ResultCollection<ResultItemCollection>> Scrape()
        {
            await PopulateHtmlNode();

            var nodes = _htmlNode.QuerySelectorAll(ItemXPath).ToList();
            var resultCollection = new ResultCollection<ResultItemCollection>();

            if (nodes.Any())
            {
                foreach (var node in nodes)
                {
                    var key = "";
                    var result = new List<ResultItem>();
                    foreach (var item in Items)
                    {
                        if (item.MultipleValue)
                            AddMultipleValues(node, item, result);
                        else
                        {
                            var tempKey = AddSingleValue(node, item, result);
                            if (item.IsIdentifier) key = tempKey;
                        }
                            
                    }

                    resultCollection.AddItem(key, result);
                }
            }

            return resultCollection;
        }

        public virtual HtmlNode GetLoadedHtmlNode()
        {
            return _htmlNode;
        }

        public virtual void Clone(PageListScrapper scrapper)
        {
            scrapper.ItemXPath = ItemXPath;
            scrapper.LoadMoreOnSamePage = LoadMoreOnSamePage;
            scrapper.WaitForJavascriptToLoad = WaitForJavascriptToLoad;
            scrapper.Url = Url;
            scrapper.Items = new List<ItemSetting>();

            if (Items != null)
            {
                foreach (var itemSetting in Items)
                {
                    scrapper.Items.Add(new ItemSetting
                    {
                        Key = itemSetting.Key,
                        AttributeName = itemSetting.AttributeName,
                        DefaultValue = itemSetting.DefaultValue,
                        EstimatedMultipleValueCount = itemSetting.EstimatedMultipleValueCount,
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

        protected async Task PopulateHtmlNode()
        {
            if (_htmlNode == null)
            {
                _logService.Logger.Information("Started Loading Page: {Url}", Url);
                _htmlNode = await LoadPage(Url);

                if (_htmlNode != null &&
                    _settingRepository.GetSettingValue<bool>("Scrapping.ScrapperSetting.LogDownloadedPage"))
                {
                    var path = _settingRepository.GetSettingValue<string>("Scrapping.ScrapperSetting.LogDownloadedPagePath");
                    _logService.Logger.Information("Writing downloaded document from : {Url}", Url);
                    _logService.LogHtmlDocument(_htmlNode, path, Url);
                    _logService.Logger.Information("Finished writing the document");
                }

                _logService.Logger.Information("Finished Loading Page {Url}", Url);
            }
        }

    }
}
