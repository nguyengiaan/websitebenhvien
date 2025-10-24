using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using websitebenhvien.Models.Enitity;
using Websitebenhvien.Models.Enitity;

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

        public DbSet<SampleMessage> SampleMessages { get; set; }

        public DbSet<ButtonSample> ButtonSamples { get; set; }

        public DbSet<Permissions> Permissions { get; set; }

        public DbSet<PermissionUser> PermissionUser { get; set; }

        public DbSet<Registerhealth> Registerhealths { get; set; }

        public DbSet<Recruitmentpost> Recruitmentposts { get; set; }

        public DbSet<Videos> Videos { get; set; }

        public DbSet<Machine> Machines { get; set; }
        public DbSet<Postpersonnel> postpersonnel { get; set; }


        public DbSet<Email> Emails { get; set; }


        public DbSet<Forbusiness> Forbusinesses { get; set; }


        public DbSet<MenuAdmin> MenuAdmins { get; set; }
        
        public DbSet<MenuAdminUser> MenuAdminUsers { get; set; }

        public DbSet<Activitycategory> Activitycategories { get; set; }

        public DbSet<Postactivity> Postactivity { get; set; }


        public DbSet<Titlemenu> Titlemenus { get; set; }

        public DbSet<Menunav> Menunavs { get; set; }
        

        public DbSet<Feedback> Feedbacks { get; set; }

        public DbSet<Pricelist> Pricelists { get; set; }


        // hướng dẫn khách hàng 
        public DbSet<Catogeryguider> Catogeryguiders { get; set; }

        public DbSet<Customerguide> Customerguides { get; set; }




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
            modelBuilder.Entity<News>().Property(x => x.Id_News).ValueGeneratedOnAdd();
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
            modelBuilder.Entity<Specialty>().Property(x => x.Id_Specialty).ValueGeneratedOnAdd();
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
            // tin nhắn mẫu
            modelBuilder.Entity<SampleMessage>().ToTable("SampleMessage");
            modelBuilder.Entity<SampleMessage>().HasKey(x => x.Id_SampleMessager);

            // nút mẫu
            modelBuilder.Entity<ButtonSample>().ToTable("ButtonSample");
            modelBuilder.Entity<ButtonSample>().HasKey(x => x.Id_ButtonSample);
            modelBuilder.Entity<ButtonSample>().HasOne(x => x.SampleMessage).WithMany(x => x.ButtonSamples).HasForeignKey(x => x.Id_SampleMessage);
            // quyền 
            modelBuilder.Entity<Permissions>().ToTable("Permissions");
            modelBuilder.Entity<Permissions>().HasKey(x => x.Id);
            // phân quyền
            modelBuilder.Entity<PermissionUser>().ToTable("PermissionUser");
            modelBuilder.Entity<PermissionUser>().HasKey(x => x.id_Permission);
            modelBuilder.Entity<PermissionUser>().HasOne(x => x.Permissions).WithMany(x => x.Users).HasForeignKey(x => x.id_PermissionUser);
            modelBuilder.Entity<PermissionUser>().HasOne(x => x.User).WithMany(x => x.PermissionUsers).HasForeignKey(x => x.id_user);
            // đăng ký khám sức khoẻ 
            modelBuilder.Entity<Registerhealth>().ToTable("Registerhealth");
            modelBuilder.Entity<Registerhealth>().HasKey(x => x.Id_Registerhealth);
            // tuyển dụng
            modelBuilder.Entity<Recruitmentpost>().ToTable("Recruitmentpost");
            modelBuilder.Entity<Recruitmentpost>().HasKey(x => x.id_recruitmentpost);
            // video
            modelBuilder.Entity<Videos>().ToTable("Videos");
            modelBuilder.Entity<Videos>().HasKey(x => x.Id_video);
            // thiết bị 
            modelBuilder.Entity<Machine>().ToTable("Machine");
            modelBuilder.Entity<Machine>().HasKey(x => x.Id_machine);

            // bài viết tuyển dụng nhân sự
            modelBuilder.Entity<Postpersonnel>().ToTable("Postpersonnel");

            modelBuilder.Entity<Postpersonnel>().HasKey(x => x.id_recruitmentpost);
            modelBuilder.Entity<Postpersonnel>().Property(x => x.id_recruitmentpost).ValueGeneratedOnAdd();
            modelBuilder.Entity<Postpersonnel>().Property(x => x.title_recruitmentpost).HasMaxLength(int.MaxValue);
            modelBuilder.Entity<Postpersonnel>().Property(x => x.Content_recruitmentpost).HasMaxLength(int.MaxValue);
            modelBuilder.Entity<Postpersonnel>().Property(x => x.Status).HasMaxLength(int.MaxValue);
            modelBuilder.Entity<Postpersonnel>().Property(x => x.Statuson).HasMaxLength(int.MaxValue);
            modelBuilder.Entity<Postpersonnel>().Property(x => x.Date_recruitmentpost).HasMaxLength(int.MaxValue);

            // Email nhân sự
            modelBuilder.Entity<Email>().ToTable("Email");
            modelBuilder.Entity<Email>().HasKey(x => x.id);
            modelBuilder.Entity<Email>().Property(x => x.email).HasMaxLength(int.MaxValue);
            // Quản lý doanh nghiệp 
            modelBuilder.Entity<Forbusiness>().ToTable("Forbusiness");
            modelBuilder.Entity<Forbusiness>().HasKey(x => x.Id_Forbusiness);

            // Quản lý menu admin
            modelBuilder.Entity<MenuAdmin>().ToTable("MenuAdmin");
            modelBuilder.Entity<MenuAdmin>().HasKey(x => x.id);
            modelBuilder.Entity<MenuAdmin>().Property(x => x.id).ValueGeneratedOnAdd();
            modelBuilder.Entity<MenuAdmin>().Property(x => x.Title).HasMaxLength(100);
            modelBuilder.Entity<MenuAdmin>().Property(x => x.Icon).HasMaxLength(50);
            modelBuilder.Entity<MenuAdmin>().Property(x => x.Url).HasMaxLength(200);
            // danh mục hoạt động
            modelBuilder.Entity<Activitycategory>().ToTable("Activitycategory");
            modelBuilder.Entity<Activitycategory>().HasKey(x => x.Id_activitycategory);
            modelBuilder.Entity<Activitycategory>().Property(x => x.Id_activitycategory).ValueGeneratedOnAdd();
            modelBuilder.Entity<Activitycategory>().Property(x => x.Title).HasMaxLength(100);
            modelBuilder.Entity<Activitycategory>().Property(x => x.Description).HasMaxLength(500);
            // bài viết hoạt động
            modelBuilder.Entity<Postactivity>().ToTable("Postactivity");
            modelBuilder.Entity<Postactivity>().HasKey(x => x.Id_Postactivity);
            modelBuilder.Entity<Postactivity>().Property(x => x.Id_Postactivity).ValueGeneratedOnAdd();
            modelBuilder.Entity<Postactivity>().Property(x => x.Title).HasMaxLength(200);
            modelBuilder.Entity<Postactivity>().Property(x => x.Description).HasMaxLength(int.MaxValue);

            modelBuilder.Entity<Postactivity>().Property(x => x.Alias_url).HasMaxLength(200);
            modelBuilder.Entity<Postactivity>().Property(x => x.Keyword).HasMaxLength(200);
            modelBuilder.Entity<Postactivity>().Property(x => x.Descriptionshort).HasMaxLength(500);
            modelBuilder.Entity<Postactivity>().HasOne(x => x.Activitycategory).WithMany(x => x.Postactivities).HasForeignKey(x => x.Id_Categoryactivity);

            // Tiêu đề menu
            modelBuilder.Entity<Titlemenu>().ToTable("Titlemenu");
            modelBuilder.Entity<Titlemenu>().HasKey(x => x.Id_titlemenu);
            modelBuilder.Entity<Titlemenu>().Property(x => x.Id_titlemenu).ValueGeneratedOnAdd();
            modelBuilder.Entity<Titlemenu>().Property(x => x.Name).HasMaxLength(100);
            // Menu điều hướng
            modelBuilder.Entity<Menunav>().ToTable("Menunav");
            modelBuilder.Entity<Menunav>().HasKey(x => x.Id_menunav);
            modelBuilder.Entity<Menunav>().Property(x => x.Id_menunav).ValueGeneratedOnAdd();
            modelBuilder.Entity<Menunav>().Property(x => x.Name).HasMaxLength(100);
            modelBuilder.Entity<Menunav>().Property(x => x.Url).HasMaxLength(200);
            modelBuilder.Entity<Menunav>().HasOne(x => x.Titlemenu).WithMany(x => x.TitlemenuList).HasForeignKey(x => x.Id_titlemenu);
            // Phản hồi
            modelBuilder.Entity<Feedback>().ToTable("Feedback");
            modelBuilder.Entity<Feedback>().HasKey(x => x.Id);
            modelBuilder.Entity<Feedback>().Property(x => x.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Feedback>().Property(x => x.Title_Name).HasMaxLength(200);
            modelBuilder.Entity<Feedback>().Property(x => x.Content).HasMaxLength(int.MaxValue);
            modelBuilder.Entity<Feedback>().Property(x => x.Thumnail).HasMaxLength(200);
            // Bảng giá 
            modelBuilder.Entity<Pricelist>().ToTable("Pricelist");
            modelBuilder.Entity<Pricelist>().HasKey(x => x.Id_pricelist);
            modelBuilder.Entity<Pricelist>().Property(x => x.Id_pricelist).ValueGeneratedOnAdd();
            modelBuilder.Entity<Pricelist>().Property(x => x.Title).HasMaxLength(200);
            modelBuilder.Entity<Pricelist>().Property(x => x.Description).HasMaxLength(int.MaxValue);
            //Hướng dẫn danh mục khách hàng
            modelBuilder.Entity<Catogeryguider>().ToTable("Catogeryguider");
            modelBuilder.Entity<Catogeryguider>().HasKey(x => x.Id_Catogeryguider);
            modelBuilder.Entity<Catogeryguider>().Property(x => x.Id_Catogeryguider).ValueGeneratedOnAdd();
            // Hướng dẫn khách hàng
            modelBuilder.Entity<Customerguide>().ToTable("Customerguide");
            modelBuilder.Entity<Customerguide>().HasKey(x => x.Id_Customerguide);
            modelBuilder.Entity<Customerguide>().Property(x => x.Id_Customerguide).ValueGeneratedOnAdd();
            modelBuilder.Entity<Customerguide>().HasOne(x => x.Catogeryguider).WithMany(x => x.Customerguide).HasForeignKey(x => x.Id_Catogeryguider);










        }
    }
}

