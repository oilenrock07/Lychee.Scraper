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
                new Setting {Key = "Scrapping.RelatedData.IsMultipleRow", Value = "false", Description = "true will insert multiple RelatedData entry; false will utilize the custom columns for the row"}
            };

            context.Settings.AddRange(defaultSettings);
            base.Seed(context);
        }
    }
}
