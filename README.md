
# cp_4.NET – E-commerce (Microservicos)

**Estrutura** (`src/`):
- **Catalog.API** – WebAPI (Swagger) + EF Core (Oracle) para **Produtos**.
- **Orders.API** – WebAPI (Swagger) que consome **API pública** com **resiliência** (Polly: retry/timeout/log).
- **ECommerce.Web** – app **MVC** que consome o catálogo.
- **ECommerce.Domain** – entidades e **abstrações** (ex.: `IProductRepository`).
- **ECommerce.Application** – **serviços de aplicação** (ex.: `ProductService`).
- **ECommerce.Infrastructure** – **EF Core** (`AppDbContext`), repositórios e **Migrations** (pasta).

## Requisitos
- 2 WebAPIs (Catalog, Orders), 1 MVC (ECommerce.Web) e 3 libraries (Domain, Application, Infrastructure).
- Swagger ativo nos dois APIs (`/swagger` em Development).
- Integração Oracle via `Oracle.EntityFrameworkCore` no **Catalog.API**.
- Consumo de API pública no **Orders.API** com resiliente `HttpClient` (Polly: retry exponencial + timeout de 10s) e log via Serilog.
- Library com **Migrations**: `src/ECommerce.Infrastructure/Migrations/` com **instruções** para `dotnet ef`.

**URLs padrão**:
   - Catalog.API Swagger: `http://localhost:5140/swagger`
   - Orders.API Swagger: `http://localhost:5150/swagger`
   - MVC: `http://localhost:5160/`

## SOLID no código:
- **SRP (Single Responsibility Principle)**  
  - `ProductService` trata **use-cases** de produto;  
  - `ProductRepository` trata **acesso a dados**.
- **DIP (Dependency Inversion Principle)**  
  - `ProductService` depende de **`IProductRepository`** (abstração do **Domain**), e não de uma implementação concreta.
- **OCP (Open/Closed Principle)**  
  - `AppDbContext.OnModelCreating` usa configuração que permite **estender mapeamentos** sem modificar o núcleo;  
  - Novos repositórios/serviços implementam **interfaces** sem alterar quem consome.

## Notas
- **Credenciais** e strings de conexão ficam em `appsettings.json` dos projetos **Web**.
