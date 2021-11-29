using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;
using Pluralize.NET;

namespace Models
{
    public class ProductContext : DbContext
    {
        private string ConnectionString { get; set; }

        public DbSet<Product> Products { get; set; } = null!;


        public ProductContext() 
        {
            ConnectionString = Environment.GetEnvironmentVariable("NpgsqlConnectionStringWithoutDatabase") ?? "";
            var contextName = this.GetType().Name;
            var databaseName = contextName.Remove(contextName.Length - "Context".Length);
            ConnectionString += ";Database=" + databaseName;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {

            options
                .UseNpgsql(ConnectionString);

            // this is for table and field names be lower cased
            // lower cased names results sqls without quotet table and field names (pgsql)
            options
               .UseLowerCaseNamingConvention();
            
#if DEBUG
            options
                .UseLoggerFactory(LoggerFactory.Create(builder => { builder.AddConsole(); }))
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();
#endif

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //This is for sub tables (refenenced only from model not from Dbcontext) be plural (eg: ProductAlternateId -> ProductAlternateIds)
            modelBuilder.PluralizingTableNameConvention();
        }



    }
    public class ProductsContextFactory : IDesignTimeDbContextFactory<ProductContext>
    {
        public ProductContext CreateDbContext(string[] args)
        {
            return new ProductContext();
        }
    }

    public static class ModelBuilderExtensions
    {
        public static void PluralizingTableNameConvention(this ModelBuilder modelBuilder)
        {
            
            var pluralizer = new Pluralizer();
            foreach (Microsoft.EntityFrameworkCore.Metadata.IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes())
            {
                string tableName = entityType.GetTableName();
                tableName = pluralizer.Pluralize(tableName);
                entityType.SetTableName(tableName);
            }
        }
    }
}
