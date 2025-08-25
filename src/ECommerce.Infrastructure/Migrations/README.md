
# EF Core Migrations (Oracle)

Comandos sugeridos (execute no diretório raiz da solução):
```bash
dotnet tool install --global dotnet-ef

# Criar uma nova migration
dotnet ef migrations add Init   -p src/ECommerce.Infrastructure/ECommerce.Infrastructure.csproj   -s src/Catalog.API/Catalog.API.csproj

# Aplicar no banco Oracle configurado em appsettings.json da Catalog.API
dotnet ef database update   -p src/ECommerce.Infrastructure/ECommerce.Infrastructure.csproj   -s src/Catalog.API/Catalog.API.csproj
```
