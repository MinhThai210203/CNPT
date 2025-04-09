using LNBT.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.Entity;
using System.Windows.Forms;
using LNBT.Dto;
using LNBT.Util;
using System.Drawing.Printing;
using System.Runtime.InteropServices.ComTypes;

namespace LNBT
{
    public partial class FrmAdmin : Form
    {
        public FrmAdmin()
        {
            InitializeComponent();
            dtpkTuNgay.Value = DateTime.Now.AddDays(-1);
        }

        private void setPageText(object sender, EventArgs e)
        {
            txtSoTrang.Text = String.Format("{0}/{1}", page.Value, totalPage.Value);
        }
        private void btnNextPage_Click(object sender, EventArgs e)
        {
            if ((int)page.Value < totalPage.Value)
            {
                page.Value++;
            }
            if (dtgvDoanhThu.DataSource == null)
            {
                return;
            }
            GetData((int)page.Value);
        }

        private void btnPrevPage_Click(object sender, EventArgs e)
        {
            if ((int)page.Value > 1)
            {
                page.Value--;
            }
            if (dtgvDoanhThu.DataSource == null)
            {
                return;
            }
            GetData((int)page.Value);
        }

        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            if (dtgvDoanhThu.DataSource == null)
            {
                return;
            }
            page.Value = 1;
            GetData((int)page.Value);
        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {
            if (dtgvDoanhThu.DataSource == null)
            {
                return;
            }
            page.Value = totalPage.Value;
            GetData((int)page.Value);
        }


        private void changePage(int page)
        {
            this.page.Value = page;
        }

        private void GetData(int currentPage)
        {
            using (Model1 db = new Model1())
            {
                DateTime startDate = dtpkTuNgay.Value;
                DateTime endDate = dtpkDenNgay.Value;

                var productRevenue = db.ChiTietDonHangs
                    .Join(db.SanPhams, ctdh => ctdh.MaSanPham, sp => sp.MaSanPham, (ctdh, sp) => new { ctdh, sp })
                    .Join(db.DonHangs, combined => combined.ctdh.MaDonHang, dh => dh.MaDonHang, (combined, dh) => new { combined.ctdh, combined.sp, dh })
                    .Where(order => order.dh.NgayDatHang >= startDate && order.dh.NgayDatHang <= endDate && order.dh.TrangThaiDonHang == "Hoàn thành")
                    .GroupBy(item => new { item.sp.MaSanPham, item.sp.TenSanPham })
                    .Select(group => new
                    {
                        ProductName = group.Key.TenSanPham,
                        TotalRevenue = group.Sum(item => item.ctdh.ThanhTien),
                        TotalUnitsSold = group.Sum(item => item.ctdh.SoLuong),
                        FirstOrderDate = group.Min(item => item.dh.NgayDatHang),
                        LastOrderDate = group.Max(item => item.dh.NgayDatHang)
                    })
                    .OrderByDescending(result => result.TotalRevenue)
                    .Skip((currentPage - 1) * 10)
                    .Take(10)
                    .ToList();
                dtgvDoanhThu.DataSource = productRevenue;
                txtSoTrang.Text = String.Format("{0}/{1}", page.Value, totalPage.Value);
            }
        }

