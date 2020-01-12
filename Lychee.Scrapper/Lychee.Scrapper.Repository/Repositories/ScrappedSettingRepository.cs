using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Lychee.Infrastructure;
using Lychee.Scrapper.Entities.Entities;
using Lychee.Scrapper.Repository.Interfaces;

namespace Lychee.Scrapper.Repository.Repositories
{
    public class ScrappedSettingRepository : IScrappedSettingRepository
    {
        protected virtual List<ScrapeSetting> Settings { get; set; }

        public virtual ICollection<ScrapeSetting> GetAllSettings()
        {
            if (Settings != null) return Settings;
            using (var context = new ScrapperContext())
            {
                var repository = new Repository<ScrapeSetting>(context);
                Settings = repository.GetAll().Include(x => x.ScrapeItemSettings).ToList();
            }
            return Settings;
        }

        public virtual List<ScrapeItemSetting> GetItemSettings(string category)
        {
            if (Settings == null)
                GetAllSettings();

            return Settings
                .Where(x => x.Category == category)
                .SelectMany(x => x.ScrapeItemSettings).ToList();
        }
    }
}
