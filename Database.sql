--DROP DATABASE FUHouseFinder
CREATE DATABASE FUHouseFinder;

GO
USE [FUHouseFinder]
GO

--------------------------------------------------[Database Design]----------------------------------------------------------------------------------

--Địa chỉ của User, House & Campus
CREATE TABLE [dbo].[Addresses] (
	AddressId int NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	Addresses nvarchar(1000) NOT NULL,							--địa chỉ cụ thể
	GoogleMapLocation nvarchar(MAX) NOT NULL,					--địa chỉ theo Google Map -> Sử dụng hỗ trợ search khoảng cách

	--Dành cho những Table CRUD dc -> History
	Deleted bit NOT NULL,
	CreatedDate datetime NOT NULL,
	LastModifiedDate datetime,
) ON [PRIMARY]
GO

--Cơ sở của FPT
CREATE TABLE [dbo].[Campuses] (
	CampusId int NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	CampusName nvarchar(100) NOT NULL,
	
	AddressId int NOT NULL,		--địa chỉ

	CreatedDate datetime NOT NULL,

	CONSTRAINT AddressId_in_Address FOREIGN KEY(AddressId) REFERENCES Addresses(AddressId),
) ON [PRIMARY]
GO

--Vai trò người dùng
CREATE TABLE [dbo].[UserRoles] (
	RoleId int NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	RoleName nvarchar(100) NOT NULL,

	CreatedDate datetime NOT NULL,
) ON [PRIMARY]
GO

--Trạng thái của 1 người dùng (dùng cho User)
--0: landlord signup request, 1: active, 2: inactive
CREATE TABLE [dbo].[UserStatuses] (
	StatusId int NOT NULL PRIMARY KEY,
	StatusName nvarchar(300) NOT NULL,

	CreatedDate datetime NOT NULL,
) ON [PRIMARY]
GO

--Người dùng
CREATE TABLE [dbo].[Users] (
	UserId nchar(30) NOT NULL PRIMARY KEY,

	--Dành cho người Login = Facebook/Google
	FacebookUserId nchar(300) NULL,
	GoogleUserId nchar(300) NULL,

	--Dành cho người Login = Email & Password
	Email nvarchar(100),								--và cũng dành cho chủ trọ
	[Password] nvarchar(100),

	DisplayName nvarchar(500) NULL,						--Tên để hiển thị, lấy từ Google/Facebook API (nếu login = fb/gg) hoặc lấy khi đăng ký (nếu login = email)
	StatusId int NOT NULL,								--0: landlord request, 1: active, 2: inactive

	--Dành cho Staff & Landlord
	ProfileImageLink nvarchar(500) NULL,				--Link ảnh profile

	--Những thông tin riêng của Landlord
	PhoneNumber nvarchar(50) NULL,
	FacebookURL nvarchar(300) NULL,
	IdentityCardFrontSideImageLink nvarchar(500) NULL,	--Link ảnh Căn cước công dân, mặt trước
	IdentityCardBackSideImageLink nvarchar(500) NULL,	--Link ảnh Căn cước công dân, mặt sau
	AddressId int NULL,									--địa chỉ

	RoleId int NOT NULL,

	--Dành cho những Table CRUD dc -> History
	CreatedDate datetime NOT NULL,
	LastModifiedDate datetime,
	CreatedBy nchar(30) NOT NULL,
	LastModifiedBy nchar(30),

	CONSTRAINT RoleId_in_UserRole FOREIGN KEY(RoleId) REFERENCES UserRoles(RoleId),
	CONSTRAINT createdUser_in_User FOREIGN KEY(CreatedBy) REFERENCES [dbo].[Users](UserId),
	CONSTRAINT updatedUser_in_User FOREIGN KEY(LastModifiedBy) REFERENCES [dbo].[Users](UserId),
	CONSTRAINT AddressId_in_Address3 FOREIGN KEY(AddressId) REFERENCES Addresses(AddressId),
	CONSTRAINT StatusId_in_Status3 FOREIGN KEY(StatusId) REFERENCES UserStatuses(StatusId),
) ON [PRIMARY]
GO

--Huyện/Quận
CREATE TABLE [dbo].[Districts] (
	DistrictId int NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	DistrictName nvarchar(100) NOT NULL,
	CampusId int,						--Campus của Huyện này (Mỗi Campus sẽ có những Huyện ở quanh đó)

	CreatedDate datetime NOT NULL,

	CONSTRAINT CampusId_in_Campus2 FOREIGN KEY(CampusId) REFERENCES Campuses(CampusId),
) ON [PRIMARY]
GO

--Phường/Xã
CREATE TABLE [dbo].[Communes] (
	CommuneId int NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	CommuneName nvarchar(100) NOT NULL,
	DistrictId int NOT NULL,

	CreatedDate datetime NOT NULL,
	CONSTRAINT DistrictId_in_District FOREIGN KEY(DistrictId) REFERENCES Districts(DistrictId),
) ON [PRIMARY]
GO

--Thôn/Xóm
CREATE TABLE [dbo].[Villages] (
	VillageId int NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	VillageName nvarchar(100) NOT NULL,
	CommuneId int NOT NULL,

	CreatedDate datetime NOT NULL,
	CONSTRAINT CommuneId_in_Commune FOREIGN KEY(CommuneId) REFERENCES Communes(CommuneId),
) ON [PRIMARY]
GO

--Nhà trọ
CREATE TABLE [dbo].[Houses] (
	HouseId int NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	HouseName nvarchar(100) NOT NULL,
	[View] int,							--số lượt xem
	Information nvarchar(MAX),			--thông tin thêm

	AddressId int NOT NULL,				--địa chỉ
	VillageId int,						--thôn/xóm -> phường/xã -> quận/huyện
	LandlordId nchar(30),				--chủ nhà
	CampusId int,						--Campus mà nhà này thuộc về
	DistanceToCampus float,				--Khoảng cách đến trường

	--Tiền
	PowerPrice money NOT NULL,			--giá điện
	WaterPrice money NOT NULL,			--giá nước

	--Tiện ích
	FingerprintLock bit,				--khóa vân tay
	Camera bit,							--camera an ninh
	Parking bit,						--khu để xe

	--Dành cho những Table CRUD dc -> History
	Deleted bit NOT NULL,
	CreatedDate datetime NOT NULL,
	LastModifiedDate datetime,
	CreatedBy nchar(30) NOT NULL,
	LastModifiedBy nchar(30),

	CONSTRAINT AddressId_in_Address2 FOREIGN KEY(AddressId) REFERENCES Addresses(AddressId),
	CONSTRAINT LandlordId_in_User FOREIGN KEY(LandlordId) REFERENCES [dbo].[Users](UserId),
	CONSTRAINT VillageId_in_Village FOREIGN KEY(VillageId) REFERENCES [dbo].[Villages](VillageId),
	CONSTRAINT CampusId_in_Campus FOREIGN KEY(CampusId) REFERENCES Campuses(CampusId),

	CONSTRAINT createdUser_in_User2 FOREIGN KEY(CreatedBy) REFERENCES [dbo].[Users](UserId),
	CONSTRAINT updatedUser_in_User2 FOREIGN KEY(LastModifiedBy) REFERENCES [dbo].[Users](UserId),
) ON [PRIMARY]
GO

--Trạng thái của 1 phòng (dùng cho Room)
CREATE TABLE [dbo].[RoomStatuses] (
	StatusId int NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	StatusName nvarchar(300) NOT NULL,

	CreatedDate datetime NOT NULL,
) ON [PRIMARY]
GO

--Loại phòng (dùng cho Room) (Không được quá 9 Record nếu không sẽ hỏng tính năng Filter ở Home)
CREATE TABLE [dbo].[RoomTypes] (
	RoomTypeId int NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	RoomTypeName nvarchar(300) NOT NULL,

	CreatedDate datetime NOT NULL,
) ON [PRIMARY]
GO

--Phòng trọ
CREATE TABLE [dbo].[Rooms] (
	RoomId int NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	RoomName nvarchar(50) NOT NULL,
	PricePerMonth money NOT NULL,		--giá theo tháng
	Information nvarchar(MAX),			--thông tin thêm & tiện ích đi kèm
	AreaByMeters float,					--diện tích, tính theo m2

	--Tiện ích
	Fridge bit NOT NULL,			--Tủ lạnh (có/ko)
	Kitchen bit NOT NULL,			--Bếp (có/không)
	WashingMachine bit NOT NULL,	--Máy giặt (có/không)
	Desk bit NOT NULL,				--Bàn học (có/không)
	NoLiveWithHost bit NOT NULL,	--Không Chung chủ (có/không)
	Bed bit NOT NULL,				--Giường (có/không)
	ClosedToilet bit NOT NULL,		--Vệ sinh khép kín (có/không)

	MaxAmountOfPeople int,		--số người ở tối đa trong phòng
	CurrentAmountOfPeople int,	--số người ở hiện tại trong phòng (cho tính năng update thông tin phòng 1/2)

	BuildingNumber int,			--tòa nhà
	FloorNumber int,			--tầng

	StatusId int NOT NULL,
	RoomTypeId int NOT NULL,
	HouseId int NOT NULL,

	--Dành cho những Table CRUD dc -> History
	Deleted bit NOT NULL,
	CreatedDate datetime NOT NULL,
	LastModifiedDate datetime,
	CreatedBy nchar(30) NOT NULL,
	LastModifiedBy nchar(30),

	CONSTRAINT StatusId_in_Status FOREIGN KEY(StatusId) REFERENCES [dbo].[RoomStatuses](StatusId),
	CONSTRAINT RoomTypeId_in_RoomType FOREIGN KEY(RoomTypeId) REFERENCES [dbo].[RoomTypes](RoomTypeId),
	CONSTRAINT HouseId_in_House FOREIGN KEY(HouseId) REFERENCES [dbo].[Houses](HouseId),

	CONSTRAINT createdUser_in_User3 FOREIGN KEY(CreatedBy) REFERENCES [dbo].[Users](UserId),
	CONSTRAINT updatedUser_in_User3 FOREIGN KEY(LastModifiedBy) REFERENCES [dbo].[Users](UserId),
) ON [PRIMARY]
GO

--Đánh giá & Comment của 1 người dùng
CREATE TABLE [dbo].[Rates] (
	RateId int NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	Star int,							--Số sao Rate
	Comment nvarchar(MAX),				--Nội dung Comment
	LandlordReply nvarchar(MAX),		--Phản hồi của chủ nhà

	HouseId int NOT NULL,						--Cái nhà dc Comment
	StudentId nchar(30) NOT NULL,				--Người viết Comment

	--Dành cho những Table CRUD dc -> History
	Deleted bit NOT NULL,
	CreatedDate datetime NOT NULL,
	LastModifiedDate datetime,
	CreatedBy nchar(30) NOT NULL,
	LastModifiedBy nchar(30),

	CONSTRAINT HouseId_in_House2 FOREIGN KEY(HouseId) REFERENCES [dbo].[Houses](HouseId),
	CONSTRAINT StudentId_in_User FOREIGN KEY(StudentId) REFERENCES [dbo].[Users](UserId),

	CONSTRAINT createdUser_in_User4 FOREIGN KEY(CreatedBy) REFERENCES [dbo].[Users](UserId),
	CONSTRAINT updatedUser_in_User4 FOREIGN KEY(LastModifiedBy) REFERENCES [dbo].[Users](UserId),
) ON [PRIMARY]
GO

--Ảnh nhà trọ
CREATE TABLE [dbo].[ImagesOfHouse] (
	ImageId int NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	ImageLink nvarchar(500) NOT NULL,

	HouseId int NOT NULL,

	--Dành cho những Table CRUD dc -> History
	Deleted bit NOT NULL,
	CreatedDate datetime NOT NULL,
	LastModifiedDate datetime,
	CreatedBy nchar(30) NOT NULL,
	LastModifiedBy nchar(30),

	CONSTRAINT HouseId_in_House3 FOREIGN KEY(HouseId) REFERENCES [dbo].[Houses](HouseId),

	CONSTRAINT createdUser_in_User5 FOREIGN KEY(CreatedBy) REFERENCES [dbo].[Users](UserId),
	CONSTRAINT updatedUser_in_User5 FOREIGN KEY(LastModifiedBy) REFERENCES [dbo].[Users](UserId),
) ON [PRIMARY]
GO

--Ảnh phòng trọ
CREATE TABLE [dbo].[ImagesOfRoom] (
	ImageId int NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	ImageLink nvarchar(500) NOT NULL,

	RoomId int NOT NULL,

	--Dành cho những Table CRUD dc -> History
	Deleted bit NOT NULL,
	CreatedDate datetime NOT NULL,
	LastModifiedDate datetime,
	CreatedBy nchar(30) NOT NULL,
	LastModifiedBy nchar(30),

	CONSTRAINT RoomId_in_Room FOREIGN KEY(RoomId) REFERENCES [dbo].[Rooms](RoomId),

	CONSTRAINT createdUser_in_User6 FOREIGN KEY(CreatedBy) REFERENCES [dbo].[Users](UserId),
	CONSTRAINT updatedUser_in_User6 FOREIGN KEY(LastModifiedBy) REFERENCES [dbo].[Users](UserId),
) ON [PRIMARY]
GO

--Trạng thái của 1 Report (dùng cho Report)
CREATE TABLE [dbo].[ReportStatuses] (
	StatusId int NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	StatusName nvarchar(300) NOT NULL,

	CreatedDate datetime NOT NULL,
) ON [PRIMARY]
GO

--Report của sinh viên đối với nhà trọ
CREATE TABLE [dbo].[Reports] (
	ReportId int NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	ReportContent nvarchar(MAX) NOT NULL,

	StudentId nchar(30) NOT NULL,
	HouseId int NOT NULL,

	StatusId int NOT NULL,

	--Dành cho những Table CRUD dc -> History
	Deleted bit NOT NULL,
	ReportedDate datetime NOT NULL,
	SolvedDate datetime,
	SolvedBy nchar(30),

	CONSTRAINT StatusId_in_StatusId9 FOREIGN KEY(StatusId) REFERENCES [dbo].[ReportStatuses](StatusId),

	CONSTRAINT HouseId_in_House4 FOREIGN KEY(HouseId) REFERENCES [dbo].[Houses](HouseId),
	CONSTRAINT StudentId_in_User3 FOREIGN KEY(StudentId) REFERENCES [dbo].[Users](UserId),

	CONSTRAINT SolvedBy_in_User2 FOREIGN KEY(SolvedBy) REFERENCES [dbo].[Users](UserId),
) ON [PRIMARY]
GO

--Lịch sử người ở phòng trọ, dành cho chủ trọ tự nguyện thêm vào nếu có nhu cầu quản lý & theo dõi
CREATE TABLE [dbo].[RoomHistories] (
	RoomHistoryId int NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	CustomerName nvarchar(800) NOT NULL,		--tên ng ở phòng
	RoomId int NOT NULL,						--phòng

	--Dành cho những Table CRUD dc -> History
	Deleted bit NOT NULL,
	CreatedDate datetime NOT NULL,
	LastModifiedDate datetime,
	CreatedBy nchar(30) NOT NULL,
	LastModifiedBy nchar(30),

	CONSTRAINT RoomId_in_Room2 FOREIGN KEY(RoomId) REFERENCES [dbo].Rooms(RoomId),

	CONSTRAINT createdUser_in_User8 FOREIGN KEY(CreatedBy) REFERENCES [dbo].[Users](UserId),
	CONSTRAINT updatedUser_in_User8 FOREIGN KEY(LastModifiedBy) REFERENCES [dbo].[Users](UserId),
) ON [PRIMARY]
GO

