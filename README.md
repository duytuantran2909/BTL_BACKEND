# Community Hall Manager (Backend + Demo UI)

Project bài tập lớn quản lý **Nhà văn hóa – Phòng – Đặt lịch – Hóa đơn – Thanh toán – Báo cáo**.

- **Backend**: ASP.NET Core Web API (.NET 8)
- **Database**: SQL Server (EF Core Migrations)
- **Demo UI**: `index.html` (gọi API ở `http://localhost:5180/api`)

---

## Yêu cầu môi trường

- .NET SDK 8.x
- SQL Server (khuyến nghị: SQL Server Express)

Tùy chọn (để chạy migrations bằng CLI):
- `dotnet-ef` tool

---

## Chạy nhanh (khuyến nghị khi nộp/chấm)

### 1) Cấu hình connection string

Mở file `CommunityHallManager/appsettings.json` và chỉnh:

- `ConnectionStrings:DefaultConnection`

Mặc định đang là ví dụ SQL Express:

```json
"Server=DESKTOP-Q4MLK51\\SQLEXPRESS;Database=CommunityHallManagerEf;Trusted_Connection=True;TrustServerCertificate=True;"
```

Trên máy khác, chỉ cần đổi `Server=...` cho đúng instance SQL Server.

### 2) Tạo database (EF Core)

Mở terminal tại thư mục repo và chạy:

```bash
dotnet tool install --global dotnet-ef
dotnet ef database update --project "d:\li\Backend\CommunityHallManager\CommunityHallManager.csproj"
```

Nếu máy đã có `dotnet-ef` thì bỏ dòng install.

### 3) Chạy backend

```bash
dotnet run --project "d:\li\Backend\CommunityHallManager\CommunityHallManager.csproj"
```

Backend chạy mặc định:
- API: `http://localhost:5180/api`

---

## Reset database + tạo dữ liệu mẫu (seed)

Project có hỗ trợ **reset database và tạo sẵn dữ liệu demo** (Nhà văn hóa, Phòng, Đặt lịch, Hóa đơn, Thanh toán, Doanh thu) bằng cấu hình:

- `Database:ResetOnStartup` trong `CommunityHallManager/appsettings.json`

### Cách dùng

1) Mở `CommunityHallManager/appsettings.json` và đổi:

```json
"Database": {
  "ResetOnStartup": true
}
```

2) Chạy backend 1 lần:

```bash
dotnet run --project "d:\li\Backend\CommunityHallManager\CommunityHallManager.csproj"
```

3) Sau khi thấy backend chạy lên, **đổi lại `ResetOnStartup` về `false`** để tránh bị reset DB mỗi lần chạy:

```json
"Database": {
  "ResetOnStartup": false
}
```

---

## Chạy giao diện demo `index.html`

File demo UI nằm ở: `d:\li\Backend\index.html`

Do trình duyệt có thể chặn gọi API khi mở file dạng `file://`, khuyến nghị mở bằng 1 web server tĩnh.

### Dùng VS Code Live Server
- Mở folder `d:\li\Backend`
- Right click `index.html` → **Open with Live Server**


Sau đó mở:
- `http://localhost:5500/index.html`

> Ghi chú: Backend đã bật CORS `AllowAll` nên `index.html` có thể gọi API ở port 5180.

---

## Các chức năng chính (tóm tắt)

- **Nhà văn hóa**: CRUD
- **Phòng**: CRUD
- **Thiết bị**: CRUD
- **Loại hoạt động**: CRUD
- **Đặt lịch**:
  - Tạo booking có kiểm tra trùng lịch
  - Duyệt / Từ chối / Hoàn thành
  - Hủy booking (đổi trạng thái)
  - Xóa cứng booking (chỉ khi chưa có hóa đơn)
- **Hóa đơn**:
  - Tạo hóa đơn từ booking
  - Hủy hóa đơn
  - Xóa hóa đơn (chỉ khi chưa có thanh toán)
- **Thanh toán**:
  - Ghi nhận thanh toán (không cho vượt quá tổng tiền)
  - Xóa thanh toán (tự cập nhật lại trạng thái hóa đơn)
- **Báo cáo**: Doanh thu theo tháng, lượt đặt theo tháng

---

## API endpoints (chính)

- Halls: `/api/halls`
- Rooms: `/api/rooms`
- Equipments: `/api/equipments`
- Activity types: `/api/activity-types`
- Bookings: `/api/bookings`
- Invoices: `/api/invoices`
- Payments: `/api/payments`
- Reports: `/api/reports`

