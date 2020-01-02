using System.Collections.Generic;
using System.Linq;

namespace Lychee.Scrapper.Domain.Models.Scrappers
{
    public class ResultCollection<T> : List<T>
        where T: ResultItemCollection, new()
    {
        public virtual ResultItemCollection GetItem(string key)
        {
            return Enumerable.FirstOrDefault(this, x => x.Key == key);
        }

        public virtual List<ResultItem> GetByKey(string key)
        {
            return this.FirstOrDefault(x => string.Equals(x.Key, key))?.Items;
        }

        public virtual void AddItem(string key, List<ResultItem> items)
        {
            this.Add(new T
            {
                Key = key,
                Items = items
            });
        }
    }
}
