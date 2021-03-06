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




