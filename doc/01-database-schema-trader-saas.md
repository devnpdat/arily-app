# Thiết kế Database Schema đầy đủ cho SaaS Quản lý Thương lái Nông sản

## 1. Mục tiêu thiết kế

Tài liệu này mô tả **database schema chi tiết** cho hệ thống SaaS quản lý thương lái nông sản, theo hướng:

- Hỗ trợ **đa tenant**
- Hỗ trợ **offline-first mobile**
- Theo dõi đầy đủ vòng đời:
  - Thu mua
  - Tạm ứng
  - Công nợ
  - Hao hụt
  - Kho
  - Bán hàng
  - Vận chuyển
  - Nhân công
  - Thanh toán
  - Báo cáo
- Có thể mở rộng lên quy mô lớn
- Phù hợp với backend `.NET + PostgreSQL + Redis + Elasticsearch`

---

## 2. Nguyên tắc thiết kế

1. Mỗi bảng nghiệp vụ chính đều có:
   - `Id`
   - `TenantId`
   - `CreatedAt`
   - `CreatedBy`
   - `UpdatedAt`
   - `UpdatedBy`
   - `IsDeleted`

2. Dùng `UUID` cho khóa chính.

3. Tách rõ:
   - master data
   - transaction data
   - accounting/debt
   - audit/integration

4. Các bảng tiền tệ dùng:
   - `numeric(18,2)` cho tiền
   - `numeric(18,3)` hoặc `numeric(18,4)` cho kg / tấn / tỷ lệ

5. Mọi dữ liệu quan trọng đều cần truy xuất nguồn gốc theo:
   - session
   - lot
   - farmer
   - warehouse
   - sale order

---

## 3. Danh sách phân hệ và số lượng bảng

| Phân hệ | Số bảng |
|---|---:|
| Multi-tenant & người dùng | 8 |
| Danh mục nền | 10 |
| CRM nông dân / khách hàng | 8 |
| Thu mua | 10 |
| Hao hụt / QC / truy xuất | 7 |
| Kho | 8 |
| Bán hàng & công nợ đầu ra | 8 |
| Nhân công & vận chuyển | 8 |
| Tài chính / sổ cái / thanh toán | 10 |
| Hệ thống / đồng bộ / audit | 8 |
| **Tổng cộng** | **85** |

---

## 4. Cấu trúc nhóm bảng

# 4.1. Multi-tenant & người dùng

## 4.1.1 Tenants
Lưu thông tin thương lái / doanh nghiệp sử dụng hệ thống.

**Các cột chính**
- Id
- Code
- Name
- OwnerName
- PhoneNumber
- BusinessType
- ProvinceCode
- DistrictCode
- Address
- SubscriptionPlanId
- Status
- ExpiredAt
- CreatedAt
- UpdatedAt

## 4.1.2 SubscriptionPlans
Gói dịch vụ SaaS.

**Các cột chính**
- Id
- Code
- Name
- PriceMonthly
- PriceYearly
- MaxUsers
- MaxWarehouses
- MaxTransactionsPerMonth
- FeaturesJson
- Status

## 4.1.3 TenantSubscriptions
Lịch sử đăng ký gói.

**Các cột chính**
- Id
- TenantId
- SubscriptionPlanId
- StartDate
- EndDate
- BillingCycle
- Price
- Status
- PaymentStatus

## 4.1.4 Users
Tài khoản người dùng.

**Các cột chính**
- Id
- TenantId
- UserName
- FullName
- PhoneNumber
- Email
- PasswordHash
- Status
- LastLoginAt

## 4.1.5 Roles
Vai trò.

**Ví dụ**
- Owner
- Buyer
- Accountant
- WarehouseKeeper
- Driver
- Manager

## 4.1.6 Permissions
Quyền thao tác hệ thống.

## 4.1.7 UserRoles
Bảng mapping user-role.

## 4.1.8 RolePermissions
Bảng mapping role-permission.

---

# 4.2. Danh mục nền

## 4.2.1 ProductCategories
Danh mục nhóm nông sản.

Ví dụ:
- Sầu riêng
- Mít
- Xoài
- Thanh long

