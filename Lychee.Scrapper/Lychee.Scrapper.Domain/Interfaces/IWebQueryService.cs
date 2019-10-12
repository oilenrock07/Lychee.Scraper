
using HtmlAgilityPack;

namespace Lychee.Scrapper.Domain.Interfaces
{
    public interface IWebQueryService
    {
        HtmlDocument GetPage(string url, string method = "GET");
    }
}
