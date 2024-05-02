# Goal
I try to create a simple azure blob usage, simple query with dapper,  and using Orm and Unit test
# multi database support
I create a context that can use with SQL server and SQLite you can use any of them by selection your DatabaseType in appsettings.json

# Dapper implementation 
to see Dapper implementation example see InvoiceRepository ./WebApplication-API/DapperRepository/InvoiceRepository.cs

# Ef Core Implementation
# migration
## SQLite
dotnet ef migrations add MigrationName --project ../SQLiteMigrations -- --provider Sqlite
## SqlServer
dotnet ef migrations add MigrationName --project ../SqlServerMigrations -- --provider SqlServer

# Unit test example
./WebApplication-APITests1/Services/InvoiceServicesTests.cs
I can also create unit test for minimal API if you want. but I didn't have time for it.
# Azure Blob sample
you can see my azure blob sample in /3Pillar/WebApplication-API/Services/BlobService.cs
I couldn't finish generate secure url sample but write it in service, I can finish it if you want
