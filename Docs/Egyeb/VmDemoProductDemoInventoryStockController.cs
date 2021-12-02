using Bl;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Controllers;
/// <summary>
/// Demo Product Controller
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
    /// Get All Demo Products
    /// </summary>
    [HttpGet]
    [EnableQuery]
    public IEnumerable<Models.VmDemoProductDemoInventoryStock> Get(Guid storeId)
    {


        var res8 = Db.DemoProducts
            //.Where(x => x.Name.StartsWith("Aa"))
            .GroupJoin(Db.DemoInventoryStocks, t => t.Id, jt => jt.ProductId, (t, jt) => new { t, jt })
            .SelectMany(
                temp => temp.jt.DefaultIfEmpty(),
                (temp, p) =>
                    new VmDemoProductDemoInventoryStock
                    {
                        Id = temp.t.Id,
                        DemoProduct = temp.t,
                        DemoInventoryStock = p,
                    }
            )
            .Where(x => x.DemoInventoryStock == null || x.DemoInventoryStock.StoreId == storeId);

        var res8a = res8.OrderBy(x => x.DemoProduct.Name).Take(5).ToList();
        return res8;


        var res7 = Db.DemoProducts
            .Where(x => x.Name.StartsWith("Aa"))
            .Join(Db.DemoInventoryStocks, t => t.Id, jt => jt.ProductId, (t, jt) => new { t, jt });
        var res7a = res7.ToList();
        ;
        var res9 = Db.DemoProducts
            .Where(x => x.Name.StartsWith("Aa"))
            .GroupJoin(Db.DemoInventoryStocks, t => t.Id, jt => jt.ProductId, (t, jt) => new { t, jt })
            .SelectMany(
                temp => temp.jt.DefaultIfEmpty(),
                (temp, p) =>
                    new
                    {
                        i = temp.t,
                        p = p
                    }
            );
        var res9a = res9.ToList();
        ;

        var res6 = Db.DemoProducts
            .Where(x => x.Name.StartsWith("Aa"))
            .SelectMany(t => t.Stocks.DefaultIfEmpty(), (t, jt) => new VmDemoProductDemoInventoryStock()
            {
                DemoProduct = t,
                DemoInventoryStock = jt,
            })

            /*
            .Where(x => x.DemoInventoryStock == null || x.DemoInventoryStock.StoreId == storeId)*/;
        ;

        var res6a = res6.ToList();


        /*var res4 = Db.DemoProducts
         .GroupJoin(Db.DemoInventoryStocks, t => t.Id, jt => jt.ProductId, (t, jt) => new { t = t, jt = jt })

         .SelectMany(x => x.jt.DefaultIfEmpty(), (x, y) => new VmDemoProductDemoInventoryStock()
         {
             DemoProduct = x.t,
             StockQuantity = x.jt.Sum(y => y.Quantity),
         })
            .Where(x => x.DemoInventoryStock == null || x.DemoInventoryStock.StoreId == storeId);
        var res4a = res4.ToList();
        ;
        return null;*/

        var res5 = (from p in Db.DemoProducts
                    join ps in Db.DemoInventoryStocks
                    on p.Id equals ps.ProductId into stocks
                    from stock in stocks.DefaultIfEmpty()
                    select (new { p.Id, p.Name, p.Price, stock.Quantity }))
                       .GroupBy(x => x, ps => ps,
                       (p, ps) =>
                       new VmDemoProductDemoInventoryStock()
                       {
                           DemoProduct = new DemoProduct() { Id = p.Id, Name = p.Name, Price = p.Price },
                           Id = p.Id,
                           StockQuantity = ps.Sum(y => y.Quantity),
                       });
        var aa = res5.ToList();
        //var ers1x = res1.ToList();
        return res5;

        var res1 = (from p in Db.DemoProducts
                                join ps in Db.DemoInventoryStocks
                                on p.Id equals ps.ProductId into stocks
                                from stock in stocks.DefaultIfEmpty()
                                select (new { p.Id, p.Name, p.Price, stock.Quantity }))
                       .GroupBy(x => new { x.Id, x.Name, x.Price,}, ps => ps,
                       (p, ps) =>
                       new VmDemoProductDemoInventoryStock()
                       {
                           DemoProduct = new DemoProduct() { Id = p.Id, Name = p.Name, Price = p.Price},
                           Id = p.Id,
                           StockQuantity = ps.Sum(y => y.Quantity),
                       });
        //var ers1x = res1.ToList();
        return res1;
        var res3 = Db.DemoProducts
            .Where(x => x.Name.StartsWith("Aa"))
            .SelectMany(t => t.Stocks.DefaultIfEmpty(), (t, jt) => new VmDemoProductDemoInventoryStock()
            {
                DemoProduct = t,
                DemoInventoryStock = jt,
            })/*
            .Where(x => x.DemoInventoryStock == null || x.DemoInventoryStock.StoreId == storeId)*/;
        ;
        return res3;

     
    }
}