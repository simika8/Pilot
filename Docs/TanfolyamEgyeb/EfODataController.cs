using Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.ModelBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Controllers
{

    public abstract class EfODataController<T> : ODataController where T : class
    {
        protected Microsoft.OData.Edm.IEdmModel EdmModel { get; set; }
        protected DbContext Db { get; set; }

        /// <summary>
        /// Apply Server Side Filter (must be well indexed, can affect multiple columns, can have unique logic or text search)
        /// </summary>
        protected abstract IQueryable<T> ApplySearchFilter(IQueryable<T> q1, string? keywords = "");

        [HttpGet]
        public async Task<IEnumerable<T>> Get(ODataQueryOptions<T> queryOptions, CancellationToken cancellationToken, string? search = "")
        {
            try
            {
                var query = Db.Set<T>().AsNoTracking();

                //apply model specific server side $search filter
                var queryString = HttpContext.Request.QueryString.ToString();
                var keywords = Others.GetSearchField(queryString);
                if (!string.IsNullOrEmpty(keywords))
                    query = ApplySearchFilter(query, keywords);


                //proba arra, hogy kiszedjem a queryoptionsból az adatbázison kívüli expandokat
                /*HttpRequest request = new Microsoft.AspNetCore.Http.Internal.DefaultHttpRequest(_httpContext)
                {
                    Method = "GET",
                    QueryString = new QueryString(requestQueryString)
                };
                ODataModelBuilder modelBuilder = new ODataConventionModelBuilder();
                modelBuilder.EntitySet<T>("Product");
                var request = new System.Net.Http.HttpRequestMessage(HttpMethod.Get, HttpContext.Request.QueryString.ToString());
                var options = new ODataQueryOptions<T>(new ODataQueryContext(EdmModel, typeof(T)), request.HttpRe);
                /**/
                

                //apply Odata query options
                var dinQuery = (IQueryable<dynamic>)queryOptions.ApplyTo(query);

                //run Database Query
                var queryResult = await dinQuery.ToArrayAsync(cancellationToken);

                //ToTypedCollection hack
                //if get results IEnumerable<dynamic> then odata controller dont wraps results to odata scpeficic result {"@odata.context":... 
                //Microsoft.AspNetCore.OData.Query.ApplyTo loose the type, and returns IQueryable<dynamic>. This cannot cast to IQueyable<T>
                //because the inner data is wrapped ISelectExpandWrapper when odata is expanded.

                var typedQueryResult = queryResult.ToTypedCollection<T>();
                return typedQueryResult;
            }
            catch (System.Threading.Tasks.TaskCanceledException e)
            {
                Console.WriteLine(e.ToString());
                //return new List<dynamic>().AsQueryable();
                return new List<T>();
            }
            


        }
        /*public async Task<IEnumerable<dynamic>> Get(ODataQueryOptions<T> queryOptions, CancellationToken cancellationToken, string? search = "")
        {
            try
            {
                var query = Db.Set<T>().AsNoTracking();

                //apply model specific server side $search filter
                var keywords = Others.GetSearchField(HttpContext.Request.QueryString.ToString());
                if (!string.IsNullOrEmpty(keywords))
                    query = ApplySearchFilter(query, keywords);

                //apply Odata query options
                var dinQuery = (IQueryable<dynamic>)queryOptions.ApplyTo(query);

                //run Database Query
                var queryResult = await dinQuery.ToArrayAsync(cancellationToken);

                return queryResult;
            }
            catch (System.Threading.Tasks.TaskCanceledException e)
            {
                Console.WriteLine(e.ToString());
                return new List<dynamic>().AsQueryable();
                //return new List<Models.Product>();
            }
        }*/

        [HttpPut]
        public async Task<IActionResult> Put(Guid key, [FromBody] Microsoft.AspNetCore.OData.Deltas.Delta<T> delta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = await Db.FindAsync<T>(key);
            if (entity == null)
            {
                return NotFound();
            }

            delta.Put(entity);
            try
            {
                await Db.SaveChangesAsync();
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException)
            {
                throw;
            }
            var res = Updated(entity);
            return res;
        }
        [HttpPatch]
        public async Task<IActionResult> Patch([Microsoft.AspNetCore.OData.Formatter.FromODataUri] Guid key, Microsoft.AspNetCore.OData.Deltas.Delta<T> delta)
        {


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = await Db.FindAsync<T>(key);
            if (entity == null)
            {
                return NotFound();
            }

            delta.Patch(entity);    

            try
            {
                await Db.SaveChangesAsync();
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException)
            {
                throw;
            }
            var res = Updated(entity);
            return res;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] T entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Db.Set<T>().Add(entity);
            await Db.SaveChangesAsync();
            return Created(entity);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid key)
        {
            var entity = await Db.FindAsync<T>(key);
            if (entity == null)
            {
                return NotFound();
            }

            Db.Remove(entity);
            await Db.SaveChangesAsync();
            return Ok();
        }

    }
}
