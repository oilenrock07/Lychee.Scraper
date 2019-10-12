using System.Collections.Generic;
using Lychee.Scrapper.Repository.Entities;

namespace Lychee.Scrapper.Repository.Interfaces
{
    public interface IScrappedSettingRepository
    {
        ICollection<ScrapeSetting> GetAllSettings();

        List<ScrapeItemSetting> GetItemSettings(string category);

    }
}
