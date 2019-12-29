using HtmlAgilityPack;

namespace Lychee.Scrapper.Domain.Interfaces
{
    public interface IPageListPaginationService
    {
        bool IsFirstPage(HtmlNode node);
        
        int GetLastPageNumber(HtmlNode node);
    }
}
