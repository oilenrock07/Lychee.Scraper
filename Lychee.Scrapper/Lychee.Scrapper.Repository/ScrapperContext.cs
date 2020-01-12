using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using Lychee.Scrapper.Entities.Entities;

namespace Lychee.Scrapper.Repository
{
    public class ScrapperContext : DbContext
    {
        public ScrapperContext() : base("Scrapper.ConnectionString.MsSql")
        {
            //Database.SetInitializer(new ScrapperContextDataInitializer());
        }

        public virtual IDbSet<ScrappedData> ScrappedData { get; set; }
        public virtual IDbSet<RelatedData> RelatedData { get; set; }
        public virtual IDbSet<ColumnDefinition> ColumnDefinitions { get; set; }
        public virtual IDbSet<Setting> Settings { get; set; }

        public virtual IDbSet<ScrapeSetting> ScrapeSettings { get; set; }
        public virtual IDbSet<ScrapeItemSetting> ScrapeItemSettings { get; set; }
        public virtual IDbSet<HeaderRequest> HeaderRequests { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ScrappedData>()
                .HasMany(x => x.RelatedData)
                .WithOptional(x => x.ScrappedData);
        }

    }
}
