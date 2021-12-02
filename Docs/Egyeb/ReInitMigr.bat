rmdir /S /Q "Migrations"
dotnet ef migrations add Init
rem dotnet ef database update
pause