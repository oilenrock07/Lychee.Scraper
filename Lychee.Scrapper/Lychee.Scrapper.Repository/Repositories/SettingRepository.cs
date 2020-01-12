using System;
using System.Collections.Generic;
using System.Linq;
using Lychee.Infrastructure;
using Lychee.Scrapper.Entities.Entities;
using Lychee.Scrapper.Repository.Interfaces;

namespace Lychee.Scrapper.Repository.Repositories
{
    public class SettingRepository : ISettingRepository
    {
        protected virtual List<Setting> _settings { get; set; }

        public virtual ICollection<Setting> GetAllSettings()
        {
            if (_settings != null) return _settings;
            using (var context = new ScrapperContext())
            {
                var repository = new Repository<Setting>(context);
                _settings = repository.GetAll().ToList();
            }
            return _settings;
        }

        public virtual Setting GetSetting(string key)
        {
            if (_settings == null)
                GetAllSettings();

            return _settings.FirstOrDefault(x => x.Key == key);
        }

        public virtual T GetSettingValue<T>(string key)
        {
            if (_settings == null)
                GetAllSettings();

            var setting = GetSetting(key);
            if (setting == null)
                return default;

            return (T) Convert.ChangeType(setting.Value, typeof(T));
        }
    }
}
