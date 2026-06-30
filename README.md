# AppChatRT - Realtime Chat App

Đồ án xây dựng ứng dụng chat realtime bằng C# WinForms, hỗ trợ nhiều client kết nối qua TCP Socket, có Load Balancer điều phối nhiều Chat Server, lưu dữ liệu bằng SQL Server và tích hợp một số tính năng mở rộng như mã hóa tin nhắn, chat riêng, phòng chat, gửi file, hồ sơ người dùng, typing indicator và nhập văn bản bằng giọng nói.

## 1. Tổng quan hệ thống

Ứng dụng gồm 5 project chính:

```text
AppChatRT
├── RealtimeChatClient       # Ứng dụng client WinForms
├── ChatServer               # Chat server TCP xử lý realtime + database
├── ChatServerGUI            # Giao diện quản lý ChatServer
├── ChatLoadBalancer         # Load balancer bản console
└── ChatLoadBalancerGUI      # Giao diện quản lý Load Balancer
```

Mô hình chạy demo:

```text
Client 1 / Client 2
        |
        v
Load Balancer : 8800
        |
        +---- Chat Server 1 : 8888
        |
        +---- Chat Server 2 : 8889
        |
        v
SQL Server - ChatAppDB
```

Load Balancer sử dụng cơ chế **Round-Robin + Health Check** để chọn Chat Server còn sống. Client ban đầu kết nối vào Load Balancer, sau đó được redirect sang một Chat Server cụ thể.

## 2. Công nghệ sử dụng

- C# WinForms
- .NET
- TCP Socket: `TcpClient`, `TcpListener`, `NetworkStream`
- SQL Server
- ADO.NET: `SqlConnection`, `SqlCommand`, `SqlDataReader`
- Guna UI2 WinForms cho giao diện
- RSA + AES cho mã hóa tin nhắn
- SMTP Gmail cho gửi OTP
- System.Speech cho tính năng âm thanh thành văn bản

## 3. Các tính năng chính

### 3.1 Đăng ký, đăng nhập, quên mật khẩu

- Đăng ký tài khoản mới.
- Xác thực email bằng OTP.
- Đăng nhập bằng tài khoản đã lưu trong SQL Server.
- Quên mật khẩu và đặt lại mật khẩu bằng OTP.
- Mật khẩu được hash trước khi lưu.

### 3.2 Chat realtime

- Chat chung giữa nhiều client.
- Client gửi tin nhắn qua TCP Socket.
- Server broadcast tin nhắn đến các client đang online.
- Tin nhắn được lưu vào SQL Server.

### 3.3 Chat riêng

- Chọn user online để chat riêng.
- Tin nhắn riêng chỉ gửi tới người nhận tương ứng.
- Có lưu lịch sử chat riêng trong database.

### 3.4 Room chat

- Tạo phòng chat.
- Join phòng bằng Room ID.
- Rời phòng và quay lại chat chung.
- Lưu lịch sử tin nhắn phòng.

### 3.5 Gửi file

- Chọn người nhận trong danh sách online.
- Gửi file bằng cách mã hóa file sang Base64.
- Giới hạn file nhỏ để demo ổn định.
- File được lưu trong bảng `FileTransfers`.

### 3.6 Hồ sơ cá nhân

- Hiển thị họ tên, email, avatar.
- Cập nhật họ tên và avatar.
- Đổi email bằng OTP.
- Đổi mật khẩu.

### 3.7 Load Balancer

- Client kết nối vào Load Balancer port `8800`.
- Load Balancer chọn server bằng Round-Robin.
- Health Check kiểm tra server `8888`, `8889` còn sống hay không.
- Nếu một server tắt, client mới được route sang server còn lại.

### 3.8 Multi-server sync

Hai Chat Server dùng chung SQL Server để đồng bộ:

- Tin nhắn chung.
- Tin nhắn riêng.
- Tin nhắn phòng.
- File transfer.
- Danh sách user online.

### 3.9 Mã hóa tin nhắn

Ứng dụng có cơ chế bắt tay khóa:

- Client tạo RSA key pair.
- Client gửi public key cho server.
- Server mã hóa AES key bằng RSA public key.
- Client giải mã AES key bằng private key.
- Nội dung chat được mã hóa bằng AES trước khi gửi.

Lưu ý: cơ chế hiện tại giúp bảo vệ dữ liệu truyền/lưu ở mức ứng dụng, nhưng chưa phải end-to-end encryption tuyệt đối vì server vẫn tham gia cấp khóa.

