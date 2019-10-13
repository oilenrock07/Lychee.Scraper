using HtmlAgilityPack;
using Serilog.Core;
using Serilog.Events;

namespace Lychee.Scrapper.Domain.Interfaces
{
    public interface ILoggingService
    {
        Logger Logger { get; set; }
        void LogHtmlDocument(HtmlNode node, string path, string url);
        void Log(LogEventLevel level, string message, bool logConsole = true);
    }
}
