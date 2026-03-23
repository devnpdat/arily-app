# 08. Full System Modules Breakdown – Trader SaaS

Tài liệu này phân rã toàn bộ hệ thống SaaS quản lý thương lái nông sản thành các **bounded contexts / modules / microservices** để phục vụ:

- thiết kế kiến trúc phần mềm
- tổ chức source code
- phân chia team dev
- lập kế hoạch scale từ MVP lên production

Mục tiêu là để bạn có thể dùng tài liệu này như một **technical blueprint** trước khi bắt tay code.

---

# 1. Nguyên tắc chia module

Hệ thống được chia theo các nguyên tắc:

- Mỗi module quản lý một nhóm nghiệp vụ rõ ràng
- Mỗi module có database ownership logic riêng
- Ưu tiên loose coupling
- Giao tiếp qua API hoặc event
- Có thể bắt đầu bằng **modular monolith**
- Khi lớn lên có thể tách thành **microservices**

---

# 2. Danh sách bounded contexts

## 2.1 Identity & Access
Quản lý:
- user
- role
- permission
- đăng nhập
- JWT / SSO / OTP

## 2.2 Tenant Management
Quản lý:
- tenant
- subscription
- plan
- billing cycle
- giới hạn tính năng theo gói

## 2.3 CRM
Quản lý:
- nông dân
- vườn
- khách hàng đầu ra
- contact
- rating
- tag

## 2.4 Collection
Quản lý:
- session thu mua
- đơn thu mua
- phiếu cân
- tạm ứng
- chi phí thu mua

## 2.5 Quality Control
Quản lý:
- kiểm hàng
- phân loại hàng
- hao hụt
- trả hàng
- traceability

## 2.6 Inventory
Quản lý:
- lô hàng
- tồn kho
- kho
- nhập xuất chuyển
- kiểm kê

## 2.7 Sales
Quản lý:
- đơn bán
- giao hàng
- hóa đơn
- thu tiền
- công nợ khách hàng

## 2.8 Finance
Quản lý:
- sổ quỹ
- thu chi
- công nợ nông dân
- ledger
- lời lỗ
- đối soát

## 2.9 Workforce
Quản lý:
- nhân công
- chấm công
- bảng lương
- thanh toán lương

## 2.10 Logistics
Quản lý:
- xe
- tài xế
- chuyến xe
- chi phí vận chuyển
- tuyến đường

## 2.11 Notification
Quản lý:
- push
- SMS
- Zalo
- email
- lịch nhắc nợ

## 2.12 File & Media
Quản lý:
- upload file
- ảnh giao dịch
- ảnh lô hàng
- file hóa đơn

## 2.13 Reporting & Analytics
Quản lý:
- dashboard
- báo cáo
- snapshot lời lỗ
- KPI tenant

## 2.14 Offline Sync
Quản lý:
- đồng bộ mobile
- conflict resolution
- sync status
- retry queue

## 2.15 System & Audit
Quản lý:
- audit log
- idempotency
- device
- app settings
- outbox/inbox events

---

# 3. Quan hệ giữa các modules

## Luồng đầu vào
CRM
→ Collection
→ Quality Control
→ Inventory
→ Finance

## Luồng đầu ra
Inventory
→ Sales
→ Finance
→ Reporting

## Luồng vận hành
Identity
→ tất cả modules

Notification
← Finance / Sales / Collection

Offline Sync
↔ Collection / CRM / Inventory

System & Audit
← tất cả modules

---

# 4. Đề xuất tách solution .NET

Nếu dùng .NET, có thể tổ chức solution như sau:

```text
src/
 ├── TraderSaaS.IdentityService
 ├── TraderSaaS.TenantService
 ├── TraderSaaS.CRMService
 ├── TraderSaaS.CollectionService
 ├── TraderSaaS.QualityService
 ├── TraderSaaS.InventoryService
 ├── TraderSaaS.SalesService
 ├── TraderSaaS.FinanceService
 ├── TraderSaaS.WorkforceService
 ├── TraderSaaS.LogisticsService
 ├── TraderSaaS.NotificationService
 ├── TraderSaaS.ReportingService
 ├── TraderSaaS.SyncService
 ├── TraderSaaS.SystemService
 └── TraderSaaS.Gateway
```

Nếu đi theo modular monolith lúc đầu:

```text
src/
 ├── TraderSaaS.HttpApi
 ├── TraderSaaS.Application
 ├── TraderSaaS.Domain
 ├── TraderSaaS.EntityFrameworkCore
 ├── TraderSaaS.Shared
 └── Modules/
      ├── Identity
      ├── Tenant
      ├── CRM
      ├── Collection
      ├── Quality
      ├── Inventory
      ├── Sales
      ├── Finance
      ├── Workforce
      ├── Logistics
      ├── Notification
      ├── Reporting
      ├── Sync
      └── System
```

---

