using Lychee.Scrapper.Domain.Interfaces;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Lychee.Scrapper.Repository.Interfaces;
using Serilog.Events;

namespace Lychee.Scrapper.Domain.Models.Scrappers
{
    public class SmartScrapper : BaseScrapper, IScrapper
    {
        private readonly ISettingRepository _settingRepository;
        private readonly ILoggingService _loggingService;

        public delegate Task CustomScrapping(Page page, ResultCollection<ResultItemCollection> resultCollection);
        public List<CustomScrapping> CustomScrappingInstructions { get; set; }
        
        public virtual bool IsHeadless { get; set; }

        private BrowserFetcher BrowserFetcher { get; set; }

        public SmartScrapper(ISettingRepository settingRepository, ILoggingService loggingService,
            IWebQueryService webQueryService) : base(webQueryService)
        {
            _settingRepository = settingRepository;
            _loggingService = loggingService;
        }


        public override async Task<ResultCollection<ResultItemCollection>> Scrape()
        {
            try
            {
                var browserFetcher = GetBrowserFetcher();
                await browserFetcher.DownloadAsync(BrowserFetcher.DefaultRevision);
                var executablePath = browserFetcher.GetExecutablePath(BrowserFetcher.DefaultRevision);
                var options = new LaunchOptions { Headless = IsHeadless, ExecutablePath = executablePath };

                var resultCollection = new ResultCollection<ResultItemCollection>();
                using (var browser = await Puppeteer.LaunchAsync(options))
                using (var page = await browser.NewPageAsync())
                {
                    await page.GoToAsync(Url);

                    if (CustomScrappingInstructions?.Any() ?? false)
                    {
                        foreach (var instruction in CustomScrappingInstructions)
                        {
                            await instruction.Invoke(page, resultCollection);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return null;

            //await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);
            //var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            //{
            //    Headless = true
            //});

            //var page = await browser.NewPageAsync();
            //await page.GoToAsync(Url);

            //await page.SelectAsync("#datatab1_1 #stock_code", "MPS");
            //await page.SelectAsync("#stkfut_frm #sel_exp_date", "2019-10-31");
            //ElementHandle[] element = await page.XPathAsync("//*[@id=\"stkfut_frm\"]/div/div[3]/div/a");
            //await element.First().ClickAsync();

            //await page.WaitForSelectorAsync(".MT15");
            //var result = await page.MainFrame.GetContentAsync();
        }


        public HtmlNode GetLoadedHtmlNode()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Load the browser fetcher from file
        /// </summary>
        /// <returns></returns>
        protected BrowserFetcher GetBrowserFetcher()
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
    }
}