## 4.2.2 Products
Danh mục mặt hàng.

**Các cột chính**
- Id
- TenantId
- ProductCategoryId
- Code
- Name
- UnitOfMeasureId
- DefaultLossRate
- Status

## 4.2.3 ProductGrades
Cấp chất lượng.

Ví dụ:
- Loại 1
- Loại 2
- Dạt

## 4.2.4 UnitOfMeasures
Đơn vị tính.

Ví dụ:
- kg
- tấn
- thùng
- bao

## 4.2.5 PaymentMethods
Phương thức thanh toán.

## 4.2.6 ExpenseCategories
Nhóm chi phí.

Ví dụ:
- Xăng xe
- Bốc vác
- Ăn uống
- Thuê xe
- Cầu đường

## 4.2.7 Provinces
## 4.2.8 Districts
## 4.2.9 Wards
Phục vụ địa chỉ / định vị vườn / kho.

## 4.2.10 BankAccounts
Tài khoản ngân hàng của tenant.

---

# 4.3. CRM nông dân / khách hàng

## 4.3.1 Farmers
Hồ sơ nông dân.

**Các cột chính**
- Id
- TenantId
- Code
- FullName
- NickName
- PhoneNumber
- ProvinceCode
- DistrictCode
- WardCode
- Address
- Latitude
- Longitude
- ReputationScore
- Note

## 4.3.2 FarmerGardens
Mỗi nông dân có thể có nhiều vườn.

**Các cột chính**
- Id
- TenantId
- FarmerId
- GardenName
- ProductCategoryId
- AreaHectare
- EstimatedYield
- Latitude
- Longitude
- Address
- Status

## 4.3.3 FarmerBankAccounts
## 4.3.4 FarmerContacts
## 4.3.5 FarmerTags
## 4.3.6 FarmerTagMappings
Phân loại nhóm nông dân.

## 4.3.7 FarmerRatings
Lưu đánh giá nội bộ.

Ví dụ:
- giao hàng chuẩn
- hay trộn hàng dạt
- dễ hợp tác
- thường chậm công nợ

## 4.3.8 FarmerAttachments
Ảnh vườn, giấy tờ, ảnh giao dịch.

---

## 4.3.9 Customers
Đầu ra: vựa, chợ đầu mối, siêu thị.

## 4.3.10 CustomerContacts
## 4.3.11 CustomerBankAccounts
## 4.3.12 CustomerRatings
## 4.3.13 CustomerAttachments

---

# 4.4. Thu mua

## 4.4.1 CollectionSessions
Phiên gom hàng.

**Ví dụ**
- "Sầu riêng Tiền Giang 23/03"
- "Mít Cai Lậy sáng 24/03"

**Các cột chính**
- Id
- TenantId
- Code
- Name
- SessionDate
- ProductCategoryId
- RegionProvinceCode
- RegionDistrictCode
- BuyerUserId
- Status
- StartedAt
- ClosedAt
- Note

## 4.4.2 CollectionTrips
Một session có thể có nhiều chuyến đi thực tế.

## 4.4.3 PurchaseOrders
Phiếu thu mua từ nông dân.

**Các cột chính**
- Id
- TenantId
- SessionId
- TripId
- FarmerId
- GardenId
- PurchaseDate
- ProductId
- ExpectedQuantity
- UnitPrice
- GrossAmount
- AdvanceAmount
- EstimatedLossAmount
- NetPayableAmount
- Status
- Note

## 4.4.4 PurchaseOrderDetails
Chi tiết theo grade / line.

Ví dụ:
- 500kg loại 1
- 200kg loại 2

## 4.4.5 WeighingTickets
Phiếu cân.

**Các cột chính**
- Id
- TenantId
- PurchaseOrderId
- TicketNo
- GrossWeight
- TareWeight
- NetWeight
- UnitOfMeasureId
- WeighedAt
- PrintedAt
- PrinterDeviceId

## 4.4.6 WeighingTicketAttachments
Ảnh cân, biên nhận.

## 4.4.7 PurchaseAdvances
Lịch sử tạm ứng cho nông dân.

