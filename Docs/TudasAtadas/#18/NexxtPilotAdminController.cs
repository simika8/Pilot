#if DEBUG
using Bl;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Controllers;

/// <summary> Baseic database commands for develompent</summary>
[Route("api/[controller]")]
[ApiController]
public class NexxtPilotAdminController : ControllerBase
{

    /// <summary> Deletes Database</summary>
    [HttpPost(nameof(DatabaseEnsureDeleted))]
    public async Task DatabaseEnsureDeleted()
    {
        using var db = new NexxtPilotContext();
        await db.Database.EnsureDeletedAsync();
    }

    /// <summary> Creates and Updates Database</summary>
    [HttpPost(nameof(DatabaseMigrate))]
    public async Task DatabaseMigrate()
    {
        using var db = new NexxtPilotContext();
        await db.Database.MigrateAsync();
    }

    /// <summary> Populates Database with some record</summary>
    [HttpPost(nameof(DatabasePopulate))]
    public async Task DatabasePopulate()
    {
        using var db = new NexxtPilotContext();
        var p1id = new Guid("00000000-0000-7250-6f64-756374343031");
        var p = new Models.DemoProduct()
        {
            Id = p1id,
            Name = "Product 1",
            Active = true,
            Price = 1230,
            ReleaseDate = DateTime.Parse("2021-01-01T00:12:34.567+01:00").ToUniversalTime(),
            Rating = 1,
            Ext = new DemoProductExt()
            {
                Description = "B short description of Product 1",
                MinimumStock = 2,
                ProductId = p1id,
            }
        };
        p.Stocks.Add(new DemoInventoryStock() { StoreId = Guid.NewGuid(), ProductId = p1id, Quantity = 3 });
        db.Add(p);

        await db.SaveChangesAsync();

    }

    /// <summary> Deletes Database</summary>
    [HttpPost(nameof(DatabaseEnsureDeletedMigratePopulate))]
    public async Task DatabaseEnsureDeletedMigratePopulate()
    {
        await DatabaseEnsureDeleted();
        await DatabaseMigrate();
        await DatabasePopulate();
    }
}
#endif