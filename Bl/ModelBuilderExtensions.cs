using Microsoft.EntityFrameworkCore;
using Pluralize.NET;

namespace Bl;

static class ModelBuilderExtensions
{
    public static void PluralizingTableNameConvention(this ModelBuilder modelBuilder)
    {

        var pluralizer = new Pluralizer();
        foreach (Microsoft.EntityFrameworkCore.Metadata.IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes())
        {
            string? tableName = entityType.GetTableName();
            if (tableName == null)
                throw new Exception("Missing table name!");
            tableName = pluralizer.Pluralize(tableName);
            entityType.SetTableName(tableName);
        }
    }
}