### 3.10 Nhập tin nhắn nhiều dòng

- `Enter`: gửi tin nhắn.
- `Shift + Enter`: xuống dòng trong ô nhập.

## 4. Cơ sở dữ liệu

Database sử dụng:

```text
ChatAppDB
```

Các bảng chính:

```text
Users
Messages
OnlineUsers
FileTransfers
Rooms
RoomMembers
RoomMessages
```

### 4.1 Script tạo database mẫu

```sql
CREATE DATABASE ChatAppDB;
GO

USE ChatAppDB;
GO

CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(256) NOT NULL,
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NULL,
    IsEmailVerified BIT NOT NULL DEFAULT 0,
    AvatarBase64 NVARCHAR(MAX) NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE()
);
GO

CREATE TABLE Messages (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Sender NVARCHAR(50) NOT NULL,
    Receiver NVARCHAR(50) NULL,
    Content NVARCHAR(MAX) NOT NULL,
    SendTime DATETIME NOT NULL DEFAULT GETDATE()
);
GO

CREATE TABLE OnlineUsers (
    Username NVARCHAR(50) NOT NULL PRIMARY KEY,
    ServerPort INT NOT NULL,
    LastSeen DATETIME NOT NULL DEFAULT GETDATE()
);
GO

CREATE TABLE FileTransfers (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Sender NVARCHAR(50) NOT NULL,
    Receiver NVARCHAR(50) NOT NULL,
    FileName NVARCHAR(255) NOT NULL,
    Base64Data NVARCHAR(MAX) NOT NULL,
    SendTime DATETIME NOT NULL DEFAULT GETDATE()
);
GO

CREATE TABLE Rooms (
    RoomId CHAR(6) NOT NULL PRIMARY KEY,
    RoomName NVARCHAR(100) NOT NULL,
    OwnerUsername NVARCHAR(50) NOT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE()
);
GO

CREATE TABLE RoomMembers (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    RoomId CHAR(6) NOT NULL,
    Username NVARCHAR(50) NOT NULL,
    JoinedAt DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT UQ_RoomMembers UNIQUE (RoomId, Username)
);
GO

CREATE TABLE RoomMessages (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    RoomId CHAR(6) NOT NULL,
    Sender NVARCHAR(50) NOT NULL,
    Content NVARCHAR(MAX) NOT NULL,
    SendTime DATETIME NOT NULL DEFAULT GETDATE()
);
GO
```

## 5. Cấu hình `AppConfig.cs`

File cấu hình nằm trong:

```text
RealtimeChatClient/AppConfig.cs
```

Ví dụ cấu hình chạy local trên máy server:

```csharp
namespace RealtimeChatClient
{
    public static class AppConfig
    {
        public static string ConnectionString =
            @"Server=.;Database=ChatAppDB;Trusted_Connection=True;TrustServerCertificate=True;";

        public static string LoadBalancerIp = "127.0.0.1";
        public static int LoadBalancerPort = 8800;

        public static string SmtpHost = "smtp.gmail.com";
        public static int SmtpPort = 587;
        public static string SmtpEmail = "your_email@gmail.com";
        public static string SmtpPassword = "your_app_password";
        public static string SmtpDisplayName = "Realtime Chat App";
    }
}
```

Không nên commit mật khẩu thật hoặc Gmail App Password lên GitHub.


## 6. Cách chạy project

### 6 Chạy local trên cùng một máy

Thứ tự chạy:

```text
1. Mở SQL Server và đảm bảo ChatAppDB đã tồn tại.
2. Chạy ChatServerGUI, nhập port 8888, bấm Start.
3. Mở thêm ChatServerGUI, nhập port 8889, bấm Start.
4. Chạy ChatLoadBalancerGUI, nhập port 8800, bấm Start.
5. Chạy RealtimeChatClient.
6. Client kết nối tới 127.0.0.1:8800.
```

## 7. Hướng phát triển tiếp theo

- Sửa login/register để client không kết nối database trực tiếp mà đi qua ChatServer.
- Nâng cấp Load Balancer từ redirect model sang TCP proxy model.
- Thêm auto reconnect khi server đang giữ client bị tắt.
- Tối ưu gửi file lớn bằng chunk thay vì Base64 một dòng.
- Nâng cấp voice-to-text bằng Whisper hoặc Azure Speech để nhận tiếng Việt tốt hơn.
- Hoàn thiện spell check bằng WPF TextBox hoặc thư viện kiểm tra chính tả riêng.
