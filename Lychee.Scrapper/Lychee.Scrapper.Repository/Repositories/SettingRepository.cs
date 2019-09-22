using System;
using System.Collections.Generic;
using System.Linq;
using Lychee.Scrapper.Repository.Entities;
using Lychee.Scrapper.Repository.Interfaces;

namespace Lychee.Scrapper.Repository.Repositories
{
    public class SettingRepository : ISettingRepository
    {
        private List<Setting> _settings = null;

        public ICollection<Setting> GetAllSettings()
        {
            using (var context = new ScrapperContext())
            {
                var repository = new Repository<Setting>(context, true);
                _settings = repository.GetAll().ToList();
            }

            return _settings;
        }

        public Setting GetSetting(string key)
        {
            if (_settings == null)
                GetAllSettings();

            return _settings.FirstOrDefault(x => x.Key == key);
        }

        public T GetSettingValue<T>(string key)
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
