using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using Lychee.Scrapper.Domain.Helpers;
using Lychee.Scrapper.Domain.Interfaces;
using Lychee.Scrapper.Repository.Interfaces;

namespace Lychee.Scrapper.Domain.Services
{
    public class PageListPaginationService : IPageListPaginationService
    {
        private readonly ISettingRepository _settingRepository;
        private readonly ILoggingService _loggingService;
        private readonly IPageListScrapper _scrapper;
        public PageListPaginationService(ISettingRepository settingRepository, ILoggingService loggingService, IPageListScrapper scrapper)
        {
            _settingRepository = settingRepository;
            _loggingService = loggingService;
            _scrapper = scrapper;
        }

        /// <summary>
        /// First step is to determine if the current page is the first page.
        /// If first page, we can implement more sophisticated logic to also scrape the other pages until the last page.
        /// Two way to determine if it is the first page, first is if it exists on the URL as a query string and the other is to manually look for the selected class in the DOM
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public virtual bool IsFirstPage(HtmlNode node)
        {
            //check first if page exists on the url
            if (!string.IsNullOrEmpty(_scrapper.Url) && _scrapper.Url.Contains("?"))
            {
                _loggingService.Logger.Information("Trying to determine if first page from the Url");

                var queryStringPageVariable = _settingRepository.GetSettingValue<string>("PageListScrapper.URL.QueryStringPageVariable");
                var uri = new Uri(_scrapper.Url);
                var page = HttpUtility.ParseQueryString(uri.Query).Get(queryStringPageVariable);

                if (!string.IsNullOrEmpty(page))
                {
                    int.TryParse(page, out var result);
                    if (result == 1)
                        return true;
                }
            }


            //check if document has pagination
            if (!string.IsNullOrEmpty(_scrapper.PaginationSettings?.PaginationSelector))
            {
                _loggingService.Logger.Information("Trying to determine if first page pagination");
                var selectedPage = node.QuerySelector(_scrapper.PaginationSettings.PaginationSelector);
                if (selectedPage != null)
                {
                    var page = selectedPage.InnerHtml.ToInt();
                    return page == 1;
                }
            }

            return false;
        }

        /// <summary>
        /// Next step after finding out first first page is to get the last page of the category list.
        /// This is so we could initiate the strategy for scrapping the other pages. e.g. Parallel scrapping the other pages.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public virtual int GetLastPageNumber(HtmlNode node)
        {
            //If the last page number is displayed at the pagination DOM. Example websites that show the last page are: https://www.glassons.com/clothing/tops, https://www.numberoneshoes.co.nz/womens
            var isLastPageGiven = _settingRepository.GetSettingValue<bool>("PageListScrapper.Pagination.IsLastPageGiven");
            if (isLastPageGiven)
            {
                return 1; //edit this
            }
            else
            {

                //if total product is given, we can compute the last page. Example sites are MightyApe
                var totalProductIsGiven = _settingRepository.GetSettingValue<bool>("PageListScrapper.Pagination.IsTotalNumberOfProductsGiven");
                if (totalProductIsGiven)
                {
                    var productsPerPage = _settingRepository.GetSettingValue<int>("PageListScrapper.Pagination.ProductsPerPage");
                    var totalProductsSelector = _settingRepository.GetSettingValue<string>("PageListScrapper.Pagination.TotalNumberOfProductsSelector");
                    var totalProducts = (node.QuerySelector(totalProductsSelector)?.InnerHtml ?? "0").ToInt();
                    if (productsPerPage > 0 && totalProducts > 0)
                    {
                        var quotient = (decimal)totalProducts / productsPerPage;
                        return (int)Math.Ceiling(quotient);
                    }
                }
            }

            return 0;
        }
    }
}
