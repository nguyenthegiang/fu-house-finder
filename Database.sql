--DROP DATABASE FUHouseFinder
CREATE DATABASE FUHouseFinder;

GO
USE [FUHouseFinder]
GO

-------------------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE [dbo].[Campus] (
	CampusId int NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	CampusName nvarchar(100)
) ON [PRIMARY]
GO

INSERT INTO [dbo].[Campus] VALUES (N'FU - Hòa Lạc');
INSERT INTO [dbo].[Campus] VALUES (N'FU - Hồ Chí Minh');
INSERT INTO [dbo].[Campus] VALUES (N'FU - Đà Nẵng');
INSERT INTO [dbo].[Campus] VALUES (N'FU - Cần Thơ');
INSERT INTO [dbo].[Campus] VALUES (N'FU - Quy Nhơn');

-------------------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE [dbo].[UserRole] (
	RoleId int NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	RoleName nvarchar(100)
) ON [PRIMARY]
GO

INSERT INTO [dbo].[UserRole] VALUES (N'Student');
INSERT INTO [dbo].[UserRole] VALUES (N'Landlord');
INSERT INTO [dbo].[UserRole] VALUES (N'Head of Admission Department');
INSERT INTO [dbo].[UserRole] VALUES (N'Head of Student Service Department');
INSERT INTO [dbo].[UserRole] VALUES (N'Staff of Admission Department');
INSERT INTO [dbo].[UserRole] VALUES (N'Staff of Student Service Department');

-------------------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE [dbo].[User] (
	UserId nchar(30) NOT NULL PRIMARY KEY,
	Username nvarchar(100),
	Password nvarchar(100),
	Email nvarchar(100),
	Active bit,		--chuyển thành false nếu User bị Disable

	--Những thông tin riêng của Landlord
	PhoneNumber nvarchar(50) NULL,
	FacebookURL nvarchar(300) NULL,
	IdentityCardImageLink nvarchar(500) NULL,	--Link ảnh Căn cước công dân

	RoleId int,
	CampusId int,
	CONSTRAINT RoleId_in_UserRole FOREIGN KEY(RoleId) REFERENCES UserRole(RoleId),
	CONSTRAINT CampusId_in_Campus FOREIGN KEY(CampusId) REFERENCES Campus(CampusId),
) ON [PRIMARY]
GO

--Students
INSERT INTO [dbo].[User] VALUES (N'HE153046', N'nguyenthegiang', N'nguyenthegiang', N'giangnthe153046@fpt.edu.vn', 1, null, null, null, 1, 1);
--Landlords
INSERT INTO [dbo].[User] VALUES (N'LA000001', N'tamle', N'tamle', N'tamle@gmail.com', 1, '0987654321', 'facebook.com/tamle12', 'identity_card.jpg', 2, 1);
--Staffs
INSERT INTO [dbo].[User] VALUES (N'SA000001', N'thanhle', N'thanhle', N'thanhle@gmail.com', 1, null, null, null, 3, 1);

-------------------------------------------------------------------------------------------------------------------------------------------

--Huyện/Quận
CREATE TABLE [dbo].[District] (
	DistrictId int NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	DistrictName nvarchar(100)
) ON [PRIMARY]
GO

INSERT INTO [dbo].[District] VALUES (N'Huyện Thạch Thất');
INSERT INTO [dbo].[District] VALUES (N'Huyện Quốc Oai');
INSERT INTO [dbo].[District] VALUES (N'Thị xã Sơn Tây');

-------------------------------------------------------------------------------------------------------------------------------------------

--Phường/Xã
CREATE TABLE [dbo].[Commune] (
	CommuneId int NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	CommunetName nvarchar(100),
	DistrictId int,
	CONSTRAINT DistrictId_in_District FOREIGN KEY(DistrictId) REFERENCES District(DistrictId),
) ON [PRIMARY]
GO

INSERT INTO [dbo].[Commune] VALUES (N'Thị trấn Liên Quan', 1);
INSERT INTO [dbo].[Commune] VALUES (N'Xã Bình Phú', 1);
INSERT INTO [dbo].[Commune] VALUES (N'Xã Bình Yên', 1);

-------------------------------------------------------------------------------------------------------------------------------------------

--Thôn/Xóm
CREATE TABLE [dbo].[Village] (
	VillageId int NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	VillageName nvarchar(100),
	CommuneId int,
	CONSTRAINT CommuneId_in_Commune FOREIGN KEY(CommuneId) REFERENCES Commune(CommuneId),
) ON [PRIMARY]
GO

INSERT INTO [dbo].[Village] VALUES (N'Chi Quan 1', 1);
INSERT INTO [dbo].[Village] VALUES (N'Chi Quan 2', 1);
INSERT INTO [dbo].[Village] VALUES (N'Đồng Cam', 1);

