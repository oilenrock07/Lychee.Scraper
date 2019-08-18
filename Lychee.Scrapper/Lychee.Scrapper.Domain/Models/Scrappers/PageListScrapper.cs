using Lychee.Scrapper.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lychee.Scrapper.Domain.Models.Scrappers
{
    public class PageListScrapper : BaseScrapper, IScrapper
    {

        public virtual PageListPagination PaginationSettings { get; set; }

        /// <summary>
        /// XPath of the item we want to scrape
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
    }
}
