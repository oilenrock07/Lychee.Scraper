using System.Collections.Generic;
using Lychee.Scrapper.Domain.Interfaces;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Lychee.Scrapper.Repository.Interfaces;

namespace Lychee.Scrapper.Domain.Models.Scrappers
{
    public class PageDetailScrapper : BaseScrapper, IScrapper
    {
        private readonly ILoggingService _loggingService;
        private readonly ISettingRepository _settingRepository;

        private HtmlNode _htmlNode;

        public PageDetailScrapper(ILoggingService loggingService, ISettingRepository settingRepository,
            IWebQueryService webQueryService, HtmlNode htmlNode = null) 
            : base (webQueryService)
        {
            _loggingService = loggingService;
            _settingRepository = settingRepository;
            _htmlNode = htmlNode;
        }

        public override async Task<ResultCollection<ResultItemCollection>> Scrape()
        {
            await PopulateHtmlNode();

            var key = "";
            var result = new List<ResultItem>();
            var resultCollection = new ResultCollection<ResultItemCollection>();
            foreach (var item in Items)
            {
                if (item.MultipleValue)
                    AddMultipleValues(_htmlNode, item, result);
                else
                {
                    var tempKey = AddSingleValue(_htmlNode, item, result);
                    if (item.IsIdentifier) key = tempKey;
                }
            }

            resultCollection.AddItem(key, result);
            return resultCollection;
        }

        protected async Task PopulateHtmlNode()
        {
            if (_htmlNode == null)
            {
                _loggingService.Logger.Information("Started Loading Page: {Url}", Url);
                _htmlNode = await LoadPage(Url);

                if (_htmlNode != null &&
                    _settingRepository.GetSettingValue<bool>("Core.Logger.LogDownloadedPage"))
                {
                    var path = _settingRepository.GetSettingValue<string>("Core.Logger.LogDownloadedPagePath");
                    _loggingService.Logger.Information("Writing downloaded document from : {Url}", Url);
                    _loggingService.LogHtmlDocument(_htmlNode, path, Url);
                    _loggingService.Logger.Information("Finished writing the document");
                }

                _loggingService.Logger.Information("Finished Loading Page {Url}", Url);
            }
        }

        public HtmlNode GetLoadedHtmlNode()
        {
            return _htmlNode;
        }
    }
}
