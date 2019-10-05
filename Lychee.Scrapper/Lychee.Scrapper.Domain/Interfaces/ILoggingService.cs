using HtmlAgilityPack;
using Serilog.Core;

namespace Lychee.Scrapper.Domain.Interfaces
{
    public interface ILoggingService
    {
        Logger Logger { get; set; }
        void LogHtmlDocument(HtmlNode node, string path, string url);
    }
}
