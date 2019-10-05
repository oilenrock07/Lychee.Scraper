using Lychee.Scrapper.Domain.Interfaces;
using Lychee.Scrapper.Domain.Models.Exceptions;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using Serilog.Core;

namespace Lychee.Scrapper.Domain.Models.Scrappers
{
    public class SmartScrapper : BaseScrapper, IScrapper
    {
        public delegate Task CustomScrapping(Page page);
        public event CustomScrapping CustomScrappingInstructions;

        public virtual bool IsHeadless { get; set; }

        public virtual Browser Browser { get; set; }

        public override async Task<ResultCollection<ResultItemCollection>> Scrape()
        {
            try
            {
                await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision); // This could be potentially reuse
                if (Browser == null)
                {
                    Browser = await Puppeteer.LaunchAsync(new LaunchOptions
                    {
                        Headless = IsHeadless
                    });
                }

                var page = await Browser.NewPageAsync();
                await page.GoToAsync(Url);

                if (CustomScrappingInstructions != null)
                    await CustomScrappingInstructions.Invoke(page);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (!(Browser?.IsClosed ?? false))
                    await Browser.CloseAsync();
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

        //protected virtual void 
        public SmartScrapper(Logger logger)
        {
        }
    }
}
