using System.Collections.Generic;
using Lychee.Scrapper.Entities.Entities;

namespace Lychee.Scrapper.Repository.Interfaces
{
    public interface IScrappedSettingRepository
    {
        ICollection<ScrapeSetting> GetAllSettings();

        List<ScrapeItemSetting> GetItemSettings(string category);

    }
}
