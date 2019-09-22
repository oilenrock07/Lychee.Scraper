using System.Collections.Generic;
using Lychee.Scrapper.Repository.Entities;

namespace Lychee.Scrapper.Repository.Interfaces
{
    public interface ISettingRepository
    {
        ICollection<Setting> GetAllSettings();
        Setting GetSetting(string key);
        T GetSettingValue<T>(string key);
    }
}