--Vấn đề của 1 phòng trọ -> student tạo ra, landlord có thể thấy được và xử lý
CREATE TABLE [dbo].[Issues] (
	IssueId int NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	Description nvarchar(100) NOT NULL,      --mô tả
	RoomId int NOT NULL,						--phòng

	--Dành cho những Table CRUD dc -> History
	Deleted bit NOT NULL,
	CreatedDate datetime NOT NULL,
	LastModifiedDate datetime,
	CreatedBy nchar(30) NOT NULL,
	LastModifiedBy nchar(30),

	CONSTRAINT RoomId_in_Room3 FOREIGN KEY(RoomId) REFERENCES [dbo].Rooms(RoomId),
) ON [PRIMARY]
GO

--Trạng thái của 1 order
CREATE TABLE [dbo].[OrderStatuses] (
	StatusId int NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	StatusName nvarchar(300) NOT NULL,

	CreatedDate datetime NOT NULL,
) ON [PRIMARY]
GO

--Wishlist của sinh viên, khi không tìm được phòng trọ theo mong muốn -> tạo -> tuyển sinh thấy được và xử lý
CREATE TABLE [dbo].[Order] (
	OrderId int NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	StudentId nchar(30),
	StudentName nvarchar(100) NOT NULL,
	PhoneNumber nvarchar(50) NOT NULL,
	Email nvarchar(100),
	OrderContent nvarchar(MAX) NOT NULL,
	StatusId int NOT NULL,

	OrderedDate datetime NOT NULL,
	SolvedDate datetime,
	SolvedBy nchar(30) NULL,			--Staff giải quyết Order này, NULL nếu chưa giải quyết xong

	CONSTRAINT StudentId_in_User4 FOREIGN KEY(StudentId) REFERENCES [dbo].[Users](UserId),
	CONSTRAINT StatusId_in_OrderStatuses FOREIGN KEY(StatusId) REFERENCES [dbo].[OrderStatuses](StatusId),
	CONSTRAINT StaffId_in_User4 FOREIGN KEY(SolvedBy) REFERENCES [dbo].[Users](UserId),
) ON [PRIMARY]
GO



--------------------------------------------------[Database Population]----------------------------------------------------------------------------------

--Dữ liệu location này chỉ là tạm thời, sau này set up dc Google Map API sẽ sửa lại
--fu hòa lạc
INSERT INTO [dbo].[Addresses] VALUES (N'Đất Thổ Cư Hòa Lạc, Km29 Đường Cao Tốc 08, Thạch Hoà, Thạch Thất, Hà Nội', 
N'21.018797378240844, 105.52740174223347', 0, GETDATE(), GETDATE());
--fu hcm
INSERT INTO [dbo].[Addresses] VALUES (N'Lô E2a-7, Đường D1, Đ. D1, Long Thạnh Mỹ, Thành Phố Thủ Đức, Thành phố Hồ Chí Minh', 
N'10.841401563327102, 106.80989372598', 0, GETDATE(), GETDATE());
--fu đà nẵng
INSERT INTO [dbo].[Addresses] VALUES (N'Khu đô thị FPT City, Ngũ Hành Sơn, Đà Nẵng', 
N'15.968692643142754, 108.2605889843167', 0, GETDATE(), GETDATE());
--fu cần thơ
INSERT INTO [dbo].[Addresses] VALUES (N'Cầu Rau Răm, đường Đ. Nguyễn Văn Cừ, An Bình, Ninh Kiều, Cần Thơ', 
N'10.014287955096187, 105.73169382835755', 0, GETDATE(), GETDATE());
--fu quy nhơn
INSERT INTO [dbo].[Addresses] VALUES (N'R639+HM2, Khu đô thị mới, Thành phố Qui Nhơn, Bình Định', 
N'13.804165702246436, 109.21920977019123', 0, GETDATE(), GETDATE());
--trọ tâm lê
INSERT INTO [dbo].[Addresses] VALUES (N'Nhà số..., Đường...; Đối diện cổng sau Đại học FPT; Cạnh quán Bún bò Huế', 
N'21.015951854163884, 105.51901041149311', 0, GETDATE(), GETDATE());
--trọ tâm thảo
INSERT INTO [dbo].[Addresses] VALUES (N'Nhách 75, thôn 4, Thạch Hoà, Thạch Thất, Hà Nội, Việt Nam', 
N'21.05713376998603, 105.51922555580272', 0, GETDATE(), GETDATE());
--Trọ Hòa Lạc Yên Lạc Viên
INSERT INTO [dbo].[Addresses] VALUES (N'Nhách 75, thôn 4, Thạch Hoà, Thạch Thất, Hà Nội, Việt Nam', 
N'21.01521162432872, 105.51780510278459', 0, GETDATE(), GETDATE());
--Nhà trọ Bình Yên
INSERT INTO [dbo].[Addresses] VALUES (N'7M3G+CR3, Kiền Sơn, Bình Xuyên, Vĩnh Phúc, Việt Nam', 
N'21.290650589369935, 105.6636771590558', 0, GETDATE(), GETDATE());
--Nhà trọ Tiến Phương
INSERT INTO [dbo].[Addresses] VALUES (N'Thôn 7 Thạch Hoà (cạnh gà Ri Huy Cường, cách gà Ri Phú Bình 2 100 m)', 
N'21.00201027585428, 105.51971682638923', 0, GETDATE(), GETDATE());
--Nhà trọ Phương Duy
INSERT INTO [dbo].[Addresses] VALUES (N'Thôn 8, cây xăng 39 Thạch Hòa', 
N'21.01105532693077, 105.51712737057241', 0, GETDATE(), GETDATE());
--HOLA Campus (chưa có tọa độ, địa chỉ chính xác)
INSERT INTO [dbo].[Addresses] VALUES (N'Nhách 75, thôn 4, Thạch Hoà, Thạch Thất, Hà Nội, Việt Nam', 
N'21.011257155392272, 105.53775892925267', 0, GETDATE(), GETDATE());
--Trọ Hoàng Nam (chưa có tọa độ chính xác)
INSERT INTO [dbo].[Addresses] VALUES (N'Thôn 3 (thôn 8 cũ), Thạch Hoà, Thạch Thất, Hà Nội', 
N'21.05713376998603, 105.51922555580272', 0, GETDATE(), GETDATE());
--Kí túc xá Ông bà (chưa có tọa độ chính xác)
INSERT INTO [dbo].[Addresses] VALUES (N'Cụm 4, thôn 8, mặt đường lộ Cu Ba, QL21A, Thạch Hoà, Thạch Thất, Hanoi, Vietnam', 
N'21.05713376998603, 105.51922555580272', 0, GETDATE(), GETDATE());
--Nhà trọ Thái Hà
INSERT INTO [dbo].[Addresses] VALUES (N'Mặt đường QL 21 thôn 7 Thạch Hòa', 
N'21.05713376998603, 105.51922555580272', 0, GETDATE(), GETDATE());
--Trọ Việt Dũng
INSERT INTO [dbo].[Addresses] VALUES (N'Gần cây xăng 39 (Cạnh Pizza91 - sau quán cắt tóc Mạnh Nguyễn)', 
N'21.02793531455853, 105.51399518075671', 0, GETDATE(), GETDATE());
--Trọ Tuấn Cường
INSERT INTO [dbo].[Addresses] VALUES (N'Công ty CP dịch vụ bảo vệ KCN Cao, Hòa Lạc, Thạch Thất, Hà Nội, Việt Nam', 
N'21.001219979853115, 105.52098402638923', 0, GETDATE(), GETDATE());

-------------------------------------------------------------------------------------------------------------------------------------------

INSERT INTO [dbo].[Campuses] VALUES (N'FU - Hòa Lạc', 1, GETDATE());
INSERT INTO [dbo].[Campuses] VALUES (N'FU - Hồ Chí Minh', 2, GETDATE());
INSERT INTO [dbo].[Campuses] VALUES (N'FU - Đà Nẵng', 3, GETDATE());
INSERT INTO [dbo].[Campuses] VALUES (N'FU - Cần Thơ', 4, GETDATE());
INSERT INTO [dbo].[Campuses] VALUES (N'FU - Quy Nhơn', 5, GETDATE());

-------------------------------------------------------------------------------------------------------------------------------------------

INSERT INTO [dbo].[UserRoles] VALUES (N'Student', GETDATE());
INSERT INTO [dbo].[UserRoles] VALUES (N'Landlord', GETDATE());
INSERT INTO [dbo].[UserRoles] VALUES (N'Head of Admission Department', GETDATE());
INSERT INTO [dbo].[UserRoles] VALUES (N'Head of Student Service Department', GETDATE());
INSERT INTO [dbo].[UserRoles] VALUES (N'Staff of Admission Department', GETDATE());
INSERT INTO [dbo].[UserRoles] VALUES (N'Staff of Student Service Department', GETDATE());

-------------------------------------------------------------------------------------------------------------------------------------------
--Statuses
INSERT INTO [dbo].[UserStatuses] VALUES (0, N'Inactive', GETDATE());
INSERT INTO [dbo].[UserStatuses] VALUES (1, N'Active', GETDATE());
INSERT INTO [dbo].[UserStatuses] VALUES (2, N'Landlord Signup Request', GETDATE());

-------------------------------------------------------------------------------------------------------------------------------------------

--Students
--dữ liệu giả định, sau này cần sửa Fb UserId
INSERT INTO [dbo].[Users] VALUES (N'HE153046', N'someFbUserid', null, null, null, 'Nguyen The Giang', 1, null , null, null, null, null, null, 1, 
GETDATE(), GETDATE(), N'HE153046', N'HE153046');
INSERT INTO [dbo].[Users] VALUES (N'HE150432', null, N'1084120350695981345670', null, null, 'Nguyen Thu An', 1, null , null, null, null, null, null, 1, 
GETDATE(), GETDATE(), N'HE150432', N'HE150432');

INSERT INTO [dbo].[Users] VALUES (N'HE150340', null, N'108412032323648134567', null, null, 'Phung Quang Thong', 1, null , null, null, null, null, null, 1, 
GETDATE(), GETDATE(), N'HE150432', N'HE150432');
INSERT INTO [dbo].[Users] VALUES (N'HE150346', null, N'108412035687598134567', null, null, 'Bui Ngoc Huyen', 1, null , null, null, null, null, null, 1, 
GETDATE(), GETDATE(), N'HE150432', N'HE150432');
INSERT INTO [dbo].[Users] VALUES (N'HE150160', null, N'1084120350695981345671', null, null, 'Nguyen Tri Kien', 1, null , null, null, null, null, null, 1, 
GETDATE(), GETDATE(), N'HE150432', N'HE150432');
INSERT INTO [dbo].[Users] VALUES (N'HE150600', null, N'1084120350695981345672', null, null, 'Nguyen Minh Hanh', 1, null , null, null, null, null, null, 1, 
GETDATE(), GETDATE(), N'HE150432', N'HE150432');
INSERT INTO [dbo].[Users] VALUES (N'HE151378', null, N'108417846369598134567', null, null, 'Nguyen Thi Ngoc Anh', 1, null , null, null, null, null, null, 1, 
GETDATE(), GETDATE(), N'HE150432', N'HE150432');
INSERT INTO [dbo].[Users] VALUES (N'HE150691', null, N'108416523069598134567', null, null, 'Nguyen Tran Hoang', 1, null , null, null, null, null, null, 1, 
GETDATE(), GETDATE(), N'HE150432', N'HE150432');
INSERT INTO [dbo].[Users] VALUES (N'HE153222', null, N'10841208034698134567', null, null, 'Tran Thi Nguyet Ha', 1, null , null, null, null, null, null, 1, 
GETDATE(), GETDATE(), N'HE150432', N'HE150432');
INSERT INTO [dbo].[Users] VALUES (N'HE153299', null, N'108412035376498134567', null, null, 'Tong Truogn Giang', 1, null , null, null, null, null, null, 1, 
GETDATE(), GETDATE(), N'HE150432', N'HE150432');
INSERT INTO [dbo].[Users] VALUES (N'HE153590', null, N'108442175069598134567', null, null, 'Dinh The Thuan', 1, null , null, null, null, null, null, 1, 
GETDATE(), GETDATE(), N'HE150432', N'HE150432');
INSERT INTO [dbo].[Users] VALUES (N'HE172884', null, N'108412035032158134567', null, null, 'Pham Vu Thai Minh', 1, null , null, null, null, null, null, 1, 
GETDATE(), GETDATE(), N'HE150432', N'HE150432');

--Staffs
-- password: thanhle
INSERT INTO [dbo].[Users] VALUES (N'SA000001', null, null, N'thanhle@gmail.com', N'AQAAAAEAACcQAAAAEJCloD0i7VZc1j5n/6cOh78keYPynrQMmdYV7Fx3/5XhDLwtreP8uf9ewo1MON/Yag==', N'Lê Thành', 1, 'image_profile_1.jpg', null, null, null, null, null, 3, 
GETDATE(), GETDATE(), N'SA000001', N'SA000001');

--Landlords
INSERT INTO [dbo].[Users] VALUES (N'LA000001', null, null, N'tamle@gmail.com', N'tamle', N'Tâm Lê', 0, 'image_profile_1.jpg', '0987654321', 'facebook.com/tamle12', 'identity_card_front.jpg', 'identity_card_back.jpg', 6, 2, 
GETDATE(), GETDATE(), N'SA000001', N'SA000001');
INSERT INTO [dbo].[Users] VALUES (N'LA000002', null, null, N'dungnhung@gmail.com', N'dungnhung', N'Dũng Nhung', 0, 'image_profile_1.jpg', '0982298681', 'facebook.com/dungnhung34', 'identity_card_front.jpg', 'identity_card_back.jpg', 6, 2, 
GETDATE(), GETDATE(), N'SA000001', N'SA000001');
INSERT INTO [dbo].[Users] VALUES (N'LA000003', null, null, N'ngahuong@gmail.com', N'ngahuong', N'Nga Hương', 1, 'image_profile_1.jpg', ' 0984530814', 'facebook.com/ngahuong12', 'identity_card_front.jpg', 'identity_card_back.jpg', 6, 2,  
GETDATE(), GETDATE(), N'SA000001', N'SA000001');
INSERT INTO [dbo].[Users] VALUES (N'LA000004', null, null, N'binhyen@gmail.com', N'binhyen', N'Bình Yên', 1, 'image_profile_1.jpg', '0973866690', 'facebook.com/binhyen56', 'identity_card_front.jpg', 'identity_card_back.jpg', 6, 2, 
GETDATE(), GETDATE(), N'SA000001', N'SA000001');
INSERT INTO [dbo].[Users] VALUES (N'LA000005', null, null, N'tienphuong@gmail.com', N'tienphuong', N'Tiến Phương', 1, 'image_profile_1.jpg', '0961602245', 'facebook.com/tienphuong88', 'identity_card_front.jpg', 'identity_card_back.jpg', 6, 2, 
GETDATE(), GETDATE(), N'SA000001', N'SA000001');
INSERT INTO [dbo].[Users] VALUES (N'LA000006', null, null, N'ngochuong@gmail.com', N'ngochuong', N'Ngọc Hương', 1, 'image_profile_1.jpg', '0981914814', 'facebook.com/ngochuong555', 'identity_card_front.jpg', 'identity_card_back.jpg', 6, 2, 
GETDATE(), GETDATE(), N'SA000001', N'SA000001');
INSERT INTO [dbo].[Users] VALUES (N'LA000007', null, null, N'mailinh@gmail.com', N'mailinh', N'Mai Linh', 1, 'image_profile_1.jpg', '0846821118', 'facebook.com/mailinh32132', 'identity_card_front.jpg', 'identity_card_back.jpg', 6, 2, 
GETDATE(), GETDATE(), N'SA000001', N'SA000001');
INSERT INTO [dbo].[Users] VALUES (N'LA000008', null, null, N'hoangkhanh@gmail.com', N'hoangkhanh', N'Hoàng Khánh', 1, 'image_profile_1.jpg', '0989639985', 'facebook.com/hoangkhanh11', 'identity_card_front.jpg', 'identity_card_back.jpg', 6, 2, 
GETDATE(), GETDATE(), N'SA000001', N'SA000001');
INSERT INTO [dbo].[Users] VALUES (N'LA000009', null, null, N'phuongoanh@gmail.com', N'phuongoanh', N'Phương Oanh', 1, 'image_profile_1.jpg', '0989639985', 'facebook.com/phuongoanh563112s', 'identity_card_front.jpg', 'identity_card_back.jpg', 6, 2, 
GETDATE(), GETDATE(), N'SA000001', N'SA000001');
INSERT INTO [dbo].[Users] VALUES (N'LA000010', null, null, N'thaiha@gmail.com', N'thaiha', N'Thái Hà', 1, 'image_profile_1.jpg', '0961602245', 'facebook.com/thaiha55tps', 'identity_card_front.jpg', 'identity_card_back.jpg', 6, 2,
GETDATE(), GETDATE(), N'SA000001', N'SA000001');
INSERT INTO [dbo].[Users] VALUES (N'LA000011', null, null, N'vietdung@gmail.com', N'vietdung', N'Việt Dũng', 1, 'image_profile_1.jpg', '0363266546', 'facebook.com/vietdung223', 'identity_card_front.jpg', 'identity_card_back.jpg', 6, 2, 
GETDATE(), GETDATE(), N'SA000001', N'SA000001');
INSERT INTO [dbo].[Users] VALUES (N'LA000012', null, null, N'tuancuong@gmail.com', N'tuancuong', N'Tuấn Cường', 1, 'image_profile_1.jpg', '0363266546', 'facebook.com/tuancuong444', 'identity_card_front.jpg', 'identity_card_back.jpg', 6, 2, 
GETDATE(), GETDATE(), N'SA000001', N'SA000001');
INSERT INTO [dbo].[Users] VALUES (N'LA000013', null, null, N'phuongduy@gmail.com', N'phuongduy', N'Phương Duy', 1, 'image_profile_1.jpg', '0365928073', 'facebook.com/phuongduy9512', 'identity_card_front.jpg', 'identity_card_back.jpg', 6, 2,  
GETDATE(), GETDATE(), N'SA000001', N'SA000001');
INSERT INTO [dbo].[Users] VALUES (N'LA000014', null, null, N'minhvu@gmail.com', N'minhvu', N'Minh Vũ', 2, 'image_profile_1.jpg', '0365928072', 'facebook.com/minhvu', 'identity_card_front.jpg', 'identity_card_back.jpg', 6, 2,  
GETDATE(), GETDATE(), N'SA000001', N'SA000001');
INSERT INTO [dbo].[Users] VALUES (N'LA000015', null, null, N'phuongnguyen@gmail.com', N'phuongnguyen', N'Phương Nguyễn', 2, 'image_profile_1.jpg', '0365928071', 'facebook.com/phuongnguyen', 'identity_card_front.jpg', 'identity_card_back.jpg', 6, 2,  
GETDATE(), GETDATE(), N'SA000001', N'SA000001');

