using HtmlAgilityPack;

namespace Lychee.Scrapper.Domain.Interfaces
{
    public interface IScrapper
    {
        HtmlNode GetLoadedHtmlNode();
    }
}
