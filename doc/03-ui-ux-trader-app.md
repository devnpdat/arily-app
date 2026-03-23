# 03. UI/UX Design – Ứng dụng cho Thương Lái Ngoài Vườn

Tài liệu này mô tả **thiết kế UI/UX dành riêng cho thương lái nông sản**, tập trung vào:

- Nhập liệu cực nhanh
- Sử dụng được ngoài vườn
- Hoạt động offline
- Dễ dùng cho người ít quen công nghệ

Nguyên tắc thiết kế:

- 1 giao dịch < 5 giây
- chữ to
- thao tác ít
- dùng được khi trời nắng

---

# 1. Personas (Người dùng chính)

## 1. Thương lái chính

Đặc điểm:

- sử dụng điện thoại nhiều
- ít thời gian
- thao tác nhanh
- thường làm việc ngoài trời

Nhu cầu:

- ghi nhanh giao dịch
- xem công nợ
- xem lời lỗ

---

## 2. Nhân viên cân hàng

Đặc điểm:

- đứng gần cân
- thao tác liên tục

Nhu cầu:

- nhập cân nhanh
- in phiếu ngay

---

## 3. Kế toán

Đặc điểm:

- làm việc tại nhà/kho

Nhu cầu:

- xem công nợ
- báo cáo

---

# 2. Nguyên tắc UI

### chữ lớn

Font >= 16px

### nút lớn

Button >= 48px

### màu sắc đơn giản

- xanh: OK
- đỏ: cảnh báo
- vàng: đang xử lý

---

# 3. Navigation chính

Thanh tab dưới:

1. Thu mua
2. Kho
3. Bán hàng
4. Công nợ
5. Báo cáo

---

# 4. Màn hình chính (Dashboard)

Hiển thị:

- thu mua hôm nay
- tồn kho
- công nợ
- lợi nhuận

Ví dụ:

```
Thu mua hôm nay: 3.2 tấn
Tồn kho: 1.4 tấn
Công nợ nông dân: 120 triệu
```

---

# 5. Màn hình tạo giao dịch thu mua

Flow:

```
Chọn nông dân
↓
Nhập số kg
↓
Nhập giá
↓
Lưu
```

UI:

```
Nông dân: [ chọn ]
Số lượng: [ 500 ]
Đơn giá: [ 62000 ]

Tổng tiền: 31,000,000

[LƯU]
```

---

# 6. Voice Input

Cho phép nói:

```
"Vườn A 500 ký giá 62"
```

AI parse thành:

- farmer
- quantity
- price

---

# 7. Scan QR lô hàng

Flow:

```
Scan QR
↓
Hiện thông tin lô
↓
Cập nhật trạng thái
```

---

# 8. Màn hình kho

Hiển thị:

```
Sầu riêng Ri6
Tồn: 1200 kg
```

Chức năng:

- nhập kho
- xuất kho
- kiểm kê

---

# 9. Màn hình công nợ

Hiển thị:

Danh sách nông dân:

```
Nguyễn Văn A: -30 triệu
Trần Văn B: -12 triệu
```

---

# 10. Màn hình báo cáo

Charts:

- doanh thu
- chi phí
- lợi nhuận

---

# 11. Offline UX

Khi mất mạng:

- hiển thị icon offline
- vẫn nhập giao dịch

Khi có mạng:

- tự sync

---

# 12. Thông báo

Push:

- nhắc trả nợ
- nhắc giao hàng

---

# 13. Dark mode

Hữu ích khi dùng ban đêm.

---

# 14. Accessibility

- chữ to
- tương phản cao

---

# 15. Prototype tool đề xuất

- Figma
- FlutterFlow

---

# 16. Kết luận

Thiết kế UI/UX cần:

- cực đơn giản
- tối ưu thao tác ngoài vườn
- hỗ trợ offline

