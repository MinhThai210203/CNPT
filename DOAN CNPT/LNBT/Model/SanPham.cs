namespace LNBT.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("SanPham")]
    public partial class SanPham
    {
        [Key]
        public int MaSanPham { get; set; }

        [Required]
        [StringLength(100)]
        public string TenSanPham { get; set; }

        public decimal Gia { get; set; }

        public int LoaiSanPham { get; set; }

        [Column(TypeName = "text")]
        public string MoTa { get; set; }

        [StringLength(50)]
        public string TrangThai { get; set; }

        public decimal? KhuyenMai { get; set; }

        [ForeignKey("LoaiSanPham")]
        public virtual DanhMuc DanhMuc { get; set; }
    }
}