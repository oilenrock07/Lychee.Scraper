using System.Collections.Generic;

namespace Lychee.Scrapper.Entities.Entities
{
    public class ScrapeSetting
    {
        public int ScrapeSettingId { get; set; }

        public string Category { get; set; }

        public virtual ICollection<ScrapeItemSetting> ScrapeItemSettings { get; set; }
    }
}
