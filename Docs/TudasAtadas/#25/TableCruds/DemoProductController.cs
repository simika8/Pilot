using Bl;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Controllers.TableCruds;
/// <summary>
/// Demo Product Controller
/// </summary>
[Route("[controller]")]
public class DemoProductController : TableCrudController<DemoProduct>
{
    /// <summary> </summary>
    public DemoProductController(NexxtPilotContext db)
    {
        Db = db;
    }
}