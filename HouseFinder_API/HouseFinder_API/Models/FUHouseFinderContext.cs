using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

#nullable disable

namespace HouseFinder_API.Models
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

        public virtual DbSet<Campus> Campuses { get; set; }
        public virtual DbSet<Commune> Communes { get; set; }
        public virtual DbSet<District> Districts { get; set; }
        public virtual DbSet<House> Houses { get; set; }
        public virtual DbSet<ImageOfHouse> ImageOfHouses { get; set; }
        public virtual DbSet<ImageOfRoom> ImageOfRooms { get; set; }
        public virtual DbSet<Rate> Rates { get; set; }
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
                //Read JSON File -> ConnectionString
                var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
                optionsBuilder.UseSqlServer(config.GetConnectionString("DBContext"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Campus>(entity =>
            {
                entity.ToTable("Campus");

                entity.Property(e => e.CampusName).HasMaxLength(100);
            });

            modelBuilder.Entity<Commune>(entity =>
            {
                entity.ToTable("Commune");

                entity.Property(e => e.CommunetName).HasMaxLength(100);

                entity.HasOne(d => d.District)
                    .WithMany(p => p.Communes)
                    .HasForeignKey(d => d.DistrictId)
                    .HasConstraintName("DistrictId_in_District");
            });

            modelBuilder.Entity<District>(entity =>
            {
                entity.ToTable("District");

                entity.Property(e => e.DistrictName).HasMaxLength(100);
            });

            modelBuilder.Entity<House>(entity =>
            {
                entity.ToTable("House");

                entity.Property(e => e.Address).HasMaxLength(500);

                entity.Property(e => e.HouseName).HasMaxLength(100);

                entity.Property(e => e.LandlordId)
                    .HasMaxLength(30)
                    .IsFixedLength(true);

                entity.HasOne(d => d.Landlord)
                    .WithMany(p => p.Houses)
                    .HasForeignKey(d => d.LandlordId)
                    .HasConstraintName("LandlordId_in_User");

                entity.HasOne(d => d.Village)
                    .WithMany(p => p.Houses)
                    .HasForeignKey(d => d.VillageId)
                    .HasConstraintName("VillageId_in_Village");
            });

            modelBuilder.Entity<ImageOfHouse>(entity =>
            {
                entity.HasKey(e => e.ImageId)
                    .HasName("PK__ImageOfH__7516F70C6ACC96D6");

                entity.ToTable("ImageOfHouse");

                entity.Property(e => e.ImageLink).HasMaxLength(500);

                entity.HasOne(d => d.House)
                    .WithMany(p => p.ImageOfHouses)
                    .HasForeignKey(d => d.HouseId)
                    .HasConstraintName("HouseId_in_House3");
            });

            modelBuilder.Entity<ImageOfRoom>(entity =>
            {
                entity.HasKey(e => e.ImageId)
                    .HasName("PK__ImageOfR__7516F70C19E6EA71");

                entity.ToTable("ImageOfRoom");

                entity.Property(e => e.ImageLink).HasMaxLength(500);

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.ImageOfRooms)
                    .HasForeignKey(d => d.RoomId)
                    .HasConstraintName("RoomId_in_Room");
            });

            modelBuilder.Entity<Rate>(entity =>
            {
                entity.ToTable("Rate");

                entity.Property(e => e.StudentId)
                    .HasMaxLength(30)
                    .IsFixedLength(true);

                entity.HasOne(d => d.House)
                    .WithMany(p => p.Rates)
                    .HasForeignKey(d => d.HouseId)
                    .HasConstraintName("HouseId_in_House2");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.Rates)
                    .HasForeignKey(d => d.StudentId)
                    .HasConstraintName("StudentId_in_User");
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.ToTable("Room");

                entity.Property(e => e.PricePerMonth).HasColumnType("money");

                entity.Property(e => e.RoomName).HasMaxLength(50);

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
            });

            modelBuilder.Entity<RoomType>(entity =>
            {
                entity.ToTable("RoomType");

                entity.Property(e => e.RoomTypeName).HasMaxLength(300);
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.ToTable("Status");

                entity.Property(e => e.StatusName).HasMaxLength(300);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.UserId)
                    .HasMaxLength(30)
                    .IsFixedLength(true);

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.FacebookUrl)
                    .HasMaxLength(300)
                    .HasColumnName("FacebookURL");

                entity.Property(e => e.IdentityCardImageLink).HasMaxLength(500);

                entity.Property(e => e.Password).HasMaxLength(100);

                entity.Property(e => e.PhoneNumber).HasMaxLength(50);

                entity.Property(e => e.Username).HasMaxLength(100);

                entity.HasOne(d => d.Campus)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.CampusId)
                    .HasConstraintName("CampusId_in_Campus");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("RoleId_in_UserRole");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => e.RoleId)
                    .HasName("PK__UserRole__8AFACE1A534AAE26");

                entity.ToTable("UserRole");

                entity.Property(e => e.RoleName).HasMaxLength(100);
            });

            modelBuilder.Entity<Village>(entity =>
            {
                entity.ToTable("Village");

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