## 4.4.8 PurchaseExpenses
Chi phí phát sinh trực tiếp theo đơn thu mua.

## 4.4.9 PurchasePayments
Thanh toán thực tế cho nông dân.

## 4.4.10 PurchaseOrderStatuses
Lịch sử thay đổi trạng thái.

---

# 4.5. Hao hụt / QC / trả hàng / truy xuất

## 4.5.1 Lots
Bảng lô hàng gốc sau thu mua.

**Các cột chính**
- Id
- TenantId
- LotCode
- PurchaseOrderId
- FarmerId
- ProductId
- ReceivedQuantity
- CurrentQuantity
- UnitOfMeasureId
- WarehouseId
- Status
- ReceivedAt

## 4.5.2 LotGradeSnapshots
Snapshot chất lượng ban đầu của lô.

## 4.5.3 QualityInspections
Phiếu kiểm hàng / phân loại.

**Các cột chính**
- Id
- TenantId
- LotId
- InspectionNo
- InspectedBy
- InspectedAt
- Result
- Note

## 4.5.4 QualityInspectionDetails
Chi tiết kết quả:
- loại 1
- loại 2
- dạt
- hư hỏng

## 4.5.5 LossAdjustmentOrders
Lệnh trừ hao hụt.

**Các cột chính**
- Id
- TenantId
- LotId
- FarmerId
- PurchaseOrderId
- AdjustmentNo
- LossQuantity
- LossAmount
- ReasonCode
- ApprovedBy
- ApprovedAt
- Status

## 4.5.6 ReturnToFarmerOrders
Hàng trả ngược lại cho nông dân.

## 4.5.7 LotTraceLogs
Nhật ký truy xuất lô:
- từ đơn mua nào
- chuyển kho nào
- bán cho ai
- bị hao hụt bao nhiêu

---

# 4.6. Kho

## 4.6.1 Warehouses
Kho hàng.

## 4.6.2 WarehouseZones
Khu vực trong kho.

## 4.6.3 WarehouseBins
Vị trí lưu hàng.

## 4.6.4 InventoryLots
Tồn kho theo lô.

**Các cột chính**
- Id
- TenantId
- WarehouseId
- LotId
- ProductId
- GradeId
- BinId
- OnHandQuantity
- ReservedQuantity
- AvailableQuantity
- LastUpdatedAt

## 4.6.5 InventoryTransactions
Phát sinh kho.

**Các loại**
- nhập mua
- nhập điều chỉnh
- xuất bán
- chuyển kho
- hao hụt
- huỷ hàng

## 4.6.6 StockTransfers
Phiếu chuyển kho.

## 4.6.7 StockTransferDetails
## 4.6.8 StockCounts
Phiếu kiểm kê.

## 4.6.9 StockCountDetails
## 4.6.10 InventoryAdjustmentOrders
Điều chỉnh tồn kho.

---

# 4.7. Bán hàng & công nợ đầu ra

## 4.7.1 SalesOrders
Đơn bán.

**Các cột chính**
- Id
- TenantId
- OrderNo
- CustomerId
- OrderDate
- DeliveryDate
- TotalAmount
- DiscountAmount
- NetAmount
- PaidAmount
- DebtAmount
- Status

## 4.7.2 SalesOrderDetails
Chi tiết bán theo lô / grade / sản phẩm.

## 4.7.3 SalesDeliveries
Phiếu giao hàng.

## 4.7.4 SalesDeliveryDetails
## 4.7.5 SalesInvoices
Hoá đơn / phiếu bán.

## 4.7.6 SalesReceipts
Thu tiền khách hàng.

## 4.7.7 CustomerDebtLedgers
Sổ công nợ đầu ra.

## 4.7.8 SalesOrderStatuses
Lịch sử trạng thái đơn bán.

---

# 4.8. Nhân công & vận chuyển

## 4.8.1 Workers
Danh sách nhân công.

## 4.8.2 WorkerRoles
Vai trò nhân công:
- cân hàng
- bốc vác
- tài xế
- phân loại

## 4.8.3 WorkerAttendance
Chấm công.

