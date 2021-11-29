using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;

namespace Bl;

public class DemoProductContext : DbContext
{
    private string ConnectionString { get; set; }

    public DbSet<Models.DemoProduct> Products { get; set; } = null!;


    public DemoProductContext()
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

    }
}
public class ProductsContextFactory : IDesignTimeDbContextFactory<DemoProductContext>
{
    public DemoProductContext CreateDbContext(string[] args)
    {
        return new DemoProductContext();
    }
}
