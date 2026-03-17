## BÁO CÁO BÀI TẬP LỚN
| STT | Mã Sinh Viên | Họ và Tên     | Ngày Sinh  | Lớp        |
| 1   | 1876102004   | Trần Duy Tuấn | 29/09/2006 | CNTT 18-01 |
| 2   | 1871020508   | Đỗ Tiến Sơn   | 26/10/2006 | CNTT 18-01 |
### Community Hall Manager 

---

## PHẦN I: MỞ ĐẦU

### 1\. Tên đề tài

“Xây dựng hệ thống quản lý nhà văn hóa (Community Hall Manager)”

### 2\. Tính cấp thiết

Tại các xã/phường, nhà văn hóa và các phòng/hội trường thường xuyên được sử dụng cho họp dân, sinh hoạt cộng đồng… Việc quản lý thủ công dẫn đến:

- Trùng lịch do không kiểm tra giao nhau thời gian  
- Khó theo dõi trạng thái booking, hóa đơn và các lần thanh toán  
- Khó tổng hợp báo cáo (lượt sử dụng, doanh thu theo tháng)

### 3\. Mục tiêu

- Xây dựng Backend REST API quản lý nhà văn hóa, phòng, thiết bị, loại hoạt động, đặt lịch, hóa đơn, thanh toán.  
- Có logic kiểm tra trùng lịch khi tạo/sửa booking.  
- Có báo cáo doanh thu và tần suất sử dụng theo tháng.  
- Có tài liệu chạy demo nhanh phục vụ nộp/chấm.

### 4\. Phạm vi và công nghệ triển khai 

