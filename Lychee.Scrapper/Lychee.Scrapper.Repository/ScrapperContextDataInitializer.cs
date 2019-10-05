using System.Collections.Generic;
using System.Data.Entity;
using Lychee.Scrapper.Repository.Entities;

namespace Lychee.Scrapper.Repository
{
    public class ScrapperContextDataInitializer : DropCreateDatabaseAlways<ScrapperContext>
    {
        protected override void Seed(ScrapperContext context)
        {
            var defaultSettings = new List<Setting>
            {
                //Schema
                new Setting {Key = "Scrapping.RelatedData.IsMultipleRow", Value = "false", Description = "true will insert multiple RelatedData entry; false will utilize the custom columns for the row"},

                //Logging
                new Setting {Key = "Scrapping.ScrapperSetting.LogDownloadedPage", Value = "true", Description = "true will save the downloaded html document to the hard drive"},
                new Setting {Key = "Scrapping.ScrapperSetting.LogDownloadedPagePath", Value = @"C:\Cawi\DEV\Lychee\Lychee.Scrapper\Lychee.Scrapper\Lychee.Scrapper.Test\MaxshopScrapper", Description = "Full directory where the downloaded html document logs will be saved"},

                //PageList Scrapping Strategy
                new Setting {Key = "Scrapping.PageListScrapper.AutomaticallyScrapeAllInCategory", Value = "false", Description = "Advance feature to get all the categories from the product list page then proceed to scrapping these categories"},

                //URL
                new Setting {Key = "Scrapping.PageListScrapper.QueryStringPageVariable", Value = "page", Description = "The variable for page used by the website. Normally this is page or p"},
                new Setting {Key = "Scrapping.URL.QueryStringMapRouted", Value = "false", Description = "Has boolean value. This is determines if the the url of the website is formatted. Meaning the URL does not contain the querystring and it is formatted. Example: http://mymovies.localhost/Movie/Index/2"},
                new Setting {Key = "Scrapping.URL.QueryStringPaginationRouteMap", Value = "", Description = "Most of the time keep this empty so the page number will just simply appended to the current url. Example http://mymovies.localhost/Movie/Index/Category/Action will become http://mymovies.localhost/Movie/Index/Category/Action/2. Add the necessary url appender here. Example http://mymovies.localhost/ appender is **Movie/Index**"},
                
                //Pagination
                new Setting {Key = "Scrapping.PageListScrapper.PaginationSelector", Value = "", Description = "This will vary per website. This is the main selector from the pagination DOM"},
                new Setting {Key = "Scrapping.PageListScrapper.PaginationIsLastPageGiven", Value = "false", Description = "If the last page number is displayed at the pagination DOM. Example websites that show the last page are: https://www.glassons.com/clothing/tops, https://www.numberoneshoes.co.nz/womens"},
                new Setting {Key = "Scrapping.PageListScrapper.PaginationIsTotalNumberOfProductsGiven", Value = "true", Description = "Some websites like MightyApe shows how many products/results there is on the category"},
                new Setting {Key = "Scrapping.PageListScrapper.PaginationTotalNumberOfProductsSelector", Value = ".products .gallery-header .summary .results .total", Description = "If the last page number is not given on the page, but "},
                new Setting {Key = "Scrapping.PageListScrapper.PaginationProductsPerPage", Value = "40", Description = "You can just manually set this value. Count the products that the website is showing per page"},

                //new Setting {Key = "Scrapping.PageListScrapper.PaginationLastPageSelector", Value = ".products .gallery-header .summary .results .total", Description = "The last page"},

            };

            context.Settings.AddRange(defaultSettings);
            base.Seed(context);
        }
    }
}