        private void GetProductRevenue(object sender, EventArgs e)
        {
            int pageNumber = (int)page.Value;
            int pageSize = 10;

            DateTime startDate = dtpkTuNgay.Value;
            DateTime endDate = dtpkDenNgay.Value;

            if (startDate > endDate)
            {
                MessageBox.Show("Ngày bắt đầu không được lớn hơn ngày kết thúc.");
                return;
            }

            using (Model1 db = new Model1())
            {

                var productRevenue = db.ChiTietDonHangs
                    .Join(db.SanPhams, ctdh => ctdh.MaSanPham, sp => sp.MaSanPham, (ctdh, sp) => new { ctdh, sp })
                    .Join(db.DonHangs, combined => combined.ctdh.MaDonHang, dh => dh.MaDonHang, (combined, dh) => new { combined.ctdh, combined.sp, dh })
                    .Where(order => order.dh.NgayDatHang >= startDate && order.dh.NgayDatHang <= endDate && order.dh.TrangThaiDonHang == "Hoàn thành")
                    .GroupBy(item => new { item.sp.MaSanPham, item.sp.TenSanPham })
                    .Select(group => new
                    {
                        ProductName = group.Key.TenSanPham,
                        TotalRevenue = group.Sum(item => item.ctdh.ThanhTien),
                        TotalUnitsSold = group.Sum(item => item.ctdh.SoLuong),
                        FirstOrderDate = group.Min(item => item.dh.NgayDatHang),
                        LastOrderDate = group.Max(item => item.dh.NgayDatHang)
                    })
                    .OrderByDescending(result => result.TotalRevenue)
                    .ToList();
                totalPage.Value = (int)Math.Ceiling((double)productRevenue.Count / pageSize);
                label3.Text = productRevenue.Sum(p => p.TotalRevenue).ToString() + " VND";

                if (productRevenue.Count > 0)
                {
                    var product = productRevenue.Skip((pageNumber - 1) * 10).Take(pageSize).ToList();
                    dtgvDoanhThu.DataSource = product;
                }
                else
                {
                    MessageBox.Show("Không tìm thấy sản phẩm nào trong phạm vi ngày nhất định.");
                    dtgvDoanhThu.DataSource = null;

                }

                setPageText(sender, e);

            }
        }


        private void delete_product(object sender, EventArgs e)
        {
            Model1 db = new Model1();
            foreach(DataGridViewRow row in dtgvDoUong.SelectedRows)
            {
                int product_id = Convert.ToInt32(row.Cells[0].Value);
                SanPham product = db.SanPhams.FirstOrDefault(p => p.MaSanPham == product_id);
                if (product != null)
                {
                    db.SanPhams.Remove(product);
                }
                db.SaveChanges();
            }
            MessageBox.Show("Sản phẩm đã được xóa thành công.");
            view_all_product(sender, e);
        }



        private void find_product(object sender, EventArgs e)
        {
            Model1 db = new Model1();
            string productName = txtTim.Text;

            if (string.IsNullOrEmpty(productName))
            {
                MessageBox.Show("Vui lòng nhập tên sản phẩm");
                return;
            } 

            List<ProductDTO> products = db.SanPhams
                    .Where(p => p.TenSanPham.Contains(productName))
                    .Include(sp => sp.DanhMuc)
                    .Select(sp => new ProductDTO
                    {
                        MaSanPham = sp.MaSanPham,
                        TenSanPham = sp.TenSanPham,
                        Gia = sp.Gia,
                        TenDanhMuc = sp.DanhMuc.TenDanhMuc,
                        MoTa = sp.MoTa,
                        TrangThai = sp.TrangThai,
                        KhuyenMai = sp.KhuyenMai
                    })
                    .ToList();

                dtgvDoUong.DataSource = products;
                dtgvDoUong.Columns["MaSanPham"].DataPropertyName = "MaSanPham";
                dtgvDoUong.Columns["TenSanPham"].DataPropertyName = "TenSanPham";
                dtgvDoUong.Columns["Gia"].DataPropertyName = "Gia";
                dtgvDoUong.Columns["TenDanhMuc"].DataPropertyName = "TenDanhMuc"; 
                dtgvDoUong.Columns["MoTa"].DataPropertyName = "MoTa";
                dtgvDoUong.Columns["TrangThai"].DataPropertyName = "TrangThai";
                dtgvDoUong.Columns["KhuyenMai"].DataPropertyName = "KhuyenMai";

        }

        private int convertFromTenDanhMucToId(string tenDanhMuc)
        {
            using (Model1 db = new Model1())
            {
                DanhMuc danhMuc = db.DanhMucs.FirstOrDefault(d => d.TenDanhMuc == tenDanhMuc);
                if (danhMuc != null)
                {
                    return danhMuc.Id;
                }
                return -1;
            }
        }

