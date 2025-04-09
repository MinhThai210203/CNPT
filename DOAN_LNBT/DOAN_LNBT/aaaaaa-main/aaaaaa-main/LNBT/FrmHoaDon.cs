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
    public partial class FrmHoaDon : Form
    {
        private HoaDon _hoaDon;
        private int _diemDaDung;
        public FrmHoaDon(HoaDon hoaDon, int diemDaDung)
        {
            InitializeComponent();
            _hoaDon = hoaDon;
            _diemDaDung = diemDaDung;
        }

        private void FrmHoaDon_Load(object sender, EventArgs e)
        {
            lbTen.Text = _hoaDon.DonHang.KhachHang.TenKhachHang;
            lbDiem.Text = _hoaDon.DonHang.KhachHang.DiemTichLuy.ToString();
            lbNgay.Text = _hoaDon.NgayLapHoaDon.ToString();
            lbTongTien.Text = _hoaDon.TongTien.ToString();
            lbNhanVien.Text = _hoaDon.DonHang.NhanVien.TenNhanVien;
            lbDiemDaDung.Text = _diemDaDung.ToString();

            List<ChiTietDonHang> chiTietDonHangs = _hoaDon.DonHang.ChiTietDonHangs.ToList();

            foreach (var item in chiTietDonHangs)
            {
                ListViewItem listViewItem = new ListViewItem(item.SanPham.TenSanPham);
                listViewItem.SubItems.Add(item.SoLuong.ToString());
                listViewItem.SubItems.Add((item.ThanhTien).ToString());
                listView1.Items.Add(listViewItem);
            }
        }
    }
}
