using System.Collections.Generic;
using System.Data.Entity;
using Lychee.Scrapper.Repository.Entities;

namespace Lychee.Scrapper.Repository
{
    public class ScrapperContextDataInitializer : DropCreateDatabaseAlways<ScrapperContext>
    {
        protected override void Seed(ScrapperContext context)
        {
            var defaultSettings = new List<Setting>
            {
                new Setting {Key = "Scrapping.RelatedData.IsMultipleRow", Value = "false", Description = "true will insert multiple RelatedData entry; false will utilize the custom columns for the row"},
                new Setting {Key = "Scrapping.ScrapperSetting.LogDownloadedPage", Value = "true", Description = "true will save the downloaded html document to the hard drive"},
                new Setting {Key = "Scrapping.ScrapperSetting.LogDownloadedPagePath", Value = @"C:\Cawi\DEV\Lychee\Lychee.Scrapper\Lychee.Scrapper\Lychee.Scrapper.Test\MaxshopScrapper", Description = "Full directory where the downloaded html document logs will be saved"},
            };

            context.Settings.AddRange(defaultSettings);
            base.Seed(context);
        }
    }
}
