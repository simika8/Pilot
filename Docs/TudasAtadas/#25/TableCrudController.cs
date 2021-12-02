using Bl;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;

namespace Controllers;
/// <summary>
/// Table CRUD Controller
/// </summary>
public abstract class TableCrudController<T> : ODataController where T : class
{
    /// <summary>Database Context</summary>
    protected NexxtPilotContext Db { get; set; } = null!;

    /// <summary>
    /// Get All data
    /// </summary>
    [HttpGet]
    [EnableQuery]
    public IQueryable<T> Get()
    {
        return Db.Set<T>().AsQueryable();
    }

    /// <summary>Create data</summary>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] T entity)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        Db.Set<T>().Add(entity);
        await Db.SaveChangesAsync();
        return Created(entity);
    }

    /// <summary>Updates data</summary>
    [HttpPut]
    public async Task<IActionResult> Put(Guid key, [FromBody] Microsoft.AspNetCore.OData.Deltas.Delta<T> delta)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var entity = await Db.FindAsync<T>(key);
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
    public async Task<IActionResult> Patch([Microsoft.AspNetCore.OData.Formatter.FromODataUri] Guid key, Microsoft.AspNetCore.OData.Deltas.Delta<T> delta)
    {


        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var entity = await Db.FindAsync<T>(key);
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
        var entity = await Db.FindAsync<T>(key);
        if (entity == null)
        {
            return NotFound();
        }

        Db.Remove(entity);
        await Db.SaveChangesAsync();
        return Ok();
    }
}