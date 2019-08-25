using System.Collections.Generic;
using System.Linq;

namespace Lychee.Scrapper.Domain.Models.Scrappers
{
    public class ResultItemCollection 
    {
        public string Key { get; set; }

        public List<ResultItem> Items { get; set; }
    }
}
