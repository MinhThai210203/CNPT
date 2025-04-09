using System.Data.Entity;

namespace LNBT.Model
{
    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=QLTSDBContext")
        {
            // Enable Lazy Loading  
            Configuration.LazyLoadingEnabled = true;
        }

        public virtual DbSet<ChiTietDonHang> ChiTietDonHangs { get; set; }

        public virtual DbSet<DonHang> DonHangs { get; set; }

        public virtual DbSet<HoaDon> HoaDons { get; set; }

        public virtual DbSet<KhachHang> KhachHangs { get; set; }

        public virtual DbSet<NhanVien> NhanViens { get; set; }

        public virtual DbSet<SanPham> SanPhams { get; set; }

        public virtual DbSet<TKNhanVien> TKNhanViens { get; set; }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<DanhMuc> DanhMucs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChiTietDonHang>()
                .Property(e => e.Gia)
                .HasPrecision(10, 2);

            modelBuilder.Entity<ChiTietDonHang>()
                .Property(e => e.ThanhTien)
                .HasPrecision(21, 2);

            modelBuilder.Entity<DonHang>()
                .Property(e => e.TongTien)
                .HasPrecision(10, 2);

            modelBuilder.Entity<HoaDon>()
                .Property(e => e.TongTien)
                .HasPrecision(10, 2);

            modelBuilder.Entity<NhanVien>()
                .Property(e => e.Luong)
                .HasPrecision(10, 2);

            modelBuilder.Entity<SanPham>()
                .Property(e => e.Gia)
                .HasPrecision(10, 2);

            modelBuilder.Entity<SanPham>()
                .Property(e => e.KhuyenMai)
                .HasPrecision(5, 2);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.UserName)
                .IsUnique();

            modelBuilder.Entity<SanPham>()
                .HasRequired(sp => sp.DanhMuc)
                .WithMany(dm => dm.SanPhams)
                .HasForeignKey(sp => sp.LoaiSanPham);
        }
    }
}
