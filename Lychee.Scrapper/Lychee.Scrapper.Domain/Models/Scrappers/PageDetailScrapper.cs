using Lychee.Scrapper.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lychee.Scrapper.Domain.Models.Scrappers
{
    public class PageDetailScrapper : BaseScrapper, IScrapper
    {
        public override Task<ResultCollection<ResultItemCollection>> Scrape()
        {
            return null;
        }
    }
}
