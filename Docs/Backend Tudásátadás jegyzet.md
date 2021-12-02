# UserStories

## 001 NexxtCore Container létrehozása
### #1 Architektúra terv átnézése
https://wiki.adg.hu/pages/viewpage.action?pageId=32604288

### #2 Git clone
```
cd c:\Develop
git clone https://devadg@dev.azure.com/devadg/NeXxt/_git/NeXxt
```

### #3 NexxtCore projekt létrehozása
```
cd c:\Develop\Nexxt
dotnet new sln
dotnet new gitignore
dotnet new webapi -o NexxtCore
dotnet sln add NexxtCore
cd c:\Develop\Nexxt\NexxtCore
```
- git commit

### #4 Azure publish
- Resource Group : RgPilot
- App service plan : AspPilot
- App service (https://portal.azure.com/#create/Microsoft.WebSite): PilotNexxtCore
- publish profile létrehozás + publish
- kipróbálni: http://pilotnexxtcore.azurewebsites.net/weatherforecast
- git commit

## 002 NexxtCore finomhangolás
### #5 Azure app service Development beállítás
- Azure / PilotNexxtCore / Konfiguráció / alkalmazás beállítások / új: ASPNETCORE_ENVIRONMENT : Development
- mentés

### #6 Swagger root beállítás
```csharp
app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "NexxtCore v1"); c.RoutePrefix = string.Empty; });
```
- launchSettings.json: "launchUrl": "",
- git commit


### #7 Cors develop beállítás
```csharp
builder.Services.AddCors();

if (app.Environment.IsDevelopment())
{
    app.UseCors(options => {
        options.AllowAnyOrigin();
        options.AllowAnyHeader();
        options.AllowAnyMethod();
    });
}
```
### #8 file scoped namespace
- solution / add / new editorconfig / code style / namespace declaration / file scoped : warning
- git commit

## 003 Modellek, Controller létrehozása
### #9 Models class library létrehozása
- sollution / Add / new project / Class Library : Models
- Class1 törlése

- git commit

-------------   

### #10 Modellek létrehozása
- Modellek kétrehozása (models1 minta fájlok másolása)
- Elnevezési konvenciók
    - Angol név. szótár: https://docs.google.com/spreadsheets/d/176d3njFe7a4SQwJaHJAKYb6gOkyeUlWRQBXABLP23Ec/edit#gid=0
    - Nincsenek rövidítések, minden szót teljesen kiírunk
    - Betűszavak létrehozása: pl "TTT kód" -> TttCode; Universal Cash Register Framework -> Ucrf.
    - Demo kezdetű nevek. 
    - egyes szám: Modell class, controller-eknél; 
    - többes szám: entity dbset
- Enumok használata (konstans értékekkel)
- Hossz korlátozás mellőzése, ahol nem 100% hogy nem fog változni a hossz
- Rekurzív navigation property-k kerülése
- Id elnevezések
- git commit
-------------

### #11 DemoProduct Odata Controller létrehozása
- OData NxT
- sollution / Add / new project / Class Library : Controllers
- Class1 törlése
- NexxtCore / Add / Project reference / Controllers
- Controllers / Add / Project reference / Models



- Microsoft.AspNetCore.OData (8.0.4) NuGet csomag telepítése Controllers projekthez
- Swashbuckle.AspNetCore (6.2.3) NuGet csomag telepítése Controllers projekthez
- Edm model létrehozása (#11\ODataEdmModel.cs másolása Controllers-be)
- Controller létrehozása (#11\DemoProductController.cs másolása Controllers-be)

  <details><summary>#11/Program.cs bővítés </summary>

  ```csharp
  builder.Services.AddControllers()
      .AddOData(opt => opt.AddRouteComponents("odata", Controllers.ODataEdmModel.GetEdmModel()).Filter().Expand().Select().Count().SkipToken().OrderBy().SetMaxTop(500));
  ```
  </details>

- Weatherforecast.* törlés NexxtCore/Controllers mappát törölni

- teszt (portot átírni): https://localhost:7078/odata/DemoProduct?$top=1
- git commit

------------------

### #12 Modellek, Controllerek kommentezése
- Kommentelési elvárás
    - angol nyelvű kommentek kötelezők a programban :( 
    - Kötelező kommentelni az apiban megjelenő összes információt. (Model, Controller)
    - Ha egy függvény/Osztály/Property nincs kommentelve, akkor warning. Warning nem maradhat a programban.
    - "/// summary" módon kommenteljük ezeket. Egyéb kommenteket csak a normál módon: "//" vagy "/**/"
    - Az Enumok értékeit duplán kell kommentelni: egyszer az érték fölött, egyszer a típus definíció fölött (ide az összes értéket be kell másolni)

<details><summary>#12/Models.csproj kötelező kommentelés swagger számára</summary>

```xml
  <PropertyGroup Condition="'$(Configuration)|$(Platform)'=='Debug|AnyCPU'">
    <DocumentationFile>Models.xml</DocumentationFile>
  </PropertyGroup>
```
</details>

<details><summary>#12/Controllers.csproj kötelező kommentelés swagger számára</summary>

```xml
  <PropertyGroup Condition="'$(Configuration)|$(Platform)'=='Debug|AnyCPU'">
    <DocumentationFile>Controllers.xml</DocumentationFile>
  </PropertyGroup>
```
</details>

<details><summary>#12/Program.cs bővítés, hogy Swagger megjelenítse a kommenteket</summary>

```csharp
builder.Services.AddSwaggerGen(x =>
    {
        x.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Controllers.xml"), includeControllerXmlComments: true);
        x.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Models.xml"), includeControllerXmlComments: true);
    });
```
</details>

- Modell kommentek kitöltése (#12/Demo*.cs, ODataEdmModel.cs fájlok másolása a Controllers, Models könyvtárakba)
- *.xml re copy always beállítása
- Todo: Generált xml fájlok ne csak akkor frissüljenek, ha törlöm őket!
- Todo: xml-eket git ignoráljuk vagy sem?
- Teszt (kommenteket ellenőrizni): https://localhost:7078/ 
- git commit

### #13 Azure publish
 
<details><summary>#12/Program.cs hekk: beállítani, hogy xmlt is publisholjuk</summary>

```csharp
builder.Services.AddSwaggerGen(x =>
    {
        x.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Controllers.xml"), includeControllerXmlComments: true);
        x.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Models.xml"), includeControllerXmlComments: true);
    });
```
</details>


<details><summary>#12/models.csproj hekk: beállítani, hogy xmlt is publisholjuk</summary>

```xml

  <ItemGroup>
    <None Update="Models.xml">
      <CopyToOutputDirectory>Always</CopyToOutputDirectory>
    </None>
  </ItemGroup>
```
</details>

- publish
- Teszt (kommenteket ellenőrizni): http://pilotnexxtcore.azurewebsites.net/index.html
- git commit




## 004 Adatbázis kapcsolat

### #14 Bl class library létrehozása
- sollution / Add / new project / Class Library : Bl
- Class1 törlése


- NuGet csomag telepítése: Npgsql.EntityFrameworkCore.PostgreSQL (6.0.0)
- NuGet csomag telepítése: Microsoft.EntityFrameworkCore.Design (6.0.0)

- Controllers / Add / Project reference / Bl
- Bl / Add / Project reference / ```Models```

- git commit

- Ef core telepítése (futtatni Bl könyvtárban)
    
    ```dotnet tool install --global dotnet-ef```

    ```dotnet tool update --global dotnet-ef```

    ```dotnet add package Microsoft.EntityFrameworkCore.Design```

### #15 Kapcsolati stringek beállítása
- Connection string beállítása helyi gépen local db-re: 
    
    ```setx POSTGRESQLCONNSTR_NexxtRdb "Host=localhost;Username=somebody;Password=somepwd;```
    
    Felhasználónevet, jelszót cseréni kell arra, amit telepítéskor megadtunk. 
    Visual studiot újra kell indítani helyi environment variable változtatása után!
- Connection string beállítása helyi gépen azure db-re 
    
    ```setx POSTGRESQLCONNSTR_NexxtRdb "Host=nexxtpilotpgsql.postgres.database.azure.com;SSL Mode=Require;Trust Server Certificate=true;Username=develop;Password=gfdmkntrejG434fFDs;"```

    Visual studiot újra kell indítani helyi environment variable változtatása után!
- Connection string beállítása azire app service-ben
    
    Azure / PilotNexxtCore / Konfiguráció / Kapcsolati sztringek / új:

    Név: ```NexxtRdb```
    
    Érték: ```Host=nexxtpilotpgsql.postgres.database.azure.com;SSL Mode=Require;Trust Server Certificate=true;Username=develop;Password=gfdmkntrejG434fFDs;```

    Típus: ```PostgreSQL```

    Mentés, app service plan újraindítása

- Program a kapcsolati string végére még oda fogja fűzni, hogy ```;Database=NexxtPilot```

### #16 DbContext létrehozása
- #16\NexxtPilotContext.cs másolása Bl-be
- git commit

### #17 Init migration létrehozása
- bl könyvtárban futtatni: ```dotnet ef migrations add Init```

### #18 Admin controller létrehozása
Itt lesznek azok a fejelsztéshez szükséges kódok amikkel létrehozzuk letöröljük, feltöltjük az adatbázist.
- #18\NexxtPilotAdminController.cs másolása
- db létrehozása: ```/api/NexxtPilotAdmin/DatabaseEnsureDeletedMigratePopulate``` api végpont futtatása swaggerből
- teszt pgadmin: ```select * from "DemoProducts" p join "DemoProductExt" e on p."Id" = e."ProductId"```
- git commit

### #19 Db context továbbfejlesztése
kisbetűs db name convention, többesszámú táblanév convention, sql naplózás
- Nuget csomagok telepítése Bl-be:
    - ```EFCore.NamingConventions``` (6.0.0)
    - ```Pluralize.NET``` (Sarath KCM 1.0.2)
    - ```Microsoft.Extensions.Logging.Console``` (6.0.0)
- NexxtCore indítót átkapcsolni IIS expressre, hogy ne command ablakba naplózzon.
- NexxtCore / proprerties / launchSettings.json / "profiles" / "IIS Express" / "launchUrl": "",
- Context tuvábbfejlesztése kisbetű, többesszám, naplózás irányba. (Másolni Bl-be:)
    - #19\NexxtPilotContext.cs 
    - #19\ModelBuilderExtensions.cs 
- Migráció újragenerálás: Bl könyvtárban futtatni: 
    - ```rmdir /S /Q "Migrations"```
    - ```dotnet ef migrations add Init```
- teszt
    - swagger: ```DatabaseEnsureDeletedMigratePopulate``` (VS / Output / NexxtCore - ASP.NET Core Web Server- sqleket megnézni)
    - pgadmin:  ```select * from DemoProducts p join DemoProductExts e on p.Id = e.ProductId```

- git commit

### #20 Random adatok feltöltése
- #20\RandomProduct.cs másolása Blbe.
- NexxtPilotAdminController / DatabasePopulate átírása 
    
    (másolás: #20 NexxtPilotAdminController.cs )
- teszt:
    - swagger: ```DatabaseEnsureDeletedMigratePopulate``` (VS / Output / NexxtCore - ASP.NET Core Web Server- sqleket megnézni)
    - pgadmin:  ```select * from DemoProducts p join DemoProductExts e on p.Id = e.ProductId```

- git commit

### #21 DemoProduct controller Db kapcsolat
- DbContext dependency injection bekapcsolása Program.cs ben: ```builder.Services.AddDbContext<Bl.NexxtPilotContext>();```
- Controller átírása (DbContext Di használat, Iqueryable visszaadás ): #21\DemoProductController.cs 
- teszt Postman: ```https://localhost:44339/odata/DemoProduct?$top=10&$select=Name&$expand=Stocks($select=StoreId,Quantity)```
- git commit

## 005 Kompozit lekérdezések

### #22 VmDemoProductDemoInventoryStockController
- controller létrehozás
    (#22\VmDemoProductDemoInventoryStockController.cs)
- ViewModel létrehozás
    (#22\VmDemoProductDemoInventoryStock.cs)
- DbContext bővítés DemoInventoryStocks tábla felsorolásával 
    ```public DbSet<Models.DemoInventoryStock> DemoInventoryStocks { get; set; } = null!;```
- Odata Edm model bővítés
    ```builder.EntitySet<Models.VmDemoProductDemoInventoryStock>(nameof(Models.VmDemoProductDemoInventoryStock));```
- teszt Postman: ```https://localhost:44339/odata/VmDemoProductDemoInventoryStock?storeId=00000000-0000-0000-0053-746f72652030&$top=10&$expand=DemoProduct($select=Name),DemoInventoryStock($select=Quantity)```
- git commit

### #23 VmDemoProductDemoInventoryStockController szerver oldali általános szűrő
- controller upgrade
    (#23\VmDemoProductDemoInventoryStockController.cs)
- teszt Postman: ```https://localhost:44339/odata/VmDemoProductDemoInventoryStock?storeId=00000000-0000-0000-0053-746f72652030&$top=10&$expand=DemoProduct($select=Name),DemoInventoryStock($select=Quantity)&search=4```
- teszt Postman: ```https://localhost:44339/odata/VmDemoProductDemoInventoryStock?storeId=00000000-0000-0000-0053-746f72652030&$top=10&$expand=DemoProduct($select=Name),DemoInventoryStock($select=Quantity)&search=Ab``` (Aawwby gim apee (131): Quantity = 2)
- teszt Postman: ```https://localhost:44339/odata/VmDemoProductDemoInventoryStock?storeId=00000000-0000-0000-0053-746f72652030&$top=10&$expand=DemoProduct($select=Name;$expand=Stocks($select=StoreId,Quantity)),DemoInventoryStock($select=Quantity)&search=4```
- git commit
- Azure publish

## 006 CRUD
### #24 DemoProductController bővítése Create, Update, Delete képességekkel
- DemoProductController bővítése (#24\DemoProductController.cs)
- teszt #24\Postman tesztek.md": 

postnál ledöglik:     
```  
    Hiba: Cannot write DateTime with Kind=Unspecified to PostgreSQL type 'timestamp with time zone', only UTC is supported. Note that it's not possible to mix DateTimes with different Kinds in an array/range. See the Npgsql.EnableLegacyTimestampBehavior AppContext switch to enable legacy behavior. at Npgsql.Internal.TypeHandlers.DateTimeHandlers.TimestampTzHandler.ValidateAndGetLength(DateTime value, NpgsqlParameter parameter)
``` 

- javítás 
    - #24\NexxtPilotContext.cs 
    - #24\RandomProduct.cs
- dotnet ef migrations add 24
- teszt újra:
    - swagger / DatabaseEnsureDeletedMigratePopulate
    - postman tesztek újra futtatáasa (patch, put közötti különbségek)

- azure publish, git commit

### #25 Új CRUD controllerek a többi táblához (generikus controller)
- #25\TableCrudController.cs másolása. összehasonlítani DemoProductController-rel.
- DemoProductController áthelyezése TableCruds mappába, #25\TableCruds\*.* másolása.

- Postman teszt: ```https://localhost:44339/odata/DemoProductExt?$top=10```
- Postman teszt: ```https://localhost:44339/odata/DemoInventoryStock?$top=10```
- Azure publish, git Commit