-------------------------------------------------------------------------------------------------------------------------------------------

--FU Hòa Lạc
INSERT INTO [dbo].[Districts] VALUES (N'Huyện Thạch Thất', 1, GETDATE());
INSERT INTO [dbo].[Districts] VALUES (N'Huyện Quốc Oai', 1, GETDATE());
INSERT INTO [dbo].[Districts] VALUES (N'Thị xã Sơn Tây', 1, GETDATE());
--FU HCM
INSERT INTO [dbo].[Districts] VALUES (N'Thành phố Thủ Đức', 2, GETDATE());
--FU Đà Nẵng
INSERT INTO [dbo].[Districts] VALUES (N'Quận Ngũ Hành Sơn', 3, GETDATE());
--FU Cần Thơ
INSERT INTO [dbo].[Districts] VALUES (N'Quận Ninh Kiều', 4, GETDATE());
--FU Quy Nhơn
INSERT INTO [dbo].[Districts] VALUES (N'Thành phố Quy Nhơn', 5, GETDATE());

-------------------------------------------------------------------------------------------------------------------------------------------

INSERT INTO [dbo].[Communes] VALUES (N'Thị trấn Liên Quan', 1, GETDATE());
INSERT INTO [dbo].[Communes] VALUES (N'Xã Bình Phú', 1, GETDATE());
INSERT INTO [dbo].[Communes] VALUES (N'Xã Bình Yên', 1, GETDATE());
INSERT INTO [dbo].[Communes] VALUES (N'Xã Canh Nậu', 1, GETDATE());
INSERT INTO [dbo].[Communes] VALUES (N'Xã Cẩm Yên', 1, GETDATE());
INSERT INTO [dbo].[Communes] VALUES (N'Xã Cần Kiệm', 1, GETDATE());
INSERT INTO [dbo].[Communes] VALUES (N'Xã Chàng Sơn', 1, GETDATE());
INSERT INTO [dbo].[Communes] VALUES (N'Xã Dị Nậu', 1, GETDATE());
INSERT INTO [dbo].[Communes] VALUES (N'Xã Đại Đồng', 1, GETDATE());
INSERT INTO [dbo].[Communes] VALUES (N'Xã Đồng Trúc', 1, GETDATE());
INSERT INTO [dbo].[Communes] VALUES (N'Xã Hạ Bằng', 1, GETDATE());
INSERT INTO [dbo].[Communes] VALUES (N'Xã Hương Ngải', 1, GETDATE());
INSERT INTO [dbo].[Communes] VALUES (N'Xã Hữu Bằng', 1, GETDATE());
INSERT INTO [dbo].[Communes] VALUES (N'Xã Kim Quan', 1, GETDATE());
INSERT INTO [dbo].[Communes] VALUES (N'Xã Lại Thượng', 1, GETDATE());
INSERT INTO [dbo].[Communes] VALUES (N'Xã Phú Kim', 1, GETDATE());
INSERT INTO [dbo].[Communes] VALUES (N'Xã Phùng Xá', 1, GETDATE());
INSERT INTO [dbo].[Communes] VALUES (N'Xã Tân Xã', 1, GETDATE());
INSERT INTO [dbo].[Communes] VALUES (N'Xã Thạch Hòa', 1, GETDATE());
INSERT INTO [dbo].[Communes] VALUES (N'Xã Thạch Xá', 1, GETDATE());
INSERT INTO [dbo].[Communes] VALUES (N'Xã Tiến Xuân', 1, GETDATE());
INSERT INTO [dbo].[Communes] VALUES (N'Xã Yên Bình', 1, GETDATE());
INSERT INTO [dbo].[Communes] VALUES (N'Xã Yên Trung', 1, GETDATE());

INSERT INTO [dbo].[Communes] VALUES (N'Thị trấn Quốc Oai', 2, GETDATE());
INSERT INTO [dbo].[Communes] VALUES (N'Xã Cấn Hữu', 2, GETDATE());
INSERT INTO [dbo].[Communes] VALUES (N'Xã Cộng Hòa', 2, GETDATE());
INSERT INTO [dbo].[Communes] VALUES (N'Xã Đại Thành', 2, GETDATE());
INSERT INTO [dbo].[Communes] VALUES (N'Xã Đồng Quang', 2, GETDATE());
INSERT INTO [dbo].[Communes] VALUES (N'Xã Đông Xuân', 2, GETDATE());
INSERT INTO [dbo].[Communes] VALUES (N'Xã Đông Yên', 2, GETDATE());
INSERT INTO [dbo].[Communes] VALUES (N'Xã Hòa Thạch', 2, GETDATE());
INSERT INTO [dbo].[Communes] VALUES (N'Xã Liệp Tuyết', 2, GETDATE());
INSERT INTO [dbo].[Communes] VALUES (N'Xã Nghĩa Hương', 2, GETDATE());
INSERT INTO [dbo].[Communes] VALUES (N'Xã Ngọc Liệp', 2, GETDATE());
INSERT INTO [dbo].[Communes] VALUES (N'Xã Ngọc Mỹ', 2, GETDATE());
INSERT INTO [dbo].[Communes] VALUES (N'Xã Phú Cát', 2, GETDATE());
INSERT INTO [dbo].[Communes] VALUES (N'Xã Phú Mãn', 2, GETDATE());
INSERT INTO [dbo].[Communes] VALUES (N'Xã Phượng Cách', 2, GETDATE());
INSERT INTO [dbo].[Communes] VALUES (N'Xã Sài Sơn', 2, GETDATE());
INSERT INTO [dbo].[Communes] VALUES (N'Xã Tân Hòa', 2, GETDATE());
INSERT INTO [dbo].[Communes] VALUES (N'Xã Tân Phú', 2, GETDATE());
INSERT INTO [dbo].[Communes] VALUES (N'Xã Thạch Thán', 2, GETDATE());
INSERT INTO [dbo].[Communes] VALUES (N'Xã Tuyết Nghĩa', 2, GETDATE());
INSERT INTO [dbo].[Communes] VALUES (N'Xã Yên Sơn', 2, GETDATE());

INSERT INTO [dbo].[Communes] VALUES (N'Phường Lê Lợi', 3, GETDATE());
INSERT INTO [dbo].[Communes] VALUES (N'Phường Ngô Quyền', 3, GETDATE());
INSERT INTO [dbo].[Communes] VALUES (N'Phường Phú Thịnh', 3, GETDATE());
INSERT INTO [dbo].[Communes] VALUES (N'Phường Quang Trung', 3, GETDATE());
INSERT INTO [dbo].[Communes] VALUES (N'Phường Sơn Lộc', 3, GETDATE());
INSERT INTO [dbo].[Communes] VALUES (N'Phường Trung Hưng', 3, GETDATE());
INSERT INTO [dbo].[Communes] VALUES (N'Phường Trung Sơn Trầm', 3, GETDATE());
INSERT INTO [dbo].[Communes] VALUES (N'Phường Viên Sơn', 3, GETDATE());
INSERT INTO [dbo].[Communes] VALUES (N'Phường Xuân Khanh', 3, GETDATE());
INSERT INTO [dbo].[Communes] VALUES (N'Xã Cổ Đông', 3, GETDATE());
INSERT INTO [dbo].[Communes] VALUES (N'Xã Đường Lâm', 3, GETDATE());
INSERT INTO [dbo].[Communes] VALUES (N'Xã Kim Sơn', 3, GETDATE());
INSERT INTO [dbo].[Communes] VALUES (N'Xã Sơn Đông', 3, GETDATE());
INSERT INTO [dbo].[Communes] VALUES (N'Xã Thanh Mỹ', 3, GETDATE());
INSERT INTO [dbo].[Communes] VALUES (N'Xã Xuân Sơn', 3, GETDATE());

INSERT INTO [dbo].[Communes] VALUES (N'Phường Long Thạnh Mỹ', 4, GETDATE());

-------------------------------------------------------------------------------------------------------------------------------------------

INSERT INTO [dbo].[Villages] VALUES (N'Chi Quan 1', 1, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Chi Quan 2', 1, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Đồng Cam', 1, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Phú Thứ', 1, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Đụn Dương', 1,  GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Hà Tân', 1, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Khu Phố', 1, GETDATE());

INSERT INTO [dbo].[Villages] VALUES (N'Bình Xá', 2, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Bình Xá', 2, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Phú Hòa', 2, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Phú Ổ 1', 2, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Phú Ổ 2', 2, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Phú Ổ 3', 2, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Phú Ổ 4', 2, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Phú Ổ 5', 2, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Phú Ổ 6', 2, GETDATE());

INSERT INTO [dbo].[Villages] VALUES (N'Yên Mỹ', 3, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Phúc Tiến', 3, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Đồi Sen', 3, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Sen Trì', 3, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Cánh Chủ', 3, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Vân Lôi', 3, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Thái Bình', 3, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Hòa Lạc', 3, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Linh Sơn', 3, GETDATE());

INSERT INTO [dbo].[Villages] VALUES (N'1', 4, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'2', 4, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'3', 4, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'4', 4, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'5', 4, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'6', 4, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'7', 4, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'8', 4, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'9', 4, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'10', 4, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'11', 4, GETDATE());

INSERT INTO [dbo].[Villages] VALUES (N'Cẩm Bào', 5, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Kinh Đạ', 5, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Yên Lỗ', 5, GETDATE());

