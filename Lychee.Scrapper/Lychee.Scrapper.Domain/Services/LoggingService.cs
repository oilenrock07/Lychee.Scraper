using System;
using System.Diagnostics;
using System.IO;
using HtmlAgilityPack;
using Lychee.Scrapper.Domain.Interfaces;
using Serilog.Core;
using Serilog.Events;

namespace Lychee.Scrapper.Domain.Services
{
    public class LoggingService : ILoggingService, IDisposable
    {
        public Logger Logger { get; set; }

        public LoggingService(Logger logger)
        {
            Logger = logger;
        }

        public virtual void LogHtmlDocument(HtmlNode node, string path, string url)
        {
            try
            {
                var fullPath = Path.Combine(path, SanitizeFileName(url) + ".txt");
                using (var file = new System.IO.StreamWriter(fullPath, true))
                {
                    file.WriteLine(node.OuterHtml);
                }
            }
            catch (Exception ex)
            {
                //do nothing
            }
        }

        public virtual void Log(LogEventLevel level, string message, bool logConsole = true)
        {
            if (logConsole)
            {
                Console.WriteLine(message);
                Debug.WriteLine(message);
            }
            
            Logger.Write(level, message);
        }

        public static string SanitizeFileName(string name)
        {
            string invalidChars = System.Text.RegularExpressions.Regex.Escape(new string(System.IO.Path.GetInvalidFileNameChars()));
            string invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);

            return System.Text.RegularExpressions.Regex.Replace(name, invalidRegStr, "_");
        }

        public virtual void Dispose()
        {
            Logger?.Dispose();
        }
    }
}
