using System.Collections.Generic;
using System.Linq;

namespace Lychee.Scrapper.Domain.Models.Scrappers
{
    public class ResultItemCollection 
    {
        public string Key { get; set; }

        public List<ResultItem> Items { get; set; }

        public virtual ResultItem GetItem(string name)
        {
            return Items?.FirstOrDefault(x => string.Equals(x.Name, name));
        }
    }
}
