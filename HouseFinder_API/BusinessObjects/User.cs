using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjects
{
    public partial class User
    {
        public User()
        {
            HouseCreatedUserNavigations = new HashSet<House>();
            HouseLandlords = new HashSet<House>();
            HouseUpdatedUserNavigations = new HashSet<House>();
            ImagesOfHouseCreatedUserNavigations = new HashSet<ImagesOfHouse>();
            ImagesOfHouseUpdatedUserNavigations = new HashSet<ImagesOfHouse>();
            ImagesOfRoomCreatedUserNavigations = new HashSet<ImagesOfRoom>();
            ImagesOfRoomUpdatedUserNavigations = new HashSet<ImagesOfRoom>();
            InverseCreatedUserNavigation = new HashSet<User>();
            InverseUpdatedUserNavigation = new HashSet<User>();
            RateCreatedUserNavigations = new HashSet<Rate>();
            RateStudents = new HashSet<Rate>();
            RateUpdatedUserNavigations = new HashSet<Rate>();
            ReportCreatedUserNavigations = new HashSet<Report>();
            ReportStudents = new HashSet<Report>();
            ReportUpdatedUserNavigations = new HashSet<Report>();
            RoomCreatedUserNavigations = new HashSet<Room>();
            RoomUpdatedUserNavigations = new HashSet<Room>();
        }

        public string UserId { get; set; }
        public string FacebookUserId { get; set; }
        public string GoogleUserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool? Active { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string FacebookUrl { get; set; }
        public string IdentityCardFrontSideImageLink { get; set; }
        public string IdentityCardBackSideImageLink { get; set; }
        public int? RoleId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreatedUser { get; set; }
        public string UpdatedUser { get; set; }

        public virtual User CreatedUserNavigation { get; set; }
        public virtual UserRole Role { get; set; }
        public virtual User UpdatedUserNavigation { get; set; }
        public virtual ICollection<House> HouseCreatedUserNavigations { get; set; }
        public virtual ICollection<House> HouseLandlords { get; set; }
        public virtual ICollection<House> HouseUpdatedUserNavigations { get; set; }
        public virtual ICollection<ImagesOfHouse> ImagesOfHouseCreatedUserNavigations { get; set; }
        public virtual ICollection<ImagesOfHouse> ImagesOfHouseUpdatedUserNavigations { get; set; }
        public virtual ICollection<ImagesOfRoom> ImagesOfRoomCreatedUserNavigations { get; set; }
        public virtual ICollection<ImagesOfRoom> ImagesOfRoomUpdatedUserNavigations { get; set; }
        public virtual ICollection<User> InverseCreatedUserNavigation { get; set; }
        public virtual ICollection<User> InverseUpdatedUserNavigation { get; set; }
        public virtual ICollection<Rate> RateCreatedUserNavigations { get; set; }
        public virtual ICollection<Rate> RateStudents { get; set; }
        public virtual ICollection<Rate> RateUpdatedUserNavigations { get; set; }
        public virtual ICollection<Report> ReportCreatedUserNavigations { get; set; }
        public virtual ICollection<Report> ReportStudents { get; set; }
        public virtual ICollection<Report> ReportUpdatedUserNavigations { get; set; }
        public virtual ICollection<Room> RoomCreatedUserNavigations { get; set; }
        public virtual ICollection<Room> RoomUpdatedUserNavigations { get; set; }
    }
}