## 4.8.4 WorkerPayrolls
Bảng lương.

## 4.8.5 WorkerPayrollDetails
Tính lương theo:
- ngày
- chuyến
- kg

## 4.8.6 Vehicles
Xe tải / xe ba gác / xe thuê.

## 4.8.7 Drivers
Thông tin tài xế.

## 4.8.8 DeliveryTrips
Chuyến vận chuyển.

## 4.8.9 DeliveryTripStops
Các điểm dừng.

## 4.8.10 DeliveryTripExpenses
Chi phí vận chuyển.

---

# 4.9. Tài chính / sổ cái / thanh toán

## 4.9.1 CashBooks
Sổ quỹ.

## 4.9.2 CashTransactions
Thu / chi tiền mặt.

## 4.9.3 BankTransactions
Giao dịch ngân hàng.

## 4.9.4 FarmerDebtLedgers
Sổ công nợ nông dân.

**Mỗi phát sinh đều ghi ledger**
- phát sinh mua
- tạm ứng
- trừ hao hụt
- thanh toán
- trả hàng

## 4.9.5 DebtSettlementOrders
Phiếu đối soát / chốt nợ.

## 4.9.6 ExpenseClaims
Phiếu chi phí chung.

## 4.9.7 ExpenseClaimDetails
## 4.9.8 ProfitLossSnapshots
Snapshot lời/lỗ theo:
- session
- chuyến xe
- ngày
- tháng

## 4.9.9 JournalEntries
Bút toán tổng hợp.

## 4.9.10 JournalEntryLines
Chi tiết bút toán.

---

# 4.10. Hệ thống / offline sync / audit / tích hợp

## 4.10.1 Devices
Thiết bị đăng nhập.

## 4.10.2 PrinterDevices
Máy in Bluetooth.

## 4.10.3 MobileSyncLogs
Lịch sử sync dữ liệu mobile.

**Các cột chính**
- Id
- TenantId
- DeviceId
- SyncType
- EntityName
- EntityId
- RequestId
- Status
- ErrorMessage
- SyncedAt

## 4.10.4 IdempotencyKeys
Chống lặp giao dịch.

## 4.10.5 IntegrationOutbox
Outbox event.

## 4.10.6 IntegrationInbox
Inbox event.

## 4.10.7 AuditLogs
Log thao tác người dùng.

## 4.10.8 NotificationLogs
Push/SMS/Zalo notification log.

## 4.10.9 FileAttachments
Kho file dùng chung.

## 4.10.10 AppSettings
Cấu hình theo tenant.

---

## 5. Quan hệ nghiệp vụ chính

### Luồng thu mua
Farmers  
→ FarmerGardens  
→ CollectionSessions  
→ PurchaseOrders  
→ PurchaseOrderDetails  
→ WeighingTickets  
→ Lots  
→ InventoryLots

### Luồng hao hụt
Lots  
→ QualityInspections  
→ QualityInspectionDetails  
→ LossAdjustmentOrders  
→ FarmerDebtLedgers

### Luồng bán hàng
InventoryLots  
→ SalesOrders  
→ SalesOrderDetails  
→ SalesDeliveries  
→ SalesInvoices  
→ CustomerDebtLedgers

### Luồng tài chính
PurchaseOrders / PurchaseAdvances / LossAdjustmentOrders / PurchasePayments  
→ FarmerDebtLedgers

SalesOrders / SalesReceipts  
→ CustomerDebtLedgers

ExpenseClaims / DeliveryTripExpenses / WorkerPayrolls  
→ ProfitLossSnapshots / JournalEntries

---

## 6. Các bảng quan trọng nhất cho MVP

Nếu làm bản đầu tiên, chỉ cần ưu tiên 20 bảng sau:

1. Tenants
2. Users
3. Roles
4. Farmers
5. FarmerGardens
6. Customers
7. Products
8. ProductGrades
9. UnitOfMeasures
10. CollectionSessions
11. PurchaseOrders
12. PurchaseOrderDetails
13. WeighingTickets
14. PurchaseAdvances
15. Lots
16. InventoryLots
17. LossAdjustmentOrders
18. FarmerDebtLedgers
19. SalesOrders
20. CustomerDebtLedgers

