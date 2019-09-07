using System.Data.Entity;
using Lychee.Scrapper.Repository.Entities;

namespace Lychee.Scrapper.Repository
{
    public class ScrapperContext : DbContext
    {
        public ScrapperContext() : base("Scrapper.ConnectionString.Access")
        {
            
        }

        public DbSet<ScrappedData> ScrappedData { get; set; }
        public DbSet<RelatedData> RelatedData { get; set; }
        public DbSet<ColumnDefinition> ColumnDefinitions { get; set; }
        
    }
}
