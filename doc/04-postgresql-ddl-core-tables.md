# 04. PostgreSQL DDL – Core Tables (MVP Version)

Tài liệu này cung cấp **DDL PostgreSQL cho các bảng cốt lõi của hệ thống SaaS quản lý thương lái**.

Mục tiêu:

- Tạo **schema MVP chạy được ngay**
- Khoảng **25–30 bảng quan trọng**
- Phù hợp với backend `.NET / ABP Framework`
- Dễ mở rộng về sau

---

# 1. Extensions

```sql
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";
```

---

# 2. Tenants

```sql
CREATE TABLE tenants (
    id uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
    code varchar(50) UNIQUE NOT NULL,
    name varchar(255) NOT NULL,
    owner_name varchar(255),
    phone_number varchar(30),
    address text,
    status int DEFAULT 1,
    created_at timestamp DEFAULT now()
);
```

---

# 3. Users

```sql
CREATE TABLE users (
    id uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
    tenant_id uuid NOT NULL,
    username varchar(100) NOT NULL,
    full_name varchar(255),
    phone_number varchar(30),
    password_hash text,
    status int DEFAULT 1,
    created_at timestamp DEFAULT now()
);

CREATE INDEX idx_users_tenant ON users(tenant_id);
```

---

# 4. Farmers

```sql
CREATE TABLE farmers (
    id uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
    tenant_id uuid NOT NULL,
    code varchar(50),
    full_name varchar(255),
    phone_number varchar(30),
    address text,
    reputation_score numeric(5,2),
    created_at timestamp DEFAULT now()
);

CREATE INDEX idx_farmers_tenant ON farmers(tenant_id);
```

---

# 5. Customers

```sql
CREATE TABLE customers (
    id uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
    tenant_id uuid NOT NULL,
    code varchar(50),
    name varchar(255),
    phone_number varchar(30),
    address text,
    created_at timestamp DEFAULT now()
);
```

---

# 6. Products

```sql
CREATE TABLE products (
    id uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
    tenant_id uuid NOT NULL,
    code varchar(50),
    name varchar(255),
    unit varchar(20),
    status int DEFAULT 1
);
```

---

# 7. Product Grades

```sql
CREATE TABLE product_grades (
    id uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
    tenant_id uuid,
    code varchar(50),
    name varchar(100)
);
```

---

# 8. Collection Sessions

```sql
CREATE TABLE collection_sessions (
    id uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
    tenant_id uuid NOT NULL,
    code varchar(50),
    name varchar(255),
    session_date date,
    buyer_user_id uuid,
    status int,
    created_at timestamp DEFAULT now()
);
```

---

# 9. Purchase Orders

```sql
CREATE TABLE purchase_orders (
    id uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
    tenant_id uuid NOT NULL,
    session_id uuid,
    farmer_id uuid,
    product_id uuid,
    purchase_date timestamp,
    expected_quantity numeric(18,3),
    unit_price numeric(18,2),
    gross_amount numeric(18,2),
    advance_amount numeric(18,2),
    net_payable_amount numeric(18,2),
    status int,
    created_at timestamp DEFAULT now()
);
```

---

# 10. Purchase Order Details

```sql
CREATE TABLE purchase_order_details (
    id uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
    purchase_order_id uuid,
    grade_id uuid,
    quantity numeric(18,3),
    unit_price numeric(18,2)
);
```

---

# 11. Weighing Tickets

```sql
CREATE TABLE weighing_tickets (
    id uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
    purchase_order_id uuid,
    gross_weight numeric(18,3),
    tare_weight numeric(18,3),
    net_weight numeric(18,3),
    weighed_at timestamp DEFAULT now()
);
```

---

# 12. Purchase Advances

```sql
CREATE TABLE purchase_advances (
    id uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
    purchase_order_id uuid,
    amount numeric(18,2),
    payment_method varchar(50),
    created_at timestamp DEFAULT now()
);
```

---

# 13. Lots

```sql
CREATE TABLE lots (
    id uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
    tenant_id uuid NOT NULL,
    lot_code varchar(50),
    purchase_order_id uuid,
    farmer_id uuid,
    product_id uuid,
    received_quantity numeric(18,3),
    current_quantity numeric(18,3),
    created_at timestamp DEFAULT now()
);
```

---

# 14. Warehouses

```sql
CREATE TABLE warehouses (
    id uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
    tenant_id uuid,
    code varchar(50),
    name varchar(255),
    address text
);
```

---

# 15. Inventory Lots

