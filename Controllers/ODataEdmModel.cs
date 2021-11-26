
namespace Controllers; 
public static class ODataEdmModel
{
    public static Microsoft.OData.Edm.IEdmModel GetEdmModel()
    {
        var builder = new Microsoft.OData.ModelBuilder.ODataConventionModelBuilder();


        builder.EntitySet<Models.DemoProduct>(nameof(Models.DemoProduct));
        builder.EntitySet<Models.DemoProductExt>(nameof(Models.DemoProductExt));
        builder.EntitySet<Models.DemoInventoryStock>(nameof(Models.DemoInventoryStock));

        return builder.GetEdmModel();

    }


}
