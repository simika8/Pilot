using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;

namespace Bl;

public class NexxtPilotContext : DbContext
{
    private string ConnectionString { get; set; }

    public DbSet<Models.DemoProduct> DemoProducts { get; set; } = null!;
    public DbSet<Models.DemoInventoryStock> DemoInventoryStocks { get; set; } = null!;


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
        //This is for sub tables (refenenced only from model not from Dbcontext) be plural (eg: ProductExt -> ProductExts)
        modelBuilder.PluralizingTableNameConvention();

		//Without this: 
		//Cannot write DateTime with Kind=Unspecified to PostgreSQL type 'timestamp with time zone', only UTC is supported. Note that it's not possible to mix DateTimes with different Kinds in an array/range. See the Npgsql.EnableLegacyTimestampBehavior AppContext switch to enable legacy behavior. at Npgsql.Internal.TypeHandlers.DateTimeHandlers.TimestampTzHandler.ValidateAndGetLength(DateTime value, NpgsqlParameter parameter)
        foreach (var property in modelBuilder.Model.GetEntityTypes()
                 .SelectMany(t => t.GetProperties())
                 .Where(p => p.ClrType == typeof(DateTime) || p.ClrType == typeof(DateTime?)))
        {
            property.SetColumnType("timestamp without time zone");
        }
            
    }
}