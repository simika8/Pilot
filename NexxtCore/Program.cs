using Microsoft.AspNetCore.OData;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddOData(opt => opt.AddRouteComponents("odata", Controllers.ODataEdmModel.GetEdmModel()).Filter().Expand().Select().Count().SkipToken().OrderBy().SetMaxTop(500));


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
    {
        if (File.Exists(Path.Combine(AppContext.BaseDirectory, "Controllers.xml")))
            x.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Controllers.xml"), includeControllerXmlComments: true);
        x.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Models.xml"), includeControllerXmlComments: true);
    });
builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "NexxtCore v1"); c.RoutePrefix = string.Empty; });
}
if (app.Environment.IsDevelopment())
{
    app.UseCors(options => {
        options.AllowAnyOrigin();
        options.AllowAnyHeader();
        options.AllowAnyMethod();
    });
}
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