INSERT INTO [dbo].[Villages] VALUES (N'Phú Đa 1', 6, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Phú Đa 2', 6, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Phú Lễ', 6, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Yên Lạc 1', 6, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Yên Lạc 2', 6, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Yên Lạc 3', 6, GETDATE());

INSERT INTO [dbo].[Villages] VALUES (N'1', 7, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'2', 7, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'3', 7, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'4', 7, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'5', 7, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'6', 7, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'7', 7, GETDATE());

INSERT INTO [dbo].[Villages] VALUES (N'Tam Nông 1', 8, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Tam Nông 2', 8, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Hòa Bình 1', 8, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Hòa Bình 2', 8, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Đoàn Kết 1', 8, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Đoàn Kết 2', 8, GETDATE());

INSERT INTO [dbo].[Villages] VALUES (N'Minh Nghĩa', 9, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Minh Đức', 9, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Ngọc Lâu', 9, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Hương Lam', 9, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Rộc Đoài', 9, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Tây Trong', 9, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Hàn Chùa', 9, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Đình Rối', 9, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Lươn Trong', 9, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Lươn Ngoài', 9, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Đồng Cầu', 9, GETDATE());

INSERT INTO [dbo].[Villages] VALUES (N'Chầm Muộn', 10, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Trúc Voi', 10, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Đồng Táng', 10, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Đồng Kho', 10, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Hòa Bình', 10, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Chiến Thắng', 10, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Khu Ba', 10, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Xóm Đông', 10, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Khoang Mè', 10, GETDATE());

INSERT INTO [dbo].[Villages] VALUES (N'Khoang Mè 1', 11, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Khoang Mè 2', 11, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Đầm Cầu', 11, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Đầm Quán', 11, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Giếng Cốc', 11, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Mương Ốc', 11, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Vực Giang', 11, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Gò Mận', 11, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Giang Nu', 11, GETDATE());

INSERT INTO [dbo].[Villages] VALUES (N'1', 12, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'2', 12, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'3', 12, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'4', 12, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'5', 12, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'6', 12, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'7', 12, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'8', 12, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'9', 12, GETDATE());

INSERT INTO [dbo].[Villages] VALUES (N'Si Chợ', 13, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Bò', 13, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Sen Trì', 13, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Bàn Giữa', 13, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Đình', 13, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Đông', 13, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Miễu', 13, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Ba Mát', 13, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Giếng Cốc', 13, GETDATE());

INSERT INTO [dbo].[Villages] VALUES (N'1', 14, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'2', 14, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'3', 14, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'4', 14, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Mơ', 14, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'6', 14, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'7', 14, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'8', 14,  GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'9', 14, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'10', 14, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'84', 14, GETDATE());

INSERT INTO [dbo].[Villages] VALUES (N'Ngũ Sơn', 15, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Lại Khánh', 15, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Lại Thượng', 15, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Phú Thụ', 15, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Thanh Câu', 15, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Hoàng Xá', 15, GETDATE());

INSERT INTO [dbo].[Villages] VALUES (N'Thủy Lai', 16, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Phú Nghĩa', 16, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Bách Kim', 16, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Nội Thôn', 16, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Ngoại Thôn', 16, GETDATE());

INSERT INTO [dbo].[Villages] VALUES (N'1', 17, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'2', 17, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'3', 17, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'4', 17, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'5', 17, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'6', 17, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'7', 17, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'8', 17, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'9', 17, GETDATE());

INSERT INTO [dbo].[Villages] VALUES (N'Phú Hữu', 18, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Cừ Viên', 18, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Cầu Giáo', 18, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Hương Trung', 18, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Cầu Sông', 18, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Xóm Mới', 18, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Xóm Quán', 18, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Xóm Hiệp', 18, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Xóm Than', 18, GETDATE());

INSERT INTO [dbo].[Villages] VALUES (N'1', 19, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'2', 19, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'3', 19, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'4', 19, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'5', 19, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'6', 19, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'7', 19, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'8', 19, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'9', 19, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'10', 19, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'11', 19, GETDATE());

INSERT INTO [dbo].[Villages] VALUES (N'1', 20, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'2', 20, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'3', 20, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'4', 20, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'5', 20, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'6', 20, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'7', 20, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'8', 20, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'9', 20, GETDATE());

INSERT INTO [dbo].[Villages] VALUES (N'Chùa 1', 21, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Chùa 2', 21, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Đồng Dâu', 21, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Đồng Cao', 21, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Miễu 1', 21, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Miễu 2', 21, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Gò Chói 1', 21, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Gò Chói 2', 21, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Gò Mè', 21, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Bình Sơn', 21, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Bãi Dài', 21, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Trại Mới 1', 21, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Trại Mới 2', 21, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Cố Đụng 1', 21, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Cố Đụng 2', 21, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Quê Vải', 21, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Gò Chè', 21, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Nhòn', 21, GETDATE());

INSERT INTO [dbo].[Villages] VALUES (N'Tân Bình', 22, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Lụa', 22, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Vao', 22, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Thung Mộ', 22, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Thạch Bình', 22, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Thuống', 22, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Dục', 22, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Đình', 22, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Cò', 22, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Dân Lập', 22, GETDATE());

INSERT INTO [dbo].[Villages] VALUES (N'Bối', 23, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Luồng', 23, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Số', 23, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Tơi', 23, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Hương', 23, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Lặt', 23, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Hội', 23, GETDATE());

INSERT INTO [dbo].[Villages] VALUES (N'Đình Tổ', 24, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Du Nghệ', 24, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Hoa Vôi', 24, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'TDP Ngô Sài', 24, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'TDP Phố Huyện', 24, GETDATE());

INSERT INTO [dbo].[Villages] VALUES (N'Cấn Hạ', 25, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Cấn Thượng', 25,GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Cây Chay', 25, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Đĩnh Tú', 25, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Thái Khê', 25, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Thượng Khê', 25, GETDATE());

INSERT INTO [dbo].[Villages] VALUES (N'1', 26, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'2', 26, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'3', 26, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'4', 26, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'5', 26, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'6', 26, GETDATE());

INSERT INTO [dbo].[Villages] VALUES (N'Đại Tảo', 27, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Độ Tràng', 27, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Tình Lam', 27, GETDATE());

INSERT INTO [dbo].[Villages] VALUES (N'Đồng Lư', 28, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Dương Cốc', 28, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Yên Nội', 28, GETDATE());

INSERT INTO [dbo].[Villages] VALUES (N'Cửu Khâu', 29, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Đà Thâm', 29, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Đồng Âm', 29, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Đồng Bèn 1', 29, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Đồng Bèn 2', 29, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Đồng Bồ', 29, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Đồng Chằn', 29, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Đồng Rằng', 29, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Lập Thành', 29, GETDATE());

INSERT INTO [dbo].[Villages] VALUES (N'Đông Hạ', 30, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Đông Thượng', 30, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Việt Yên', 30, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Yên Thái', 30, GETDATE());

INSERT INTO [dbo].[Villages] VALUES (N'Bạch Thạch', 31, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Hòa Phú', 31, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Hòa Trúc', 31, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Long Phú', 31, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Thắng Đầu', 31, GETDATE());

INSERT INTO [dbo].[Villages] VALUES (N'Bái Ngoại', 32, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Bái Nội', 32, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Đại Phu', 32, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Thông Đạt', 32, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Vĩnh Phúc', 32, GETDATE());

INSERT INTO [dbo].[Villages] VALUES (N'Thế Trụ', 33, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Văn Khê', 33, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Văn Quang', 33, GETDATE());

INSERT INTO [dbo].[Villages] VALUES (N'Đồng Bụt', 34, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Liệp Mai', 34, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Ngọc Bài', 34, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Ngọc Phúc', 34, GETDATE());

INSERT INTO [dbo].[Villages] VALUES (N'Ngọc Than', 35, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Phúc Mỹ', 35, GETDATE());

INSERT INTO [dbo].[Villages] VALUES (N'1', 36, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'2', 36, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'3', 36, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'4', 36, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'5', 36, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'6', 36, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'7', 36, GETDATE());

INSERT INTO [dbo].[Villages] VALUES (N'1 (Đồng Vàng)', 37, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'2 (Cổ Rùa)', 37, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'3 (Đồng Âm)', 37, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'4 (Trán Voi)', 37, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'5 (Đồng Vỡ)', 37, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'(Làng Trên)', 37, GETDATE());

INSERT INTO [dbo].[Villages] VALUES (N'1', 38, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'2', 38, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'3', 38, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'4', 38, GETDATE());

INSERT INTO [dbo].[Villages] VALUES (N'Đa Phúc', 39, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Khánh Tân', 39, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Năm Trại', 39, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Phúc Đức', 39, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Sài Khê', 39, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Thụy Khê', 39, GETDATE());

INSERT INTO [dbo].[Villages] VALUES (N'An Ninh', 40, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Bờ Hồ', 40, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Đồng Găng', 40, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Thị Ngoại', 40, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Thổ Ngõa', 40, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Yên Mã', 40, GETDATE());

INSERT INTO [dbo].[Villages] VALUES (N'Hạ Hòa', 41, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Phú Hạng', 41, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Yên Quán', 41, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Yên Quang', 41, GETDATE());

INSERT INTO [dbo].[Villages] VALUES (N'1', 42, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'2', 42, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'3', 42, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'4', 42, GETDATE());

INSERT INTO [dbo].[Villages] VALUES (N'Cổ Hiền', 43, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Đại Đồng', 43, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Độ Lân', 43, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Đông Sơn', 43, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Liên Trì', 43, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Muôn', 43, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Ro', 43, GETDATE());

INSERT INTO [dbo].[Villages] VALUES (N'Ba Nhà', 44, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Quảng Yên', 44, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Sơn Trung', 44, GETDATE());

INSERT INTO [dbo].[Villages] VALUES (N'TDP Hậu An', 45, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'TDP Hậu Bình', 45, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'TDP Hậu Ninh', 45, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'TDP Hậu Thái', 45, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'TDP Hậu Tĩnh', 45, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'TDP Hồng Hà', 45, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'TDP Lạc Sơn', 45, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'TDP Trạng Trình', 45, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'TDP Trưng Vương', 45, GETDATE());

INSERT INTO [dbo].[Villages] VALUES (N'1', 46, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'2', 46, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'3', 46, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'4', 46, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'5', 46, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'6', 46, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'7', 46, GETDATE());

INSERT INTO [dbo].[Villages] VALUES (N'Hồng Hậu', 47, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Phố Hàng', 47, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Phú Mai', 47, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Phú Nhi 1', 47, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Phú Nhi 2', 47, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Phú Nhi 3', 47, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Yên Thịnh', 47, GETDATE());

INSERT INTO [dbo].[Villages] VALUES (N'Tổ 1', 48, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Tổ 2', 48, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Tổ 3', 48, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Tổ 4', 48, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Tổ 5', 48, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Tổ 6', 48, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Tổ 7', 48, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Tổ 8', 48, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Tổ 9', 48, GETDATE());

INSERT INTO [dbo].[Villages] VALUES (N'Tổ 1', 49, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Tổ 2', 49, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Tổ 3', 49, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Tổ 4', 49, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Tổ 5', 49, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Tổ 6', 49, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Tổ 7', 49, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Tổ 8', 49, GETDATE());

INSERT INTO [dbo].[Villages] VALUES (N'TDP 1', 50, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'TDP 2', 50, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'TDP 3', 50, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'TDP 4', 50, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'TDP 5', 50, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'TDP 6', 50, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'TDP 7', 50, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'TDP 8', 50, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'TDP 9', 50, GETDATE());

INSERT INTO [dbo].[Villages] VALUES (N'Khu phố 1', 51, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Khu phố 2', 51, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Khu phố 3', 51, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Khu phố 4', 51, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Khu phố 5', 51, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Khu phố 6', 51, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Khu phố 7', 51, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Khu phố 8', 51, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Khu phố 9', 51, GETDATE());

INSERT INTO [dbo].[Villages] VALUES (N'TDP La Thành', 52, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'TDP Thiều Xuân', 52, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'TDP1 Phù Sa', 52, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'TDP1 Tiền Huân', 52, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'TDP2 Phù Xa', 52, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'TDP2 Tiền Huân', 52, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'TDP3 Phù Sa', 52, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'TDP3 Tiền Huân', 52, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'TDP4 Tiền Huân', 52, GETDATE());

INSERT INTO [dbo].[Villages] VALUES (N'TDP 1', 53, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'TDP 2', 53, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'TDP 3', 53, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'TDP 4', 53, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'TDP 5', 53, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'TDP 6', 53, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'TDP 7', 53, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'TDP 8', 53, GETDATE());

INSERT INTO [dbo].[Villages] VALUES (N'Cổ Liễn', 54, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Đại Trung', 54, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Đoàn Kết', 54, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Đồng Trạng', 54, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'La Gián', 54, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Ngõ Bắc', 54, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Ngọc Kiên', 54, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Phúc Lộc', 54, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Thiên Mã', 54, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Trại Hồ', 54, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Trại Láng', 54, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Triều Đông', 54, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Trung Lạc', 54, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Vĩnh Lộc', 54, GETDATE());

INSERT INTO [dbo].[Villages] VALUES (N'Cam Lâm', 55, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Cam Thịnh', 55, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Đoài Giáp', 55, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Đông Sàng', 55, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Hà tân', 55, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Hưng Thịnh', 55, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Mông Phụ', 55, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Phụ Khang', 55, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Văn Miếu', 55, GETDATE());

INSERT INTO [dbo].[Villages] VALUES (N'Kim Chung', 56, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Kim Đái 1', 56, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Kim Đái 2', 56, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Kim Tân', 56, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Lòng Hồ', 56, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Ngải Sơn', 56, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Nhà Thờ', 56, GETDATE());

INSERT INTO [dbo].[Villages] VALUES (N'Bắc', 57, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Bình Sơn', 57, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Bồng', 57, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Cao Sơn', 57, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Đa AB', 57, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Đại Quang', 57, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Đậu', 57, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Điếm Ba', 57, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Đình', 57, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Đồi Chợ', 57, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Đồi Vua', 57, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Đông A', 57, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Khoang Sau', 57, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Năn', 57, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Tân An', 57, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Tân Phú', 57, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Tân Phúc', 57, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Vạn An', 57, GETDATE());

INSERT INTO [dbo].[Villages] VALUES (N'400', 58, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Đồng Đổi', 58, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Phố Vị Thủy', 58, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Quảng Đại', 58, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Tây Vị', 58, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Thanh Tiến', 58, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Thanh Vị', 58, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Thủ Trung', 58, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'TDP Vị Thủy', 58, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'TDP Z155', 58, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Yên Mỹ', 58, GETDATE());

INSERT INTO [dbo].[Villages] VALUES (N'An Sơn', 59, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Kỳ Sơn', 59, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Lễ Khê', 59, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Nhân Lý', 59, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Tam Sơn', 59, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Văn Khê', 59, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Xuân Khanh', 59, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Z 151', 59, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Z 175', 59, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Xóm Bướm', 59, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Xóm Chằm', 59, GETDATE());

INSERT INTO [dbo].[Villages] VALUES (N'13KP Long Hòa', 60, GETDATE());

-------------------------------------------------------------------------------------------------------------------------------------------

INSERT INTO [dbo].[Houses] VALUES (N'Trọ Tâm Lê', 34, N'Rất đẹp', 6, 3, N'LA000001', 1, 4.5, 3700, 1200, 0, 0, 0 , 0, GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[Houses] VALUES (N'Trọ Tâm Thảo', 34, N'Rất đẹp', 7, 3, N'LA000002', 1, 3.5, 3500, 1300, 0, 1, 0, 0, GETDATE(), GETDATE(), N'LA000002', N'LA000002');
INSERT INTO [dbo].[Houses] VALUES (N'Trọ Hòa Lạc Yên Lạc Viên', 76, N'Vị trí trung tâm khu giao lộ ngã tư Hòa Lạc. Cảnh quan trong lành, yên tĩnh', 8, 3, N'LA000003', 1, 5.5, 3400, 1500, 0, 0, 0 , 0, GETDATE(), GETDATE(), N'LA000003', N'LA000003');
INSERT INTO [dbo].[Houses] VALUES (N'Nhà trọ Bình Yên', 87, N'Không chung chủ, giờ giấc thoải mái', 9, 3, N'LA000004', 1, 6.5, 3500, 1300, 0, 0, 0, 0, GETDATE(), GETDATE(), N'LA000004', N'LA000004');
INSERT INTO [dbo].[Houses] VALUES (N'Nhà trọ Tiến Phương', 102, N'Sân rộng rãi, chỗ để xe thuận tiện, đảm bảo an ninh', 10, 3, N'LA000005', 1, 7, 3600, 1200, 0, 1, 0, 0, GETDATE(), GETDATE(), N'LA000005', N'LA000005');
INSERT INTO [dbo].[Houses] VALUES (N'Nhà trọ Phương Duy', 31, N' Phòng full nội thất, có chỗ nấu ăn riêng ', 11, 3, N'LA000006', 1, 8, 3800, 1300, 0, 1, 0, 0, GETDATE(), GETDATE(), N'LA000006', N'LA000006');
INSERT INTO [dbo].[Houses] VALUES (N'HOLA Campus', 102, N'Điện nước theo khung giá nhà nước', 12, 3, N'LA000007', 1, 9, 3200, 1500, 0, 1, 1, 0, GETDATE(), GETDATE(), N'LA000007', N'LA000007');
INSERT INTO [dbo].[Houses] VALUES (N'Trọ Hoàng Nam', 102, N'Có siêu thị ngay tại tầng 1 cùng khuôn viên vui chơi rộng rãi.', 13, 3, N'LA000008', 1, 0.5, 3400, 1250, 0, 0, 0, 0, GETDATE(), GETDATE(), N'LA000008', N'LA000008');
INSERT INTO [dbo].[Houses] VALUES (N'Kí túc xá Ông bà', 31, N'Có sân vườn, ao cá, hợp với các bạn thích chill và yêu thiên nhiên', 14, 3, N'LA000009', 1, 2.3, 3600, 1400, 0, 0, 0, 0, GETDATE(), GETDATE(), N'LA000009', N'LA000009');
INSERT INTO [dbo].[Houses] VALUES (N'Nhà trọ Thái Hà', 31, N'Đồ dùng đã có đủ, chỉ việc xách Vali đến ở.', 15, 3, N'LA000010', 1, 3.2, 3500, 1500, 0, 0, 1, 0, GETDATE(), GETDATE(), N'LA000010', N'LA000010');
INSERT INTO [dbo].[Houses] VALUES (N'Trọ Việt Dũng', 87, N'Đã đầy đủ nội thất từ cái tăm cái đũa yên tâm toàn đồ đẹp tại vì mình sống như gia đình nên chỉ yêu cầu sạch sẽ biết giữ gìn tài sản.', 16, 3, N'LA000011', 1, 1.7, 3500, 1700, 0, 0, 0, 0, GETDATE(), GETDATE(), N'LA000011', N'LA000011');
INSERT INTO [dbo].[Houses] VALUES (N'Trọ Tuấn Cường 1', 87, N'Rất đẹp', 17, 3, N'LA000012', 1, 0.8, 3000, 1100, 0, 0, 0, 0, GETDATE(), GETDATE(), N'LA000012', N'LA000012');
INSERT INTO [dbo].[Houses] VALUES (N'Trọ Linh Lê', 72, N'Không chung chủ', 6, 3, N'LA000001', 1, 0.9, 3700, 1200, 0, 0, 0, 0, GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[Houses] VALUES (N'Trọ Tâm Nguyễn', 72, N'Vệ sinh khép kín', 6, 3, N'LA000001', 1, 0.1, 3700, 1200, 0, 1, 0, 0, GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[Houses] VALUES (N'Trọ Thu Thảo', 31, N'Không chung chủ', 7, 3, N'LA000002', 1, 0.5, 3500, 1300, 0, 1, 0, 0, GETDATE(), GETDATE(), N'LA000002', N'LA000002');
INSERT INTO [dbo].[Houses] VALUES (N'Trọ Tâm Lê', 87, N'Chỗ để xe rộng rãi', 7, 3, N'LA000002', 1, 1.2, 3500, 1300, 0, 1, 0, 0, GETDATE(), GETDATE(), N'LA000002', N'LA000002');
INSERT INTO [dbo].[Houses] VALUES (N'Trọ 123', 72, N'Cảnh quan trong lành, yên tĩnh', 8, 3, N'LA000003', 1, 2.4, 3400, 1500, 0, 0, 0, 0, GETDATE(), GETDATE(), N'LA000003', N'LA000003');
INSERT INTO [dbo].[Houses] VALUES (N'Trọ Chương Văn', 31, N'Không chung chủ', 8, 3, N'LA000003', 1, 3.8, 3400, 1500, 0, 1, 0, 0, GETDATE(), GETDATE(), N'LA000003', N'LA000003');
INSERT INTO [dbo].[Houses] VALUES (N'Trọ test ko available', 72, N'Không chung chủ', 8, 3, N'LA000003', 1, 4.6, 3400, 1500, 0, 0, 1, 0, GETDATE(), GETDATE(), N'LA000003', N'LA000003');
INSERT INTO [dbo].[Houses] VALUES (N'Trọ Campus 1', 31, N'Không chung chủ', 8, 3, N'LA000001', 1, 2.7, 3400, 1500, 0, 0, 1, 0, GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[Houses] VALUES (N'Trọ Campus 2', 72, N'Không chung chủ', 8, 3, N'LA000002', 2, 2.5, 3400, 1500, 0, 0, 1, 0, GETDATE(), GETDATE(), N'LA000002', N'LA000002');
INSERT INTO [dbo].[Houses] VALUES (N'Trọ Campus 3', 87, N'Không chung chủ', 8, 3, N'LA000003', 3, 1.9, 3400, 1500, 0, 0, 1, 0, GETDATE(), GETDATE(), N'LA000003', N'LA000003');
INSERT INTO [dbo].[Houses] VALUES (N'Trọ Campus 4', 72, N'Không chung chủ', 8, 3, N'LA000004', 4, 9, 3400, 1500, 0, 0, 1, 0, GETDATE(), GETDATE(), N'LA000004', N'LA000004');
INSERT INTO [dbo].[Houses] VALUES (N'Trọ Campus 5', 72, N'Không chung chủ', 8, 3, N'LA000005', 5, 4.4, 3400, 1500, 0, 0, 1, 0, GETDATE(), GETDATE(), N'LA000005', N'LA000005');
INSERT INTO [dbo].[Houses] VALUES (N'Trọ RoomType 1', 87, N'Không chung chủ', 8, 3, N'LA000003', 3, 4.3, 3400, 1500, 0, 0, 1, 0, GETDATE(), GETDATE(), N'LA000003', N'LA000003');
INSERT INTO [dbo].[Houses] VALUES (N'Trọ RoomType 2', 102, N'Không chung chủ', 8, 3, N'LA000004', 4, 3.3, 3400, 1500, 0, 0, 1, 0, GETDATE(), GETDATE(), N'LA000004', N'LA000004');
INSERT INTO [dbo].[Houses] VALUES (N'Trọ RoomType 3', 102, N'Không chung chủ', 8, 3, N'LA000005', 5, 3.1, 3400, 1500, 0, 0, 1, 0, GETDATE(), GETDATE(), N'LA000005', N'LA000005');
INSERT INTO [dbo].[Houses] VALUES (N'Trọ District 1', 72, N'Không chung chủ', 8, 1, N'LA000003', 3, 4.9, 3400, 1500, 0, 0, 1, 0, GETDATE(), GETDATE(), N'LA000003', N'LA000003');
INSERT INTO [dbo].[Houses] VALUES (N'Trọ District 2', 72, N'Không chung chủ', 8, 206, N'LA000004', 4, 10, 3400, 1500, 0, 0, 1, 0, GETDATE(), GETDATE(), N'LA000004', N'LA000004');
INSERT INTO [dbo].[Houses] VALUES (N'Trọ District 3', 102, N'Không chung chủ', 8, 303, N'LA000005', 5, 5, 3400, 1500, 0, 0, 1, 0, GETDATE(), GETDATE(), N'LA000005', N'LA000005');
INSERT INTO [dbo].[Houses] VALUES (N'Trọ Hồ Chí Minh', 106, N'Không chung chủ', 8, 448, N'LA000005', 2, 8, 3400, 1500, 0, 0, 1, 0, GETDATE(), GETDATE(), N'LA000005', N'LA000005');

-------------------------------------------------------------------------------------------------------------------------------------------

INSERT INTO [dbo].[RoomStatuses] VALUES (N'Available', GETDATE());	--có thể thuê	-> hiển thị khi search
INSERT INTO [dbo].[RoomStatuses] VALUES (N'Occupied', GETDATE());	--đã có ng thuê	-> ko hiển thị khi search
INSERT INTO [dbo].[RoomStatuses] VALUES (N'Disabled', GETDATE());	--ko dùng dc vì lý do nào đó	-> ko hiển thị khi search

-------------------------------------------------------------------------------------------------------------------------------------------

INSERT INTO [dbo].[RoomTypes] VALUES (N'Khép kín', GETDATE());
INSERT INTO [dbo].[RoomTypes] VALUES (N'Không khép kín', GETDATE());
INSERT INTO [dbo].[RoomTypes] VALUES (N'Chung cư mini', GETDATE());

-------------------------------------------------------------------------------------------------------------------------------------------

--AreaByMeters, Aircon, Wifi, WaterHeater, Furniture, MaxAmountOfPeople, CurrentAmountOfPeople, BuildingNumber,  FloorNumber, StatusId, RoomTypeId, HouseId
INSERT INTO [dbo].[Rooms] VALUES (N'101', 3000000, N'Gạch sàn nhà có họa tiết hình con cá', 20, 1, 1, 0, 0, 1, 1, 1, 3, 0, 1, 1, 1, 2, 1,  0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[Rooms] VALUES (N'102', 2500000, N'Đã có Bếp từ, hút mùi và đầy đủ phụ kiện', 15, 0, 1, 1, 0, 1, 1, 1, 2, 1, 1, 1, 3, 2, 1,  0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[Rooms] VALUES (N'103', 2000000, N'Tổng diện tích sử dụng: 20m2', 30, 1, 1, 1, 0, 1, 1, 1, 4, 0, 1, 1, 2, 2, 1,  0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[Rooms] VALUES (N'201', 3000000, N'Gạch sàn nhà có họa tiết hình con cá', 25, 1, 0, 0, 0, 1, 1, 1, 4, 2, 1, 1, 3, 2, 1,  0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[Rooms] VALUES (N'202', 2500000, N'Đã có Bếp từ, hút mùi và đầy đủ phụ kiện', 20, 1, 0, 0, 1, 1, 1, 1, 4, 4, 1, 1, 1, 2, 1,  0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[Rooms] VALUES (N'203', 2000000, N'Tổng diện tích sử dụng: 20m2', 25, 1, 0, 0, 0, 1, 1, 1, 4, 3, 1, 1, 2, 2, 1,  0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[Rooms] VALUES (N'301', 3000000, N'Gạch sàn nhà có họa tiết hình con cá', 20, 1, 0, 0, 0, 1, 1, 1, 2, 1, 1, 1, 1, 2, 1, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[Rooms] VALUES (N'302', 2500000, N'Đã có Bếp từ, hút mùi và đầy đủ phụ kiện', 15, 1, 0, 0, 1, 1, 1, 1, 2, 1, 1, 1, 1, 2, 1,  0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[Rooms] VALUES (N'303', 2000000, N'Tổng diện tích sử dụng: 20m2', 25, 1, 0, 0, 1, 1, 1, 1, 2, 1, 1, 1, 2, 2, 1,  0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');

INSERT INTO [dbo].[Rooms] VALUES (N'101', 4000000, N'Vệ sinh khép kín', 6, 1, 1, 1, 1, 1, 1, 1, 2, 0, 1, 1, 1, 2, 2,  0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[Rooms] VALUES (N'102', 3500000, N'Vệ sinh khép kín', 6, 1, 0, 0, 1, 1, 1, 1, 2, 1, 1, 1, 3, 3, 2,  0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[Rooms] VALUES (N'103', 3000000, N'Vệ sinh khép kín', 6, 1, 1, 1, 1, 1, 1, 1, 2, 0, 1, 1, 1, 2, 2,  0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');

INSERT INTO [dbo].[Rooms] VALUES (N'101', 3500000, N'Giao thông thuận lợi', 7, 1, 0, 0, 1, 1, 1, 1, 2, 0, 1, 1, 2, 1, 3,  0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[Rooms] VALUES (N'102', 2600000, N'Giao thông thuận lợi', 7, 1, 0, 0, 1, 1, 1, 1, 2, 1, 1, 1, 3, 2, 3,  0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[Rooms] VALUES (N'103', 2100000, N'Giao thông thuận lợi', 7, 1, 0, 0, 1, 1, 0, 1, 2, 1, 1, 1, 2, 3, 3,  0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');

INSERT INTO [dbo].[Rooms] VALUES (N'101', 3100000, N'Trật tự an ninh tốt', 4, 1, 0, 0, 1, 1, 0, 1, 2, 1, 1, 1, 1, 2, 4,  0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[Rooms] VALUES (N'102', 2400000, N'Trật tự an ninh tốt', 4, 1, 0, 0, 1, 1, 0, 1, 2, 1, 1, 1, 1, 2, 4,  0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[Rooms] VALUES (N'103', 2200000, N'Trật tự an ninh tốt', 4, 1, 0, 0, 1, 1, 0, 1, 2, 1, 1, 1, 1, 2, 4,  0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');

INSERT INTO [dbo].[Rooms] VALUES (N'101', 3200000, N'Đủ điều hòa, bình nóng lạnh', 5, 1, 0, 1, 0, 1, 0, 1, 2, 1, 1, 1, 1, 2, 5, 0, 
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[Rooms] VALUES (N'102', 2500000, N'Đủ điều hòa, bình nóng lạnh', 5, 1, 0, 1, 0, 1, 0, 1, 2, 1, 1, 1, 1, 2, 5,  0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[Rooms] VALUES (N'103', 2300000, N'Đủ điều hòa, bình nóng lạnh', 5, 1, 0, 1, 0, 1, 0, 1, 2, 1, 1, 1, 1, 2, 5,  0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');

INSERT INTO [dbo].[Rooms] VALUES (N'101', 2000000, N'Có sân phơi trần thượng, và nhiều tiện ích khác', 3, 1, 0, 0, 1, 0, 1, 1, 2, 1, 1, 1, 1, 2, 6,  0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[Rooms] VALUES (N'102', 2100000, N'Có sân phơi trần thượng, và nhiều tiện ích khác', 3, 1, 0, 0, 1, 0, 1, 1, 2, 1, 1, 1, 1, 2, 6,  0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[Rooms] VALUES (N'103', 2200000, N'Có sân phơi trần thượng, và nhiều tiện ích khác', 3, 1, 0, 0, 1, 0, 1, 1, 2, 1, 1, 1, 1, 2, 6,  0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');

INSERT INTO [dbo].[Rooms] VALUES (N'101', 2400000, N'Nhà vệ sinh khép kín xịn, tiện nghi', 8, 1, 1, 0, 1, 0, 0, 1, 2, 1, 1, 1, 1, 2, 7, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[Rooms] VALUES (N'102', 2500000, N'Nhà vệ sinh khép kín xịn, tiện nghi', 8, 1, 1, 0, 1, 0, 0, 1, 2, 1, 1, 1, 1, 2, 7,  0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[Rooms] VALUES (N'103', 2600000, N'Nhà vệ sinh khép kín xịn, tiện nghi', 8, 1, 1, 0, 1, 0, 0, 1, 2, 1, 1, 1, 1, 2, 7,  0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');

INSERT INTO [dbo].[Rooms] VALUES (N'101', 3700000, N'Có cáp Tivi, camera 24/24 yên tâm an ninh', 4, 1, 0, 1, 0, 1, 0, 1, 2, 1, 1, 1, 1, 2, 8, 0, 
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[Rooms] VALUES (N'102', 2500000, N'Có cáp Tivi, camera 24/24 yên tâm an ninh', 4, 1, 0, 1, 0, 1, 0, 1, 2, 1, 1, 1, 1, 2, 8,  0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[Rooms] VALUES (N'103', 2800000, N'Có cáp Tivi, camera 24/24 yên tâm an ninh', 4, 1, 0, 1, 0, 1, 0, 1, 2, 1, 1, 1, 1, 2, 8,  0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');

INSERT INTO [dbo].[Rooms] VALUES (N'101', 3200000, N'Để xe tầng 1 tiện lợi miễn phí, ô tô đỗ đón tận cửa', 7, 1, 0, 0, 1, 1, 0, 1, 2, 1, 1, 1, 1, 2, 9,  0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[Rooms] VALUES (N'102', 3500000, N'Để xe tầng 1 tiện lợi miễn phí, ô tô đỗ đón tận cửa', 7, 1, 0, 0, 1, 1, 0, 1, 2, 1, 1, 1, 1, 2, 9,  0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[Rooms] VALUES (N'103', 3000000, N'Để xe tầng 1 tiện lợi miễn phí, ô tô đỗ đón tận cửa', 7, 1, 0, 0, 1, 1, 0, 1, 2, 1, 1, 1, 1, 2, 9,  0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');

INSERT INTO [dbo].[Rooms] VALUES (N'101', 2500000, N'Điện, nước, internet giá rẻ, công tơ điện riêng', 2, 1, 0, 0, 1, 0, 1, 1, 2, 1, 1, 1, 1, 2, 10,  0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[Rooms] VALUES (N'102', 2500000, N'Điện, nước, internet giá rẻ, công tơ điện riêng', 2, 1, 0, 0, 1, 0, 1, 1, 2, 1, 1, 1, 1, 2, 10,  0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[Rooms] VALUES (N'103', 2500000, N'Điện, nước, internet giá rẻ, công tơ điện riêng', 2, 1, 0, 0, 1, 0, 1, 1, 2, 1, 1, 1, 1, 2, 10,  0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');

INSERT INTO [dbo].[Rooms] VALUES (N'101', 2700000, N'Không gian sinh hoạt riêng tư độc lập', 3, 1, 0, 0, 1, 1, 1, 1, 2, 1, 1, 1, 1, 2, 11,  0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[Rooms] VALUES (N'102', 2700000, N'Không gian sinh hoạt riêng tư độc lập', 3, 1, 1, 1, 1, 0, 0, 1, 2, 1, 1, 1, 1, 2, 11,  0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[Rooms] VALUES (N'103', 2700000, N'Không gian sinh hoạt riêng tư độc lập', 3, 1, 1, 1, 1, 0, 0, 1, 2, 1, 1, 1, 1, 2, 11,  0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');

INSERT INTO [dbo].[Rooms] VALUES (N'101', 2800000, N'Phòng mới đẹp thiết kế hài hòa đầy đủ các khu sinh hoạt như bêp và wc nên sinh hoạt rất thuận tiện', 9, 1, 0, 1, 1, 1, 0, 1, 2, 1, 1, 1, 1, 2, 12,  0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[Rooms] VALUES (N'102', 2800000, N'Phòng mới đẹp thiết kế hài hòa đầy đủ các khu sinh hoạt như bêp và wc nên sinh hoạt rất thuận tiện', 9, 1, 0, 1, 1, 1, 0, 1, 2, 1, 1, 1, 1, 2, 12,  0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[Rooms] VALUES (N'103', 2800000, N'Phòng mới đẹp thiết kế hài hòa đầy đủ các khu sinh hoạt như bêp và wc nên sinh hoạt rất thuận tiện', 9, 1, 0, 1, 1, 1, 0, 1, 2, 1, 1, 1, 1, 2, 12,  0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');

INSERT INTO [dbo].[Rooms] VALUES (N'101', 2750000, N'Không gian sinh hoạt riêng tư độc lập', 3, 1, 0, 0, 1, 1, 1, 1, 2, 1, 1, 1, 1, 2, 13,  0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[Rooms] VALUES (N'102', 2750000, N'Không gian sinh hoạt riêng tư độc lập', 3, 1, 0, 0, 1, 1, 1, 1, 2, 1, 1, 1, 1, 2, 13,  0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[Rooms] VALUES (N'103', 2750000, N'Không gian sinh hoạt riêng tư độc lập', 3, 1, 0, 0, 1, 1, 1, 1, 2, 1, 1, 1, 1, 2, 13,  0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');

INSERT INTO [dbo].[Rooms] VALUES (N'101', 2900000, N'Có cáp Tivi, camera 24/24 yên tâm an ninh', 3, 1, 1, 1, 1, 0, 0, 1, 2, 1, 1, 1, 1, 2, 14, 0, 
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[Rooms] VALUES (N'102', 2900000, N'Có cáp Tivi, camera 24/24 yên tâm an ninh', 3, 1, 1, 1, 1, 0, 0, 1, 2, 1, 1, 1, 1, 2, 14,  0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[Rooms] VALUES (N'103', 2900000, N'Có cáp Tivi, camera 24/24 yên tâm an ninh', 3, 1, 1, 1, 1, 0, 0, 1, 2, 1, 1, 1, 1, 2, 14,  0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');

INSERT INTO [dbo].[Rooms] VALUES (N'101', 3000000, N'Điện, nước, internet giá rẻ, công tơ điện riêng', 3, 1, 1, 1, 1, 0, 0, 1, 2, 1, 1, 1, 1, 2, 15, 0, 
GETDATE(), GETDATE(), N'LA000002', N'LA000002');
INSERT INTO [dbo].[Rooms] VALUES (N'102', 3000000, N'Điện, nước, internet giá rẻ, công tơ điện riêng', 3, 1, 1, 1, 1, 0, 0, 1, 2, 1, 1, 1, 1, 2, 15,  0,
GETDATE(), GETDATE(), N'LA000002', N'LA000002');
INSERT INTO [dbo].[Rooms] VALUES (N'103', 3000000, N'Điện, nước, internet giá rẻ, công tơ điện riêng', 3, 1, 1, 1, 1, 0, 0, 1, 2, 1, 1, 1, 1, 2, 15,  0,
GETDATE(), GETDATE(), N'LA000002', N'LA000002');

INSERT INTO [dbo].[Rooms] VALUES (N'101', 2750000, N'Vệ sinh khép kín', 3, 1, 0, 0, 1, 1, 1, 1, 2, 1, 1, 1, 1, 2, 16,  0,
GETDATE(), GETDATE(), N'LA000002', N'LA000002');
INSERT INTO [dbo].[Rooms] VALUES (N'102', 2750000, N'Vệ sinh khép kín', 3, 1, 0, 0, 1, 1, 1, 1, 2, 1, 1, 1, 1, 2, 16,  0,
GETDATE(), GETDATE(), N'LA000002', N'LA000002');
INSERT INTO [dbo].[Rooms] VALUES (N'103', 2750000, N'Vệ sinh khép kín', 3, 1, 0, 0, 1, 1, 1, 1, 2, 1, 1, 1, 1, 2, 16,  0,
GETDATE(), GETDATE(), N'LA000002', N'LA000002');

INSERT INTO [dbo].[Rooms] VALUES (N'101', 2900000, N'24/24 an ninh', 3, 1, 1, 1, 1, 0, 0, 1, 2, 1, 1, 1, 1, 2, 17,  0,
GETDATE(), GETDATE(), N'LA000003', N'LA000003');
INSERT INTO [dbo].[Rooms] VALUES (N'102', 2900000, N'24/24 an ninh', 3, 1, 1, 1, 1, 0, 0, 1, 2, 1, 1, 1, 1, 2, 17,  0,
GETDATE(), GETDATE(), N'LA000003', N'LA000003');
INSERT INTO [dbo].[Rooms] VALUES (N'103', 2900000, N'24/24 an ninh', 3, 1, 1, 1, 1, 0, 0, 1, 2, 1, 1, 1, 1, 2, 17,  0,
GETDATE(), GETDATE(), N'LA000003', N'LA000003');

INSERT INTO [dbo].[Rooms] VALUES (N'101', 3000000, N'Giao thông thuận lợi', 3, 1, 1, 1, 1, 0, 0, 1, 2, 1, 1, 1, 1, 2, 18, 0, 
GETDATE(), GETDATE(), N'LA000003', N'LA000003');
INSERT INTO [dbo].[Rooms] VALUES (N'102', 3000000, N'Giao thông thuận lợi', 3, 1, 1, 1, 1, 0, 0, 1, 2, 1, 1, 1, 1, 2, 18, 0,
GETDATE(), GETDATE(), N'LA000003', N'LA000003');
INSERT INTO [dbo].[Rooms] VALUES (N'103', 3000000, N'Giao thông thuận lợi', 3, 1, 1, 1, 1, 0, 0, 1, 2, 1, 1, 1, 1, 2, 18, 0,
GETDATE(), GETDATE(), N'LA000003', N'LA000003');

INSERT INTO [dbo].[Rooms] VALUES (N'101', 3000000, N'Giao thông thuận lợi', 3, 1, 0, 0, 1, 1, 1, 1, 2, 1, 1, 1, 2, 2, 19, 0,
GETDATE(), GETDATE(), N'LA000003', N'LA000003');

INSERT INTO [dbo].[Rooms] VALUES (N'101', 3000000, N'Vệ sinh khép kín', 3, 1, 0, 0, 1, 1, 1, 1, 2, 1, 1, 1, 1, 2, 20, 0,
GETDATE(), GETDATE(), N'LA000003', N'LA000003');

INSERT INTO [dbo].[Rooms] VALUES (N'101', 3000000, N'Vệ sinh khép kín', 3, 1, 0, 0, 1, 1, 1, 1, 2, 1, 1, 1, 1, 2, 21, 0,
GETDATE(), GETDATE(), N'LA000003', N'LA000003');

INSERT INTO [dbo].[Rooms] VALUES (N'101', 3000000, N'Vệ sinh khép kín', 3, 1, 0, 0, 1, 1, 1, 1, 2, 1, 1, 1, 1, 2, 22, 0,
GETDATE(), GETDATE(), N'LA000003', N'LA000003');

INSERT INTO [dbo].[Rooms] VALUES (N'101', 3000000, N'Vệ sinh khép kín', 3, 1, 0, 0, 1, 1, 1, 1, 2, 1, 1, 1, 1, 2, 23, 0,
GETDATE(), GETDATE(), N'LA000003', N'LA000003');

INSERT INTO [dbo].[Rooms] VALUES (N'101', 3000000, N'Vệ sinh khép kín', 3, 1, 1, 1, 1, 0, 0, 1, 2, 1, 1, 1, 1, 2, 24, 0,
GETDATE(), GETDATE(), N'LA000003', N'LA000003');

INSERT INTO [dbo].[Rooms] VALUES (N'101', 3000000, N'Vệ sinh khép kín', 3, 1, 0, 1, 1, 1, 0, 1, 2, 1, 1, 1, 1, 2, 25, 0,
GETDATE(), GETDATE(), N'LA000003', N'LA000003');

INSERT INTO [dbo].[Rooms] VALUES (N'101', 3000000, N'Vệ sinh khép kín', 3, 1, 0, 1, 1, 1, 0, 1, 2, 1, 1, 1, 1, 1, 26, 0,
GETDATE(), GETDATE(), N'LA000003', N'LA000003');

INSERT INTO [dbo].[Rooms] VALUES (N'101', 3000000, N'Vệ sinh khép kín', 3, 1, 0, 0, 1, 1, 1, 1, 2, 1, 1, 1, 1, 2, 27, 0,
GETDATE(), GETDATE(), N'LA000003', N'LA000003');

INSERT INTO [dbo].[Rooms] VALUES (N'101', 3000000, N'Vệ sinh khép kín', 3, 1, 0, 0, 1, 1, 1, 1, 2, 1, 1, 1, 1, 3, 28, 0,
GETDATE(), GETDATE(), N'LA000003', N'LA000003');

INSERT INTO [dbo].[Rooms] VALUES (N'101', 3000000, N'Vệ sinh khép kín', 3, 1, 0, 0, 1, 1, 1, 1, 2, 1, 1, 1, 1, 2, 29, 0,
GETDATE(), GETDATE(), N'LA000003', N'LA000003');

INSERT INTO [dbo].[Rooms] VALUES (N'101', 3000000, N'Vệ sinh khép kín', 3, 1, 0, 0, 1, 1, 1, 1, 2, 1, 1, 1, 1, 3, 30, 0,
GETDATE(), GETDATE(), N'LA000003', N'LA000003');

INSERT INTO [dbo].[Rooms] VALUES (N'101', 3000000, N'Vệ sinh khép kín', 3, 1, 0, 0, 1, 1, 1, 1, 2, 1, 1, 1, 1, 3, 31, 0,
GETDATE(), GETDATE(), N'LA000003', N'LA000003');

-------------------------------------------------------------------------------------------------------------------------------------------

INSERT INTO [dbo].[Rates] VALUES (5, N'Rất tuyệt vời, gần trường nữa', N'Cảm ơn bạn', 1, N'HE153046',  0,
GETDATE(), GETDATE(), N'HE153046', N'HE153046');

INSERT INTO [dbo].[Rates] VALUES (4, N'Rất tuyệt vời, gần trường nữa 2', N'Cảm ơn bạn 2', 2, N'HE153046',  0,
GETDATE(), GETDATE(), N'HE153046', N'HE153046');

INSERT INTO [dbo].[Rates] VALUES (3, N'Rất tuyệt vời, gần trường nữa 3', N'Cảm ơn bạn 3', 3, N'HE153046',  0,
GETDATE(), GETDATE(), N'HE153046', N'HE153046');

INSERT INTO [dbo].[Rates] VALUES (2, N'Rất tuyệt vời, gần trường nữa 4', N'Cảm ơn bạn 4', 4, N'HE153046',  0,
GETDATE(), GETDATE(), N'HE153046', N'HE153046');

INSERT INTO [dbo].[Rates] VALUES (1, N'Rất tuyệt vời, gần trường nữa 5', N'Cảm ơn bạn 5', 5, N'HE153046',  0,
GETDATE(), GETDATE(), N'HE153046', N'HE153046');

INSERT INTO [dbo].[Rates] VALUES (2, N'Rất tuyệt vời, gần trường nữa 6', N'Cảm ơn bạn 6', 1, N'HE153046',  0,
GETDATE(), GETDATE(), N'HE153046', N'HE153046');

INSERT INTO [dbo].[Rates] VALUES (3, N'Rất tuyệt vời, gần trường nữa 7', N'Cảm ơn bạn 7', 2, N'HE153046',  0,
GETDATE(), GETDATE(), N'HE153046', N'HE153046');

INSERT INTO [dbo].[Rates] VALUES (5, N'Rất tuyệt vời, gần trường nữa 7', N'Cảm ơn bạn 7', 6, N'HE153046',  0,
GETDATE(), GETDATE(), N'HE153046', N'HE153046');

INSERT INTO [dbo].[Rates] VALUES (5, N'Rất tuyệt vời, gần trường nữa 7', N'Cảm ơn bạn 7', 7, N'HE153046',  0,
GETDATE(), GETDATE(), N'HE153046', N'HE153046');

INSERT INTO [dbo].[Rates] VALUES (4, N'Rất tuyệt vời, gần trường nữa 7', N'Cảm ơn bạn 7', 8, N'HE153046',  0,
GETDATE(), GETDATE(), N'HE153046', N'HE153046');

-------------------------------------------------------------------------------------------------------------------------------------------

INSERT INTO [dbo].[ImagesOfHouse] VALUES (N'house1.jpg', 1, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfHouse] VALUES (N'house1(1).jpg', 1, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfHouse] VALUES (N'house1(2).jpg', 1, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfHouse] VALUES (N'house2.jpg', 2, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfHouse] VALUES (N'house2(1).jpg', 2, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfHouse] VALUES (N'house2(2).jpg', 2, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfHouse] VALUES (N'house3.jpg', 3, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfHouse] VALUES (N'house3(1).jpg', 3, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfHouse] VALUES (N'house3(2).jpg', 3, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfHouse] VALUES (N'house4.jpg', 4, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfHouse] VALUES (N'house5.jpg', 5, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfHouse] VALUES (N'house6.jpg', 6, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfHouse] VALUES (N'house7.jpg', 7, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfHouse] VALUES (N'house8.jpg', 8, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfHouse] VALUES (N'house9.jpg', 9, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfHouse] VALUES (N'house10.jpg', 10, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfHouse] VALUES (N'house11.jpg', 11, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfHouse] VALUES (N'house12.jpg', 12, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfHouse] VALUES (N'house13.jpg', 13, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfHouse] VALUES (N'house14.jpg', 14, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfHouse] VALUES (N'house15.jpg', 15, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000002');
INSERT INTO [dbo].[ImagesOfHouse] VALUES (N'house16.jpg', 16, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000002');
INSERT INTO [dbo].[ImagesOfHouse] VALUES (N'house17.jpg', 17, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000003');
INSERT INTO [dbo].[ImagesOfHouse] VALUES (N'house18.jpg', 18, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000003');
INSERT INTO [dbo].[ImagesOfHouse] VALUES (N'house19.jpg', 19, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000003');
INSERT INTO [dbo].[ImagesOfHouse] VALUES (N'house20.jpg', 20, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000002');
INSERT INTO [dbo].[ImagesOfHouse] VALUES (N'house21.jpg', 21, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000002');
INSERT INTO [dbo].[ImagesOfHouse] VALUES (N'house22.jpg', 22, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000003');
INSERT INTO [dbo].[ImagesOfHouse] VALUES (N'house23.jpg', 23, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000003');
INSERT INTO [dbo].[ImagesOfHouse] VALUES (N'house24.jpg', 24, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000003');
INSERT INTO [dbo].[ImagesOfHouse] VALUES (N'house25.jpg', 25, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000002');
INSERT INTO [dbo].[ImagesOfHouse] VALUES (N'house26.jpg', 26, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000002');
INSERT INTO [dbo].[ImagesOfHouse] VALUES (N'house27.jpg', 27, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000003');
INSERT INTO [dbo].[ImagesOfHouse] VALUES (N'house28.jpg', 28, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000003');
INSERT INTO [dbo].[ImagesOfHouse] VALUES (N'house29.jpg', 29, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000003');
INSERT INTO [dbo].[ImagesOfHouse] VALUES (N'house30.jpg', 30, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000003');
INSERT INTO [dbo].[ImagesOfHouse] VALUES (N'house30.jpg', 31, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000003');

-------------------------------------------------------------------------------------------------------------------------------------------

INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room1.jpg', 1, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room2.jpg', 1, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room3.jpg', 1, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');

INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room1.jpg', 2, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room2.jpg', 2, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room3.jpg', 2, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');

INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room1.jpg', 3, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room2.jpg', 3, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room3.jpg', 3, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');

INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room1.jpg', 4, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room2.jpg', 4, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room3.jpg', 4, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');

INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room1.jpg', 5, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room2.jpg', 5, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room3.jpg', 5, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');

INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room1.jpg', 6, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room2.jpg', 6, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room3.jpg', 6, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');

INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room1.jpg', 7, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room2.jpg', 7, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room3.jpg', 7, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');

INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room1.jpg', 8, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room2.jpg', 8, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room3.jpg', 8, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');

INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room1.jpg', 9, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room2.jpg', 9, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room3.jpg', 9, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');

INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room1.jpg', 10, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room2.jpg', 10, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room3.jpg', 10, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');

INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room1.jpg', 11, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room2.jpg', 11, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room3.jpg', 11, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');

INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room1.jpg', 12, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room2.jpg', 12, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room3.jpg', 12, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');

INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room1.jpg', 13, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room2.jpg', 13, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room3.jpg', 13, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');

INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room1.jpg', 14, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room2.jpg', 14, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room3.jpg', 14, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');

INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room1.jpg', 15, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room2.jpg', 15, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room3.jpg', 15, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');

INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room1.jpg', 16, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room2.jpg', 16, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room3.jpg', 16, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');

INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room1.jpg', 17, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room2.jpg', 17, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room3.jpg', 17, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');

INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room1.jpg', 18, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room2.jpg', 18, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room3.jpg', 18, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');

INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room1.jpg', 19, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room2.jpg', 19, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room3.jpg', 19, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');

INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room1.jpg', 20, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room2.jpg', 20, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room3.jpg', 20, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');

INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room1.jpg', 21, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room2.jpg', 21, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room3.jpg', 21, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');

INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room1.jpg', 22, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room2.jpg', 22, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room3.jpg', 22, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');

INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room1.jpg', 23, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room2.jpg', 23, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room3.jpg', 23, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');

INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room1.jpg', 24, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room2.jpg', 24, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room3.jpg', 24, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');

INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room1.jpg', 25, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room2.jpg', 25, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room3.jpg', 25, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');

INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room1.jpg', 26, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room2.jpg', 26, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room3.jpg', 26, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');

INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room1.jpg', 27, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room2.jpg', 27, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room3.jpg', 27, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');

INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room1.jpg', 28, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room2.jpg', 28, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room3.jpg', 28, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');

INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room1.jpg', 29, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room2.jpg', 29, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room3.jpg', 29, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');

INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room1.jpg', 30, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room2.jpg', 30, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room3.jpg', 30, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');

INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room1.jpg', 31, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room2.jpg', 31, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room3.jpg', 31, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');

INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room1.jpg', 32, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room2.jpg', 32, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room3.jpg', 32, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');

INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room1.jpg', 33, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room2.jpg', 33, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room3.jpg', 33, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');

INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room1.jpg', 34, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room2.jpg', 34, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room3.jpg', 34, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');

INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room1.jpg', 35, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room2.jpg', 35, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room3.jpg', 35, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');

INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room1.jpg', 36, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room2.jpg', 36, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room3.jpg', 36, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');

INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room1.jpg', 37, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room2.jpg', 37, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room3.jpg', 37, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');

INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room1.jpg', 38, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room2.jpg', 38, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room3.jpg', 38, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');

INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room1.jpg', 39, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room2.jpg', 39, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room3.jpg', 39, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');

INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room1.jpg', 40, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room2.jpg', 40, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room3.jpg', 40, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');

INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room1.jpg', 41, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room2.jpg', 41, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room3.jpg', 41, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');

INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room1.jpg', 42, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room2.jpg', 42, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room3.jpg', 42, 0,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');

INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room1.jpg', 43, 0,
GETDATE(), GETDATE(), N'LA000002', N'LA000002');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room2.jpg', 43, 0,
GETDATE(), GETDATE(), N'LA000002', N'LA000002');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room3.jpg', 43, 0,
GETDATE(), GETDATE(), N'LA000002', N'LA000002');

INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room1.jpg', 44, 0,
GETDATE(), GETDATE(), N'LA000002', N'LA000002');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room2.jpg', 44, 0,
GETDATE(), GETDATE(), N'LA000002', N'LA000002');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room3.jpg', 44, 0,
GETDATE(), GETDATE(), N'LA000002', N'LA000002');

INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room1.jpg', 45, 0,
GETDATE(), GETDATE(), N'LA000002', N'LA000002');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room2.jpg', 45, 0,
GETDATE(), GETDATE(), N'LA000002', N'LA000002');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room3.jpg', 45, 0,
GETDATE(), GETDATE(), N'LA000002', N'LA000002');

INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room1.jpg', 46, 0,
GETDATE(), GETDATE(), N'LA000002', N'LA000002');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room2.jpg', 46, 0,
GETDATE(), GETDATE(), N'LA000002', N'LA000002');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room3.jpg', 46, 0,
GETDATE(), GETDATE(), N'LA000002', N'LA000002');

INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room1.jpg', 47, 0,
GETDATE(), GETDATE(), N'LA000002', N'LA000002');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room2.jpg', 47, 0,
GETDATE(), GETDATE(), N'LA000002', N'LA000002');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room3.jpg', 47, 0,
GETDATE(), GETDATE(), N'LA000002', N'LA000002');

INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room1.jpg', 48, 0,
GETDATE(), GETDATE(), N'LA000002', N'LA000002');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room2.jpg', 48, 0,
GETDATE(), GETDATE(), N'LA000002', N'LA000002');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room3.jpg', 48, 0,
GETDATE(), GETDATE(), N'LA000002', N'LA000002');

INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room1.jpg', 49, 0,
GETDATE(), GETDATE(), N'LA000003', N'LA000003');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room2.jpg', 49, 0,
GETDATE(), GETDATE(), N'LA000003', N'LA000003');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room3.jpg', 49, 0,
GETDATE(), GETDATE(), N'LA000003', N'LA000003');

INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room1.jpg', 50, 0,
GETDATE(), GETDATE(), N'LA000003', N'LA000003');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room2.jpg', 50, 0,
GETDATE(), GETDATE(), N'LA000003', N'LA000003');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room3.jpg', 50, 0,
GETDATE(), GETDATE(), N'LA000003', N'LA000003');

INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room1.jpg', 51, 0,
GETDATE(), GETDATE(), N'LA000003', N'LA000003');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room2.jpg', 51, 0,
GETDATE(), GETDATE(), N'LA000003', N'LA000003');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room3.jpg', 51, 0,
GETDATE(), GETDATE(), N'LA000003', N'LA000003');

INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room1.jpg', 52, 0,
GETDATE(), GETDATE(), N'LA000003', N'LA000003');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room2.jpg', 52, 0,
GETDATE(), GETDATE(), N'LA000003', N'LA000003');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room3.jpg', 52, 0,
GETDATE(), GETDATE(), N'LA000003', N'LA000003');

INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room1.jpg', 53, 0,
GETDATE(), GETDATE(), N'LA000003', N'LA000003');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room2.jpg', 53, 0,
GETDATE(), GETDATE(), N'LA000003', N'LA000003');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room3.jpg', 53, 0,
GETDATE(), GETDATE(), N'LA000003', N'LA000003');

INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room1.jpg', 54, 0,
GETDATE(), GETDATE(), N'LA000003', N'LA000003');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room2.jpg', 54, 0,
GETDATE(), GETDATE(), N'LA000003', N'LA000003');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room3.jpg', 54, 0,
GETDATE(), GETDATE(), N'LA000003', N'LA000003');

INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room1.jpg', 55, 0,
GETDATE(), GETDATE(), N'LA000002', N'LA000002');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room2.jpg', 55, 0,
GETDATE(), GETDATE(), N'LA000002', N'LA000002');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room3.jpg', 55, 0,
GETDATE(), GETDATE(), N'LA000002', N'LA000002');

INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room1.jpg', 56, 0,
GETDATE(), GETDATE(), N'LA000002', N'LA000002');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room2.jpg', 56, 0,
GETDATE(), GETDATE(), N'LA000002', N'LA000002');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room3.jpg', 56, 0,
GETDATE(), GETDATE(), N'LA000002', N'LA000002');

INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room1.jpg', 57, 0,
GETDATE(), GETDATE(), N'LA000002', N'LA000002');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room2.jpg', 57, 0,
GETDATE(), GETDATE(), N'LA000002', N'LA000002');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room3.jpg', 57, 0,
GETDATE(), GETDATE(), N'LA000002', N'LA000002');

INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room1.jpg', 58, 0,
GETDATE(), GETDATE(), N'LA000002', N'LA000002');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room2.jpg', 58, 0,
GETDATE(), GETDATE(), N'LA000002', N'LA000002');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room3.jpg', 58, 0,
GETDATE(), GETDATE(), N'LA000002', N'LA000002');

INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room1.jpg', 59, 0,
GETDATE(), GETDATE(), N'LA000002', N'LA000002');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room2.jpg', 59, 0,
GETDATE(), GETDATE(), N'LA000002', N'LA000002');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room3.jpg', 59, 0,
GETDATE(), GETDATE(), N'LA000002', N'LA000002');

INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room1.jpg', 60, 0,
GETDATE(), GETDATE(), N'LA000002', N'LA000002');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room2.jpg', 60, 0,
GETDATE(), GETDATE(), N'LA000002', N'LA000002');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room3.jpg', 60, 0,
GETDATE(), GETDATE(), N'LA000002', N'LA000002');

INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room1.jpg', 61, 0,
GETDATE(), GETDATE(), N'LA000003', N'LA000003');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room2.jpg', 61, 0,
GETDATE(), GETDATE(), N'LA000003', N'LA000003');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room3.jpg', 61, 0,
GETDATE(), GETDATE(), N'LA000003', N'LA000003');

INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room1.jpg', 62, 0,
GETDATE(), GETDATE(), N'LA000003', N'LA000003');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room2.jpg', 62, 0,
GETDATE(), GETDATE(), N'LA000003', N'LA000003');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room3.jpg', 62, 0,
GETDATE(), GETDATE(), N'LA000003', N'LA000003');

INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room1.jpg', 63, 0,
GETDATE(), GETDATE(), N'LA000003', N'LA000003');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room2.jpg', 63, 0,
GETDATE(), GETDATE(), N'LA000003', N'LA000003');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room3.jpg', 63, 0,
GETDATE(), GETDATE(), N'LA000003', N'LA000003');

INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room1.jpg', 64, 0,
GETDATE(), GETDATE(), N'LA000003', N'LA000003');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room2.jpg', 64, 0,
GETDATE(), GETDATE(), N'LA000003', N'LA000003');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room3.jpg', 64, 0,
GETDATE(), GETDATE(), N'LA000003', N'LA000003');

INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room1.jpg', 65, 0,
GETDATE(), GETDATE(), N'LA000003', N'LA000003');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room2.jpg', 65, 0,
GETDATE(), GETDATE(), N'LA000003', N'LA000003');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room3.jpg', 65, 0,
GETDATE(), GETDATE(), N'LA000003', N'LA000003');

INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room1.jpg', 66, 0,
GETDATE(), GETDATE(), N'LA000003', N'LA000003');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room2.jpg', 66, 0,
GETDATE(), GETDATE(), N'LA000003', N'LA000003');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room3.jpg', 66, 0,
GETDATE(), GETDATE(), N'LA000003', N'LA000003');

INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room1.jpg', 67, 0,
GETDATE(), GETDATE(), N'LA000003', N'LA000003');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room2.jpg', 67, 0,
GETDATE(), GETDATE(), N'LA000003', N'LA000003');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room3.jpg', 67, 0,
GETDATE(), GETDATE(), N'LA000003', N'LA000003');

INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room1.jpg', 68, 0,
GETDATE(), GETDATE(), N'LA000003', N'LA000003');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room2.jpg', 68, 0,
GETDATE(), GETDATE(), N'LA000003', N'LA000003');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room3.jpg', 68, 0,
GETDATE(), GETDATE(), N'LA000003', N'LA000003');

INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room1.jpg', 69, 0,
GETDATE(), GETDATE(), N'LA000003', N'LA000003');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room2.jpg', 69, 0,
GETDATE(), GETDATE(), N'LA000003', N'LA000003');
INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'room3.jpg', 69, 0,
GETDATE(), GETDATE(), N'LA000003', N'LA000003');

-------------------------------------------------------------------------------------------------------------------------------------------

INSERT INTO [dbo].[ReportStatuses] VALUES (N'Unsolved', GETDATE());		--chưa giải quyết
INSERT INTO [dbo].[ReportStatuses] VALUES (N'Processing', GETDATE());	--đang giải quyết
INSERT INTO [dbo].[ReportStatuses] VALUES (N'Solved', GETDATE());		--đã giải quyết

-------------------------------------------------------------------------------------------------------------------------------------------

INSERT INTO [dbo].[Reports] VALUES (N'Chủ trọ tăng giá phòng trái với hợp đồng', N'HE153046', 1, 1, 0,
'01/03/2022', NULL, NULL);
INSERT INTO [dbo].[Reports] VALUES (N'Chủ trọ thay khóa cổng không cho vào nhà', N'HE153046', 2, 1, 1,
'01/04/2022', NULL, NULL);
INSERT INTO [dbo].[Reports] VALUES (N'Chủ trọ tự ý vào phòng của bạn và tháo bóng đèn trong nhà vệ sinh của bạn', N'HE153046', 3, 1, 0,
'01/05/2022', NULL, NULL);
INSERT INTO [dbo].[Reports] VALUES (N'Chủ trọ tăng giá phòng trái với hợp đồng', N'HE153046', 4, 1, 0,
'02/03/2022', NULL, NULL);
INSERT INTO [dbo].[Reports] VALUES (N'Chủ trọ xâm phạm quyền riêng tư', N'HE153046', 1, 1, 0,
'03/03/2022', NULL, NULL);
INSERT INTO [dbo].[Reports] VALUES (N'Chủ trọ thu tiền điện vượt quá giá niêm yết', N'HE153046', 6, 2, 0,
'03/13/2022', NULL, NULL);
INSERT INTO [dbo].[Reports] VALUES (N'Chủ trọ tăng giá phòng trái với hợp đồng', N'HE153046', 9, 2, 0,
'04/03/2022', NULL, NULL);
INSERT INTO [dbo].[Reports] VALUES (N'Chủ trọ tăng giá phòng trái với hợp đồng', N'HE153046', 4, 2, 0,
'04/23/2022', NULL, NULL);
INSERT INTO [dbo].[Reports] VALUES (N'Chủ trọ tăng mức đóng tiền điện so với hợp đồng thuê phòng trước đó mà hai bên đã kí', N'HE153046', 7, 2, 0,
'04/14/2022', NULL, NULL);
INSERT INTO [dbo].[Reports] VALUES (N'Chủ trọ tăng giá phòng trái với hợp đồng', N'HE153046', 8, 2, 0,
'04/17/2022', NULL, NULL);
INSERT INTO [dbo].[Reports] VALUES (N'Chủ trọ tăng giá phòng trái với hợp đồng', N'HE153046', 1, 2, 0,
'04/03/2022', NULL, NULL);
INSERT INTO [dbo].[Reports] VALUES (N'Chủ trọ tăng giá phòng trái với hợp đồng', N'HE153046', 1, 2, 0,
'05/03/2022', NULL, NULL);
INSERT INTO [dbo].[Reports] VALUES (N'Chủ trọ tăng giá phòng trái với hợp đồng', N'HE153046', 3, 2, 0,
'07/03/2022', NULL, NULL);
INSERT INTO [dbo].[Reports] VALUES (N'Chủ trọ tăng giá phòng trái với hợp đồng', N'HE153046', 2, 1, 0,
'07/03/2022', NULL, NULL);
INSERT INTO [dbo].[Reports] VALUES (N'Chủ trọ tăng giá phòng trái với hợp đồng', N'HE153046', 1, 1, 0,
'08/03/2022', GETDATE(), NULL);
INSERT INTO [dbo].[Reports] VALUES (N'Chủ trọ tăng giá phòng trái với hợp đồng', N'HE153046', 10, 3, 0,
'09/03/2022', GETDATE(), N'SA000001');
INSERT INTO [dbo].[Reports] VALUES (N'Chủ trọ tăng giá phòng trái với hợp đồng', N'HE153046', 11, 3, 0,
'09/03/2022', GETDATE(), N'SA000001');
INSERT INTO [dbo].[Reports] VALUES (N'Chủ trọ tăng giá phòng trái với hợp đồng', N'HE153046', 13, 3, 0,
'09/03/2022', GETDATE(), N'SA000001');
INSERT INTO [dbo].[Reports] VALUES (N'Chủ trọ tăng giá phòng trái với hợp đồng', N'HE153046', 8, 3, 0,
'09/03/2022', GETDATE(), N'SA000001');
INSERT INTO [dbo].[Reports] VALUES (N'Chủ trọ tăng giá phòng trái với hợp đồng', N'HE153046', 8, 3, 0,
'10/03/2022', GETDATE(), N'SA000001');
INSERT INTO [dbo].[Reports] VALUES (N'Chủ trọ tăng giá phòng trái với hợp đồng', N'HE153046', 6, 3, 0,
'11/03/2022', GETDATE(), N'SA000001');
INSERT INTO [dbo].[Reports] VALUES (N'Chủ trọ tăng giá phòng trái với hợp đồng', N'HE153046', 7, 1, 0,
'11/05/2022', NULL, NULL);