---

## 7. Đề xuất kiểu dữ liệu chuẩn PostgreSQL

### Khóa
- `uuid`

### Tiền
- `numeric(18,2)`

### Khối lượng
- `numeric(18,3)` hoặc `numeric(18,4)`

### Tỷ lệ phần trăm / hao hụt
- `numeric(8,4)`

### Thời gian
- `timestamp without time zone` hoặc `timestamp with time zone`

### Dữ liệu linh hoạt
- `jsonb`

### Trạng thái
- `smallint` hoặc `integer`

---

## 8. Chuẩn cột dùng chung nên áp dụng

Hầu hết bảng nên có:

- Id
- TenantId
- Code
- Status
- Note
- ExtraProperties
- CreatedAt
- CreatedBy
- UpdatedAt
- UpdatedBy
- DeletedAt
- DeletedBy
- IsDeleted

---

## 9. Index nên có

### Index bắt buộc
- `(TenantId, Code)`
- `(TenantId, Status)`
- `(TenantId, CreatedAt)`
- `(TenantId, FarmerId)`
- `(TenantId, CustomerId)`
- `(TenantId, SessionId)`
- `(TenantId, LotId)`
- `(TenantId, WarehouseId)`

### Unique index gợi ý
- `Tenants(Code)`
- `(TenantId, Users(UserName))`
- `(TenantId, Farmers(Code))`
- `(TenantId, Customers(Code))`
- `(TenantId, Products(Code))`
- `(TenantId, CollectionSessions(Code))`
- `(TenantId, Lots(LotCode))`
- `(TenantId, SalesOrders(OrderNo))`

---

## 10. Phân vùng dữ liệu nên tính trước

Khi hệ thống lớn, nên partition các bảng lớn theo tháng hoặc theo tenant:

- AuditLogs
- MobileSyncLogs
- InventoryTransactions
- FarmerDebtLedgers
- CustomerDebtLedgers
- PurchaseOrders
- SalesOrders
- NotificationLogs

---

## 11. Gợi ý mapping sang Elasticsearch

Một số index nên có để search nhanh:

### purchase-order-index
- PurchaseOrder
- FarmerName
- SessionName
- ProductName
- Quantity
- UnitPrice
- DebtAmount

### lot-index
- LotCode
- FarmerName
- ProductName
- Grade
- CurrentQuantity
- WarehouseName
- TraceStatus

### sales-order-index
- OrderNo
- CustomerName
- ProductName
- NetAmount
- PaidAmount
- DebtAmount

### farmer-index
- Farmer profile
- total purchase volume
- current debt
- reputation score

---

## 12. Gợi ý chuẩn hóa số lượng bảng theo release

### Release 1 — MVP
20 bảng cốt lõi

### Release 2
35–45 bảng
- thêm kho
- thêm bán hàng
- thêm công nợ đầy đủ

### Release 3
60+ bảng
- thêm nhân công
- vận chuyển
- tài chính
- báo cáo nâng cao

### Release 4
80+ bảng
- AI
- sync nâng cao
- audit
- integration outbox/inbox

---

## 13. Kết luận

Schema này được thiết kế theo hướng:

- đủ sâu để làm sản phẩm thật
- phù hợp SaaS đa tenant
- phù hợp mobile offline-first
- dễ scale dần từ MVP lên enterprise

Nếu đi tiếp bước sau, nên làm ngay:

1. Vẽ **ERD tổng thể**
2. Chốt **naming convention**
3. Sinh **DDL PostgreSQL**
4. Tạo **module theo bounded context**:
   - CRM
   - Collection
   - Inventory
   - Sales
   - Finance
   - System

---

## 14. File tiếp theo nên làm

Sau file schema này, nên làm tiếp theo thứ tự:

1. `02-system-architecture-scale-1m-users.md`
2. `03-ui-ux-for-traders-field-operations.md`
3. `04-postgresql-ddl-core-tables.md`
4. `05-erd-mermaid.md`
