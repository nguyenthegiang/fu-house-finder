using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace BusinessObjects
{
    public partial class FUHouseFinderContext : DbContext
    {
        public FUHouseFinderContext()
        {
        }

        public FUHouseFinderContext(DbContextOptions<FUHouseFinderContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Campus> Campuses { get; set; }
        public virtual DbSet<Commune> Communes { get; set; }
        public virtual DbSet<District> Districts { get; set; }
        public virtual DbSet<House> Houses { get; set; }
        public virtual DbSet<ImagesOfHouse> ImagesOfHouses { get; set; }
        public virtual DbSet<ImagesOfRoom> ImagesOfRooms { get; set; }
        public virtual DbSet<Rate> Rates { get; set; }
        public virtual DbSet<Report> Reports { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<RoomType> RoomTypes { get; set; }
        public virtual DbSet<Status> Statuses { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<Village> Villages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("server=MSI\\SQLEXPRESS;database=FUHouseFinder;uid=sa;pwd=sa");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Address>(entity =>
            {
                entity.Property(e => e.Addresses)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("createdDate");

                entity.Property(e => e.UpdatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("updatedDate");
            });

            modelBuilder.Entity<Campus>(entity =>
            {
                entity.Property(e => e.CampusName).HasMaxLength(100);
            });

            modelBuilder.Entity<Commune>(entity =>
            {
                entity.Property(e => e.CommuneName).HasMaxLength(100);

                entity.HasOne(d => d.District)
                    .WithMany(p => p.Communes)
                    .HasForeignKey(d => d.DistrictId)
                    .HasConstraintName("DistrictId_in_District");
            });

            modelBuilder.Entity<District>(entity =>
            {
                entity.Property(e => e.DistrictName).HasMaxLength(100);
            });

            modelBuilder.Entity<House>(entity =>
            {
                entity.Property(e => e.Address).HasMaxLength(500);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("createdDate");

                entity.Property(e => e.CreatedUser)
                    .HasMaxLength(30)
                    .HasColumnName("createdUser")
                    .IsFixedLength(true);

                entity.Property(e => e.HouseName).HasMaxLength(100);

                entity.Property(e => e.LandlordId)
                    .HasMaxLength(30)
                    .IsFixedLength(true);

                entity.Property(e => e.UpdatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("updatedDate");

                entity.Property(e => e.UpdatedUser)
                    .HasMaxLength(30)
                    .HasColumnName("updatedUser")
                    .IsFixedLength(true);

                entity.HasOne(d => d.Campus)
                    .WithMany(p => p.Houses)
                    .HasForeignKey(d => d.CampusId)
                    .HasConstraintName("CampusId_in_Campus");

                entity.HasOne(d => d.CreatedUserNavigation)
                    .WithMany(p => p.HouseCreatedUserNavigations)
                    .HasForeignKey(d => d.CreatedUser)
                    .HasConstraintName("createdUser_in_User2");

                entity.HasOne(d => d.Landlord)
                    .WithMany(p => p.HouseLandlords)
                    .HasForeignKey(d => d.LandlordId)
                    .HasConstraintName("LandlordId_in_User");

                entity.HasOne(d => d.UpdatedUserNavigation)
                    .WithMany(p => p.HouseUpdatedUserNavigations)
                    .HasForeignKey(d => d.UpdatedUser)
                    .HasConstraintName("updatedUser_in_User2");

                entity.HasOne(d => d.Village)
                    .WithMany(p => p.Houses)
                    .HasForeignKey(d => d.VillageId)
                    .HasConstraintName("VillageId_in_Village");
            });

            modelBuilder.Entity<ImagesOfHouse>(entity =>
            {
                entity.HasKey(e => e.ImageId)
                    .HasName("PK__ImagesOf__7516F70C4E03FB3D");

                entity.ToTable("ImagesOfHouse");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("createdDate");

                entity.Property(e => e.CreatedUser)
                    .HasMaxLength(30)
                    .HasColumnName("createdUser")
                    .IsFixedLength(true);

                entity.Property(e => e.ImageLink).HasMaxLength(500);

                entity.Property(e => e.UpdatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("updatedDate");

                entity.Property(e => e.UpdatedUser)
                    .HasMaxLength(30)
                    .HasColumnName("updatedUser")
                    .IsFixedLength(true);

                entity.HasOne(d => d.CreatedUserNavigation)
                    .WithMany(p => p.ImagesOfHouseCreatedUserNavigations)
                    .HasForeignKey(d => d.CreatedUser)
                    .HasConstraintName("createdUser_in_User5");

                entity.HasOne(d => d.House)
                    .WithMany(p => p.ImagesOfHouses)
                    .HasForeignKey(d => d.HouseId)
                    .HasConstraintName("HouseId_in_House3");

                entity.HasOne(d => d.UpdatedUserNavigation)
                    .WithMany(p => p.ImagesOfHouseUpdatedUserNavigations)
                    .HasForeignKey(d => d.UpdatedUser)
                    .HasConstraintName("updatedUser_in_User5");
            });

            modelBuilder.Entity<ImagesOfRoom>(entity =>
            {
                entity.HasKey(e => e.ImageId)
                    .HasName("PK__ImagesOf__7516F70CEB1E393C");

                entity.ToTable("ImagesOfRoom");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("createdDate");

                entity.Property(e => e.CreatedUser)
                    .HasMaxLength(30)
                    .HasColumnName("createdUser")
                    .IsFixedLength(true);

                entity.Property(e => e.ImageLink).HasMaxLength(500);

                entity.Property(e => e.UpdatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("updatedDate");

                entity.Property(e => e.UpdatedUser)
                    .HasMaxLength(30)
                    .HasColumnName("updatedUser")
                    .IsFixedLength(true);

                entity.HasOne(d => d.CreatedUserNavigation)
                    .WithMany(p => p.ImagesOfRoomCreatedUserNavigations)
                    .HasForeignKey(d => d.CreatedUser)
                    .HasConstraintName("createdUser_in_User6");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.ImagesOfRooms)
                    .HasForeignKey(d => d.RoomId)
                    .HasConstraintName("RoomId_in_Room");

                entity.HasOne(d => d.UpdatedUserNavigation)
                    .WithMany(p => p.ImagesOfRoomUpdatedUserNavigations)
                    .HasForeignKey(d => d.UpdatedUser)
                    .HasConstraintName("updatedUser_in_User6");
            });

            modelBuilder.Entity<Rate>(entity =>
            {
                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("createdDate");

                entity.Property(e => e.CreatedUser)
                    .HasMaxLength(30)
                    .HasColumnName("createdUser")
                    .IsFixedLength(true);

                entity.Property(e => e.StudentId)
                    .HasMaxLength(30)
                    .IsFixedLength(true);

                entity.Property(e => e.UpdatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("updatedDate");

                entity.Property(e => e.UpdatedUser)
                    .HasMaxLength(30)
                    .HasColumnName("updatedUser")
                    .IsFixedLength(true);

                entity.HasOne(d => d.CreatedUserNavigation)
                    .WithMany(p => p.RateCreatedUserNavigations)
                    .HasForeignKey(d => d.CreatedUser)
                    .HasConstraintName("createdUser_in_User4");

                entity.HasOne(d => d.House)
                    .WithMany(p => p.Rates)
                    .HasForeignKey(d => d.HouseId)
                    .HasConstraintName("HouseId_in_House2");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.RateStudents)
                    .HasForeignKey(d => d.StudentId)
                    .HasConstraintName("StudentId_in_User");

                entity.HasOne(d => d.UpdatedUserNavigation)
                    .WithMany(p => p.RateUpdatedUserNavigations)
                    .HasForeignKey(d => d.UpdatedUser)
                    .HasConstraintName("updatedUser_in_User4");
            });

            modelBuilder.Entity<Report>(entity =>
            {
                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("createdDate");

                entity.Property(e => e.CreatedUser)
                    .HasMaxLength(30)
                    .HasColumnName("createdUser")
                    .IsFixedLength(true);

                entity.Property(e => e.StudentId)
                    .HasMaxLength(30)
                    .IsFixedLength(true);

                entity.Property(e => e.UpdatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("updatedDate");

                entity.Property(e => e.UpdatedUser)
                    .HasMaxLength(30)
                    .HasColumnName("updatedUser")
                    .IsFixedLength(true);

                entity.HasOne(d => d.CreatedUserNavigation)
                    .WithMany(p => p.ReportCreatedUserNavigations)
                    .HasForeignKey(d => d.CreatedUser)
                    .HasConstraintName("createdUser_in_User7");

                entity.HasOne(d => d.House)
                    .WithMany(p => p.Reports)
                    .HasForeignKey(d => d.HouseId)
                    .HasConstraintName("HouseId_in_House4");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.ReportStudents)
                    .HasForeignKey(d => d.StudentId)
                    .HasConstraintName("StudentId_in_User3");

                entity.HasOne(d => d.UpdatedUserNavigation)
                    .WithMany(p => p.ReportUpdatedUserNavigations)
                    .HasForeignKey(d => d.UpdatedUser)
                    .HasConstraintName("updatedUser_in_User7");
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("createdDate");

                entity.Property(e => e.CreatedUser)
                    .HasMaxLength(30)
                    .HasColumnName("createdUser")
                    .IsFixedLength(true);

                entity.Property(e => e.PricePerMonth).HasColumnType("money");

                entity.Property(e => e.RoomName).HasMaxLength(50);

                entity.Property(e => e.UpdatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("updatedDate");

                entity.Property(e => e.UpdatedUser)
                    .HasMaxLength(30)
                    .HasColumnName("updatedUser")
                    .IsFixedLength(true);

                entity.HasOne(d => d.CreatedUserNavigation)
                    .WithMany(p => p.RoomCreatedUserNavigations)
                    .HasForeignKey(d => d.CreatedUser)
                    .HasConstraintName("createdUser_in_User3");

                entity.HasOne(d => d.House)
                    .WithMany(p => p.Rooms)
                    .HasForeignKey(d => d.HouseId)
                    .HasConstraintName("HouseId_in_House");

                entity.HasOne(d => d.RoomType)
                    .WithMany(p => p.Rooms)
                    .HasForeignKey(d => d.RoomTypeId)
                    .HasConstraintName("RoomTypeId_in_RoomType");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Rooms)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("StatusId_in_Status");

                entity.HasOne(d => d.UpdatedUserNavigation)
                    .WithMany(p => p.RoomUpdatedUserNavigations)
                    .HasForeignKey(d => d.UpdatedUser)
                    .HasConstraintName("updatedUser_in_User3");
            });

            modelBuilder.Entity<RoomType>(entity =>
            {
                entity.Property(e => e.RoomTypeName).HasMaxLength(300);
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.Property(e => e.StatusName).HasMaxLength(300);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.UserId)
                    .HasMaxLength(30)
                    .IsFixedLength(true);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("createdDate");

                entity.Property(e => e.CreatedUser)
                    .HasMaxLength(30)
                    .HasColumnName("createdUser")
                    .IsFixedLength(true);

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.FacebookUrl)
                    .HasMaxLength(300)
                    .HasColumnName("FacebookURL");

                entity.Property(e => e.FacebookUserId)
                    .HasMaxLength(300)
                    .IsFixedLength(true);

                entity.Property(e => e.FullName).HasMaxLength(500);

                entity.Property(e => e.GoogleUserId)
                    .HasMaxLength(300)
                    .IsFixedLength(true);

                entity.Property(e => e.IdentityCardBackSideImageLink).HasMaxLength(500);

                entity.Property(e => e.IdentityCardFrontSideImageLink).HasMaxLength(500);

                entity.Property(e => e.Password).HasMaxLength(100);

                entity.Property(e => e.PhoneNumber).HasMaxLength(50);

                entity.Property(e => e.UpdatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("updatedDate");

                entity.Property(e => e.UpdatedUser)
                    .HasMaxLength(30)
                    .HasColumnName("updatedUser")
                    .IsFixedLength(true);

                entity.Property(e => e.Username).HasMaxLength(100);

                entity.HasOne(d => d.CreatedUserNavigation)
                    .WithMany(p => p.InverseCreatedUserNavigation)
                    .HasForeignKey(d => d.CreatedUser)
                    .HasConstraintName("createdUser_in_User");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("RoleId_in_UserRole");

                entity.HasOne(d => d.UpdatedUserNavigation)
                    .WithMany(p => p.InverseUpdatedUserNavigation)
                    .HasForeignKey(d => d.UpdatedUser)
                    .HasConstraintName("updatedUser_in_User");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => e.RoleId)
                    .HasName("PK__UserRole__8AFACE1AEAE2D835");

                entity.Property(e => e.RoleName).HasMaxLength(100);
            });

            modelBuilder.Entity<Village>(entity =>
            {
                entity.Property(e => e.VillageName).HasMaxLength(100);

                entity.HasOne(d => d.Commune)
                    .WithMany(p => p.Villages)
                    .HasForeignKey(d => d.CommuneId)
                    .HasConstraintName("CommuneId_in_Commune");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
