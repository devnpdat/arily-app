# Arily – SaaS Quản Lý Thương Lái Nông Sản

Nền tảng SaaS giúp thương lái nông sản quản lý toàn bộ quy trình kinh doanh: thu mua, công nợ, kho, bán hàng, vận chuyển, nhân công. Hoạt động **offline-first**, kiến trúc **multi-tenant**.

---

## Yêu cầu môi trường

| Tool | Phiên bản |
|------|-----------|
| .NET SDK | 9.0+ |
| Node.js | 20.11+ |
| Docker | 24+ |
| dotnet-ef (CLI) | 9.x |

Cài dotnet-ef nếu chưa có:
```bash
dotnet tool install --global dotnet-ef
```

---

## Khởi động nhanh

### 1. Chạy infrastructure (Docker)

```bash
docker compose up -d
```

Các services được khởi động:

| Service | URL / Port | Thông tin |
|---------|-----------|-----------|
| PostgreSQL | `localhost:5432` | DB: `Arily`, User: `root`, Pass: `myPassword` |
| Redis | `localhost:6379` | — |
| Elasticsearch | `localhost:9200` | — |
| Kibana | `localhost:5601` | UI cho Elasticsearch |
| Kafka | `localhost:9092` | — |
| Kafka UI | `localhost:8080` | UI quản lý Kafka topics |
| MinIO | `localhost:9000` / `9001` | User: `arily_minio`, Pass: `arily_minio_secret` |

### 2. Chạy migration & seed data

```bash
dotnet run --project src/Arily.DbMigrator
```

> Log bình thường khi chạy lần đầu: EF Core sẽ báo lỗi `__EFMigrationsHistory` không tồn tại rồi tự tạo — không phải lỗi thật. Chờ đến dòng `Successfully completed all database migrations.`

### 3. Chạy API

```bash
dotnet run --project src/Arily.HttpApi.Host
```

API chạy tại: `https://localhost:44379`
Swagger UI: `https://localhost:44379/swagger`

Tài khoản mặc định: `admin` / `1q2w3E*`

---

## Cấu trúc solution

```
aspnet-core/
├── docker-compose.yml               # Infrastructure: PG, Redis, ES, Kafka, MinIO
├── src/
│   ├── Arily.Domain.Shared          # Enums, constants, error codes
│   ├── Arily.Domain                 # Entities theo modules:
│   │   ├── Catalog/                 #   ProductCategory, Product, ProductGrade, UnitOfMeasure
│   │   ├── Crm/                     #   Farmer, FarmerGarden, Customer
│   │   ├── Collection/              #   CollectionSession, PurchaseOrder, WeighingTicket, PurchaseAdvance
│   │   ├── Inventory/               #   Warehouse, Lot, InventoryLot
│   │   ├── Finance/                 #   LossAdjustmentOrder, FarmerDebtLedger
│   │   └── Sales/                   #   SalesOrder, CustomerDebtLedger
│   ├── Arily.Application.Contracts  # DTOs, IAppService interfaces, Permissions
│   │   └── Crm/Farmers/             #   FarmerDto, CreateUpdateFarmerDto, IFarmerAppService
│   ├── Arily.Application            # App services, AutoMapper
│   │   └── Crm/                     #   FarmerAppService (CRUD + filter)
│   ├── Arily.EntityFrameworkCore    # DbContext, EF Fluent API, Migrations
│   ├── Arily.HttpApi                # Controllers (ABP convention-based)
│   ├── Arily.HttpApi.Host           # Entry point, middleware pipeline
│   └── Arily.DbMigrator             # Migration runner + data seeder
└── test/
    └── Arily.TestBase
```

---

## Migrations

```bash
# Tạo migration mới
dotnet ef migrations add <TênMigration> \
  --project src/Arily.EntityFrameworkCore \
  --startup-project src/Arily.HttpApi.Host

# Xem danh sách migrations
dotnet ef migrations list \
  --project src/Arily.EntityFrameworkCore \
  --startup-project src/Arily.HttpApi.Host
```

Migrations hiện có (đã apply):
- `20260323082423_Initial` — ABP system tables
- `20260323101934_AddMvpBusinessTables` — 19 business tables MVP

---

## Modules & Permissions

Permissions được định nghĩa trong `ArilyPermissions.cs`:

```
Arily.Farmers          → .Create / .Edit / .Delete
Arily.Customers        → .Create / .Edit / .Delete
Arily.CollectionSessions → .Create / .Edit / .Delete
Arily.PurchaseOrders   → .Create / .Edit / .Delete
Arily.SalesOrders      → .Create / .Edit / .Delete
```

---

## Authentication

Dùng **OpenIddict** (OAuth2 / JWT).

Tài khoản mặc định sau khi seed:
- Admin: `admin` / `1q2w3E*`

Swagger UI hỗ trợ đăng nhập OAuth2 trực tiếp.

---

## Tài liệu thiết kế

Xem thư mục `doc/`:

| File | Mô tả |
|------|-------|
| `00-progress-summary.md` | Tổng hợp tiến độ dự án |
| `01-database-schema-trader-saas.md` | Schema 85 bảng |
| `02-system-architecture-trader-saas.md` | Kiến trúc scale 1M user |
| `03-ui-ux-trader-app.md` | User flow mobile |
| `04-postgresql-ddl-core-tables.md` | DDL PostgreSQL |
| `05-erd-diagram-mermaid.md` | ERD Mermaid |
| `06-startup-roadmap-trader-saas.md` | Startup roadmap |
| `07-mvp-build-plan-90-days.md` | MVP 90-day plan |
| `08-full-system-modules-breakdown.md` | Module breakdown |

---

## Liên kết tham khảo

- [ABP Framework Docs](https://abp.io/docs/latest)
- [ABP DDD Architecture](https://abp.io/docs/latest/framework/architecture/domain-driven-design)
- [OpenIddict Config](https://documentation.openiddict.com)
