# Arily – Tổng hợp tiến độ dự án

> Cập nhật lần cuối: 23/03/2026 — 17:50

---

## 1. Tổng quan dự án

**Arily** là nền tảng **SaaS quản lý thương lái nông sản** tại Việt Nam.

Mục tiêu:
- Giúp thương lái quản lý thu mua, công nợ, kho, bán hàng, vận chuyển, nhân công
- Hoạt động **offline-first** (ngoài vườn, không cần mạng)
- Kiến trúc **multi-tenant SaaS**, scale lên 1 triệu user

---

## 2. Tài liệu thiết kế đã hoàn thành

| File | Nội dung |
|------|----------|
| `ProductPlanning.md` | Toàn bộ tính năng, roadmap, chi phí, mô hình kinh doanh |
| `01-database-schema-trader-saas.md` | Schema 85 bảng, 10 phân hệ, chuẩn multi-tenant |
| `02-system-architecture-trader-saas.md` | Kiến trúc scale 1M user: API Gateway, Kafka, Redis, Elasticsearch |
| `03-ui-ux-trader-app.md` | User flow thương lái ngoài vườn |
| `04-postgresql-ddl-core-tables.md` | DDL PostgreSQL cho các bảng cốt lõi |
| `05-erd-diagram-mermaid.md` | ERD toàn hệ thống bằng Mermaid |
| `06-startup-roadmap-trader-saas.md` | Lộ trình startup từ MVP đến scale |
| `07-mvp-build-plan-90-days.md` | Kế hoạch build MVP 90 ngày, 12 sprint |
| `08-full-system-modules-breakdown.md` | Phân rã 15 modules, API contracts, event map |

---

## 3. Thiết kế hệ thống đã chốt

### Tech Stack

| Layer | Technology |
|-------|------------|
| Backend | .NET 9 + ABP Framework 9.2 |
| Database | PostgreSQL |
| Cache | Redis |
| Search | Elasticsearch |
| Event Queue | Kafka |
| Mobile | Flutter (offline SQLite + sync) |
| File Storage | AWS S3 / MinIO |
| Infra | Docker + Kubernetes + GitHub Actions |

### Kiến trúc

- Bắt đầu bằng **Modular Monolith**
- Tách dần thành **Microservices** khi scale
- **Event-driven** qua Kafka (outbox/inbox pattern)
- **Offline sync** qua Sync API + Message Queue + idempotency key

### 15 Modules đã thiết kế

| # | Module | Trạng thái thiết kế | Trạng thái code |
|---|--------|---------------------|-----------------|
| 1 | Identity & Access | Hoàn thành | Dùng ABP built-in |
| 2 | Tenant Management | Hoàn thành | Dùng ABP built-in |
| 3 | CRM (Farmers & Customers) | Hoàn thành | Entities + CRUD Farmers ✅ |
| 4 | Collection (Thu mua) | Hoàn thành | Entities ✅, AppService chưa làm |
| 5 | Quality Control (Hao hụt/QC) | Hoàn thành | Chưa làm |
| 6 | Inventory (Kho) | Hoàn thành | Entities ✅, AppService chưa làm |
| 7 | Sales (Bán hàng) | Hoàn thành | Entities ✅, AppService chưa làm |
| 8 | Finance (Tài chính/Sổ cái) | Hoàn thành | Entities ✅, AppService chưa làm |
| 9 | Workforce (Nhân công) | Hoàn thành | Chưa làm |
| 10 | Logistics (Vận chuyển) | Hoàn thành | Chưa làm |
| 11 | Notification | Hoàn thành | Chưa làm |
| 12 | File & Media | Hoàn thành | Chưa làm |
| 13 | Reporting & Analytics | Hoàn thành | Chưa làm |
| 14 | Offline Sync | Hoàn thành | Chưa làm |
| 15 | System & Audit | Hoàn thành | Dùng ABP built-in |

---

## 4. Database Schema

Tổng **85 bảng** thiết kế. **19 bảng MVP** đã được tạo trong database.

### Bảng đã có trong database

| Module | Tables | Migration |
|--------|--------|-----------|
| ABP System | AbpUsers, AbpRoles, AbpTenants, AbpAuditLogs, AbpOpenIddict*, AbpSettings... | `Initial` |
| Catalog | AppProductCategories, AppProducts, AppProductGrades, AppUnitOfMeasures | `AddMvpBusinessTables` |
| CRM | AppFarmers, AppFarmerGardens, AppCustomers | `AddMvpBusinessTables` |
| Collection | AppCollectionSessions, AppPurchaseOrders, AppPurchaseOrderDetails, AppWeighingTickets, AppPurchaseAdvances | `AddMvpBusinessTables` |
| Inventory | AppWarehouses, AppLots, AppInventoryLots | `AddMvpBusinessTables` |
| Finance | AppLossAdjustmentOrders, AppFarmerDebtLedgers | `AddMvpBusinessTables` |
| Sales | AppSalesOrders, AppCustomerDebtLedgers | `AddMvpBusinessTables` |

### Chuẩn thiết kế áp dụng

- UUID primary key
- `TenantId` trên tất cả bảng nghiệp vụ (multi-tenant filter tự động)
- Soft delete: `IsDeleted`, `DeletedAt`, `DeletedBy` (ABP FullAuditedAggregateRoot)
- Tiền: `numeric(18,2)` | Khối lượng: `numeric(18,3)` | Tỷ lệ: `numeric(8,4)`
- Composite index: `(TenantId, Code)` unique, `(TenantId, Status)`, `(TenantId, FarmerId)`...

---

## 5. Codebase hiện tại

### Trạng thái hệ thống

