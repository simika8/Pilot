using Bl;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Models;

namespace Controllers;
/// <summary>
/// Demo Product Controller
/// </summary>
[Route("[controller]")]
public class DemoProductController : ODataController
{
    private NexxtPilotContext Db { get; set; }
    /// <summary> </summary>
    public DemoProductController(NexxtPilotContext db)
    {
        Db = db;
    }

    /// <summary>
    /// Get All Demo Products
    /// </summary>
    [HttpGet]
    [EnableQuery]
    public IQueryable<Models.DemoProduct> Get()
    {
        return Db.Set<DemoProduct>().AsQueryable();
    }

    /// <summary>Create data</summary>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] DemoProduct entity)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        Db.Set<DemoProduct>().Add(entity);
        await Db.SaveChangesAsync();
        return Created(entity);
    }

    /// <summary>Updates data</summary>
    [HttpPut]
    public async Task<IActionResult> Put(Guid key, [FromBody] Microsoft.AspNetCore.OData.Deltas.Delta<DemoProduct> delta)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var entity = await Db.FindAsync<DemoProduct>(key);
        if (entity == null)
        {
            return NotFound();
        }

        delta.Put(entity);
        try
        {
            await Db.SaveChangesAsync();
        }
        catch (Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException)
        {
            throw;
        }
        var res = Updated(entity);
        return res;
    }

    /// <summary>Updates data. Only specified fields</summary>
    [HttpPatch]
    public async Task<IActionResult> Patch([Microsoft.AspNetCore.OData.Formatter.FromODataUri] Guid key, Microsoft.AspNetCore.OData.Deltas.Delta<DemoProduct> delta)
    {


        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var entity = await Db.FindAsync<DemoProduct>(key);
        if (entity == null)
        {
            return NotFound();
        }

        delta.Patch(entity);

        try
        {
            await Db.SaveChangesAsync();
        }
        catch (Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException)
        {
            throw;
        }
        var res = Updated(entity);
        return res;
    }


    /// <summary>Deletes Data</summary>
    [HttpDelete]
    public async Task<IActionResult> Delete(Guid key)
    {
        var entity = await Db.FindAsync<DemoProduct>(key);
        if (entity == null)
        {
            return NotFound();
        }

        Db.Remove(entity);
        await Db.SaveChangesAsync();
        return Ok();
    }
}