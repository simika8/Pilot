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
    public IEnumerable<Models.DemoProduct> Get()
    {
        return Db.Set<DemoProduct>().AsQueryable();
    }
}