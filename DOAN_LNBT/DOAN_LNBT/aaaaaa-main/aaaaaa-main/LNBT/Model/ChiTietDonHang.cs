namespace LNBT.Model
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ChiTietDonHang")]
    public partial class ChiTietDonHang
    {
        [Key]
        public int MaChiTietDonHang { get; set; }

        public int? MaDonHang { get; set; }

        public int? MaSanPham { get; set; }

        public int SoLuong { get; set; }

        public decimal Gia { get; set; }

        public decimal? ThanhTien { get; set; }

        public virtual DonHang DonHang { get; set; }

        public virtual SanPham SanPham { get; set; }
    }
}
