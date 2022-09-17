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
INSERT INTO [dbo].[UserRole] VALUES (N'Staff');

-------------------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE [dbo].[User] (
	UserId nchar(30) NOT NULL PRIMARY KEY,
	Username nvarchar(100),
	Password nvarchar(100),
	Email nvarchar(100),
	Active bit,		--chuyển thành false nếu User bị Disable

	RoleId int,
	CampusId int,
	CONSTRAINT RoleId_in_UserRole FOREIGN KEY(RoleId) REFERENCES UserRole(RoleId),
	CONSTRAINT CampusId_in_Campus FOREIGN KEY(CampusId) REFERENCES Campus(CampusId),
) ON [PRIMARY]
GO

INSERT INTO [dbo].[User] VALUES (N'HE153046', N'nguyenthegiang', N'nguyenthegiang', N'giangnthe153046@fpt.edu.vn', 1, 1, 1);
INSERT INTO [dbo].[User] VALUES (N'LA000001', N'tamle', N'tamle', N'tamle@gmail.com', 1, 2, 1);
INSERT INTO [dbo].[User] VALUES (N'SA000001', N'thanhle', N'thanhle', N'thanhle@gmail.com', 1, 3, 1);

-------------------------------------------------------------------------------------------------------------------------------------------

--Những thông tin chi tiết chỉ Chủ trọ mới có (quan hệ 1-1 với Table User)
CREATE TABLE [dbo].[LandlordDetail] (
	LandlordId nchar(30) NOT NULL PRIMARY KEY FOREIGN KEY REFERENCES [dbo].[User](UserId),
	PhoneNumber nvarchar(50),
	FacebookURL nvarchar(300),
	IdentityCardImageLink nvarchar(500)		--Link ảnh Căn cước công dân
) ON [PRIMARY]
GO

INSERT INTO [dbo].[LandlordDetail] VALUES (N'LA000001', N'0987654321', N'facebook.com/tamle123', N'identity_card_image.jpg');

-------------------------------------------------------------------------------------------------------------------------------------------

--Phòng ban (dùng cho StaffDetail)
CREATE TABLE [dbo].[StaffDepartment] (
	DepartmentId int NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	DepartmentName nvarchar(500)
) ON [PRIMARY]
GO

INSERT INTO [dbo].[StaffDepartment] VALUES (N'Admission Department');
INSERT INTO [dbo].[StaffDepartment] VALUES (N'Student Service Department');

-------------------------------------------------------------------------------------------------------------------------------------------

--Chức vụ (dùng cho StaffDetail)
CREATE TABLE [dbo].[StaffPosition] (
	PositionId int NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	PositiontName nvarchar(500)
) ON [PRIMARY]
GO

INSERT INTO [dbo].[StaffPosition] VALUES (N'Manager');
INSERT INTO [dbo].[StaffPosition] VALUES (N'Member');

-------------------------------------------------------------------------------------------------------------------------------------------

--Những thông tin chi tiết chỉ Nhân viên nhà trường mới có (quan hệ 1-1 với Table User)
CREATE TABLE [dbo].[StaffDetail] (
	StaffId nchar(30) NOT NULL PRIMARY KEY FOREIGN KEY REFERENCES [dbo].[User](UserId),
	DepartmentId int,
	PositionId int,
	CONSTRAINT DepartmentId_in_Department FOREIGN KEY(DepartmentId) REFERENCES StaffDepartment(DepartmentId),
	CONSTRAINT PositionId_in_Position FOREIGN KEY(PositionId) REFERENCES StaffPosition(PositionId),
) ON [PRIMARY]
GO

INSERT INTO [dbo].[StaffDetail] VALUES (N'SA000001', 1, 1);

-------------------------------------------------------------------------------------------------------------------------------------------

--Thông tin đơn vị hành chính: Quận/Huyện, Phường/Xã, Thôn/Xóm
CREATE TABLE [dbo].[AdministrativeUnit] (
	UnitCode int NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	UnitName nvarchar(500),	--Tên đơn vị
	UnitLevel int,			--Level của đơn vị: 1 là Quận/Huyện; 2 là Phường/Xã; 3 là Thôn/Xóm
	ParentCode int			--UnitCode của đơn vị hành chính cha của nó: những đơn vị có Level là 1 thì ParentCode là 0
) ON [PRIMARY]
GO

--Huyện
INSERT INTO [dbo].[AdministrativeUnit] VALUES (N'Huyện Thạch Thất', 1, 0);
INSERT INTO [dbo].[AdministrativeUnit] VALUES (N'Huyện Quốc Oai', 1, 0);
INSERT INTO [dbo].[AdministrativeUnit] VALUES (N'Thị xã Sơn Tây', 1, 0);
--Xã
INSERT INTO [dbo].[AdministrativeUnit] VALUES (N'Thị trấn Liên Quan', 2, 1);
INSERT INTO [dbo].[AdministrativeUnit] VALUES (N'Xã Bình Phú', 2, 1);
INSERT INTO [dbo].[AdministrativeUnit] VALUES (N'Xã Bình Yên', 2, 1);
--Thôn
INSERT INTO [dbo].[AdministrativeUnit] VALUES (N'Chi Quan 1', 3, 1);
INSERT INTO [dbo].[AdministrativeUnit] VALUES (N'Chi Quan 2', 3, 1);
INSERT INTO [dbo].[AdministrativeUnit] VALUES (N'Đồng Cam', 3, 1);

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
	UnitCode int,			--đơn vị hành chính
	LandlordId nchar(30),				--chủ nhà
	CONSTRAINT LandlordId_in_User FOREIGN KEY(LandlordId) REFERENCES [dbo].[User](UserId),
	CONSTRAINT VillageId_in_Village FOREIGN KEY(VillageId) REFERENCES [dbo].[Village](VillageId),
	CONSTRAINT UnitCode_in_AdministrativeUnit FOREIGN KEY(UnitCode) REFERENCES [dbo].[AdministrativeUnit](UnitCode),
) ON [PRIMARY]
GO

INSERT INTO [dbo].[House] VALUES (N'Trọ Tâm Lê', N'Gần Bún bò Huế', N'someStringGeneratedByGoogleMap', N'Rất đẹp', 3, 7, N'LA000001');

-------------------------------------------------------------------------------------------------------------------------------------------

--Trạng thái của 1 phòng
CREATE TABLE [dbo].[Status] (
	StatusId int NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	StatusName nvarchar(300)
) ON [PRIMARY]
GO

INSERT INTO [dbo].[Status] VALUES (N'Available');	--có thể thuê	-> hiển thị khi search
INSERT INTO [dbo].[Status] VALUES (N'Occupied');	--đã có ng thuê	-> ko hiển thị khi search
INSERT INTO [dbo].[Status] VALUES (N'Disabled');	--ko dùng dc vì lý do nào đó	-> ko hiển thị khi search

-------------------------------------------------------------------------------------------------------------------------------------------

--Loại phòng
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
	MaxAmountOfPeople int,		--số người ở tối đa trong phòng

	LengthByMeters float,		--chiều dài theo mét
	WidthByMeters float,		--chiều rộng theo mét
	--từ 2 chỉ số này có thể tính ra diện tích

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

INSERT INTO [dbo].[Room] VALUES (N'101', 3000000, N'Gạch sàn nhà có họa tiết hình con cá', 2, 3, 5, 1, 1, 1, 1, 1);

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