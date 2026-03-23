# 05. ERD Diagram (Mermaid) – Trader SaaS Database

Tài liệu này cung cấp **ERD (Entity Relationship Diagram)** cho hệ thống SaaS quản lý thương lái nông sản.

Sơ đồ được viết bằng **Mermaid**, có thể render trực tiếp trên:

- GitHub
- GitLab
- Notion
- Obsidian
- Mermaid Live Editor
- Confluence (plugin)

---

# 1. ERD Tổng thể

```mermaid
erDiagram

TENANTS {
    uuid id PK
    varchar code
    varchar name
}

USERS {
    uuid id PK
    uuid tenant_id FK
    varchar username
    varchar full_name
}

FARMERS {
    uuid id PK
    uuid tenant_id FK
    varchar full_name
    varchar phone_number
}

CUSTOMERS {
    uuid id PK
    uuid tenant_id FK
    varchar name
}

PRODUCTS {
    uuid id PK
    uuid tenant_id FK
    varchar name
    varchar unit
}

PRODUCT_GRADES {
    uuid id PK
    uuid tenant_id FK
    varchar name
}

COLLECTION_SESSIONS {
    uuid id PK
    uuid tenant_id FK
    date session_date
}

PURCHASE_ORDERS {
    uuid id PK
    uuid session_id FK
    uuid farmer_id FK
    uuid product_id FK
    numeric quantity
    numeric unit_price
}

PURCHASE_ORDER_DETAILS {
    uuid id PK
    uuid purchase_order_id FK
    uuid grade_id FK
    numeric quantity
}

WEIGHING_TICKETS {
    uuid id PK
    uuid purchase_order_id FK
    numeric net_weight
}

LOTS {
    uuid id PK
    uuid purchase_order_id FK
    uuid product_id FK
    numeric received_quantity
}

WAREHOUSES {
    uuid id PK
    uuid tenant_id FK
    varchar name
}

INVENTORY_LOTS {
    uuid id PK
    uuid warehouse_id FK
    uuid lot_id FK
    numeric quantity
}

SALES_ORDERS {
    uuid id PK
    uuid customer_id FK
    numeric total_amount
}

SALES_ORDER_DETAILS {
    uuid id PK
    uuid sales_order_id FK
    uuid lot_id FK
    numeric quantity
}

FARMER_DEBT_LEDGERS {
    uuid id PK
    uuid farmer_id FK
    numeric debit
    numeric credit
}

CUSTOMER_DEBT_LEDGERS {
    uuid id PK
    uuid customer_id FK
    numeric debit
    numeric credit
}

WORKERS {
    uuid id PK
    uuid tenant_id FK
    varchar full_name
}

VEHICLES {
    uuid id PK
    uuid tenant_id FK
    varchar plate_number
}

DELIVERY_TRIPS {
    uuid id PK
    uuid vehicle_id FK
}

EXPENSES {
    uuid id PK
    uuid tenant_id FK
    numeric amount
}

TENANTS ||--o{ USERS : has
TENANTS ||--o{ FARMERS : owns
TENANTS ||--o{ CUSTOMERS : owns
TENANTS ||--o{ PRODUCTS : owns
TENANTS ||--o{ WAREHOUSES : owns
TENANTS ||--o{ WORKERS : employs
TENANTS ||--o{ VEHICLES : owns

COLLECTION_SESSIONS ||--o{ PURCHASE_ORDERS : contains
FARMERS ||--o{ PURCHASE_ORDERS : sells_to
PRODUCTS ||--o{ PURCHASE_ORDERS : product

PURCHASE_ORDERS ||--o{ PURCHASE_ORDER_DETAILS : has
PRODUCT_GRADES ||--o{ PURCHASE_ORDER_DETAILS : grade

PURCHASE_ORDERS ||--o{ WEIGHING_TICKETS : weighed_by

PURCHASE_ORDERS ||--o{ LOTS : generates

LOTS ||--o{ INVENTORY_LOTS : stored_in
WAREHOUSES ||--o{ INVENTORY_LOTS : holds

CUSTOMERS ||--o{ SALES_ORDERS : places
SALES_ORDERS ||--o{ SALES_ORDER_DETAILS : contains
LOTS ||--o{ SALES_ORDER_DETAILS : sold_from

FARMERS ||--o{ FARMER_DEBT_LEDGERS : ledger
CUSTOMERS ||--o{ CUSTOMER_DEBT_LEDGERS : ledger

VEHICLES ||--o{ DELIVERY_TRIPS : used_for
```

---

# 2. Luồng nghiệp vụ chính

## Thu mua

Farmer  
→ Purchase Order  
→ Weighing Ticket  
→ Lot

## Kho

Lot  
→ Inventory Lots  
→ Warehouse

## Bán hàng

Inventory Lot  
→ Sales Order  
→ Sales Order Detail

## Công nợ

Purchase Order  
→ Farmer Debt Ledger

Sales Order  
→ Customer Debt Ledger

---

# 3. Cách render ERD

Bạn có thể:

### Cách 1

Copy vào:

https://mermaid.live

### Cách 2

Đặt trong file `.md` trên GitHub.

GitHub sẽ tự render.

### Cách 3

Import vào Notion / Obsidian.

---

# 4. Lợi ích của ERD này

Giúp team:

- hiểu cấu trúc database nhanh
- dễ review schema
- dễ mở rộng feature

---

# 5. Mở rộng trong tương lai

Có thể bổ sung thêm:

- logistics tables
- payroll tables
- qc tables
- analytics tables
