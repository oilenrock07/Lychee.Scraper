﻿using System.Data.Entity;
using Lychee.Scrapper.Repository.Entities;

namespace Lychee.Scrapper.Repository
{
    public class ScrapperContext : DbContext
    {
        public ScrapperContext() : base("Scrapper.ConnectionString.Access")
        {
            Database.SetInitializer(new ScrapperContextDataInitializer());
        }

        public DbSet<ScrappedData> ScrappedData { get; set; }
        public DbSet<RelatedData> RelatedData { get; set; }
        public DbSet<ColumnDefinition> ColumnDefinitions { get; set; }
        public DbSet<Setting> Settings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ScrappedData>()
                .HasMany(x => x.RelatedData)
                .WithOptional(x => x.ScrappedData);
        }

    }
}
