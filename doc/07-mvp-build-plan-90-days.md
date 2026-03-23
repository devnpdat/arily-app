# 07. MVP Build Plan – 90 Days

Tài liệu này mô tả kế hoạch **xây dựng MVP trong 90 ngày** cho hệ thống SaaS quản lý thương lái nông sản.

Mục tiêu:

- Có sản phẩm chạy thật
- 10–30 thương lái dùng thử
- Thu thập feedback
- Chuẩn bị scale

---

# 1. Team đề xuất

## Core team

1 Product / Tech Lead  
2 Backend Engineers  
1 Mobile Engineer  
1 UI/UX Designer  

Optional:

1 QA part-time

---

# 2. Công nghệ

Backend

```
.NET 8
ABP Framework
PostgreSQL
Redis
```

Mobile

```
Flutter
```

Infra

```
Docker
Nginx
AWS / VPS
```

---

# 3. Sprint Structure

90 ngày ≈ **12 tuần**

Chia thành:

- Sprint 1–4: Core system
- Sprint 5–8: Business features
- Sprint 9–12: Production readiness

---

# 4. Sprint 1–2 (Week 1–2)

## Setup nền tảng

Tasks

- setup git repo
- setup CI/CD
- setup docker
- create database schema
- create base project structure

Modules

- tenant
- user
- authentication

Deliverable

- login system
- multi-tenant base

---

# 5. Sprint 3–4 (Week 3–4)

## CRM Module

Tables

- farmers
- customers

Features

- create farmer
- update farmer
- list farmers

Mobile

- farmer list screen

Deliverable

CRM basic working

---

# 6. Sprint 5–6 (Week 5–6)

## Collection Module

Features

- create session
- create purchase order
- weighing ticket

Mobile

- quick purchase screen

Deliverable

Thu mua hoạt động

---

# 7. Sprint 7–8 (Week 7–8)

## Debt Management

Features

- farmer debt ledger
- advances
- payment

Mobile

- debt screen

Deliverable

Công nợ hoạt động

---

# 8. Sprint 9–10 (Week 9–10)

## Inventory Module

Features

- create lot
- inventory tracking
- warehouse

Deliverable

Kho cơ bản

---

# 9. Sprint 11 (Week 11)

## Sales Module

Features

- create sales order
- update inventory
- customer debt

Deliverable

Luồng bán hàng

---

# 10. Sprint 12 (Week 12)

## Production readiness

Tasks

- performance tuning
- logging
- monitoring
- backup
- security review

Deliverable

MVP production ready

---

# 11. MVP Features Summary

Modules:

- authentication
- CRM
- collection
- debt management
- inventory
- sales

---

# 12. Success Criteria

MVP thành công nếu:

- 10 thương lái dùng thật
- mỗi ngày có giao dịch
- feedback tích cực

---

# 13. Next Steps

Sau MVP:

- optimize UX
- build analytics
- add logistics
- add worker payroll
