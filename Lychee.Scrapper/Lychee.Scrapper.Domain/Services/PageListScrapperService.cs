﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using Lychee.Scrapper.Domain.Interfaces;
using Lychee.Scrapper.Domain.Models.Scrappers;
using Lychee.Scrapper.Repository.Interfaces;

namespace Lychee.Scrapper.Domain.Services
{
    public class PageListScrapperService : IScrapperService, IPageListScrapperService
    {
        private readonly ISettingRepository _settingRepository;
        private readonly PageListScrapper _scrapper;
        private readonly ILoggingService _loggingService;
        private readonly IResultCollectionService _resultCollectionService;
        private readonly IWebQueryService _webQueryService;
        private readonly IPageListPaginationService _pageListPaginationService;

        public PageListScrapperService(ISettingRepository settingRepository, PageListScrapper scrapper,
            ILoggingService loggingService,
            IResultCollectionService resultCollectionService, IWebQueryService webQueryService, 
            IPageListPaginationService pageListPaginationService)
        {
            _settingRepository = settingRepository;
            _scrapper = scrapper;
            _loggingService = loggingService;
            _resultCollectionService = resultCollectionService;
            _webQueryService = webQueryService;
            _pageListPaginationService = pageListPaginationService;
        }

        public virtual async Task Scrape()
        {
            //Loads the initial page
            var scrappedData = await _scrapper.Scrape();

            //Save to db
            _resultCollectionService.SaveScrappedData(scrappedData);

            var htmlNode = _scrapper.GetLoadedHtmlNode();
            var isFirstPage = _pageListPaginationService.IsFirstPage(htmlNode);
            
            if (isFirstPage)
            {
                var lastPage = _pageListPaginationService.GetLastPageNumber(htmlNode);
                if (lastPage > 1)
                {
                    ScrapeOtherPages(lastPage, _scrapper);
                }
            }
            

            //https://www.mightyape.co.nz/games/ps4/best-sellers?page=1
            //Scrape the whole list? PageListScrapper.Strategy.AutomaticallyScrapeAllInCategory


            //This is only for scraping page with normal pagination.
            //If the page has LoadMore button like ComputerLounge or Carousell, then use SmartScrapper


            //Goal is to find the last page number
            //if not given,
            //Determine if there is a summary of pagination. Like how many items are displaying per page and what is the total products in the category
            //Check if total number of product in the list is given. Example. 1 to 40 of 601. Total product is 601
            //Determine how many products are displaying in the list. Example. 1 to 40 of 601. Products in list are 40

            //if these are all available then it's a little bit easier.
            //However if not available, like on maxshop site, it will be a little bit complicated
            //Check if there's a ViewAll button? Most cases there will be not, In Maxshop's case there is.

            //if last page is given then we do not do the complicated steps above. Example of website that already give the last page, https://www.glassons.com/clothing/tops, https://www.numberoneshoes.co.nz/womens

            //if on page 1,
            //Determine the last page. Given the total number of products and product display per page, we can compute the last page (if not given) by Math.Ceil(totalproducts/productperpage)
            //if not just execute the code below to scrape the current page.


            //after determining the pages, we can decide how we want to scrape the products to the other pages.
            //if we want to parallel scrapping or linear. Scrapping.PageListScrapper.IsParallelScrapping
            //Maximum of 4 thread running
        }



        /// <summary>
        /// If the page list has more than 1 page, we scrape all the other pages on the same category too
        /// </summary>
        public virtual void ScrapeOtherPages(int lastPage, IPageListScrapper firstPageScrapper)
        {
            var actions = new List<Action>();

            for (var i = 2; i <= lastPage; i++)
            {
                var scrapper = new PageListScrapper(_settingRepository, _loggingService, _webQueryService);
                firstPageScrapper.Clone(scrapper);
                scrapper.Url = GetNextUrl(i, firstPageScrapper.Url);

                actions.Add(() =>
                {
                    var data = scrapper.Scrape();
                    _resultCollectionService.SaveScrappedData(data.Result);
                });
            }

            //Invoke all the tasks
            try
            {
                Parallel.Invoke(new ParallelOptions { MaxDegreeOfParallelism = 8 }, actions.ToArray());
            }
            catch (AggregateException ex)
            {
                var exceptions = string.Join(ex.InnerExceptions.ToString(), ",");
                _loggingService.Logger.Error(exceptions);
            }
        }

        /// <summary>
        /// This will add page to the url if not exists, otherwise update it with the passed page number
        /// </summary>
        /// <param name="page"></param>
        /// <param name="currentUrl"></param>
        /// <returns></returns>
        protected virtual string GetNextUrl(int page, string currentUrl)
        {
            var queryStringMapRouted = _settingRepository.GetSettingValue<bool>("Scrapping.PageListScrapper.QueryStringMapRouted");

            return queryStringMapRouted
                ? GetNextUrlWithMapRoute(page, currentUrl)
                : GetNextURlWithoutMapRoute(page, currentUrl);
        }

        private string GetNextUrlWithMapRoute(int page, string currentUrl)
        {
            var queryStringPaginationRouteMap = _settingRepository.GetSettingValue<string>("PageListScrapper.Pagination.QueryStringRouteMap");

            if (string.IsNullOrEmpty(queryStringPaginationRouteMap))
            {
                return currentUrl.EndsWith("/") ? currentUrl + page : currentUrl + "/" + page;
            }

            return currentUrl + queryStringPaginationRouteMap + page;
        }

        private string GetNextURlWithoutMapRoute(int page, string currentUrl)
        {
            var queryStringPageVariable = _settingRepository.GetSettingValue<string>("Scrapping.URL.QueryStringPageVariable");

            var uri = new Uri(currentUrl);
            var queryString = HttpUtility.ParseQueryString(uri.Query);

            if (!string.IsNullOrEmpty(queryString.Get(queryStringPageVariable)))
            {
                queryString.Set(queryStringPageVariable, page.ToString());
            }
            else
            {
                queryString.Add(queryStringPageVariable, page.ToString());
            }

            var uriBuilder = new UriBuilder(currentUrl) { Query = queryString.ToString() };
            return uriBuilder.Uri.ToString();
        }
    }
}
