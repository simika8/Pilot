using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Bl;

public class NexxtPilotContext : DbContext
{
    private string ConnectionString { get; set; }

    public DbSet<Models.DemoProduct> Products { get; set; } = null!;


    public NexxtPilotContext()
    {

        ConnectionString = Environment.GetEnvironmentVariable("POSTGRESQLCONNSTR_NexxtRdb") ?? "";
        if (string.IsNullOrWhiteSpace(ConnectionString))
            throw new Exception("Connection string is empty");
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
public class ProductsContextFactory : IDesignTimeDbContextFactory<NexxtPilotContext>
{
    public NexxtPilotContext CreateDbContext(string[] args)
    {
        return new NexxtPilotContext();
    }
}
