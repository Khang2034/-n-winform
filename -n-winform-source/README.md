# Đồ án Quản lý thư viện (WinForms + SQL Server)

## 📁 Cấu trúc
- `WindowsFormsApp1/` – Mã nguồn ứng dụng
- `WindowsFormsApp1/QuanLyThuVien.sql` – Script tạo cơ sở dữ liệu SQL Server

## 🚀 Hướng dẫn chạy
1. Mở SQL Server Management Studio (SSMS)
2. Mở file `QuanLyThuVien.sql`, Run để tạo database `QuanLyThuVien`
3. Mở `WindowsFormsApp1.sln` bằng Visual Studio 2022
4. Build và chạy WinForms App

## ⚙ Cấu hình chuỗi kết nối
```csharp
Data Source=.;Initial Catalog=QuanLyQuanCafe;Integrated Security=True
