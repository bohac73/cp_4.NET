
# cp_4.NET – E-commerce (Microservices)

Solução didática para estudante de .NET, alinhada a microsserviços.

**Estrutura** (`src/`):
- **Catalog.API** – WebAPI (Swagger) + EF Core (Oracle) para **Produtos**.
- **Orders.API** – WebAPI (Swagger) que consome **API pública** com **resiliência** (Polly: retry/timeout/log).
- **ECommerce.Web** – app **MVC** que consome o catálogo.
- **ECommerce.Domain** – entidades e **abstrações** (ex.: `IProductRepository`).
- **ECommerce.Application** – **serviços de aplicação** (ex.: `ProductService`).
- **ECommerce.Infrastructure** – **EF Core** (`AppDbContext`), repositórios e **Migrations** (pasta).

## Requisitos atendidos
- Visual Studio 2022 / .NET 8.
- Arquivo `.sln` incluso.
- 2 WebAPIs (Catalog, Orders), 1 MVC (ECommerce.Web) e 3 libraries (Domain, Application, Infrastructure).
- Swagger ativo nos dois APIs (`/swagger` em Development).
- Integração Oracle via `Oracle.EntityFrameworkCore` no **Catalog.API**.
- Consumo de API pública no **Orders.API** com resiliente `HttpClient` (Polly: retry exponencial + timeout de 10s) e log via Serilog.
- Library com **Migrations**: `src/ECommerce.Infrastructure/Migrations/` com **instruções** para `dotnet ef`.

## Como rodar localmente
> Supondo a pasta do Git: `C:\\Users\\Rodrigo Rios\\Source\\Repos\\cp_4.NET`

1. **Abrir a solução**: `cp_4.NET.sln` no Visual Studio 2022 (ou usar CLI).
2. **Restaurar pacotes** (VS faz automático) ou:  
   ```bash
   dotnet restore
   ```
3. **Configurar Oracle**: edite `src/Catalog.API/appsettings.json`:
   ```json
   "ConnectionStrings": {
     "OracleDb": "User Id=USER;Password=PWD;Data Source=localhost:1521/XEPDB1"
   }
   ```
   > Use um usuário/tabela válidos do seu Oracle (ex.: Oracle XE).

4. **Criar/Aplicar Migrations** (seu Oracle deve estar acessível):
   ```bash
   dotnet tool install --global dotnet-ef

   # do diretório raiz da solução:
   dotnet ef migrations add Init ^
     -p src/ECommerce.Infrastructure/ECommerce.Infrastructure.csproj ^
     -s src/Catalog.API/Catalog.API.csproj

   dotnet ef database update ^
     -p src/ECommerce.Infrastructure/ECommerce.Infrastructure.csproj ^
     -s src/Catalog.API/Catalog.API.csproj
   ```
   > Windows PowerShell/CMD usa `^` para quebra de linha; no bash use `\\`.

5. **Executar serviços** (cada um em um terminal):
   ```bash
   # Catalog.API (Oracle)
   dotnet run --project src/Catalog.API/Catalog.API.csproj

   # Orders.API (consome jsonplaceholder)
   dotnet run --project src/Orders.API/Orders.API.csproj

   # MVC (consome Catalog.API)
   dotnet run --project src/ECommerce.Web/ECommerce.Web.csproj
   ```

6. **URLs padrão** (Development; a porta pode variar):
   - Catalog.API Swagger: `http://localhost:5140/swagger`
   - Orders.API Swagger: `http://localhost:5150/swagger`
   - MVC: `http://localhost:5160/`

> Se as portas diferirem, ajuste `ECommerce.Web/appsettings.json -> Services:CatalogApi` para o endereço do Catalog.API.

## SOLID no código (exemplos)
- **SRP (Single Responsibility Principle)**  
  - `ProductService` trata **use-cases** de produto;  
  - `ProductRepository` trata **acesso a dados**.
- **DIP (Dependency Inversion Principle)**  
  - `ProductService` depende de **`IProductRepository`** (abstração do **Domain**), e não de uma implementação concreta.
- **OCP (Open/Closed Principle)**  
  - `AppDbContext.OnModelCreating` usa configuração que permite **estender mapeamentos** sem modificar o núcleo;  
  - Novos repositórios/serviços implementam **interfaces** sem alterar quem consome.

## Notas
- **Credenciais** e strings de conexão ficam em `appsettings.json` dos projetos **Web** (conforme restrição).
- O **Orders.API** demonstra resiliência: *retry exponencial*, *timeout* (Polly) e log (Serilog).
- A pasta `ECommerce.Infrastructure/Migrations` está pronta e documentada para seus comandos `dotnet ef`.

Boa prática: versionar apenas o **código**, e manter variáveis sensíveis via *User Secrets* ou variáveis de ambiente em produção.
