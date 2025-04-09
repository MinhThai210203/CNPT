CREATE DATABASE QLTS_8
GO  
USE QLTS_8
GO  
  
-- 1. Bảng Nhân viên (Staff)  
CREATE TABLE NhanVien (  
    MaNhanVien INT PRIMARY KEY IDENTITY(1,1),    
    TenNhanVien NVARCHAR(100) NOT NULL,  
    VaiTro NVARCHAR(50),  
    Luong DECIMAL(10, 2),  
    CaLamViec NVARCHAR(50),  
    SoDienThoai NVARCHAR(15),  
    DiaChi NVARCHAR(200),  
    NgayBatDau DATE,  
    TrangThai NVARCHAR(50)  
);  
  
-- 2. Bảng Khách hàng (Customer)  
CREATE TABLE KhachHang (  
    MaKhachHang INT PRIMARY KEY IDENTITY(1,1),   
    TenKhachHang NVARCHAR(100) NOT NULL,  
    SoDienThoai NVARCHAR(15),  
    Email NVARCHAR(100),  
    DiemTichLuy INT DEFAULT 0,  
    NgaySinh DATE  
);  
  
-- 3. Bảng Danh mục (Category)  
CREATE TABLE DanhMuc (  
    Id INT PRIMARY KEY IDENTITY(1,1),  
    TenDanhMuc NVARCHAR(50) UNIQUE  
);  
  
-- 4. Bảng Sản phẩm (Product)  
CREATE TABLE SanPham (  
    MaSanPham INT PRIMARY KEY IDENTITY(1,1),  
    TenSanPham NVARCHAR(50),  
    Gia DECIMAL(10, 2) NOT NULL,  
    LoaiSanPham INT,  
    MoTa NTEXT,  
    SoLuongTonKho INT DEFAULT 0,  
    TrangThai NVARCHAR(50),  
    KhuyenMai DECIMAL(5, 2),  
    FOREIGN KEY (LoaiSanPham) REFERENCES DanhMuc(Id)
);  
  
-- 5. Bảng Đơn hàng (Order)  
CREATE TABLE DonHang (  
    MaDonHang INT PRIMARY KEY IDENTITY(1,1),   
    MaKhachHang INT,  
    MaNhanVien INT,  
    NgayDatHang DATE NOT NULL,  
    TongTien DECIMAL(10, 2) NOT NULL,  
    PhuongThucThanhToan NVARCHAR(50),  
    TrangThaiDonHang NVARCHAR(50),  
    FOREIGN KEY (MaKhachHang) REFERENCES KhachHang(MaKhachHang),  
    FOREIGN KEY (MaNhanVien) REFERENCES NhanVien(MaNhanVien)  
);  
  
-- 6. Bảng Chi tiết đơn hàng (OrderDetail)  
CREATE TABLE ChiTietDonHang (  
    MaChiTietDonHang INT PRIMARY KEY IDENTITY(1,1),   
    MaDonHang INT,  
    MaSanPham INT,  
    SoLuong INT NOT NULL,  
    Gia DECIMAL(10, 2) NOT NULL,  
    ThanhTien DECIMAL(10, 2),  
    FOREIGN KEY (MaDonHang) REFERENCES DonHang(MaDonHang),  
    FOREIGN KEY (MaSanPham) REFERENCES SanPham(MaSanPham)  
);  
  
-- 7. Bảng TKNhanVien 
CREATE TABLE TKNhanVien (  
    EmployeeID INT PRIMARY KEY IDENTITY(1,1),  
    Username NVARCHAR(50) NOT NULL,  
    PasswordHash NVARCHAR(255) NOT NULL,  
    Role NVARCHAR(50),  
    CreatedAt DATETIME NULL DEFAULT GETDATE(),  
    UpdatedAt DATETIME NULL DEFAULT GETDATE()  
); 
  
-- 8. Bảng Hóa đơn (Invoice)  
CREATE TABLE HoaDon (  
    MaHoaDon INT PRIMARY KEY IDENTITY(1,1),   
    MaDonHang INT,  
    NgayLapHoaDon DATE NOT NULL,  
    TongTien DECIMAL(10, 2) NOT NULL,  
    TrangThaiThanhToan NVARCHAR(50),  
    FOREIGN KEY (MaDonHang) REFERENCES DonHang(MaDonHang)  
);  
  
-- Insert vào bảng NhanVien  
INSERT INTO NhanVien (TenNhanVien, VaiTro, Luong, CaLamViec, SoDienThoai, DiaChi, NgayBatDau, TrangThai)  
VALUES  
(N'Nguyễn Văn A', N'Quản lý', 15000000, N'Sáng', '0901234567', N'Hà Nội', '2023-01-01', N'Đang làm'),  
(N'Trần Thị B', N'Thu ngân', 8000000, N'Chiều', '0902234567', N'TP.HCM', '2023-02-01', N'Đang làm');  
SELECT * FROM NhanVien;  
  
-- Insert vào bảng KhachHang  
INSERT INTO KhachHang (TenKhachHang, SoDienThoai, Email, DiemTichLuy, NgaySinh)  
VALUES  
(N'Phạm Văn C', '0903234567', 'c@example.com', 100, '1990-05-15'),  
(N'Lê Thị D', '0904234567', 'd@example.com', 200, '1992-07-20');  
SELECT * FROM KhachHang;  
  
-- Insert vào bảng DanhMuc  
INSERT INTO DanhMuc (TenDanhMuc)  
VALUES  
(N'Trà sữa'),  
(N'Nước ép');  
  
-- Insert vào bảng SanPham  
INSERT INTO SanPham (TenSanPham, Gia, LoaiSanPham, MoTa, TrangThai, KhuyenMai)  
VALUES  
(N'Trà Sữa Truyền Thống', 30000, 1, N'Trà sữa truyền thống với trân châu', N'Còn hàng', 5),  
(N'Trà Đào', 35000, 2, N'Nước ép đào', N'Còn hàng', 10);  
SELECT * FROM SanPham;  
  
-- Insert vào bảng DonHang  
INSERT INTO DonHang (MaKhachHang, MaNhanVien, NgayDatHang, TongTien, TrangThaiDonHang)  
VALUES  
(1, 1, '2024-10-01', 65000, N'Hoàn thành'),  
(2, 2, '2024-10-02', 35000, N'Hoàn thành');  
SELECT * FROM DonHang;  
  
-- Insert vào bảng ChiTietDonHang  
INSERT INTO ChiTietDonHang (MaDonHang, MaSanPham, SoLuong, Gia)  
VALUES  
(1, 1, 2, 30000),  
(1, 2, 1, 35000),  
(2, 2, 1, 35000);  
SELECT * FROM ChiTietDonHang;  
  
-- Insert vào bảng HoaDon  
INSERT INTO HoaDon (MaDonHang, NgayLapHoaDon, TongTien, TrangThaiThanhToan)  
VALUES  
(1, '2024-10-01', 65000, N'Đã thanh toán'),  
(2, '2024-10-02', 35000, N'Đã thanh toán');  
SELECT * FROM HoaDon;  
  
-- Insert vào bảng TKNhanVien  
INSERT INTO TKNhanVien (Username, PasswordHash, Role, CreatedAt, UpdatedAt)  
VALUES  
('user1', 'K1ALiSaA0n9lkSMSzMTFCg==', 'Admin', GETDATE(), GETDATE()),
('user2', 'K1ALiSaA0n9lkSMSzMTFCg==', 'User', GETDATE(), GETDATE());


