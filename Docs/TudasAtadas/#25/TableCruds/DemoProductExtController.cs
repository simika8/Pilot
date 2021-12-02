using Bl;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Controllers.TableCruds;
/// <summary>
/// Demo Product Controller
/// </summary>
[Route("[controller]")]
public class DemoProductExtController : TableCrudController<DemoProductExt>
{
    /// <summary> </summary>
    public DemoProductExtController(NexxtPilotContext db)
    {
        Db = db;
    }
}