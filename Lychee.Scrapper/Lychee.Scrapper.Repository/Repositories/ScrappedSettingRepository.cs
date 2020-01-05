using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Lychee.Scrapper.Entities.Entities;
using Lychee.Scrapper.Repository.Interfaces;

namespace Lychee.Scrapper.Repository.Repositories
{
    public class ScrappedSettingRepository : IScrappedSettingRepository
    {
        private List<ScrapeSetting> _settings = null;

        public virtual ICollection<ScrapeSetting> GetAllSettings()
        {
            if (_settings != null) return _settings;
            using (var context = new ScrapperContext())
            {
                var repository = new Repository<ScrapeSetting>(context, true);
                _settings = repository.GetAll().Include(x => x.ScrapeItemSettings).ToList();
            }
            return _settings;
        }

        public virtual List<ScrapeItemSetting> GetItemSettings(string category)
        {
            if (_settings == null)
                GetAllSettings();

            return _settings
                .Where(x => x.Category == category)
                .SelectMany(x => x.ScrapeItemSettings).ToList();
        }
    }
}
