using LNBT.Model;
using LNBT.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LNBT
{
    public partial class FrmThongTinCaNhan : Form
    {
        private TKNhanVien _tKNhanVien;

        public FrmThongTinCaNhan(TKNhanVien tKNhanVien)
        {
            InitializeComponent();
            _tKNhanVien = tKNhanVien;
        }

        private void FrmThongTinCaNhan_Load(object sender, EventArgs e)
        {
            Model1 db = new Model1();
            TKNhanVien tKNhanVien = db.TKNhanViens.SingleOrDefault(x => x.Username == _tKNhanVien.Username);
            NhanVien nhanVien = db.NhanViens.SingleOrDefault(x => x.MaNhanVien == tKNhanVien.EmployeeID);
            txtTenDangNhap.Text = tKNhanVien.Username;
            txtTenHienThi.Text = nhanVien.TenNhanVien;
            txtMatKhau.Text = PasswordUtil.DecryptPassword(tKNhanVien.PasswordHash);
            txtMatKhauMoi.Text = PasswordUtil.DecryptPassword(tKNhanVien.PasswordHash);
            txtNhapLai.Text = PasswordUtil.DecryptPassword(tKNhanVien.PasswordHash);
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtTenDangNhap_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            using(Model1 db = new Model1())
            {
                TKNhanVien tKNhanVien = db.TKNhanViens.SingleOrDefault(x => x.Username == _tKNhanVien.Username);
                tKNhanVien.PasswordHash = PasswordUtil.EncryptPassword(txtMatKhauMoi.Text);
                NhanVien nhanVien = db.NhanViens.SingleOrDefault(x => x.MaNhanVien == tKNhanVien.EmployeeID);
                nhanVien.TenNhanVien = txtTenHienThi.Text;
                if (txtNhapLai.Text != txtMatKhauMoi.Text)
                {
                    MessageBox.Show("Mật khẩu nhập lại không khớp");
                    return;
                }
                db.SaveChanges();
                MessageBox.Show("Cập nhật thành công");
                this.Close();
            }
        }
    }
}
