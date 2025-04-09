using LNBT.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace LNBT
{
    public partial class FrmMain : Form
    {
        private TKNhanVien _tKNhanVien;

        private decimal sum;
        public FrmMain(TKNhanVien tKNhanVien)
        {
            InitializeComponent();
            _tKNhanVien = tKNhanVien;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmDangNhap FrmDangNhap = new FrmDangNhap();
            FrmDangNhap.Show();
            this.Close();

        }

        private void xemDoanhThuToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void txbThanhTien_TextChanged(object sender, EventArgs e)
        {

        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            List<string> types = selectTypeProduct();
            cbLoaiDoUong.DataSource = types;
            List<string> products = getProductByType();
            cbDoUong.DataSource = products;
        }

        private void ThongTinCaNhanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmThongTinCaNhan f = new FrmThongTinCaNhan(_tKNhanVien);
            f.ShowDialog();
        }

        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_tKNhanVien.Role != "Admin")
            {
                MessageBox.Show("Bạn không có quyền truy cập.");
                return;
            }
            FrmAdmin f = new FrmAdmin();
            f.FormClosed += new FormClosedEventHandler(AdminForm_FormClosed);
            f.ShowDialog();
        }

        private void AdminForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            List<string> types = selectTypeProduct();
            cbLoaiDoUong.DataSource = types;
            List<string> products = getProductByType();
            cbDoUong.DataSource = products;
        }

        private void cbLoaiDoUong_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<string> products = getProductByType();
            cbDoUong.DataSource = products;

        }

        private void cbDoUong_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private List<string> getProductByType()
        {
            using (Model1 db = new Model1())
            {
                List<string> list = new List<string>();
                string type = cbLoaiDoUong.Text;
                foreach (SanPham item in db.SanPhams)
                {
                    if (item.DanhMuc.TenDanhMuc == type)
                    {
                        list.Add(item.TenSanPham);
                    }
                }
                return list;
            }
        }

        private List<string> selectTypeProduct()
        {
            using(Model1 model1 = new Model1())
            {
                List<string> list = new List<string>();
                foreach (var item in model1.DanhMucs)
                {
                    list.Add(item.TenDanhMuc);
                }
                return list;
            }
        }

        private void btnThemMon_Click(object sender, EventArgs e)
        {
            using(Model1 db = new Model1())
            {
                string productName = cbDoUong.Text;
                int quantity = (int)nmSoLuongMon.Value;
                int discount = (int)numericUpDown1.Value;
                var product = db.SanPhams.FirstOrDefault(p => p.TenSanPham == productName);
                decimal price = (product.Gia - discount) * quantity;
                if (discount > product.Gia)
                {
                    MessageBox.Show("Số tiền giảm không được vượt quá giá của sản phẩm");
                    return;
                }
                if (product == null)
                {
                    MessageBox.Show("Không tìm thấy sản phẩm");
                    return;
                }

                ListViewItem item = new ListViewItem(productName);
                item.SubItems.Add(quantity.ToString());
                item.SubItems.Add(product.Gia.ToString());
                item.SubItems.Add(price.ToString());
                listView1.Items.Add(item);
                decimal total = 0;
                sum += price;
                foreach (ListViewItem i in listView1.Items)
                {
                    total += decimal.Parse(i.SubItems[3].Text);
                }
                txbThanhTien.Text = total.ToString();
            }
        }

        private void nmSoLuongMon_ValueChanged(object sender, EventArgs e)
        {
            if (nmSoLuongMon.Value < 1)
            {
                nmSoLuongMon.Value = 1;
            }
        }

        private void btnXoaMon_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một món để xóa.");
                return;
            }

            foreach (ListViewItem item in listView1.SelectedItems)
            {
                listView1.Items.Remove(item);
            }

            decimal total = 0;
            sum = 0;
            foreach (ListViewItem i in listView1.Items)
            {
                total += decimal.Parse(i.SubItems[3].Text);
                sum += decimal.Parse(i.SubItems[3].Text);
            }

            txbThanhTien.Text = total.ToString();
        }

        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn món.");
                return;
            }
            using (Model1 db = new Model1())
            {
                // Thêm đơn hàng
                DonHang donHang = new DonHang()
                {
                    MaKhachHang = 1,
                    MaNhanVien = _tKNhanVien.EmployeeID,
                    NgayDatHang = DateTime.Now,
                    TongTien = sum,
                    TrangThaiDonHang = "Đang thanh toán"
                };

                db.DonHangs.Add(donHang);
                db.SaveChanges();

                foreach (ListViewItem item in listView1.Items)
                {
                    string productName = item.SubItems[0].Text;
                    int quantity = int.Parse(item.SubItems[1].Text);
                    decimal total = decimal.Parse(item.SubItems[3].Text);
                    SanPham product = db.SanPhams.FirstOrDefault(p => p.TenSanPham == productName);
                    if (product == null)
                    {
                        MessageBox.Show("Không tìm thấy sản phẩm");
                        return;
                    }
                    // Thêm chi tiết đơn hàng
                    ChiTietDonHang chiTietDonHang = new ChiTietDonHang()
                    {
                        MaDonHang = donHang.MaDonHang,
                        MaSanPham = product.MaSanPham,
                        SoLuong = quantity,
                        Gia = product.Gia,
                        ThanhTien = total
                    };

                    db.ChiTietDonHangs.Add(chiTietDonHang);
                    db.SaveChanges();
                }
                FrmThongTinNguoiMua f = new FrmThongTinNguoiMua(donHang);
                f.ShowDialog();
                
                listView1.Items.Clear();
                txbThanhTien.Text = "0";
                numericUpDown1.Value = 0;
                sum = 0;
            }
        }
    }
}
