using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using websitebenhvien.Models.Enitity;

namespace websitebenhvien.Data
{
    public class MyDbcontext : IdentityDbContext<ApplicationUser>
    {
            public MyDbcontext(DbContextOptions<MyDbcontext> options) : base(options)
            {

            }
            #region DbSet
            public DbSet<Header> Header { get; set; }
            public DbSet<Titleheader> Titleheader { get; set; }
             public DbSet<ApplicationUser> ApplicationUser { get; set; }
             public DbSet<Slidepage> Slidepage { get; set; }

            public DbSet<Footer> Footers { get; set; }

            public DbSet<Logocustomer> Logocustomer { get; set; }   

            public DbSet<Sharecustomer> Sharecustomers { get; set; }

            public DbSet<TimeWork> TimeWork { get; set; }

            public DbSet<Menu> Menus { get; set; }
        public DbSet<Menuchild> Menuchilds { get; set; }

        public DbSet<Categorynews> Categorynews { get; set; }

        public DbSet<News> News { get; set; }

        public DbSet<Categoryproduct> Categoryproducts { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Proimages> Proimages { get; set; }
        #endregion
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
             base.OnModelCreating(modelBuilder);
            //Tiêu đề header
            modelBuilder.Entity<Header>().ToTable("Header");
            modelBuilder.Entity<Header>().HasKey(x => x.Id_header);
            //Tiêu đề title
            modelBuilder.Entity<Titleheader>().ToTable("Titleheader");
            modelBuilder.Entity<Titleheader>().HasKey(x => x.Id_titleheader);
            modelBuilder.Entity<Titleheader>().HasOne(x => x.Header).WithMany(x => x.titleheader).HasForeignKey(x => x.Id_header);
            // Slide
            modelBuilder.Entity<Slidepage>().ToTable("Slidepage");
            modelBuilder.Entity<Slidepage>().HasKey(x => x.Id_slidepage);
            //Footer
            modelBuilder.Entity<Footer>().ToTable("Footer");
            modelBuilder.Entity<Footer>().HasKey(x => x.Id_footer);
            //Logo khách hàng
            modelBuilder.Entity<Logocustomer>().ToTable("Logocustomer");
            modelBuilder.Entity<Logocustomer>().HasKey(x => x.Id_logocustomer);
            // chia sẻ khách hàng
            modelBuilder.Entity<Sharecustomer>().ToTable("Sharecustomer");
            modelBuilder.Entity<Sharecustomer>().HasKey(x => x.Id_share);
            //Thời gian làm việc
            modelBuilder.Entity<TimeWork>().ToTable("TimeWork");
            modelBuilder.Entity<TimeWork>().HasKey(x => x.Id_timework);
            //Menu
            modelBuilder.Entity<Menu>().ToTable("Menu");
            modelBuilder.Entity<Menu>().HasKey(x => x.Id_menu);
            //Menu con
            modelBuilder.Entity<Menuchild>().ToTable("MenuChild");
            modelBuilder.Entity<Menuchild>().HasKey(x => x.Id_MenuChild);
            modelBuilder.Entity<Menuchild>().HasOne(x => x.Menu).WithMany(x => x.Menuchilds).HasForeignKey(x => x.Id_menu);
            //Catorynews
            modelBuilder.Entity<Categorynews>().ToTable("Categorynews");
            modelBuilder.Entity<Categorynews>().HasKey(x => x.Id_Categorynews);
            //News
            modelBuilder.Entity<News>().ToTable("News");
            modelBuilder.Entity<News>().HasKey(x => x.Id_News);
            modelBuilder.Entity<News>().HasOne(x => x.Categorynews).WithMany(x => x.News).HasForeignKey(x => x.Id_Categorynews);
            // danh mục sản phẩm
            modelBuilder.Entity<Categoryproduct>().ToTable("Categoryproduct");
            modelBuilder.Entity<Categoryproduct>().HasKey(x => x.Id_Categoryproduct);
            // sản phẩm
            modelBuilder.Entity<Product>().ToTable("Product");
            modelBuilder.Entity<Product>().HasKey(x => x.Id_product);
            modelBuilder.Entity<Product>().HasOne(x => x.Categoryproduct).WithMany(x => x.Products).HasForeignKey(x => x.Id_Categoryproduct);
            // hình ảnh sản phẩm
            modelBuilder.Entity<Proimages>().ToTable("Proimages");
            modelBuilder.Entity<Proimages>().HasKey(x => x.Id_proimages);
            modelBuilder.Entity<Proimages>().HasOne(x => x.Product).WithMany(x => x.Proimages).HasForeignKey(x => x.Id_product);


        }
    }
}

