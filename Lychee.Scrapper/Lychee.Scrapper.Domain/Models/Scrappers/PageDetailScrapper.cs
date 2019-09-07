using Lychee.Scrapper.Domain.Interfaces;
using System.Threading.Tasks;
using Serilog.Core;

namespace Lychee.Scrapper.Domain.Models.Scrappers
{
    public class PageDetailScrapper : BaseScrapper, IScrapper
    {
        public override Task<ResultCollection<ResultItemCollection>> Scrape()
        {
            return null;
        }

        public PageDetailScrapper(Logger logger) : base(logger)
        {
        }
    }
}
