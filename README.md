# Identity Server Solution
## To start the project follow below

1. You need to create EF Migrations:
    - `dotnet ef migrations add init -c AppDbContext -o Data/Migrations/AppMigrations`
    - `dotnet ef migrations add InitialIdentityServerPersistedGrantDbMigration -c PersistedGrantDbContext -o Data/Migrations/IdentityServer/PersistedGrantDb`
    - `dotnet ef migrations add InitialIdentityServerConfigurationDbMigration -c ConfigurationDbContext -o Data/Migrations/IdentityServer/ConfigurationDb`

2. Then you have to init database:
    - `dotnet ef database update -c AppDbContext`
    - `dotnet ef database update -c PersistedGrantDbContext`
    - `dotnet ef database update -c ConfigurationDbContext`

3. Finally you can run the project, enjoy :)