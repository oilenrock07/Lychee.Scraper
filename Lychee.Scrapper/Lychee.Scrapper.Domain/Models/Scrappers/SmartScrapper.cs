using Lychee.Scrapper.Domain.Interfaces;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Lychee.Infrastructure.Interfaces;
using Serilog.Events;

namespace Lychee.Scrapper.Domain.Models.Scrappers
{
    public class SmartScrapper : BaseScrapper, IScrapper
    {
        private readonly ISettingRepository _settingRepository;
        private readonly ILoggingService _loggingService;

        public delegate Task CustomScrapping(Page page, ResultCollection<ResultItemCollection> resultCollection, Dictionary<string, object> args);

        /// <summary>
        /// This is a required property that you need to pass.
        /// This is the instructions that the smart scrapper needs to perform
        /// </summary>
        public List<CustomScrapping> CustomScrappingInstructions { get; set; }
        
        /// <summary>
        /// If you did not pass any custom scripts it will use the default from the website hosted by this project
        /// CustomScripts might be the scripts you have hosted on your site
        /// </summary>
        public List<string> CustomScripts { get; set; }

        public virtual bool IsHeadless { get; set; }
        public bool IsFirstPage { get; set; }
        public string PageContent { get; set; }

        public Dictionary<string, object> Parameters { get; set; }

        private BrowserFetcher BrowserFetcher { get; set; }

        public SmartScrapper(ISettingRepository settingRepository, ILoggingService loggingService,
            IWebQueryService webQueryService) : base(webQueryService)
        {
            _settingRepository = settingRepository;
            _loggingService = loggingService;
        }


        public override async Task<ResultCollection<ResultItemCollection>> Scrape()
        {
            var resultCollection = new ResultCollection<ResultItemCollection>();

            try
            {
                var browserFetcher = GetBrowserFetcher();
                await browserFetcher.DownloadAsync(BrowserFetcher.DefaultRevision);
                var executablePath = browserFetcher.GetExecutablePath(BrowserFetcher.DefaultRevision);
                var options = new LaunchOptions { Headless = IsHeadless, ExecutablePath = executablePath };

                
                using (var browser = await Puppeteer.LaunchAsync(options))
                using (var page = await browser.NewPageAsync())
                {
                    await page.GoToAsync(Url);

                    if (CustomScrappingInstructions?.Any() ?? false)
                    {
                        foreach (var instruction in CustomScrappingInstructions)
                        {
                            await AddScripts(page);
                            await instruction.Invoke(page, resultCollection, Parameters);
                        }
                    }

                    if (IsFirstPage)
                        PageContent = await page.MainFrame.GetContentAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return resultCollection;
        }


        public virtual HtmlNode GetLoadedHtmlNode()
        {
            if (string.IsNullOrEmpty(PageContent))
                return null;

            var doc = new HtmlDocument();
            doc.LoadHtml(PageContent);

            return doc.DocumentNode;
        }

        /// <summary>
        /// Load the browser fetcher from file
        /// </summary>
        /// <returns></returns>
        protected virtual BrowserFetcher GetBrowserFetcher()
        {
            if (BrowserFetcher != null) return BrowserFetcher;

            var downloadPath = _settingRepository.GetSettingValue<string>("SmartScrapper.Chromium.DownloadPath");
            _loggingService.Log(LogEventLevel.Information, $"Attempting to set up puppeteer to use Chromium found under directory {downloadPath}");

            if (!Directory.Exists(downloadPath))
            {
                _loggingService.Log(LogEventLevel.Information, "Custom directory not found. Creating directory");
                Directory.CreateDirectory(downloadPath);
            }

            _loggingService.Log(LogEventLevel.Information, "Downloading Chromium");

            var browserFetcherOptions = new BrowserFetcherOptions { Path = downloadPath };
            BrowserFetcher = new BrowserFetcher(browserFetcherOptions);

            return BrowserFetcher;
        }

        protected virtual async Task AddScripts(Page page)
        {
            if (CustomScripts?.Any() ?? false)
            {
                foreach(var script in CustomScripts)
                    await page.AddScriptTagAsync(script);
            }
            else
            {
                await page.AddScriptTagAsync("http://lychee.scrapper.localhost/Scripts/ScrapperFunctions.js"); //add custom scrapper functions
                await page.AddScriptTagAsync("http://lychee.scrapper.localhost/Scripts/jquery-3.3.1.min.js"); //add jquery for faster searching for element

                //await page.EvaluateFunctionAsync(@"(url1, url2) => {
                //var scriptTag = document.createElement('script');
                //scriptTag.src = url1;

                //var scriptTag2 = document.createElement('script');
                //scriptTag2.src = url2;

                //document.body.appendChild(scriptTag);
                //document.body.appendChild(scriptTag2);}", "http://lychee.scrapper.localhost/Scripts/ScrapperFunctions.js", "http://lychee.scrapper.localhost/Scripts/jquery-3.3.1.min.js");

            }
        }

    }
}
