using Lychee.Scrapper.Domain.Interfaces;
using Lychee.Scrapper.Domain.Models.Scrappers;
using System;

namespace Lychee.Scrapper.Domain
{
    public class ScrapperFactory
    {
        private string _scrapper;

        public ScrapperFactory(string scrapper)
        {
            _scrapper = scrapper;
        }

        public IScrapper GetScrapper()
        {
            if (String.Equals(_scrapper, "PAGE_LIST", StringComparison.InvariantCultureIgnoreCase))
                return new PageListScrapper();
            else if (String.Equals(_scrapper, "PAGE_LIST", StringComparison.InvariantCultureIgnoreCase))
                return new PageDetailScrapper();

            return new SmartScrapper();
        }
    }
}
