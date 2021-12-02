using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace Controllers;

[Route("[controller]")]
public class DemoProductController : ODataController
{
    [HttpGet]
    [EnableQuery]
    public IEnumerable<Models.DemoProduct> Get()
    {
        var res = new List<Models.DemoProduct>();
        res.Add(new Models.DemoProduct() { Name = "Cikk1" });
        return res;
    }
}