# 5. Breakdown chi tiết từng module

# 5.1 Identity Module

## Chức năng
- login
- logout
- refresh token
- OTP
- reset password
- role / permission

## Entities
- User
- Role
- Permission
- UserRole
- RolePermission
- LoginHistory

## APIs
- POST /auth/login
- POST /auth/refresh-token
- POST /auth/logout
- GET /users
- POST /users
- PUT /users/{id}
- GET /roles

## Events
- user.created
- user.updated
- user.locked

---

# 5.2 Tenant Module

## Chức năng
- tạo tenant
- gói dịch vụ
- giới hạn user
- giới hạn kho
- cấu hình tenant

## Entities
- Tenant
- SubscriptionPlan
- TenantSubscription
- TenantSetting
- BillingInvoice

## APIs
- POST /tenants
- GET /tenants/{id}
- POST /subscriptions
- GET /plans

## Events
- tenant.created
- subscription.activated
- subscription.expired

---

# 5.3 CRM Module

## Chức năng
- quản lý nông dân
- quản lý vườn
- quản lý khách hàng
- phân nhóm / đánh giá

## Entities
- Farmer
- FarmerGarden
- FarmerContact
- FarmerRating
- Customer
- CustomerContact
- CustomerRating

## APIs
- GET /farmers
- POST /farmers
- PUT /farmers/{id}
- GET /customers
- POST /customers

## Events
- farmer.created
- farmer.updated
- customer.created

---

# 5.4 Collection Module

## Chức năng
- tạo session thu mua
- tạo đơn mua
- phiếu cân
- tạm ứng
- chi phí thu mua

## Entities
- CollectionSession
- CollectionTrip
- PurchaseOrder
- PurchaseOrderDetail
- WeighingTicket
- PurchaseAdvance
- PurchaseExpense

## APIs
- POST /collection/sessions
- GET /collection/sessions
- POST /purchase-orders
- GET /purchase-orders/{id}
- POST /weighing-tickets
- POST /purchase-advances

## Events
- purchase.created
- weighing.completed
- advance.created

---

# 5.5 Quality Control Module

## Chức năng
- kiểm hàng
- phân loại
- ghi nhận hao hụt
- trả hàng
- lưu vết lô

## Entities
- QualityInspection
- QualityInspectionDetail
- LossAdjustmentOrder
- ReturnToFarmerOrder
- LotTraceLog

## APIs
- POST /quality/inspections
- POST /quality/loss-adjustments
- POST /quality/returns

## Events
- qc.completed
- loss.adjusted
- return.to_farmer.created

---

# 5.6 Inventory Module

## Chức năng
- quản lý lô
- tồn kho
- nhập xuất chuyển
- kiểm kê

## Entities
- Lot
- Warehouse
- WarehouseZone
- InventoryLot
- InventoryTransaction
- StockTransfer
- StockCount

## APIs
- GET /lots
- POST /lots
- GET /inventory
- POST /stock-transfers
- POST /stock-counts

## Events
- lot.created
- inventory.updated
- stock.transferred

---

# 5.7 Sales Module

## Chức năng
- tạo đơn bán
- giao hàng
- hóa đơn
- thu tiền
- công nợ đầu ra

## Entities
- SalesOrder
- SalesOrderDetail
- SalesDelivery
- SalesInvoice
- SalesReceipt
- CustomerDebtLedger

## APIs
- POST /sales-orders
- GET /sales-orders
- POST /sales-deliveries
- POST /sales-receipts

## Events
- sales.created
- sales.delivered
- receipt.created

---

# 5.8 Finance Module

## Chức năng
- sổ quỹ
- công nợ nông dân
- thu chi
- lời lỗ
- đối soát

## Entities
- CashBook
- CashTransaction
- BankTransaction
- FarmerDebtLedger
- DebtSettlementOrder
- ProfitLossSnapshot
- JournalEntry

## APIs
- GET /finance/farmer-debts
- POST /finance/payments
- GET /finance/profit-loss
- POST /finance/settlements

## Events
- debt.updated
- payment.completed
- pnl.snapshot.created

---

# 5.9 Workforce Module

## Chức năng
- danh sách nhân công
- chấm công
- tính lương
- thanh toán lương

## Entities
- Worker
- WorkerAttendance
- WorkerPayroll
- WorkerPayrollDetail

## APIs
- GET /workers
- POST /workers
- POST /attendances
- POST /payrolls

## Events
- worker.created
- attendance.recorded
- payroll.created

---

# 5.10 Logistics Module

## Chức năng
- quản lý xe
- tài xế
- chuyến xe
- chi phí

## Entities
- Vehicle
- Driver
- DeliveryTrip
- DeliveryTripStop
- DeliveryTripExpense

## APIs
- GET /vehicles
- POST /vehicles
- POST /delivery-trips
- POST /delivery-trip-expenses

## Events
- delivery_trip.created
- delivery_trip.completed