        private void add_product(object sender, EventArgs e)
        {
            string product_name = txtTenMon.Text;
            string ProductDescription = txtMoTa.Text;
            int ProductCategory = convertFromTenDanhMucToId(cbLoai.Text);
            decimal price = nmGia.Value;
            string ProductStatus = txtTrangThai.Text;

            if (string.IsNullOrEmpty(product_name))
            {
                MessageBox.Show("Vui lòng nhập tên sản phẩm");
                return;
            }

            if (string.IsNullOrEmpty(ProductDescription))
            {
                MessageBox.Show("Vui lòng nhập mô tả sản phẩm");
                return;
            }

            if (string.IsNullOrEmpty(ProductStatus))
            {
                MessageBox.Show("Vui lòng nhập trạng thái của sản phẩm");
                return;
            }

            if (price <= 0)
            {
                MessageBox.Show("Vui lòng nhập giá của sản phẩm");
                return;
            }

            using (Model1 db = new Model1())
            {
                SanPham sanPham = db.SanPhams.FirstOrDefault(p => p.TenSanPham == product_name);
                if (sanPham != null)
                {
                    MessageBox.Show("Sản phẩm đã tồn tại");
                    return;
                }
                sanPham = new SanPham
                {
                    TenSanPham = product_name,
                    MoTa = ProductDescription,
                    LoaiSanPham = ProductCategory,
                    Gia = price,
                    TrangThai = ProductStatus,
                    KhuyenMai = 0
                };

                db.SanPhams.Add(sanPham);
                db.SaveChanges();

                MessageBox.Show("Sản phẩm đã thêm thành công");

            }
            view_all_product(sender, e);
        }

        private void view_all_product(object sender, EventArgs e)
        {
            using (Model1 db = new Model1())
            {
                List<ProductDTO> products = db.SanPhams
                    .Include(sp => sp.DanhMuc)
                    .Select(sp => new ProductDTO
                    {
                        MaSanPham = sp.MaSanPham,
                        TenSanPham = sp.TenSanPham,
                        Gia = sp.Gia,
                        TenDanhMuc = sp.DanhMuc.TenDanhMuc,
                        MoTa = sp.MoTa,
                        TrangThai = sp.TrangThai,
                        KhuyenMai = sp.KhuyenMai
                    })
                    .ToList();

                dtgvDoUong.DataSource = products;
                dtgvDoUong.Columns["MaSanPham"].DataPropertyName = "MaSanPham";
                dtgvDoUong.Columns["TenSanPham"].DataPropertyName = "TenSanPham";
                dtgvDoUong.Columns["Gia"].DataPropertyName = "Gia";
                dtgvDoUong.Columns["TenDanhMuc"].DataPropertyName = "TenDanhMuc"; 
                dtgvDoUong.Columns["MoTa"].DataPropertyName = "MoTa";
                dtgvDoUong.Columns["TrangThai"].DataPropertyName = "TrangThai";
                dtgvDoUong.Columns["KhuyenMai"].DataPropertyName = "KhuyenMai";
                List<string> danhMucNames = db.DanhMucs.Select(d => d.TenDanhMuc).ToList();
                cbLoai.DataSource = danhMucNames;

            }
        }

        private void update_product(object sender, EventArgs e)
        {
            if (dtgvDoUong.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm để cập nhật.");
                return;
            }
            using (Model1 db = new Model1()) { 
                foreach (DataGridViewRow row in dtgvDoUong.SelectedRows)
                {
                    int product_id = Convert.ToInt32(row.Cells[0].Value);
                    int LoaiSanPham = convertFromTenDanhMucToId(row.Cells[3].Value.ToString());
                    SanPham product = db.SanPhams.FirstOrDefault(p => p.MaSanPham == product_id);
                    if (LoaiSanPham == -1)
                    {
                        MessageBox.Show("Loại sản phẩm không tồn tại.");
                        view_all_product(sender, e);
                        return;
                    }
                    if (product != null)
                    {
                        product.TenSanPham = row.Cells[1].Value.ToString();
                        product.Gia = Convert.ToDecimal(row.Cells[2].Value);
                        product.LoaiSanPham = LoaiSanPham;
                        product.MoTa = row.Cells[4].Value.ToString();
                        product.TrangThai = row.Cells[5].Value.ToString();
                        product.KhuyenMai = Convert.ToDecimal(row.Cells[6].Value);
                    }
                    db.SaveChanges();
                }
            MessageBox.Show("Cập nhật sản phẩm thành công");
            }
            view_all_product(sender, e);
        }

