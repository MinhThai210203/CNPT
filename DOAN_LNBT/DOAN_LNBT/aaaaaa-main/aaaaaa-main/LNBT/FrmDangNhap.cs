using LNBT.Model;
using LNBT.Util;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LNBT
{
    public partial class FrmDangNhap : Form
    {
        public FrmDangNhap()
        {
            InitializeComponent();
        }

        private void FrmDangNhap_Load(object sender, EventArgs e)
        {

        }

        private async void btnDangNhap_Click(object sender, EventArgs e)
        {
            using (Loading loading = new Loading())
            {
                loading.Show();
                string username = txtTenDangNhap.Text;
                string passwordHash = PasswordUtil.EncryptPassword(txtMatKhau.Text);

                TKNhanVien tkNhanVien = await Task.Run(() =>
                {
                    using (Model1 db = new Model1())
                    {
                        return db.TKNhanViens.SingleOrDefault(x => x.Username == username && x.PasswordHash == passwordHash);
                    }
                });

                loading.Close();

                if (tkNhanVien != null)
                {
                    FrmMain f = new FrmMain(tkNhanVien);
                    f.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Đăng nhập thất bại");
                }
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Bạn có muốn thoát !");
            Application.Exit();
        }

        private void linklabelQuenMK_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (txtTenDangNhap.Text == "")
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập để lấy lại mật khẩu");
                return;
            }
            using (Model1 db = new Model1())
            {
                TKNhanVien tkNhanVien = db.TKNhanViens.SingleOrDefault(x => x.Username == txtTenDangNhap.Text);
                if (tkNhanVien == null)
                {
                    MessageBox.Show("Tên đăng nhập không tồn tại");
                    return;
                }
                tkNhanVien.PasswordHash = PasswordUtil.EncryptPassword("1234");
                MessageBox.Show("Mật khẩu mới của bạn là: " + 1234);
                db.SaveChanges();

            }
        }
    }
}
