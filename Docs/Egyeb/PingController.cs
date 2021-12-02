using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PingController : ControllerBase
    {

        // GET: api/Sales?productId=3fa85f64-5717-4562-b3fc-2c963f66afa6&minTimeMs=3000&maxTimeMs=3000
        [HttpGet()]
        public async Task<ActionResult> Ping()
        {
            await Task.Delay(10);
            return Ok();
        }

    }
}