| Thành phần | Trạng thái |
|------------|-----------|
| Docker infrastructure | ✅ Đang chạy (PG, Redis, ES, Kafka, MinIO) |
| Database migration | ✅ Đã apply — 2 migrations, 19 business tables |
| API server | ✅ Chạy được tại `https://localhost:44379` |
| Swagger UI | ✅ `https://localhost:44379/swagger` |
| Seed data | ✅ Admin account seeded |

### Solution structure

```
aspnet-core/
├── docker-compose.yml               ✅ PostgreSQL, Redis, ES, Kafka, MinIO
├── src/
│   ├── Arily.Domain.Shared
│   │   └── Enums/                   ✅ FarmerStatus, CollectionSessionStatus,
│   │                                   PurchaseOrderStatus, LotStatus,
│   │                                   SalesOrderStatus, DebtLedgerType, CommonStatus
│   ├── Arily.Domain
│   │   ├── Catalog/                 ✅ ProductCategory, Product, ProductGrade, UnitOfMeasure
│   │   ├── Crm/                     ✅ Farmer, FarmerGarden, Customer
│   │   ├── Collection/              ✅ CollectionSession, PurchaseOrder, PurchaseOrderDetail,
│   │   │                               WeighingTicket, PurchaseAdvance
│   │   ├── Inventory/               ✅ Warehouse, Lot, InventoryLot
│   │   ├── Finance/                 ✅ LossAdjustmentOrder, FarmerDebtLedger
│   │   └── Sales/                   ✅ SalesOrder, CustomerDebtLedger
│   ├── Arily.Application.Contracts
│   │   ├── Permissions/             ✅ ArilyPermissions (Farmers, Customers,
│   │   │                               CollectionSessions, PurchaseOrders, SalesOrders)
│   │   └── Crm/Farmers/             ✅ FarmerDto, CreateUpdateFarmerDto,
│   │                                   GetFarmerListInput, IFarmerAppService
│   ├── Arily.Application
│   │   ├── Crm/                     ✅ FarmerAppService (CRUD + filter)
│   │   └── ArilyApplicationAutoMapperProfile.cs  ✅ Farmer mapping
│   ├── Arily.EntityFrameworkCore
│   │   ├── ArilyDbContext.cs        ✅ 19 DbSet business + ABP system tables
│   │   ├── ArilyDbContextModelBuilderExtensions.cs  ✅ Fluent API toàn bộ
│   │   └── Migrations/
│   │       ├── 20260323082423_Initial.cs          ✅ applied
│   │       └── 20260323101934_AddMvpBusinessTables.cs  ✅ applied
│   ├── Arily.HttpApi.Host
│   │   ├── appsettings.json         ✅
│   │   └── appsettings.Development.json  ✅
│   └── Arily.DbMigrator             ✅ Migration runner (fix content root)
└── doc/                             ✅ 9 tài liệu thiết kế + file này
```

### Commits

```
0ee5391 Initial ABP 9.2 monolith setup — net9, PostgreSQL, API-only, no UI theme
```

---

## 6. Kế hoạch MVP (90 ngày)

| Sprint | Tuần | Nội dung | Trạng thái |
|--------|------|----------|-----------|
| 1–2 | 1–2 | Setup nền tảng, docker, entities, migrations, API chạy được | ✅ Hoàn thành |
| 3–4 | 3–4 | CRM Module: Farmers CRUD, Customers CRUD | 🔄 Đang làm (Farmers ✅) |
| 5–6 | 5–6 | Collection Module: Session, PurchaseOrder, WeighingTicket | Chưa bắt đầu |
| 7–8 | 7–8 | Debt Management: FarmerDebtLedger, PurchaseAdvance | Chưa bắt đầu |
| 9–10 | 9–10 | Inventory Module: Lot, InventoryLot, Warehouse | Chưa bắt đầu |
| 11 | 11 | Sales Module: SalesOrder, CustomerDebtLedger | Chưa bắt đầu |
| 12 | 12 | Production readiness: logging, monitoring, security | Chưa bắt đầu |

---

## 7. Việc cần làm tiếp theo

### Sprint 3–4 (CRM hoàn thiện)
- [ ] `CustomerAppService` — CRUD khách hàng
- [ ] `FarmerGardenAppService` — quản lý vườn
- [ ] Unit test cho FarmerAppService

### Sprint 5–6 (Collection)
- [ ] `CollectionSessionAppService` — tạo/mở/đóng phiên thu mua
- [ ] `PurchaseOrderAppService` — tạo đơn mua, phiếu cân, tạm ứng
- [ ] Domain events: `purchase.created`

### Hạ tầng còn lại
- [ ] Cấu hình Redis cache cho queries thường dùng
- [ ] Setup Kafka producer/consumer cơ bản
- [ ] CI/CD pipeline (GitHub Actions)

---

## 8. Mô hình kinh doanh

| Gói | Giá | Mục tiêu khách hàng |
|-----|-----|---------------------|
| Basic | 100k/tháng | Thương lái nhỏ |
| Pro | 300k/tháng | Thương lái vừa |
| Enterprise | 1 triệu/tháng | Doanh nghiệp nông sản |

Tiềm năng: **10,000 user × 100k = 1 tỷ VND/tháng**

---

## 9. Tài liệu cần làm thêm

- [ ] `09-api-contracts-core-modules.md` — API contracts chi tiết
- [ ] `10-domain-workflows-sequence-diagrams.md` — Sequence diagrams
- [ ] `11-microservice-splitting-strategy.md` — Chiến lược tách microservice
- [ ] `12-mobile-offline-sync-design.md` — Thiết kế offline sync mobile
