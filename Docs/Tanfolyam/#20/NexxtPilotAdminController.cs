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

        var prodcount = 100000;
        for (int i = 1; i <= prodcount; i++)
        {
            var p = RandomProduct.GenerateProduct(i, prodcount);
            db.Add(p);
        }

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