-------------------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE [dbo].[House] (
	HouseId int NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	HouseName nvarchar(100),
	Address nvarchar(500),				--địa chỉ cụ thể 
	GoogleMapLocation nvarchar(MAX),	--địa chỉ theo Google Map -> Sử dụng khi hiển thị Map & search khoảng cách
	Information nvarchar(MAX),			--thông tin thêm

	VillageId int,						--thôn/xóm -> phường/xã -> quận/huyện
	LandlordId nchar(30),				--chủ nhà
	CONSTRAINT LandlordId_in_User FOREIGN KEY(LandlordId) REFERENCES [dbo].[User](UserId),
	CONSTRAINT VillageId_in_Village FOREIGN KEY(VillageId) REFERENCES [dbo].[Village](VillageId),
) ON [PRIMARY]
GO

INSERT INTO [dbo].[House] VALUES (N'Trọ Tâm Lê', N'Gần Bún bò Huế', N'someStringGeneratedByGoogleMap', N'Rất đẹp', 3, N'LA000001');

-------------------------------------------------------------------------------------------------------------------------------------------

--Trạng thái của 1 phòng (dùng cho Room)
CREATE TABLE [dbo].[Status] (
	StatusId int NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	StatusName nvarchar(300)
) ON [PRIMARY]
GO

INSERT INTO [dbo].[Status] VALUES (N'Available');	--có thể thuê	-> hiển thị khi search
INSERT INTO [dbo].[Status] VALUES (N'Occupied');	--đã có ng thuê	-> ko hiển thị khi search
INSERT INTO [dbo].[Status] VALUES (N'Disabled');	--ko dùng dc vì lý do nào đó	-> ko hiển thị khi search

-------------------------------------------------------------------------------------------------------------------------------------------

--Loại phòng (dùng cho Room)
CREATE TABLE [dbo].[RoomType] (
	RoomTypeId int NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	RoomTypeName nvarchar(300)
) ON [PRIMARY]
GO

INSERT INTO [dbo].[RoomType] VALUES (N'Open');
INSERT INTO [dbo].[RoomType] VALUES (N'Closed');
INSERT INTO [dbo].[RoomType] VALUES (N'Mini flat');

-------------------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE [dbo].[Room] (
	RoomId int NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	RoomName nvarchar(50),
	PricePerMonth money,		--giá theo tháng
	Information nvarchar(MAX),	--thông tin thêm & tiện ích đi kèm
	AreaByMeters float,			--diện tích, tính theo m2

	MaxAmountOfPeople int,		--số người ở tối đa trong phòng
	CurrentAmountOfPeople int,	--số người ở hiện tại trong phòng (cho tính năng update thông tin phòng 1/2)

	BuildingNumber int,			--tòa nhà
	FloorNumber int,			--tầng

	StatusId int,
	RoomTypeId int,
	HouseId int,
	CONSTRAINT StatusId_in_Status FOREIGN KEY(StatusId) REFERENCES [dbo].[Status](StatusId),
	CONSTRAINT RoomTypeId_in_RoomType FOREIGN KEY(RoomTypeId) REFERENCES [dbo].[RoomType](RoomTypeId),
	CONSTRAINT HouseId_in_House FOREIGN KEY(HouseId) REFERENCES [dbo].[House](HouseId),
) ON [PRIMARY]
GO

INSERT INTO [dbo].[Room] VALUES (N'101', 3000000, N'Gạch sàn nhà có họa tiết hình con cá', 5, 2, 1, 1, 1, 1, 2, 1);

-------------------------------------------------------------------------------------------------------------------------------------------

--Đánh giá & Comment của 1 người dùng
CREATE TABLE [dbo].[Rate] (
	RateId int NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	Star int,							--Số sao Rate
	Comment nvarchar(MAX),				--Nội dung Comment
	LandlordReply nvarchar(MAX),		--Phản hồi của chủ nhà

	HouseId int,						--Cái nhà dc Comment
	StudentId nchar(30),				--Người viết Comment
	CONSTRAINT HouseId_in_House2 FOREIGN KEY(HouseId) REFERENCES [dbo].[House](HouseId),
	CONSTRAINT StudentId_in_User FOREIGN KEY(StudentId) REFERENCES [dbo].[User](UserId),
) ON [PRIMARY]
GO

INSERT INTO [dbo].[Rate] VALUES (5, N'Rất tuyệt vời, gần trường nữa', N'Cảm ơn bạn', 1, N'HE153046');

-------------------------------------------------------------------------------------------------------------------------------------------

--Ảnh nhà
CREATE TABLE [dbo].[ImageOfHouse] (
	ImageId int NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	ImageLink nvarchar(500),

	HouseId int,
	CONSTRAINT HouseId_in_House3 FOREIGN KEY(HouseId) REFERENCES [dbo].[House](HouseId),
) ON [PRIMARY]
GO

INSERT INTO [dbo].[ImageOfHouse] VALUES (N'link_of_image.jpg', 1);

-------------------------------------------------------------------------------------------------------------------------------------------

--Ảnh phòng
CREATE TABLE [dbo].[ImageOfRoom] (
	ImageId int NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	ImageLink nvarchar(500),

	RoomId int,
	CONSTRAINT RoomId_in_Room FOREIGN KEY(RoomId) REFERENCES [dbo].[Room](RoomId),
) ON [PRIMARY]
GO

INSERT INTO [dbo].[ImageOfRoom] VALUES (N'link_of_image2.jpg', 1);