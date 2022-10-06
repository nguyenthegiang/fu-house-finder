using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjects
{
    public partial class User
    {
        public User()
        {
            HouseCreatedByNavigations = new HashSet<House>();
            HouseLandlords = new HashSet<House>();
            HouseLastModifiedByNavigations = new HashSet<House>();
            ImagesOfHouseCreatedByNavigations = new HashSet<ImagesOfHouse>();
            ImagesOfHouseLastModifiedByNavigations = new HashSet<ImagesOfHouse>();
            ImagesOfRoomCreatedByNavigations = new HashSet<ImagesOfRoom>();
            ImagesOfRoomLastModifiedByNavigations = new HashSet<ImagesOfRoom>();
            InverseCreatedByNavigation = new HashSet<User>();
            InverseLastModifiedByNavigation = new HashSet<User>();
            RateCreatedByNavigations = new HashSet<Rate>();
            RateLastModifiedByNavigations = new HashSet<Rate>();
            RateStudents = new HashSet<Rate>();
            ReportCreatedByNavigations = new HashSet<Report>();
            ReportLastModifiedByNavigations = new HashSet<Report>();
            ReportStudents = new HashSet<Report>();
            RoomCreatedByNavigations = new HashSet<Room>();
            RoomHistoryCreatedByNavigations = new HashSet<RoomHistory>();
            RoomHistoryLastModifiedByNavigations = new HashSet<RoomHistory>();
            RoomLastModifiedByNavigations = new HashSet<Room>();
        }

        public string UserId { get; set; }
        public string FacebookUserId { get; set; }
        public string GoogleUserId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; }
        public bool? Active { get; set; }
        public string ProfileImageLink { get; set; }
        public string PhoneNumber { get; set; }
        public string FacebookUrl { get; set; }
        public string IdentityCardFrontSideImageLink { get; set; }
        public string IdentityCardBackSideImageLink { get; set; }
        public int? AddressId { get; set; }
        public int? RoleId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string LastModifiedBy { get; set; }

        public virtual Address Address { get; set; }
        public virtual User CreatedByNavigation { get; set; }
        public virtual User LastModifiedByNavigation { get; set; }
        public virtual UserRole Role { get; set; }
        public virtual ICollection<House> HouseCreatedByNavigations { get; set; }
        public virtual ICollection<House> HouseLandlords { get; set; }
        public virtual ICollection<House> HouseLastModifiedByNavigations { get; set; }
        public virtual ICollection<ImagesOfHouse> ImagesOfHouseCreatedByNavigations { get; set; }
        public virtual ICollection<ImagesOfHouse> ImagesOfHouseLastModifiedByNavigations { get; set; }
        public virtual ICollection<ImagesOfRoom> ImagesOfRoomCreatedByNavigations { get; set; }
        public virtual ICollection<ImagesOfRoom> ImagesOfRoomLastModifiedByNavigations { get; set; }
        public virtual ICollection<User> InverseCreatedByNavigation { get; set; }
        public virtual ICollection<User> InverseLastModifiedByNavigation { get; set; }
        public virtual ICollection<Rate> RateCreatedByNavigations { get; set; }
        public virtual ICollection<Rate> RateLastModifiedByNavigations { get; set; }
        public virtual ICollection<Rate> RateStudents { get; set; }
        public virtual ICollection<Report> ReportCreatedByNavigations { get; set; }
        public virtual ICollection<Report> ReportLastModifiedByNavigations { get; set; }
        public virtual ICollection<Report> ReportStudents { get; set; }
        public virtual ICollection<Room> RoomCreatedByNavigations { get; set; }
        public virtual ICollection<RoomHistory> RoomHistoryCreatedByNavigations { get; set; }
        public virtual ICollection<RoomHistory> RoomHistoryLastModifiedByNavigations { get; set; }
        public virtual ICollection<Room> RoomLastModifiedByNavigations { get; set; }
    }
}