        private void btnThemTaiKhoan_Click(object sender, EventArgs e)
        {
            string username = txtTenTk.Text;
            string password = txtMatKhau.Text;
            string role = cbRole.Text;
            string fullname = txtFullName.Text;
            string email = txtEmail.Text;

            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Vui lòng nhập tên tài khoản");
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập password");
                return;
            }

            if (string.IsNullOrEmpty(role))
            {
                MessageBox.Show("Vui lòng chọn Role");
                return;
            }

            if (string.IsNullOrEmpty(fullname))
            {
                MessageBox.Show("Vui lòng nhập họ và tên");
                return;
            }

            if (string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Vui lòng nhập email");
                return;
            }


            using (Model1 db = new Model1())
            {

                TKNhanVien tKNhanVien = new TKNhanVien
                {
                    Username = username,
                    PasswordHash = PasswordUtil.EncryptPassword(password),
                    Role = role,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                NhanVien nhanVien = new NhanVien
                {
                    TenNhanVien = fullname,
                    MaNhanVien = tKNhanVien.EmployeeID,
                    VaiTro = role,
                };

                bool userNameExists = db.TKNhanViens.Any(u => u.Username == tKNhanVien.Username);
                if (userNameExists)
                {
                    MessageBox.Show("Tên tài khoản đã tồn tại.");
                    return;
                }

                db.TKNhanViens.Add(tKNhanVien);
                db.NhanViens.Add(nhanVien);
                db.SaveChanges();

                MessageBox.Show("Tài khoản đã được thêm thành công.");

                dtgvTaiKhoan.DataSource = db.TKNhanViens.ToList();
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void btnXemTaiKhoan_Click(object sender, EventArgs e)
        {
            using (Model1 db = new Model1())
            {
                dtgvTaiKhoan.DataSource = db.TKNhanViens.ToList();
            }
        }

        private void btnXoaTaiKhoan_Click(object sender, EventArgs e)
        {
            if (dtgvTaiKhoan.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn tài khoản để xóa.");
                return;
            }
            using (Model1 db = new Model1())
            {
                List<TKNhanVien> usersToRemove = new List<TKNhanVien>();
                foreach (DataGridViewRow row in dtgvTaiKhoan.SelectedRows)
                {
                    int user_id = Convert.ToInt32(row.Cells[0].Value);
                    TKNhanVien tkNhanvien = db.TKNhanViens.FirstOrDefault(u => u.EmployeeID == user_id);
                    if (tkNhanvien != null)
                    {
                        usersToRemove.Add(tkNhanvien);
                    }
                }

                if (usersToRemove.Count > 0)
                {
                    db.TKNhanViens.RemoveRange(usersToRemove);
                    db.SaveChanges();
                    MessageBox.Show("Tài khoản đã được xóa thành công.");
                }
                else
                {
                    MessageBox.Show("Không có tài khoản nào được chọn để xóa.");
                }

                // Update the data grid view
                dtgvTaiKhoan.DataSource = db.TKNhanViens.ToList();
            }
        }

        private void btnSuaTaiKhoan_Click(object sender, EventArgs e)
        {
            if (dtgvTaiKhoan.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn tài khoản để cập nhật.");
                return;
            }
            using (Model1 db = new Model1())
            {
                foreach (DataGridViewRow row in dtgvTaiKhoan.SelectedRows)
                {
                    int UserId = Convert.ToInt32(row.Cells[0].Value);

                    // Select the user from the database
                    TKNhanVien user = db.TKNhanViens.FirstOrDefault(u => u.EmployeeID == UserId);

                    if (user == null)
                    {
                        MessageBox.Show($"Không tìm thấy người dùng với UserId: {UserId}");
                        return;
                    }

                    // Update the user's information
                    user.Username = row.Cells[1].Value.ToString();
                    user.PasswordHash = PasswordUtil.EncryptPassword(row.Cells[2].Value.ToString());
                    user.Role = row.Cells[3].Value.ToString();

                    // Save the changes to the database
                    db.SaveChanges();
                }
                MessageBox.Show("Tài khoản đã được cập nhật thành công.");

                // Update the data grid view
                dtgvTaiKhoan.DataSource = db.TKNhanViens.ToList();
            }
        }

        private void btnXemDanhMuc_Click(object sender, EventArgs e)
        {
            using(Model1 db = new Model1())
            {
                dtgvDanhMuc.DataSource = db.DanhMucs.OrderBy(d => d.Id).ToList();
                dtgvDanhMuc.Columns["SanPhams"].Visible = false;
            }
        }

        private void btnThemDanhMuc_Click(object sender, EventArgs e)
        {
            
            string name = txtTenDanhMuc.Text;

            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Vui lòng nhập tên danh mục.");
                return;
            }

            using (Model1 db = new Model1()) 
            {
                DanhMuc newCategory = db.DanhMucs.FirstOrDefault(d => d.TenDanhMuc == name);

                if (newCategory != null)
                {
                    MessageBox.Show("Danh mục đã tồn tại.");
                    return;
                }

                newCategory = new DanhMuc
                {
                    TenDanhMuc = name
                };

                db.DanhMucs.Add(newCategory);
                db.SaveChanges();

                MessageBox.Show("Danh mục đã được thêm thành công.");

                dtgvDanhMuc.DataSource = db.DanhMucs.OrderBy(d => d.Id).ToList();
                dtgvDanhMuc.Columns["SanPhams"].Visible = false;

                List<string> danhMucNames = db.DanhMucs.Select(d => d.TenDanhMuc).ToList();
                cbLoai.DataSource = danhMucNames;
            }
        }

        private void btnXoaDanhMuc_Click(object sender, EventArgs e)
        {
            if (dtgvDanhMuc.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn danh mục để xóa.");
                return;
            }
            using (Model1 db = new Model1())
            {
                foreach (DataGridViewRow row in dtgvDanhMuc.SelectedRows)
                {
                    int categoryId = Convert.ToInt32(row.Cells[0].Value);
                    DanhMuc category = db.DanhMucs.FirstOrDefault(d => d.Id == categoryId);
                    if (category == null)
                    {
                        MessageBox.Show($"Không tìm thấy danh mục với Id: {categoryId}");
                        return;
                    }
                    db.DanhMucs.Remove(category);
                }
                db.SaveChanges();
                dtgvDanhMuc.DataSource = db.DanhMucs.OrderBy(d => d.Id).ToList();
                dtgvDanhMuc.Columns["SanPhams"].Visible = false;
                MessageBox.Show("Danh mục đã được xóa thành công.");

                List<string> danhMucNames = db.DanhMucs.Select(d => d.TenDanhMuc).ToList();
                cbLoai.DataSource = danhMucNames;
            }
        }

        private void btnSuaDanhMuc_Click(object sender, EventArgs e)
        {
            if (dtgvDanhMuc.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn danh mục để cập nhật.");
                return;
            }
            using (Model1 db = new Model1())
            {
                foreach (DataGridViewRow row in dtgvDanhMuc.SelectedRows)
                {
                    int categoryId = Convert.ToInt32(row.Cells[0].Value);
                    DanhMuc category = db.DanhMucs.FirstOrDefault(d => d.Id == categoryId);
                    if (category == null)
                    {
                        MessageBox.Show($"Không tìm thấy danh mục với Id: {categoryId}");
                        return;
                    }
                    category.TenDanhMuc = row.Cells[1].Value.ToString();
                }
                db.SaveChanges();
                MessageBox.Show("Danh mục đã được cập nhật thành công.");
                dtgvDanhMuc.DataSource = db.DanhMucs.OrderBy(d => d.Id).ToList();
                dtgvDanhMuc.Columns["SanPhams"].Visible = false;

                List<string> danhMucNames = db.DanhMucs.Select(d => d.TenDanhMuc).ToList();
                cbLoai.DataSource = danhMucNames;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            using(Model1 db = new Model1())
            {
                List<KhachHang> khachHangs = db.KhachHangs.ToList();
                dtgvKhachHang.DataSource = khachHangs;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string tenKhachHang = TbTenKH.Text;
            string soDienThoai = tbSdt.Text;
            string email = tbEmail.Text;
            int diem = Convert.ToInt32(diemTichLuy.Value);
            DateTime ngaySinh = dtpNgaySinh.Value;

            if (string.IsNullOrEmpty(tenKhachHang))
            {
                MessageBox.Show("Vui lòng nhập tên khách hàng");
            }

            if (string.IsNullOrEmpty(soDienThoai))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại khách hàng");
            }

            if (string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Vui lòng nhập email khách hàng");
            }

            using (Model1 db = new Model1())
            {
                KhachHang khachHang = db.KhachHangs.FirstOrDefault(kh => kh.SoDienThoai == soDienThoai);
                if (khachHang != null)
                {
                    MessageBox.Show("Khách hàng đã tồn tại.");
                    return;
                }
                khachHang = new KhachHang
                {
                    TenKhachHang = tenKhachHang,
                    SoDienThoai = soDienThoai,
                    Email = email,
                    DiemTichLuy = diem,
                    NgaySinh = ngaySinh
                };

                db.KhachHangs.Add(khachHang);
                db.SaveChanges();
                MessageBox.Show("Thêm khách hàng thành công.");
                List<KhachHang> khachHangs = db.KhachHangs.ToList();
                dtgvKhachHang.DataSource = khachHangs;
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (Model1 db = new Model1())
            {
                foreach (DataGridViewRow row in dtgvKhachHang.SelectedRows)
                {
                    int maKhachHang = Convert.ToInt32(row.Cells[0].Value);
                    KhachHang khachHang = db.KhachHangs.FirstOrDefault(kh => kh.MaKhachHang == maKhachHang);
                    if (khachHang != null)
                    {
                        db.KhachHangs.Remove(khachHang);
                    }
                }
                db.SaveChanges();
                MessageBox.Show("Xóa khách hàng thành công.");
                List<KhachHang> khachHangs = db.KhachHangs.ToList();
                dtgvKhachHang.DataSource = khachHangs;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            using (Model1 db = new Model1())
            {
                foreach (DataGridViewRow row in dtgvKhachHang.SelectedRows)
                {
                    int maKhachHang = Convert.ToInt32(row.Cells[0].Value);
                    KhachHang khachHang = db.KhachHangs.FirstOrDefault(kh => kh.MaKhachHang == maKhachHang);
                    if (khachHang != null)
                    {
                        khachHang.TenKhachHang = row.Cells[1].Value.ToString();
                        khachHang.SoDienThoai = row.Cells[2].Value.ToString();
                        khachHang.Email = row.Cells[3].Value.ToString();
                        khachHang.DiemTichLuy = Convert.ToInt32(row.Cells[4].Value);
                        khachHang.NgaySinh = Convert.ToDateTime(row.Cells[5].Value);
                    }
                }
                db.SaveChanges();
                MessageBox.Show("Cập nhật khách hàng thành công.");
                List<KhachHang> khachHangs = db.KhachHangs.ToList();
                dtgvKhachHang.DataSource = khachHangs;
            }
        }
    }
}
