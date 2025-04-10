using LNBT.Model;
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
    public partial class FrmThongTinNguoiMua : Form
    {
        private DonHang _donHang;
        public FrmThongTinNguoiMua(DonHang donHang)
        {
            InitializeComponent();
            _donHang = donHang;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string tenKhachHang = tbTenKh.Text;
            string sdt = tbSdt.Text;
            string email = tbEmail.Text;
            DateTime ngaySinh = dtNgaySinh.Value;
            decimal diemConLai = 0 ;
            int diemDaDung = 0;

            using (Model1 db = new Model1())
            {
                KhachHang khachHang = db.KhachHangs.SingleOrDefault(x => x.SoDienThoai == sdt);
                bool isUsePoint = cbDiemTichLuy.Checked;
                if (khachHang != null)
                {
                    DonHang donHang1 = db.DonHangs.SingleOrDefault(x => x.MaDonHang == _donHang.MaDonHang);
                    donHang1.MaKhachHang = khachHang.MaKhachHang;
                    donHang1.TrangThaiDonHang = "Hoàn thành";
                    khachHang.DiemTichLuy += (int)(_donHang.TongTien * (decimal)0.05);
                    HoaDon hoaDon1 = new HoaDon
                    {
                        MaDonHang = _donHang.MaDonHang,
                        NgayLapHoaDon = DateTime.Now,
                        TongTien = _donHang.TongTien,
                        TrangThaiThanhToan = "Hoàn thành"
                    };

                    if (isUsePoint)
                    {
                        decimal tongTien = _donHang.TongTien > khachHang.DiemTichLuy ? _donHang.TongTien - khachHang.DiemTichLuy : 0;
                        diemDaDung = (int)(hoaDon1.TongTien - tongTien);
                        diemConLai = _donHang.TongTien > khachHang.DiemTichLuy ? 0 : khachHang.DiemTichLuy - _donHang.TongTien;
                        khachHang.DiemTichLuy = (int)diemConLai;
                        hoaDon1.TongTien = tongTien;
                    }

                    db.HoaDons.Add(hoaDon1);
                    db.SaveChanges();
                    MessageBox.Show("Đã cộng điểm tích lũy cho khách hàng");
                    FrmHoaDon formHoaDon = new FrmHoaDon(hoaDon1, diemDaDung);
                    formHoaDon.ShowDialog();
                    this.Close();
                    return;
                }

                if (string.IsNullOrEmpty(tenKhachHang) || string.IsNullOrEmpty(sdt) || string.IsNullOrEmpty(email))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin");
                    return;
                }
                khachHang = new KhachHang
                {
                    TenKhachHang = tenKhachHang,
                    SoDienThoai = sdt,
                    Email = email,
                    NgaySinh = ngaySinh,
                    DiemTichLuy = (int)(_donHang.TongTien * (decimal)0.05)
                };
                db.KhachHangs.Add(khachHang);
                db.SaveChanges();
                DonHang donHang = db.DonHangs.SingleOrDefault(x => x.MaDonHang == _donHang.MaDonHang);
                donHang.MaKhachHang = khachHang.MaKhachHang;
                donHang.TrangThaiDonHang = "Hoàn thành";
                MessageBox.Show("Thêm thành công");
                HoaDon hoaDon = new HoaDon
                {
                    MaDonHang = _donHang.MaDonHang,
                    NgayLapHoaDon = DateTime.Now,
                    TongTien = _donHang.TongTien,
                    TrangThaiThanhToan = "Hoàn thành"

                };
                db.HoaDons.Add(hoaDon);

                if (isUsePoint)
                {
                    decimal tongTien = _donHang.TongTien > khachHang.DiemTichLuy ? _donHang.TongTien - khachHang.DiemTichLuy : 0;
                    diemDaDung = (int)(hoaDon.TongTien - tongTien);
                    diemConLai = _donHang.TongTien > khachHang.DiemTichLuy ? 0 : khachHang.DiemTichLuy - _donHang.TongTien;
                    khachHang.DiemTichLuy = (int)diemConLai;
                    hoaDon.TongTien = tongTien;
                }

                db.SaveChanges();
                FrmHoaDon f = new FrmHoaDon(hoaDon, diemDaDung);
                f.ShowDialog();
                this.Close();
            }
        }
    }
}
