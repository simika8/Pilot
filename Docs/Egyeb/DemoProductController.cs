using Bl;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Query.Wrapper;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Models;
using System.Collections;

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
    public IQueryable<Models.DemoProduct> Get()
    {
        //var a = Db.Set<DemoProduct>().OrderBy(x => x.Name.ToLower()).AsQueryable().ToList();
        return Db.Set<DemoProduct>().AsQueryable();
    }
    /*
    [HttpGet]
    public async Task<IEnumerable<Models.DemoProduct>> Get(ODataQueryOptions<Models.DemoProduct> queryOptions, CancellationToken cancellationToken, string? search = "")
    {
        try
        {
            var query = Db.Set<DemoProduct>().OrderBy(x => x.Name.ToLower()).AsQueryable();


            //apply Odata query options
            var dinQuery = (IQueryable<dynamic>)queryOptions.ApplyTo(query);
            ;
            //run Query
            var queryResult = await Task.Run(() =>
            {
                var queryResult = dinQuery.ToArray();
                return queryResult;
            });

            //Extracts Data from MS.odada shits. If we return untyped queryResult, then api result will not contain "@odata.context and "value" parts
            var typedQueryResult = ToTypedCollection<Models.DemoProduct>(queryResult);
            return typedQueryResult;

        }
        catch (System.Threading.Tasks.TaskCanceledException e)
        {
            Console.WriteLine(e.ToString());
            return new List<Models.DemoProduct>();
        }

    }

    public static IEnumerable<T> ToTypedCollection<T>(IEnumerable<dynamic>? queryResult) where T : class
    {
        var res = new List<T>();
        if (queryResult == null)
            return res;

        foreach (var item in queryResult)
        {
            var itemUw = UnWrappObject(item);
            if (itemUw is T)
            {
                var model = itemUw as T;
                res.Add((T)itemUw);
            }
        }

        dynamic? UnWrappObject(object item)
        {
            if (item is null)
                return null;
            if (IsISelectExpandWrapper(item.GetType()))
            {
                ISelectExpandWrapper itemSelectExpandWrapper = (ISelectExpandWrapper)item;
                var instance = itemSelectExpandWrapper.GetType().GetProperty("Instance");
                var instancePropertyValue = itemSelectExpandWrapper?.GetType()?.GetProperty("Instance")?.GetValue(item, null);
                var instancePropertyType = instance?.PropertyType;

                //unshitting v1
                if (instancePropertyValue != null)
                    return instancePropertyValue;

                //unshitting v2
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                var dict = itemSelectExpandWrapper.ToDictionary();
#pragma warning restore CS8602 // Dereference of a possibly null reference.

                var itemValueType = itemSelectExpandWrapper.GetType();

                if (instancePropertyType == null)
                    return null;
                var model = ObjectFromDictionary(instancePropertyType, dict);

                return model;
            }
            return item;
        }


        bool IsISelectExpandWrapper(Type type)
        {
            var interfaces = type.GetInterfaces();
            var containsISelectExpandWrapper = interfaces.Contains(typeof(ISelectExpandWrapper));
            return containsISelectExpandWrapper;
        }

        dynamic? ObjectFromDictionary(Type type, IDictionary<string, object> dict)
        {
            if (type == null)
                return null;
            var result = Activator.CreateInstance(type);
            foreach (var item in dict)
            {
                if (item.Value == null)
                    continue;
                var isCollection = IsCollection(item.Value.GetType());
                var isWrapper = IsISelectExpandWrapper(item.Value.GetType());
                if (isWrapper)
                {
                    var model = UnWrappObject(item.Value);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    type.GetProperty(item.Key)?.SetValue(result, model, null);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                }
                else if (isCollection)
                {
                    var l = new List<string>();
                    l.Add("");
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    var list = type.GetProperty(item.Key)?.GetValue(result);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                    var addmethod = list?.GetType().GetMethod("Add");
                    var listtype = list?.GetType();
                    ;
                    foreach (var enumitemValue in (IEnumerable)item.Value)
                    {
                        var model = UnWrappObject(enumitemValue);
                        if (model != null)
                            addmethod?.Invoke(list, new object[] { model });
                    }
                }
                else
                {
                    type?.GetProperty(item.Key)?.SetValue(result, item.Value, null);
                }
            }
            return result;
            bool IsCollection(Type type)
            {
                var interfaces = type.GetInterfaces();
                var nonStringEnumerable = interfaces.Contains(typeof(IEnumerable))
                    && (type != typeof(string));
                return nonStringEnumerable;
            }
        }

        return res;
    }*/

}