using Lychee.Scrapper.Domain.Models.Scrappers;

namespace Lychee.Scrapper.Domain.Interfaces
{
    public interface IPageListScrapper : IScrapper
    {
        /// <summary>
        /// Selector of the item we want to scrape.
        /// The repeating DIVs that contain the products
        /// Example from Solutionists page list, this is the article
        /// </summary>
        string ItemXPath { get; set; }

        /// <summary>
        /// Some Product List page does not have pagination and only have Load More button
        /// on the same page. Example is Carousell
        /// </summary>
        bool LoadMoreOnSamePage { get; set; }

        /// <summary>
        /// Some pages are using ajax to load it's content.
        /// Set the property to wait for the content to load
        /// </summary>
        bool WaitForJavascriptToLoad { get; set; }

        PageListPagination PaginationSettings { get; set; }

        /// <summary>
        /// Clone only the properties that are native (string, int, bool)
        /// If you want to copy all the complex properties like classes, use DeepClone
        /// </summary>
        /// <param name="scrapper"></param>
        void Clone(IPageListScrapper scrapper);

        /// <summary>
        /// Clone all the properties of the class including the complex types
        /// </summary>
        /// <param name="scrapper"></param>
        void DeepClone(PageListScrapper scrapper);
    }
}