-------------------------------------------------------------------------------------------------------------------------------------------

INSERT INTO [dbo].[RoomHistories] VALUES (N'Nguyễn Thế Giang', 1, 0, GETDATE(), GETDATE(), N'LA000001', N'LA000001');

-------------------------------------------------------------------------------------------------------------------------------------------

INSERT INTO [dbo].[OrderStatuses] VALUES (N'Unsolved', GETDATE());		--chưa giải quyết
INSERT INTO [dbo].[OrderStatuses] VALUES (N'Processing', GETDATE());	--đang giải quyết
INSERT INTO [dbo].[OrderStatuses] VALUES (N'Solved', GETDATE());		--đã giải quyết

-------------------------------------------------------------------------------------------------------------------------------------------

--StudentId, StudentName, PhoneNumber, Email, OrderContent, Solved, OrderedDate, SolvedDate

INSERT INTO [dbo].[Order] VALUES (N'HE153046', N'Bùi Ngọc Huyền', N'0346034217', N'huyenbnhe150346@fpt.edu.vn', N'Em muốn tìm 1 nhà trọ với mức giá dưới 2 triệu và ở trong vòng 2km quanh trường nhưng không có.', 1, '02/03/2022', '02/10/2022', 'SA000001');


INSERT INTO [dbo].[Order] VALUES (N'HE153046', N'Bùi Ngọc Huyền', N'0346034217', N'huyenbnhe150346@fpt.edu.vn', N'Em muốn tìm 1 nhà trọ với mức giá dưới 2 triệu và ở trong vòng 2km quanh trường nhưng không có.', 1, '07/05/2022', NULL, NULL);
INSERT INTO [dbo].[Order] VALUES (N'HE153046', N'Bùi Ngọc Huyền', N'0346034217', N'huyenbnhe150346@fpt.edu.vn', N'Em muốn tìm 1 nhà trọ với mức giá dưới 2 triệu và ở trong vòng 2km quanh trường nhưng không có.', 2, '04/23/2022', NULL, NULL);
INSERT INTO [dbo].[Order] VALUES (N'HE153222', N'Trần Thị Nguyệt Hà', N'0346034217', N'huyenbnhe150346@fpt.edu.vn', N'Em muốn tìm 1 nhà trọ với mức giá dưới 2 triệu và ở trong vòng 2km quanh trường nhưng không có', 2, '08/03/2022', NULL, NULL);
INSERT INTO [dbo].[Order] VALUES (N'HE153299', N'Tống Trường Giang', N'0346034217', N'huyenbnhe150346@fpt.edu.vn', N'Em muốn tìm 1 nhà trọ với mức giá dưới 2 triệu và ở trong vòng 2km quanh trường nhưng không có.', 1, '03/31/2022', NULL, NULL);
INSERT INTO [dbo].[Order] VALUES (N'HE153299', N'Tống Trường Giang', N'0346034217', N'huyenbnhe150346@fpt.edu.vn', N'Em muốn tìm 1 nhà trọ với mức giá dưới 2 triệu và ở trong vòng 2km quanh trường nhưng không có.', 1, '04/30/2022', NULL, NULL);
INSERT INTO [dbo].[Order] VALUES (N'HE153299', N'Tống Trường Giang', N'0346034217', N'huyenbnhe150346@fpt.edu.vn', N'Em muốn tìm 1 nhà trọ với mức giá dưới 2 triệu và ở trong vòng 2km quanh trường nhưng không có.', 1, '04/03/2022', NULL, NULL);
INSERT INTO [dbo].[Order] VALUES (N'HE153299', N'Bùi Ngọc Huyền', N'0346034217', N'huyenbnhe150346@fpt.edu.vn', N'Em muốn tìm 1 nhà trọ với mức giá dưới 2 triệu và ở trong vòng 2km quanh trường nhưng không có.', 2, '05/03/2022', NULL, NULL);
INSERT INTO [dbo].[Order] VALUES (N'HE153222', N'Trần Thị Nguyệt Hà', N'0346034217', N'huyenbnhe150346@fpt.edu.vn', N'Em muốn tìm 1 nhà trọ với mức giá dưới 2 triệu và ở trong vòng 2km quanh trường nhưng không có.', 1, '07/03/2022', NULL, NULL);
INSERT INTO [dbo].[Order] VALUES (N'HE153046', N'Bùi Ngọc Huyền', N'0346034217', N'huyenbnhe150346@fpt.edu.vn', N'Em muốn tìm 1 nhà trọ với mức giá dưới 2 triệu và ở trong vòng 2km quanh trường nhưng không có.', 2, '09/05/2022', NULL, NULL);
INSERT INTO [dbo].[Order] VALUES (N'HE153046', N'Bùi Ngọc Huyền', N'0346034217', N'huyenbnhe150346@fpt.edu.vn', N'Em muốn tìm 1 nhà trọ với mức giá dưới 2 triệu và ở trong vòng 2km quanh trường nhưng không có.', 1, '01/31/2022', NULL, NULL);
INSERT INTO [dbo].[Order] VALUES (N'HE153299', N'Tống Trường Giang', N'0346034217', N'huyenbnhe150346@fpt.edu.vn', N'Em muốn tìm 1 nhà trọ với mức giá dưới 2 triệu và ở trong vòng 2km quanh trường nhưng không có.', 1, '05/16/2022', NULL, NULL);
INSERT INTO [dbo].[Order] VALUES (N'HE150691', N'Nguyễn Trần Hoàng', N'0346034217', N'huyenbnhe150346@fpt.edu.vn', N'Em muốn tìm 1 nhà trọ với mức giá dưới 2 triệu và ở trong vòng 2km quanh trường nhưng không có.', 1, '04/13/2022', NULL, NULL);
INSERT INTO [dbo].[Order] VALUES (N'HE153222', N'Trần Thị Nguyệt Hà', N'0346034217', N'huyenbnhe150346@fpt.edu.vn', N'Em muốn tìm 1 nhà trọ với mức giá dưới 2 triệu và ở trong vòng 2km quanh trường nhưng không có.', 2, '05/03/2022', NULL, NULL);

INSERT INTO [dbo].[Order] VALUES (N'HE150340', N'Phùng Quang Thông', N'0346034217', N'huyenbnhe150346@fpt.edu.vn', N'Em muốn tìm 1 nhà trọ với mức giá dưới 2 triệu và ở trong vòng 2km quanh trường nhưng không có.', 3, '02/03/2022', '02/10/2022', 'SA000001');
INSERT INTO [dbo].[Order] VALUES (N'HE150160', N'Nguyễn Trí Kiên', N'0346034217', N'huyenbnhe150346@fpt.edu.vn', N'Em muốn tìm 1 nhà trọ với mức giá dưới 2 triệu và ở trong vòng 2km quanh trường nhưng không có.', 3, '02/03/2022', '03/10/2022', 'SA000001');
INSERT INTO [dbo].[Order] VALUES (N'HE150432', N'Nguyễn Thu An', N'0346034217', N'huyenbnhe150346@fpt.edu.vn', N'Em muốn tìm 1 nhà trọ với mức giá dưới 2 triệu và ở trong vòng 2km quanh trường nhưng không có.', 3, '04/03/2022', '04/17/2022', 'SA000001');
INSERT INTO [dbo].[Order] VALUES (N'HE150600', N'Nguyễn Minh Hạnh', N'0346034217', N'huyenbnhe150346@fpt.edu.vn', N'Em muốn tìm 1 nhà trọ với mức giá dưới 2 triệu và ở trong vòng 2km quanh trường nhưng không có.', 3, '04/03/2022', '05/10/2022', 'SA000001');
INSERT INTO [dbo].[Order] VALUES (N'HE150340', N'Phùng Quang Thông', N'0346034217', N'huyenbnhe150346@fpt.edu.vn', N'Em muốn tìm 1 nhà trọ với mức giá dưới 2 triệu và ở trong vòng 2km quanh trường nhưng không có.', 3, '05/03/2022', '05/23/2022', 'SA000001');
INSERT INTO [dbo].[Order] VALUES (N'HE150340', N'Phùng Quang Thông', N'0346034217', N'huyenbnhe150346@fpt.edu.vn', N'Em muốn tìm 1 nhà trọ với mức giá dưới 2 triệu và ở trong vòng 2km quanh trường nhưng không có.', 3, '05/03/2022', '05/13/2022', 'SA000001');
INSERT INTO [dbo].[Order] VALUES (N'HE150160', N'Nguyễn Trí Kiên', N'0346034217', N'huyenbnhe150346@fpt.edu.vn', N'Cần tìm nhà nguyên căn 4 phòng ngủ, đầy đủ nội thất, gần đh fpt.', 3, '05/03/2022', '07/10/2022', 'SA000001');
INSERT INTO [dbo].[Order] VALUES (N'HE150160', N'Nguyễn Trí Kiên', N'0346034217', N'huyenbnhe150346@fpt.edu.vn', N'Em muốn tìm 1 nhà trọ với mức giá dưới 2 triệu và ở trong vòng 2km quanh trường nhưng không có.', 3, '05/03/2022', '06/17/2022', 'SA000001');
INSERT INTO [dbo].[Order] VALUES (N'HE150340', N'Phùng Quang Thông', N'0346034217', N'huyenbnhe150346@fpt.edu.vn', N'Yêu cầu ngoài : không chung chủ hoặc là tự do về giờ giấc, có máy giặt càng tốt ạ', 3, '05/03/2022', '05/17/2022', 'SA000001');
INSERT INTO [dbo].[Order] VALUES (N'HE150160', N'Nguyễn Trí Kiên', N'0346034217', N'huyenbnhe150346@fpt.edu.vn', N'Em muốn tìm 1 nhà trọ với mức giá dưới 2 triệu và ở trong vòng 2km quanh trường nhưng không có.', 3, '06/03/2022', '06/10/2022', 'SA000001');
INSERT INTO [dbo].[Order] VALUES (N'HE150600', N'Nguyễn Minh Hạnh', N'0346034217', N'huyenbnhe150346@fpt.edu.vn', N'Cần tìm nhà trọ giá 1tr9 quay đầu,ở thạch hoà hay có ai cần tìm roomate cho mình ghép với ạ', 3, '06/03/2022', '07/10/2022', 'SA000001');
INSERT INTO [dbo].[Order] VALUES (N'HE150600', N'Nguyễn Minh Hạnh', N'0346034217', N'huyenbnhe150346@fpt.edu.vn', N'Em muốn tìm 1 nhà trọ với mức giá dưới 2 triệu và ở trong vòng 2km quanh trường nhưng không có.', 3, '06/03/2022', '07/11/2022', 'SA000001');
INSERT INTO [dbo].[Order] VALUES (N'HE150160', N'Nguyễn Trí Kiên', N'0346034217', N'huyenbnhe150346@fpt.edu.vn', N'Em muốn tìm 1 nhà trọ với mức giá dưới 2 triệu và ở trong vòng 2km quanh trường nhưng không có.', 3, '06/03/2022', '08/10/2022', 'SA000001');
INSERT INTO [dbo].[Order] VALUES (N'HE153590', N'Đinh Thế Thuận', N'0346034217', N'huyenbnhe150346@fpt.edu.vn', N'Em muốn tìm 1 nhà trọ với mức giá dưới 2 triệu và ở trong vòng 2km quanh trường nhưng không có.', 3, '06/03/2022', '08/28/2022', 'SA000001');
INSERT INTO [dbo].[Order] VALUES (N'HE153590', N'Đinh Thế Thuận', N'0346034217', N'huyenbnhe150346@fpt.edu.vn', N'Em muốn tìm 1 nhà trọ với mức giá dưới 2 triệu và ở trong vòng 2km quanh trường nhưng không có.', 3, '07/03/2022', '07/10/2022', 'SA000001');
INSERT INTO [dbo].[Order] VALUES (N'HE150432', N'Nguyễn Thu An', N'0346034217', N'huyenbnhe150346@fpt.edu.vn', N'Em muốn tìm 1 nhà trọ với mức giá dưới 2 triệu và ở trong vòng 2km quanh trường nhưng không có.', 3, '07/03/2022', '09/30/2022', 'SA000001');
INSERT INTO [dbo].[Order] VALUES (N'HE150340', N'Phùng Quang Thông', N'0346034217', N'huyenbnhe150346@fpt.edu.vn', N'Em muốn tìm 1 nhà trọ với mức giá dưới 2 triệu và ở trong vòng 2km quanh trường nhưng không có.', 3, '07/03/2022', '08/10/2022', 'SA000001');
INSERT INTO [dbo].[Order] VALUES (N'HE150600', N'Nguyễn Minh Hạnh', N'0346034217', N'huyenbnhe150346@fpt.edu.vn', N'Em muốn tìm 1 nhà trọ với mức giá dưới 2 triệu và ở trong vòng 2km quanh trường nhưng không có.', 3, '08/03/2022', '08/10/2022', 'SA000001');
INSERT INTO [dbo].[Order] VALUES (N'HE153590', N'Đinh Thế Thuận', N'0346034217', N'huyenbnhe150346@fpt.edu.vn', N'Mình cần tìm phòng trọ 2 người tầm 2tr ạ. Hay trọ 1 người giá cả hợp lý ạ', 3, '09/03/2022', '09/10/2022', 'SA000001');
INSERT INTO [dbo].[Order] VALUES (N'HE153590', N'Đinh Thế Thuận', N'0346034217', N'huyenbnhe150346@fpt.edu.vn', N'Em muốn tìm 1 nhà trọ với mức giá dưới 2 triệu và ở trong vòng 2km quanh trường nhưng không có.', 3, '09/03/2022', '09/21/2022', 'SA000001');
INSERT INTO [dbo].[Order] VALUES (N'HE150432', N'Nguyễn Thu An', N'0346034217', N'huyenbnhe150346@fpt.edu.vn', N'Em muốn tìm 1 nhà trọ với mức giá dưới 2 triệu và ở trong vòng 2km quanh trường nhưng không có.', 3, '10/03/2022', '10/22/2022', 'SA000001');
INSERT INTO [dbo].[Order] VALUES (N'HE150600', N'Nguyễn Minh Hạnh', N'0346034217', N'huyenbnhe150346@fpt.edu.vn', N'Em muốn tìm 1 nhà trọ với mức giá dưới 2 triệu và ở trong vòng 2km quanh trường nhưng không có.', 3, '01/03/2022', '02/10/2022', 'SA000001');
INSERT INTO [dbo].[Order] VALUES (N'HE150432', N'Nguyễn Thu An', N'0346034217', N'huyenbnhe150346@fpt.edu.vn', N'Cần tìm nhà trọ giá 1tr9 quay đầu,ở thạch hoà hay có ai cần tìm roomate cho mình ghép với ạ', 3, '01/03/2022', '01/10/2022', 'SA000001');
