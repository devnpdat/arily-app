# 02. System Architecture – SaaS Quản Lý Thương Lái (Scale 1 Triệu User)

Tài liệu này mô tả **kiến trúc hệ thống backend + hạ tầng** cho ứng dụng quản lý thương lái nông sản với khả năng mở rộng lên **~1,000,000 người dùng**.

Thiết kế theo hướng:

- Cloud native
- Microservice-ready (có thể bắt đầu monolith modular)
- Offline-first mobile
- Multi-tenant SaaS
- Event-driven architecture
- High concurrency

---

# 1. Mục tiêu kiến trúc

Hệ thống cần đáp ứng:

- 1 triệu user
- ~50k user hoạt động đồng thời
- ~5–10 triệu request/ngày
- Mobile sync liên tục
- Search dữ liệu nhanh
- Đảm bảo **idempotency và data integrity**

---

# 2. Kiến trúc tổng thể

```
Mobile App
   │
   ▼
API Gateway
   │
   ▼
Application Services
   │
   ├── PostgreSQL
   ├── Redis
   ├── Elasticsearch
   ├── Kafka
   └── Object Storage
```

---

# 3. Kiến trúc tầng ứng dụng

## 3.1 Client Layer

### Mobile App

Công nghệ đề xuất:

- Flutter

Chức năng:

- Thu mua ngoài vườn
- Offline data entry
- Sync background
- Push notification

### Web Admin

Công nghệ:

- NextJS

Dùng cho:

- quản lý hệ thống
- báo cáo
- kế toán

---

# 4. API Layer

## API Gateway

Chức năng:

- routing request
- authentication
- rate limit
- logging

Công nghệ:

- Azure

---

# 5. Application Layer

Có bắt đầu bằng **Modular Monolith**.

Sau đó tách thành microservices.

### Core services

1. Identity Service
2. Tenant Service
3. CRM Service
4. Collection Service
5. Inventory Service
6. Sales Service
7. Finance Service
8. Worker Service
9. Logistics Service
10. Notification Service

---

# 6. Database Layer

## PostgreSQL

Primary database.

Chứa:

- transactions
- CRM
- accounting
- inventory

### Scale strategy

- read replica
- connection pooling
- partitioning

---

# 7. Cache Layer

## Redis

Dùng cho:

- caching query
- session storage
- rate limit
- distributed lock

Ví dụ cache:

```
farmer-profile
product-list
inventory-summary
price-market
```

---

# 8. Search Layer

## Elasticsearch

Dùng cho:

- search đơn hàng
- search nông dân
- search lô hàng
- analytics

Indexes:

- purchase-order-index
- lot-index
- farmer-index
- sales-order-index

---

# 9. Event Streaming

## Kafka

Dùng cho:

- event-driven processing
- async jobs
- audit stream
- integration

Ví dụ event:

```
purchase.created
inventory.updated
sales.created
debt.updated
notification.created
```

---

# 10. File Storage

Dùng để lưu:

- ảnh giao dịch
- ảnh vườn
- file hóa đơn

Công nghệ:

- AWS S3
- MinIO

---

# 11. Offline Sync Architecture

Mobile có local database:

```
SQLite
```

Flow sync:

```
Mobile
  ↓
Sync API
  ↓
Message Queue
  ↓
Processing Service
  ↓
Database
```

Cơ chế:

- conflict resolution
- idempotency key
- retry queue

---

# 12. Security

### Authentication

- JWT
- OAuth2

### Authorization

- RBAC
- Tenant isolation

### Data protection

- TLS
- encryption at rest

---

# 13. Observability

Monitoring stack:

- Prometheus
- Grafana
- Loki

Logs:

- centralized logging

Tracing:

- OpenTelemetry

---

# 14. CI/CD

Pipeline:

```
GitHub
  ↓
CI Pipeline
  ↓
Docker Build
  ↓
Container Registry
  ↓
Kubernetes Deploy
```

Công cụ:

- GitHub Actions
- Jenkins

---

# 15. Kubernetes Deployment

Cluster gồm:

- API pods
- worker pods
- kafka cluster
- redis cluster
- elasticsearch cluster

Scale bằng:

- HPA
- cluster autoscaling

---

# 16. Scale Strategy

## Horizontal scaling

Scale:

- API servers
- workers
- search nodes

## Database scaling

- read replicas
- partition tables

## Cache scaling

Redis cluster.

---

# 17. Performance Optimization

### Techniques

- caching
- async processing
- batching
- connection pooling
- query optimization

---

# 18. Disaster Recovery

Backup:

- PostgreSQL PITR
- S3 versioning

Multi-zone deployment.

---

# 19. Estimated Infrastructure Cost

Early stage:

```
$200–300 / tháng
```

Growth stage:

```
$800–1500 / tháng
```

1M users scale:

```
$4000–8000 / tháng
```

---

# 20. Kiến trúc đề xuất (Stack)

Backend

```
.NET 8
ABP Framework
```

Database

```
PostgreSQL
```

Cache

```
Redis
```

Search

```
Elasticsearch
```

Event

```
Kafka
```

Storage

```
S3 / MinIO
```

Mobile

```
Flutter
```

---

# 21. Kết luận

Kiến trúc này:

- có thể bắt đầu nhỏ
- scale lên hàng triệu user
- phù hợp SaaS
- phù hợp mobile offline-first

