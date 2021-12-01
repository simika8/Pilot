using Bl;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Controllers;

/// <summary> Baseic database commands for develompent</summary>
[Route("api/[controller]")]
[ApiController]
public class ProbaController : ControllerBase
{
    private NexxtPilotContext Db { get; set; }
    /// <summary> </summary>
    public ProbaController(NexxtPilotContext db)
    {
        Db = db;
    }


    /// <summary> </summary>
    [HttpGet(nameof(SyncProba))]
    public string SyncProba()
    {
        //Thread.Sleep(1000);


        //var a = Db.Set<DemoProduct>().Where(x => x.Id == new Guid("00000000-0000-7250-6f64-756374343031")).ToArray();
        var a = Db.Set<DemoProduct>().OrderBy(x => x.Name).Take(1).ToArray();
        return a.First().Name;
    }

    [HttpGet(nameof(AsyncProba))]
    public async Task<string> AsyncProba()
    {
        //await Task.Delay(1000);

        var a = await Db.Set<DemoProduct>().Take(1).ToArrayAsync();
        return a.First().Name;
    }

    [HttpGet(nameof(AsyncProbaO))]
    public async Task<string> AsyncProbaO()
    {
        //await Task.Delay(1000);

        var a = await Db.Set<DemoProduct>().OrderBy(x => x.Name).Take(1).Skip(90000).ToArrayAsync();
        return a.First().Name;
    }
}