---

# 5.11 Notification Module

## Chức năng
- gửi push
- gửi SMS
- gửi email
- reminder nợ / giao hàng

## Entities
- NotificationTemplate
- NotificationMessage
- NotificationLog
- ReminderSchedule

## APIs
- POST /notifications/send
- GET /notifications/logs
- POST /reminders

## Events
- notification.created
- reminder.triggered

---

# 5.12 File & Media Module

## Chức năng
- upload ảnh
- lưu file hóa đơn
- gắn file vào entity

## Entities
- FileAttachment
- MediaFolder
- UploadSession

## APIs
- POST /files/upload
- GET /files/{id}
- DELETE /files/{id}

## Events
- file.uploaded
- file.deleted

---

# 5.13 Reporting Module

## Chức năng
- dashboard
- báo cáo thu mua
- báo cáo công nợ
- báo cáo lời lỗ
- báo cáo tồn kho

## Entities
- ReportSnapshot
- DashboardMetric
- ExportJob

## APIs
- GET /reports/dashboard
- GET /reports/purchase
- GET /reports/debt
- GET /reports/inventory
- POST /reports/export

## Events
- report.export.requested
- report.snapshot.generated

---

# 5.14 Offline Sync Module

## Chức năng
- nhận payload sync từ mobile
- chống trùng
- retry khi lỗi
- merge conflict

## Entities
- Device
- MobileSyncLog
- SyncConflict
- SyncJob

## APIs
- POST /sync/push
- POST /sync/pull
- GET /sync/status/{deviceId}

## Events
- sync.received
- sync.completed
- sync.conflict.detected

---

# 5.15 System & Audit Module

## Chức năng
- audit log
- idempotency
- app settings
- outbox / inbox
- health check

## Entities
- AuditLog
- IdempotencyKey
- IntegrationOutbox
- IntegrationInbox
- AppSetting
- HealthCheckLog

## APIs
- GET /audit-logs
- POST /idempotency/validate
- GET /settings
- GET /health

## Events
- audit.logged
- outbox.dispatched

---

# 6. Module nào nên làm trước

## Giai đoạn MVP
Ưu tiên:

1. Identity
2. Tenant
3. CRM
4. Collection
5. Finance
6. Inventory cơ bản
7. Reporting cơ bản
8. Offline Sync tối thiểu
9. System & Audit cơ bản

## Giai đoạn Phase 2
Thêm:

1. Sales
2. Logistics
3. Workforce
4. Notification nâng cao

## Giai đoạn Phase 3
Thêm:

1. Reporting nâng cao
2. AI / pricing engine
3. marketplace / community

---

# 7. Event-driven integration map

Các event quan trọng:

```text
purchase.created
 ├─> Finance cập nhật công nợ nông dân
 ├─> Inventory tạo lot
 └─> Reporting cập nhật số liệu

loss.adjusted
 ├─> Finance cập nhật ledger
 ├─> Inventory cập nhật tồn
 └─> Reporting cập nhật hao hụt

sales.created
 ├─> Inventory reserve stock
 ├─> Finance cập nhật công nợ khách hàng
 └─> Notification gửi xác nhận

receipt.created
 ├─> Finance chốt công nợ
 └─> Reporting cập nhật cashflow
```

---

# 8. Đề xuất mapping team theo module

## Team nhỏ 3–5 người
Đi theo modular monolith.

### Backend A
- Identity
- Tenant
- CRM
- Collection

### Backend B
- Inventory
- Finance
- Sales

### Mobile
- app offline
- sync
- UX field operations

### UI/UX
- mobile-first
- flow tối giản

---

## Team lớn 8–15 người
Có thể tách theo domain squads:

### Squad 1 – Core Business
- CRM
- Collection
- Finance

### Squad 2 – Supply Chain
- Quality
- Inventory
- Logistics

### Squad 3 – Commerce & Platform
- Sales
- Reporting
- Identity
- Notification
- Sync

---

# 9. Thứ tự tách microservice khi scale

Không nên tách tất cả ngay từ đầu.

## Bước 1
Modular monolith.

## Bước 2
Tách 3 service trước:
- Identity
- Notification
- Reporting

## Bước 3
Khi traffic lớn, tách tiếp:
- Collection
- Inventory
- Finance
- Sales

## Bước 4
Khi có nhiều integration, tách:
- Sync
- File
- Audit

---

# 10. Kết luận

Tài liệu này giúp bạn:

- nhìn hệ thống theo đúng góc nhìn Tech Lead / Solution Architect
- biết module nào là core
- biết module nào làm sau
- biết cách tổ chức source code và team

Nếu đi tiếp bước sau, nên làm thêm:

1. `09-api-contracts-core-modules.md`
2. `10-domain-workflows-sequence-diagrams.md`
3. `11-microservice-splitting-strategy.md`
4. `12-mobile-offline-sync-design.md`
