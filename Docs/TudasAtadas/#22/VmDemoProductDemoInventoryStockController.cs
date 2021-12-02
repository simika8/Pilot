using Bl;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Controllers;
/// <summary>
/// Demo Product+Stock(Store) Controller
/// </summary>
[Route("[controller]")]
public class VmDemoProductDemoInventoryStockController : ODataController
{
    private NexxtPilotContext Db { get; set; }
    /// <summary> </summary>
    public VmDemoProductDemoInventoryStockController(NexxtPilotContext db)
    {
        Db = db;
    }

    /// <summary>
    /// Get All Demo Products+Stock(Store)
    /// </summary>
    [HttpGet]
    [EnableQuery]
    public IQueryable<Models.VmDemoProductDemoInventoryStock> Get(Guid storeId)
    {
        var query = Db.DemoProducts
            .GroupJoin(Db.DemoInventoryStocks, t => t.Id, jt => jt.ProductId, (t, jt) => new { t, jt })
            .SelectMany(
                temp => temp.jt.DefaultIfEmpty(),
                (temp, jt) =>
                    new VmDemoProductDemoInventoryStock
                    {
                        Id = temp.t.Id,
                        DemoProduct = temp.t,
                        DemoInventoryStock = jt,
                    }
            )
            .Where(x => x.DemoInventoryStock == null || x.DemoInventoryStock.StoreId == storeId);
        return query;
    }
}