```sql
CREATE TABLE inventory_lots (
    id uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
    tenant_id uuid,
    warehouse_id uuid,
    lot_id uuid,
    product_id uuid,
    grade_id uuid,
    onhand_quantity numeric(18,3),
    reserved_quantity numeric(18,3),
    updated_at timestamp
);
```

---

# 16. Inventory Transactions

```sql
CREATE TABLE inventory_transactions (
    id uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
    tenant_id uuid,
    lot_id uuid,
    warehouse_id uuid,
    transaction_type varchar(50),
    quantity numeric(18,3),
    created_at timestamp DEFAULT now()
);
```

---

# 17. Sales Orders

```sql
CREATE TABLE sales_orders (
    id uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
    tenant_id uuid,
    order_no varchar(50),
    customer_id uuid,
    order_date timestamp,
    total_amount numeric(18,2),
    paid_amount numeric(18,2),
    debt_amount numeric(18,2),
    status int
);
```

---

# 18. Sales Order Details

```sql
CREATE TABLE sales_order_details (
    id uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
    sales_order_id uuid,
    product_id uuid,
    lot_id uuid,
    quantity numeric(18,3),
    unit_price numeric(18,2)
);
```

---

# 19. Sales Receipts

```sql
CREATE TABLE sales_receipts (
    id uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
    sales_order_id uuid,
    amount numeric(18,2),
    payment_method varchar(50),
    created_at timestamp DEFAULT now()
);
```

---

# 20. Farmer Debt Ledger

```sql
CREATE TABLE farmer_debt_ledgers (
    id uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
    tenant_id uuid,
    farmer_id uuid,
    reference_type varchar(50),
    reference_id uuid,
    debit numeric(18,2),
    credit numeric(18,2),
    balance numeric(18,2),
    created_at timestamp DEFAULT now()
);
```

---

# 21. Customer Debt Ledger

```sql
CREATE TABLE customer_debt_ledgers (
    id uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
    tenant_id uuid,
    customer_id uuid,
    reference_type varchar(50),
    reference_id uuid,
    debit numeric(18,2),
    credit numeric(18,2),
    balance numeric(18,2),
    created_at timestamp DEFAULT now()
);
```

---

# 22. Workers

```sql
CREATE TABLE workers (
    id uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
    tenant_id uuid,
    full_name varchar(255),
    phone_number varchar(30),
    role varchar(100)
);
```

---

# 23. Vehicles

```sql
CREATE TABLE vehicles (
    id uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
    tenant_id uuid,
    plate_number varchar(50),
    vehicle_type varchar(50),
    capacity numeric(10,2)
);
```

---

# 24. Delivery Trips

```sql
CREATE TABLE delivery_trips (
    id uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
    tenant_id uuid,
    vehicle_id uuid,
    driver_name varchar(255),
    departure_time timestamp,
    arrival_time timestamp
);
```

---

# 25. Expenses

```sql
CREATE TABLE expenses (
    id uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
    tenant_id uuid,
    category varchar(100),
    amount numeric(18,2),
    description text,
    created_at timestamp DEFAULT now()
);
```

---

# 26. Audit Logs

```sql
CREATE TABLE audit_logs (
    id uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
    tenant_id uuid,
    user_id uuid,
    action varchar(255),
    entity_name varchar(255),
    entity_id uuid,
    created_at timestamp DEFAULT now()
);
```

---

# 27. File Attachments

```sql
CREATE TABLE file_attachments (
    id uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
    tenant_id uuid,
    file_name varchar(255),
    file_url text,
    entity_name varchar(100),
    entity_id uuid,
    created_at timestamp DEFAULT now()
);
```

---

# 28. Notification Logs

```sql
CREATE TABLE notification_logs (
    id uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
    tenant_id uuid,
    user_id uuid,
    message text,
    status int,
    created_at timestamp DEFAULT now()
);
```

---

# 29. Mobile Sync Logs

```sql
CREATE TABLE mobile_sync_logs (
    id uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
    tenant_id uuid,
    device_id varchar(100),
    entity_name varchar(100),
    entity_id uuid,
    sync_status int,
    synced_at timestamp
);
```

---

# 30. Idempotency Keys

```sql
CREATE TABLE idempotency_keys (
    id uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
    key varchar(255) UNIQUE,
    created_at timestamp DEFAULT now()
);
```

---

# Kết luận

DDL trên cung cấp **30 bảng cốt lõi đủ để build MVP hoàn chỉnh**:

- thu mua
- công nợ
- kho
- bán hàng
- vận chuyển
- nhân công
- audit
- sync mobile

Các bảng này có thể mở rộng thành **80+ bảng đầy đủ trong hệ thống production**.
