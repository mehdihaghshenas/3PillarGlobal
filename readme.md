# Goal
I try to create a simple azure blob usage, simple query with dapper, using Orm and Unit test
you can see my sample api calls in *WebApplication-API.http*
# multi database support
I created a context that can be used with SQL server and SQLite you can use any of them by selecting your DatabaseType in appsettings.json

# Dapper implementation 
to see Dapper implementation example see InvoiceRepository ./WebApplication-API/DapperRepository/InvoiceRepository.cs

# Ef Core Implementation
# migration
## SQLite
dotnet ef migrations add MigrationName --project ../SQLiteMigrations -- --provider Sqlite
## SqlServer
dotnet ef migrations add MigrationName --project ../SqlServerMigrations -- --provider SqlServer
## update database 
dotnet ef database update

# Unit test example
./WebApplication-APITests1/Services/InvoiceServicesTests.cs
I can also create a unit test for minimal API if you want. but I didn't have time for it.
# Azure Blob sample
you can see my azure blob sample in /3Pillar/WebApplication-API/Services/BlobService.cs
I couldn't finish generating a secure URL sample but I write it in service, I can finish it if you want
