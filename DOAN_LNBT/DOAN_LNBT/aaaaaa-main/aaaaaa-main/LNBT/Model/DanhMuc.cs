using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LNBT.Model
{
    [Table("DanhMuc")]
    public partial class DanhMuc
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Index(IsUnique = true)]
        [StringLength(50)]
        public string TenDanhMuc { get; set; }

        public virtual ICollection<SanPham> SanPhams { get; set; }
    }
}
