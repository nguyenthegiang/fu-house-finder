﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

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
        public virtual DbSet<Issue> Issues { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Rate> Rates { get; set; }
        public virtual DbSet<Report> Reports { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<RoomHistory> RoomHistories { get; set; }
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

            modelBuilder.Entity<Address>(entity =>
            {
                entity.Property(e => e.Addresses)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Campus>(entity =>
            {
                entity.Property(e => e.CampusName).HasMaxLength(100);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.Campuses)
                    .HasForeignKey(d => d.AddressId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("AddressId_in_Address");
            });

            modelBuilder.Entity<Commune>(entity =>
            {
                entity.Property(e => e.CommuneName).HasMaxLength(100);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.District)
                    .WithMany(p => p.Communes)
                    .HasForeignKey(d => d.DistrictId)
                    .HasConstraintName("DistrictId_in_District");
            });

            modelBuilder.Entity<District>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DistrictName).HasMaxLength(100);
            });

            modelBuilder.Entity<House>(entity =>
            {
                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(30)
                    .IsFixedLength(true);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.HouseName).HasMaxLength(100);

                entity.Property(e => e.LandlordId)
                    .HasMaxLength(30)
                    .IsFixedLength(true);

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(30)
                    .IsFixedLength(true);

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.PowerPrice).HasColumnType("money");

                entity.Property(e => e.WaterPrice).HasColumnType("money");

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.Houses)
                    .HasForeignKey(d => d.AddressId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("AddressId_in_Address2");

                entity.HasOne(d => d.Campus)
                    .WithMany(p => p.Houses)
                    .HasForeignKey(d => d.CampusId)
                    .HasConstraintName("CampusId_in_Campus");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.HouseCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("createdUser_in_User2");

                entity.HasOne(d => d.Landlord)
                    .WithMany(p => p.HouseLandlords)
                    .HasForeignKey(d => d.LandlordId)
                    .HasConstraintName("LandlordId_in_User");

                entity.HasOne(d => d.LastModifiedByNavigation)
                    .WithMany(p => p.HouseLastModifiedByNavigations)
                    .HasForeignKey(d => d.LastModifiedBy)
                    .HasConstraintName("updatedUser_in_User2");

                entity.HasOne(d => d.Village)
                    .WithMany(p => p.Houses)
                    .HasForeignKey(d => d.VillageId)
                    .HasConstraintName("VillageId_in_Village");
            });

            modelBuilder.Entity<ImagesOfHouse>(entity =>
            {
                entity.HasKey(e => e.ImageId)
                    .HasName("PK__ImagesOf__7516F70CAABF7662");

                entity.ToTable("ImagesOfHouse");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(30)
                    .IsFixedLength(true);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ImageLink).HasMaxLength(500);

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(30)
                    .IsFixedLength(true);

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.ImagesOfHouseCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("createdUser_in_User5");

                entity.HasOne(d => d.House)
                    .WithMany(p => p.ImagesOfHouses)
                    .HasForeignKey(d => d.HouseId)
                    .HasConstraintName("HouseId_in_House3");

                entity.HasOne(d => d.LastModifiedByNavigation)
                    .WithMany(p => p.ImagesOfHouseLastModifiedByNavigations)
                    .HasForeignKey(d => d.LastModifiedBy)
                    .HasConstraintName("updatedUser_in_User5");
            });

            modelBuilder.Entity<ImagesOfRoom>(entity =>
            {
                entity.HasKey(e => e.ImageId)
                    .HasName("PK__ImagesOf__7516F70CFB27F375");

                entity.ToTable("ImagesOfRoom");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(30)
                    .IsFixedLength(true);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ImageLink).HasMaxLength(500);

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(30)
                    .IsFixedLength(true);

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.ImagesOfRoomCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("createdUser_in_User6");

                entity.HasOne(d => d.LastModifiedByNavigation)
                    .WithMany(p => p.ImagesOfRoomLastModifiedByNavigations)
                    .HasForeignKey(d => d.LastModifiedBy)
                    .HasConstraintName("updatedUser_in_User6");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.ImagesOfRooms)
                    .HasForeignKey(d => d.RoomId)
                    .HasConstraintName("RoomId_in_Room");
            });

            modelBuilder.Entity<Issue>(entity =>
            {
                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(30)
                    .IsFixedLength(true);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(100);

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(30)
                    .IsFixedLength(true);

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.Issues)
                    .HasForeignKey(d => d.RoomId)
                    .HasConstraintName("RoomId_in_Room3");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.OrderedDate).HasColumnType("datetime");

                entity.Property(e => e.PhoneNumber).HasMaxLength(50);

                entity.Property(e => e.SolvedDate).HasColumnType("datetime");

                entity.Property(e => e.StudentId)
                    .HasMaxLength(30)
                    .IsFixedLength(true);

                entity.Property(e => e.StudentName).HasMaxLength(100);

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.StudentId)
                    .HasConstraintName("StudentId_in_User4");
            });

            modelBuilder.Entity<Rate>(entity =>
            {
                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(30)
                    .IsFixedLength(true);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(30)
                    .IsFixedLength(true);

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.StudentId)
                    .HasMaxLength(30)
                    .IsFixedLength(true);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.RateCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("createdUser_in_User4");

                entity.HasOne(d => d.House)
                    .WithMany(p => p.Rates)
                    .HasForeignKey(d => d.HouseId)
                    .HasConstraintName("HouseId_in_House2");

                entity.HasOne(d => d.LastModifiedByNavigation)
                    .WithMany(p => p.RateLastModifiedByNavigations)
                    .HasForeignKey(d => d.LastModifiedBy)
                    .HasConstraintName("updatedUser_in_User4");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.RateStudents)
                    .HasForeignKey(d => d.StudentId)
                    .HasConstraintName("StudentId_in_User");
            });

            modelBuilder.Entity<Report>(entity =>
            {
                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(30)
                    .IsFixedLength(true);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(30)
                    .IsFixedLength(true);

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.StudentId)
                    .HasMaxLength(30)
                    .IsFixedLength(true);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.ReportCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("createdUser_in_User7");

                entity.HasOne(d => d.House)
                    .WithMany(p => p.Reports)
                    .HasForeignKey(d => d.HouseId)
                    .HasConstraintName("HouseId_in_House4");

                entity.HasOne(d => d.LastModifiedByNavigation)
                    .WithMany(p => p.ReportLastModifiedByNavigations)
                    .HasForeignKey(d => d.LastModifiedBy)
                    .HasConstraintName("updatedUser_in_User7");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.ReportStudents)
                    .HasForeignKey(d => d.StudentId)
                    .HasConstraintName("StudentId_in_User3");
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(30)
                    .IsFixedLength(true);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(30)
                    .IsFixedLength(true);

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.PricePerMonth).HasColumnType("money");

                entity.Property(e => e.RoomName).HasMaxLength(50);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.RoomCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("createdUser_in_User3");

                entity.HasOne(d => d.House)
                    .WithMany(p => p.Rooms)
                    .HasForeignKey(d => d.HouseId)
                    .HasConstraintName("HouseId_in_House");

                entity.HasOne(d => d.LastModifiedByNavigation)
                    .WithMany(p => p.RoomLastModifiedByNavigations)
                    .HasForeignKey(d => d.LastModifiedBy)
                    .HasConstraintName("updatedUser_in_User3");

                entity.HasOne(d => d.RoomType)
                    .WithMany(p => p.Rooms)
                    .HasForeignKey(d => d.RoomTypeId)
                    .HasConstraintName("RoomTypeId_in_RoomType");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Rooms)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("StatusId_in_Status");
            });

            modelBuilder.Entity<RoomHistory>(entity =>
            {
                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(30)
                    .IsFixedLength(true);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.CustomerName).HasMaxLength(800);

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(30)
                    .IsFixedLength(true);

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.RoomHistoryCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("createdUser_in_User8");

                entity.HasOne(d => d.LastModifiedByNavigation)
                    .WithMany(p => p.RoomHistoryLastModifiedByNavigations)
                    .HasForeignKey(d => d.LastModifiedBy)
                    .HasConstraintName("updatedUser_in_User8");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.RoomHistories)
                    .HasForeignKey(d => d.RoomId)
                    .HasConstraintName("RoomId_in_Room2");
            });

            modelBuilder.Entity<RoomType>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.RoomTypeName)
                    .IsRequired()
                    .HasMaxLength(300);
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.StatusName).HasMaxLength(300);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.UserId)
                    .HasMaxLength(30)
                    .IsFixedLength(true);

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(30)
                    .IsFixedLength(true);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DisplayName).HasMaxLength(500);

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.FacebookUrl)
                    .HasMaxLength(300)
                    .HasColumnName("FacebookURL");

                entity.Property(e => e.FacebookUserId)
                    .HasMaxLength(300)
                    .IsFixedLength(true);

                entity.Property(e => e.GoogleUserId)
                    .HasMaxLength(300)
                    .IsFixedLength(true);

                entity.Property(e => e.IdentityCardBackSideImageLink).HasMaxLength(500);

                entity.Property(e => e.IdentityCardFrontSideImageLink).HasMaxLength(500);

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(30)
                    .IsFixedLength(true);

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Password).HasMaxLength(100);

                entity.Property(e => e.PhoneNumber).HasMaxLength(50);

                entity.Property(e => e.ProfileImageLink).HasMaxLength(500);

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.AddressId)
                    .HasConstraintName("AddressId_in_Address3");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.InverseCreatedByNavigation)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("createdUser_in_User");

                entity.HasOne(d => d.LastModifiedByNavigation)
                    .WithMany(p => p.InverseLastModifiedByNavigation)
                    .HasForeignKey(d => d.LastModifiedBy)
                    .HasConstraintName("updatedUser_in_User");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("RoleId_in_UserRole");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => e.RoleId)
                    .HasName("PK__UserRole__8AFACE1A8C19C9FD");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.RoleName).HasMaxLength(100);
            });

            modelBuilder.Entity<Village>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

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
