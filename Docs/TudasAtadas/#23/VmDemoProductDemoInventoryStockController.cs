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

    private static IQueryable<Models.VmDemoProductDemoInventoryStock> ApplyServerSideSearch(IQueryable<Models.VmDemoProductDemoInventoryStock> query, string search)
    {
        var querywork = query;
        if (!string.IsNullOrWhiteSpace(search))
        {
            if (double.TryParse(search, out var searchDouble))
            {
                querywork = querywork.Where(x => x.DemoProduct.Stocks.Any(x => x.Quantity == searchDouble)); // bármelyik raktárban a mennyiség egyezik a search-ben megadott számmal
                //querywork = querywork.Where(x => x.DemoInventoryStock.Quantity == searchDouble); // a vizsgál raktárban a mennyiség egyezik a search-ben megadott számmal
            }
            else
            {
                querywork = querywork.Where(x => x.DemoProduct.Name.StartsWith(search));
            }
        }

        return querywork;
    }

    /// <summary>
    /// Get All Demo Products+Stock(Store)
    /// </summary>
    [HttpGet]
    [EnableQuery]
    public IQueryable<Models.VmDemoProductDemoInventoryStock> Get(Guid storeId, string search)
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
        query = ApplyServerSideSearch(query, search);


        return query;
    }
}