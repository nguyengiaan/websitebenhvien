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

        public DbSet<Recruitment> Recruitments { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<Chat> Chats { get; set; }

        public DbSet<Specialty> Specialties { get; set; }

        public DbSet<Postrelate> Postrelates { get; set; }

        public DbSet<ListvideoSpectialty> ListvideoSpectialtys {get;set; }

        public DbSet<Doctor> Doctors { get; set; }

        public DbSet<Feeldoctor> Feeldoctors { get; set; }

        public DbSet<Workschedule> Workschedules { get; set; }

        public DbSet<Worksdoctor> Worksdoctors { get; set; }

        public DbSet<Makeanappointment> Makeanappointments { get; set; }


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
            modelBuilder.Entity<News>().Property(x=>x.Id_News).ValueGeneratedOnAdd();
            modelBuilder.Entity<News>().HasIndex(x => x.Alias_url).IsUnique();
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
            //Tuyển dụng
            modelBuilder.Entity<Recruitment>().ToTable("Recruitment");
            modelBuilder.Entity<Recruitment>().HasKey(x => x.Id_Recruitment);
            //Thông báo
            modelBuilder.Entity<Notification>().ToTable("Notification");
            modelBuilder.Entity<Notification>().HasKey(x => x.Id_Notification);
            // chat
            modelBuilder.Entity<Chat>().ToTable("Chat");
            modelBuilder.Entity<Chat>().HasKey(x => x.Id_chat);
            // chuyên gia 
            modelBuilder.Entity<Specialty>().ToTable("Specialty");
            modelBuilder.Entity<Specialty>().HasKey(x => x.Id_Specialty);
            modelBuilder.Entity<Specialty>().Property(x=>x.Id_Specialty).ValueGeneratedOnAdd();
            // bài viết liên quan
            modelBuilder.Entity<Postrelate>().ToTable("Postrelate");
            modelBuilder.Entity<Postrelate>().HasKey(x => x.Id_Postrelate);
      
            modelBuilder.Entity<Postrelate>().HasOne(x => x.Specialty).WithMany(x => x.Postrelate).HasForeignKey(x => x.Id_Specialty);
            // video chuyên gia
            modelBuilder.Entity<ListvideoSpectialty>().ToTable("ListvideoSpectialty");
            modelBuilder.Entity<ListvideoSpectialty>().HasKey(x => x.Id_ListvideoSpectialty);
            modelBuilder.Entity<ListvideoSpectialty>().HasOne(x => x.Specialty).WithMany(x => x.ListvideoSpectialty).HasForeignKey(x => x.Id_Specialty);
            // bác sĩ
            modelBuilder.Entity<Doctor>().ToTable("Doctor");
            modelBuilder.Entity<Doctor>().HasKey(x => x.Id_doctor);
            modelBuilder.Entity<Doctor>().Property(x => x.Id_doctor).ValueGeneratedOnAdd();
            modelBuilder.Entity<Doctor>().HasOne(x => x.Specialty).WithMany(x => x.Doctor).HasForeignKey(x => x.Id_specialty);
            // chuyên khoa
            modelBuilder.Entity<Feeldoctor>().ToTable("Feeldoctor");
            modelBuilder.Entity<Feeldoctor>().HasKey(x => x.Id_Feeldoctor);
            modelBuilder.Entity<Feeldoctor>().Property(x => x.Id_Feeldoctor).ValueGeneratedOnAdd();
            modelBuilder.Entity<Feeldoctor>().HasOne(x => x.Doctor).WithMany(x => x.Feeldoctor).HasForeignKey(x => x.Id_Doctor);
            // lịch làm việc
            modelBuilder.Entity<Workschedule>().ToTable("Workschedule");
            modelBuilder.Entity<Workschedule>().HasKey(x => x.Id_workschedule);
            modelBuilder.Entity<Workschedule>().HasOne(x => x.Worksdoctor).WithMany(x => x.Workschedules).HasForeignKey(x => x.Id_worksdoctor);
            // công việc bác sĩ
            modelBuilder.Entity<Worksdoctor>().ToTable("Worksdoctor");
            modelBuilder.Entity<Worksdoctor>().HasKey(x => x.Id_worksdoctor);
            modelBuilder.Entity<Worksdoctor>().HasOne(x => x.Doctor).WithMany(x => x.Worksdoctor).HasForeignKey(x => x.Id_doctor);
            // đặt lịch hẹn
            modelBuilder.Entity<Makeanappointment>().ToTable("Makeanappointment");
            modelBuilder.Entity<Makeanappointment>().HasKey(x => x.Id_Make);
            modelBuilder.Entity<Makeanappointment>().HasOne(x => x.Specialty).WithMany(x => x.Makeanappointment).HasForeignKey(x => x.Id_Specialty);








        }
    }
}

