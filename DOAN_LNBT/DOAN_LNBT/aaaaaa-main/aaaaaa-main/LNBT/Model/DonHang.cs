namespace LNBT.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DonHang")]
    public partial class DonHang
    {

        [Key]
        public int MaDonHang { get; set; }

        public int? MaKhachHang { get; set; }

        public int? MaNhanVien { get; set; }

        [Column(TypeName = "date")]
        public DateTime NgayDatHang { get; set; }

        public decimal TongTien { get; set; }

        [StringLength(50)]
        public string TrangThaiDonHang { get; set; }

        public virtual KhachHang KhachHang { get; set; }
        public virtual NhanVien NhanVien { get; set; }
        public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; }
    }
}