- Backend: ASP.NET Core Web API (.NET 8\)  
- ORM: Entity Framework Core 8  
- Database: Microsoft SQL Server (SQL Server Express)  
- CORS: mở cho mọi origin (AllowAll) để index.html gọi API  
- Frontend demo: file index.html (gọi API tại http://localhost:5180/api)

Lưu ý: phiên bản hiện tại chưa triển khai đăng nhập/phân quyền. Các API tập trung vào nghiệp vụ và demo CRUD.

---

## PHẦN II: PHÂN TÍCH YÊU CẦU & CHỨC NĂNG

### 1\. Chức năng chính

- Quản lý nhà văn hóa (CommunityHall): CRUD  
- Quản lý phòng (Room): CRUD \+ lọc theo nhà văn hóa  
- Quản lý thiết bị (Equipment): CRUD \+ lọc theo nhà văn hóa  
- Quản lý loại hoạt động (ActivityType): CRUD  
- Đặt lịch (BookingRequest):  
  - CRUD cơ bản  
  - Duyệt / từ chối / hoàn thành  
  - Hủy booking (đổi trạng thái)  
  - Xóa cứng booking (chỉ khi chưa có hóa đơn)  
- Hóa đơn (UsageInvoice):  
  - Tạo hóa đơn từ booking (tính giờ \+ tiền phòng \+ phụ phí)  
  - Hủy hóa đơn (đổi trạng thái)  
  - Xóa hóa đơn (chỉ khi chưa có thanh toán)  
- Thanh toán (Payment):  
  - Ghi nhận thanh toán (không cho vượt quá tổng tiền hóa đơn)  
  - Xóa thanh toán (tự cập nhật lại trạng thái hóa đơn)  
- Báo cáo:  
  - Doanh thu theo tháng  
  - Lượt đặt theo tháng

---

## PHẦN III: THIẾT KẾ CƠ SỞ DỮ LIỆU 

### 1\. Danh sách bảng 

Hệ thống có 8 bảng chính:

\- Community\_Hall

| STT | Tên trường  | Kiểu dữ liệu  | Đặc điểm      | Mô tả                                 |
| 1   | HallId      | int           | PK, Identity  | Mã định danh duy nhất của nhà văn hóa |
| 2   | HallName    | nvarchar(150) | Not Null      | Tên gọi nhà văn hóa                   |
| 3   | ManagerName | nvarchar(150) | Not Null      | Địa chỉ chi tiết                      |
| 4   | Address     | nvarchar(150) | Null          | Tên cán bộ quản lý phụ trách          |
| 5   | PhoneNumber | nvarchar(11)  | Null          | Số điện thoại liên hệ                 |
| 6   | Status      | nvarchar(30)  | Default       | Trạng thái (Mặc định: 'DangHoatDong') |
| 7   | CreatedDate | datetime2     | Default       | Ngày khởi tạo hệ thống                |

 

\- Room

| STT | Tên trường       | Kiểu dữ liệu | Đặc điểm        | Mô tả                                   |
| 1   | RoomId           | int          | PK, Identity    | Mã định danh duy nhất của phòng         |
| 2   | HallId           | int          | FK              | Liên kết tới bảng Community\_Hall       |
| 3   | RoomName         | nvarchar(150)| Not Null        | Tên phòng/hội trường                    |
| 4   | RoomType         | nvarchar(50) | Default         | Loại phòng (Mặc định: 'HoiTruong')      |
| 5   | Capacity         | int          | Null            | Sức chứa tối đa                         |
| 6   | BasePricePerHour | decimal(12,2)| Not Null        | Đơn giá thuê trên mỗi giờ sử dụng       |
| 7   | Status           | nvarchar(20) | Default         | Tình trạng phòng (Mặc định: 'SanSang')  |

 

\- Equipment

| STT | Tên trường      | Kiểu dữ liệu | Đặc điểm     | Mô tả                                     |
| 1   | EquipmentId     | int          | PK, Identity | Mã định danh thiết bị                     |
| 2   | HallId          | int          | FK           | Liên kết Community\_Hall (Cascade Delete) |
| 3   | EquipmentName   | nvarchar(150)| Not Null     | Tên thiết bị (Loa, đèn, ghế...)           |
| 4   | Quantity        | int          | Not Null     | Số lượng thiết bị hiện có                 |
| 5   | UnitPricePerUse | decimal(12,2)| Not Null     | Giá thuê thiết bị cho mỗi lần sử dụng     |
| 6   | Status          | nvarchar(30) | Default      | Tình trạng thiết bị (Mặc định: 'SanSang') |

 

 

\- Activity\_Type

| STT | Tên trường            | Kiểu dữ liệu  | Đặc điểm      | Mô tả                                                                               |
| 1   | ActivityId            | int           | PK, Identity  | Mã định danh duy nhất của loại hoạt động.                                           |
| 2   | ActivityName          | nvarchar(150) | Not Null      | Tên loại hình hoạt động (Ví dụ: Hội thảo, Tập văn nghệ, Họp tổ dân phố...).         |
| 3   | Description           | nvarchar(255) | Null          | Mô tả chi tiết hoặc ghi chú về đặc điểm của loại hoạt động này.                     |
| 4   | DefaultDurationHours  | int           | Not Null      | Thời lượng sử dụng dự kiến mặc định (tính theo giờ) để hệ thống gợi ý khi đặt lịch. |
| 5   | Status                | nvarchar(30)  | Default       | Trạng thái của loại hoạt động (Đang sử dụng hoặc Ngừng sử dụng).                    |
| 6   | CreatedDate           | datetime2     | Default       | Ngày giờ danh mục này được tạo trên hệ thống.                                       |

 

\- Booking\_Request

| STT | Tên trường    | Kiểu dữ liệu  | Đặc điểm      | Mô tả                                                 |
| 1   | BookingId     | int           | PK, Identity  | Mã số phiếu đặt lịch                                  |
| 2   | RoomId        | int           | FK            | Liên kết bảng Room (Cascade Delete)                   |
| 3   | ActivityId    | int           | FK            | Liên kết bảng Activity\_Type (Set Null khi xóa loại)  |
| 4   | RequesterName | nvarchar(150) | Not Null      | Tên cá nhân/đơn vị đăng ký                            |
| 5   | StartDateTime | datetime2     | Not Null      | Thời gian bắt đầu sử dụng                             |
| 6   | EndDateTime   | datetime2     | Not Null      | Thời gian kết thúc sử dụng                            |
| 7   | Status        | nvarchar(50)  | Default       | Trạng thái phiếu (Mặc định: 'ChoDuyet')               |
| 8   | CreatedAt     | datetime2     | Default       | Ngày tạo phiếu                                        |

 

\- Usage\_Invoice

| STT | Tên trường    | Kiểu dữ liệu  | Đặc điểm      | Mô tả                                       |
| 1   | InvoiceId     | int           | PK, Identity  | Mã số hóa đơn                               |
| 2   | BookingId     | int           | FK            | Liên kết Booking\_Request (Cascade Delete)  |
| 3   | TotalHours    | decimal(10,2) | Not Null      | Tổng số giờ sử dụng thực tế                 |
| 4   | TotalAmount   | decimal(12,2) | Not Null      | Tổng tiền cuối cùng sau thuế/giảm giá       |
| 5   | InvoiceStatus | nvarchar(30)  | Default       | Trạng thái (Mặc định: 'ChuaThanhToan')      |
| 6   | InvoiceStatus | datetime2     | Default       | Ngày xuất hóa đơn                           |

 

 

\- Payment

| STT | Tên trường    | Kiểu dữ liệu  | Đặc điểm      | Mô tả                                     |
| 1   | PaymentId     | int           | PK, Identity  | Mã số giao dịch thanh toán                |
| 2   | InvoiceId     | int           | FK            | Liên kết Usage\_Invoice (Cascade Delete)  |
| 3   | PayerName     | nvarchar(150) | Not Null      | Tên người thực hiện trả tiền              |
| 4   | AmountPaid    | decimal(12,2) | Not Null      | Số tiền khách đã thanh toán thực tế       |
| 5   | PaymentMethod | nvarchar(30)  | Default       | Hình thức (Mặc định: 'TienMat')           |

### 2\. Quan hệ chính

CommunityHall (1) ── (N) Room

CommunityHall (1) ── (N) Equipment

Room (1) ── (N) BookingRequest

BookingRequest (1) ── (1) UsageInvoice

UsageInvoice (1) ── (N) Payment

BookingRequest (N) ── (1) ActivityType (nullable)

### 3\. Ghi chú về hành vi xóa (DeleteBehavior)

- Xóa CommunityHall sẽ cascade xóa Room và Equipment.  
- Xóa Room sẽ cascade xóa BookingRequest.  
- Xóa BookingRequest sẽ cascade xóa UsageInvoice (và Payment cascade theo hóa đơn).

---

## PHẦN IV: THIẾT KẾ API 

Base URL chạy mặc định: http://localhost:5180

### 1\. Nhóm API danh mục

Nhà văn hóa

- GET /api/halls  
- GET /api/halls/{id}  
- POST /api/halls  
- PUT /api/halls/{id}  
- DELETE /api/halls/{id}

Phòng

- GET /api/rooms  
- GET /api/rooms/{id}  
- GET /api/rooms/by-hall/{hallId}  
- POST /api/rooms  
- PUT /api/rooms/{id}  
- DELETE /api/rooms/{id}

Thiết bị

- GET /api/equipments  
- GET /api/equipments/{id}  
- GET /api/equipments/by-hall/{hallId}  
- POST /api/equipments  
- PUT /api/equipments/{id}  
- DELETE /api/equipments/{id}

Loại hoạt động

- GET /api/activity-types  
- GET /api/activity-types/{id}  
- POST /api/activity-types  
- PUT /api/activity-types/{id}  
- DELETE /api/activity-types/{id}

### 2\. Nhóm API nghiệp vụ

Booking

- GET /api/bookings  
- GET /api/bookings/{id}  
- POST /api/bookings (có kiểm tra trùng lịch)  
- PUT /api/bookings/{id} (có kiểm tra trùng lịch)  
- PUT /api/bookings/{id}/approve  
- PUT /api/bookings/{id}/reject  
- PUT /api/bookings/{id}/complete  
- DELETE /api/bookings/{id} (hủy: đổi trạng thái DaHuy)  
- DELETE /api/bookings/{id}/hard (xóa cứng: chỉ khi chưa có hóa đơn)

Hóa đơn

- GET /api/invoices  
- GET /api/invoices/{id}  
- POST /api/invoices/generate  
- PUT /api/invoices/{id}/cancel  
- DELETE /api/invoices/{id} (chỉ khi chưa có thanh toán)

Thanh toán

- GET /api/payments  
- GET /api/payments/{id}  
- POST /api/payments  
- DELETE /api/payments/{id}

Báo cáo

- GET /api/reports/revenue  
- GET /api/reports/usage

---

## PHẦN V: LOGIC NGHIỆP VỤ TIÊU BIỂU

### 1\. Kiểm tra trùng lịch booking

Booking bị xem là trùng lịch nếu giao nhau theo điều kiện: \[start \< existingEnd\] và \[end \> existingStart\]

Chỉ xét các booking trong trạng thái: ChoDuyet hoặc DaDuyet.

### 2\. Tạo hóa đơn từ booking

- Lấy thời lượng (end \- start) theo giờ.  
- Làm tròn 0.5 giờ.  
- Tổng tiền \= tiền phòng \+ phí thiết bị \+ phí khác − giảm giá.

### 3\. Ràng buộc thanh toán

- Không cho AmountPaid \<= 0\.  
- Không cho tổng tiền đã trả vượt quá TotalAmount.  
- Nếu trả đủ thì cập nhật InvoiceStatus \= DaThanhToan, ngược lại ChuaThanhToan.

---

## KẾT LUẬN

Project hoàn thành các nhóm chức năng chính của bài toán quản lý nhà văn hóa: danh mục, đặt lịch có kiểm tra trùng, hóa đơn – thanh toán, và báo cáo tổng quan. Hệ thống được triển khai bằng ASP.NET Core \+ EF Core \+ SQL Server, có trang demo index.html để thao tác nhanh.  
