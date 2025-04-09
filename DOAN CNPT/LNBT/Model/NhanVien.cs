namespace LNBT.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NhanVien")]
    public partial class NhanVien
    {
        [Key]
        public int MaNhanVien { get; set; }

        [Required]
        [StringLength(100)]
        public string TenNhanVien { get; set; }

        [StringLength(50)]
        public string VaiTro { get; set; }

        public decimal? Luong { get; set; }

        [StringLength(50)]
        public string CaLamViec { get; set; }

        [StringLength(15)]
        public string SoDienThoai { get; set; }

        [StringLength(200)]
        public string DiaChi { get; set; }

        [Column(TypeName = "date")]
        public DateTime? NgayBatDau { get; set; }

        [StringLength(50)]
        public string TrangThai { get; set; }
    }
}
