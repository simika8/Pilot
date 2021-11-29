using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductAdminController : ControllerBase
    {
        [HttpPost(nameof(DatabaseEnsureDeletedMigratePopulate))]
        public async Task DatabaseEnsureDeletedMigratePopulate()
        {
            await DatabaseEnsureDeleted();
            await DatabaseMigrate();
            await DatabasePopulate();
            await DatabaseModifyData();
            
        }

        [HttpPost(nameof(DatabaseEnsureDeleted))]
        public async Task DatabaseEnsureDeleted()
        {
            using var db = new ProductContext();
            await db.Database.EnsureDeletedAsync();
        }

        [HttpPost(nameof(DatabaseMigrate))]
        public async Task DatabaseMigrate()
        {
            using var db = new ProductContext();
            await db.Database.MigrateAsync();
        }


        [HttpPost(nameof(DatabasePopulate))]
        public async Task DatabasePopulate()
        {
            using var db = new ProductContext();
            var p1id = new Guid("00000000-0000-7250-6f64-756374343031");
            var product = new Models.Product()
            {
                Id = p1id,
                Name = "Product 1",
                Active = true,
                Price = 1230,
                CreateTime = DateTime.Parse("2021-01-01T00:12:34.567+01:00"),
                ProductGroup = 12,
                Ext = new ProductExt()
                {
                    SpecialOfferExpiryTime = DateTime.Parse("2021-01-01T00:12:34.567+01:00"),
                    Description = "B short description of Product 1",
                    ExpiryMonths = 3,
                    HighPriority = true,
                    MinimumStock = 2,
                }
            };
            product.AlternateIds.Add(new ProductAlternateId() { AlternateId = Guid.NewGuid() });
            product.AlternateIds.Add(new ProductAlternateId() { AlternateId = Guid.NewGuid() });

            product.History.Add(new ProductHistory() { Expiry = DateTime.Parse("2020-01-01T00:00:00+01:00"), Price = 1210 });
            db.Add(product);
            var p = JsonSerializer.Serialize(product, null);

            var prod2 = new Models.Product()
            {
                Name = "Product 2",
                Price = 567,
                Ext = new ProductExt()
                {
                    Description = "A short description of Product 2",
                }
            };
            db.Add(prod2);

            for (var i = 0; i < 200; i++)
            {
                var px = new Models.Product()
                {
                    Name = "Prodx "+i.ToString(),
                    Price = i,
                    ProductGroup = i+100000,

                };
                db.Add(px);
                /*if (i % 1000 == 0)
                    await db.SaveChangesAsync();*/
            }

            await db.SaveChangesAsync();

        }
        [HttpPost(nameof(DatabaseModifyData))]
        public async Task DatabaseModifyData()
        {
            using var db = new ProductContext();

            var p1 = db.Products
                .Include(x => x.AlternateIds)
                .Include(x => x.Ext)
                .FirstOrDefault(x => x.Name == "Product 1")
                ;

            var pa1 = new ProductAlternateId() { AlternateId = p1.Id};
            db.Add(pa1);
            p1.AlternateIds.Add(pa1);
            p1.Price *= 10;
            p1.Ext.SpecialOfferExpiryTime = DateTime.Now;
            await db.SaveChangesAsync();


            var p2 = db.Products
                .Include(x => x.AlternateIds)
                .Include(x => x.Ext)
                .FirstOrDefault(x => x.Name == "Product 1")
                ;
            ;
        }


    }


}
