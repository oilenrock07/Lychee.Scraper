using HtmlAgilityPack;

namespace Lychee.Scrapper.Domain.Interfaces
{
    public interface ILoggingService
    {
        void LogHtmlDocument(HtmlNode node, string path, string url);
    }
}
