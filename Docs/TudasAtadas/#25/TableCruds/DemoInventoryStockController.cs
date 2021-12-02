using Bl;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Controllers.TableCruds;
/// <summary>
/// Demo Product Controller
/// </summary>
[Route("[controller]")]
public class DemoInventoryStockController : TableCrudController<DemoInventoryStock>
{
    /// <summary> </summary>
    public DemoInventoryStockController(NexxtPilotContext db)
    {
        Db = db;
    }
}