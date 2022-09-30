--DROP DATABASE FUHouseFinder
CREATE DATABASE FUHouseFinder;

GO
USE [FUHouseFinder]
GO

-------------------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE [dbo].[Campuses] (
	CampusId int NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	CampusName nvarchar(100),
	GoogleMapLocation nvarchar(MAX),	--địa chỉ theo Google Map -> Sử dụng hỗ trợ search khoảng cách

	CreatedDate datetime,
) ON [PRIMARY]
GO

--Dữ liệu location này chỉ là tạm thời, sau này set up dc Google Map API sẽ sửa lại
INSERT INTO [dbo].[Campuses] VALUES (N'FU - Hòa Lạc', N'21.018797378240844, 105.52740174223347', GETDATE());
INSERT INTO [dbo].[Campuses] VALUES (N'FU - Hồ Chí Minh', N'10.841401563327102, 106.80989372598', GETDATE());
INSERT INTO [dbo].[Campuses] VALUES (N'FU - Đà Nẵng', N'15.968692643142754, 108.2605889843167', GETDATE());
INSERT INTO [dbo].[Campuses] VALUES (N'FU - Cần Thơ', N'10.014287955096187, 105.73169382835755', GETDATE());
INSERT INTO [dbo].[Campuses] VALUES (N'FU - Quy Nhơn', N'13.804165702246436, 109.21920977019123', GETDATE());

-------------------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE [dbo].[UserRoles] (
	RoleId int NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	RoleName nvarchar(100),

	CreatedDate datetime,
) ON [PRIMARY]
GO

INSERT INTO [dbo].[UserRoles] VALUES (N'Student', GETDATE());
INSERT INTO [dbo].[UserRoles] VALUES (N'Landlord', GETDATE());
INSERT INTO [dbo].[UserRoles] VALUES (N'Head of Admission Department', GETDATE());
INSERT INTO [dbo].[UserRoles] VALUES (N'Head of Student Service Department', GETDATE());
INSERT INTO [dbo].[UserRoles] VALUES (N'Staff of Admission Department', GETDATE());
INSERT INTO [dbo].[UserRoles] VALUES (N'Staff of Student Service Department', GETDATE());

-------------------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE [dbo].[Addresses] (
	AddressId int NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	Addresses nvarchar(1000) NOT NULL,

	--Dành cho những Table CRUD dc -> History
	CreatedDate datetime,
	LastModifiedDate datetime,
) ON [PRIMARY]
GO

INSERT INTO [dbo].[Addresses] VALUES (N'Nhà số..., Đường...; Đối diện cổng sau Đại học FPT; Cạnh quán Bún bò Huế', GETDATE(), GETDATE());

-------------------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE [dbo].[Users] (
	UserId nchar(30) NOT NULL PRIMARY KEY,

	--Dành cho người Login = Facebook/Google
	FacebookUserId nchar(300) NULL,
	GoogleUserId nchar(300) NULL,

	Username nvarchar(100),
	[Password] nvarchar(100),
	Email nvarchar(100),
	Active bit,		--chuyển thành false nếu User bị Disable

	--Dành cho Staff & Landlord
	FullName nvarchar(500) NULL,
	ProfileImageLink nvarchar(500) NULL,	--Link ảnh profile

	--Những thông tin riêng của Landlord
	PhoneNumber nvarchar(50) NULL,
	FacebookURL nvarchar(300) NULL,
	IdentityCardFrontSideImageLink nvarchar(500) NULL,	--Link ảnh Căn cước công dân, mặt trước
	IdentityCardBackSideImageLink nvarchar(500) NULL,	--Link ảnh Căn cước công dân, mặt sau

	RoleId int,

	--Dành cho những Table CRUD dc -> History
	CreatedDate datetime,
	LastModifiedDate datetime,
	CreatedBy nchar(30),
	LastModifiedBy nchar(30),

	CONSTRAINT RoleId_in_UserRole FOREIGN KEY(RoleId) REFERENCES UserRoles(RoleId),
	CONSTRAINT createdUser_in_User FOREIGN KEY(CreatedBy) REFERENCES [dbo].[Users](UserId),
	CONSTRAINT updatedUser_in_User FOREIGN KEY(LastModifiedBy) REFERENCES [dbo].[Users](UserId),
) ON [PRIMARY]
GO

--Students
INSERT INTO [dbo].[Users] VALUES (N'HE153046', null, null, N'nguyenthegiang', N'nguyenthegiang', N'giangnthe153046@fpt.edu.vn', 1, null, null , null, null, null, null, 1, 
GETDATE(), GETDATE(), N'HE153046', N'HE153046');
--Staffs
INSERT INTO [dbo].[Users] VALUES (N'SA000001', null, null, N'thanhle', N'thanhle', N'thanhle@gmail.com', 1, 'Lê Thành', 'image_profile_1.jpg', null, null, null, null, 3, 
GETDATE(), GETDATE(), N'SA000001', N'SA000001');
--Landlords
INSERT INTO [dbo].[Users] VALUES (N'LA000001', null, null, N'tamle', N'tamle', N'tamle@gmail.com', 1, 'Tâm Lê', 'image_profile_1.jpg', '0987654321', 'facebook.com/tamle12', 'identity_card_front.jpg', 'identity_card_back.jpg', 2, 
GETDATE(), GETDATE(), N'SA000001', N'SA000001');

-------------------------------------------------------------------------------------------------------------------------------------------

--Huyện/Quận
CREATE TABLE [dbo].[Districts] (
	DistrictId int NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	DistrictName nvarchar(100),

	CreatedDate datetime,
) ON [PRIMARY]
GO

INSERT INTO [dbo].[Districts] VALUES (N'Huyện Thạch Thất', GETDATE());
INSERT INTO [dbo].[Districts] VALUES (N'Huyện Quốc Oai', GETDATE());
INSERT INTO [dbo].[Districts] VALUES (N'Thị xã Sơn Tây', GETDATE());

-------------------------------------------------------------------------------------------------------------------------------------------

--Phường/Xã
CREATE TABLE [dbo].[Communes] (
	CommuneId int NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	CommuneName nvarchar(100),
	DistrictId int,

	CreatedDate datetime,
	CONSTRAINT DistrictId_in_District FOREIGN KEY(DistrictId) REFERENCES Districts(DistrictId),
) ON [PRIMARY]
GO

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

-------------------------------------------------------------------------------------------------------------------------------------------

--Thôn/Xóm
CREATE TABLE [dbo].[Villages] (
	VillageId int NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	VillageName nvarchar(100),
	CommuneId int,

	CreatedDate datetime,
	CONSTRAINT CommuneId_in_Commune FOREIGN KEY(CommuneId) REFERENCES Communes(CommuneId),
) ON [PRIMARY]
GO

INSERT INTO [dbo].[Villages] VALUES (N'Chi Quan 1', 1, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Chi Quan 2', 1, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Đồng Cam', 1, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Phú Thứ', 1, GETDATE());
INSERT INTO [dbo].[Villages] VALUES (N'Đụn Dương', 1, GETDATE());
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
INSERT INTO [dbo].[Villages] VALUES (N'8', 14, GETDATE());
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
INSERT INTO [dbo].[Villages] VALUES (N'Cấn Thượng', 25, GETDATE());
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

-------------------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE [dbo].[Houses] (
	HouseId int NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	HouseName nvarchar(100),
	Address nvarchar(500),				--địa chỉ cụ thể 
	GoogleMapLocation nvarchar(MAX),	--địa chỉ theo Google Map -> Sử dụng khi hiển thị Map & search khoảng cách
	Information nvarchar(MAX),			--thông tin thêm

	VillageId int,						--thôn/xóm -> phường/xã -> quận/huyện
	LandlordId nchar(30),				--chủ nhà
	CampusId int,						--Campus mà nhà này thuộc về

	--Dành cho những Table CRUD dc -> History
	CreatedDate datetime,
	LastModifiedDate datetime,
	CreatedBy nchar(30),
	LastModifiedBy nchar(30),

	CONSTRAINT LandlordId_in_User FOREIGN KEY(LandlordId) REFERENCES [dbo].[Users](UserId),
	CONSTRAINT VillageId_in_Village FOREIGN KEY(VillageId) REFERENCES [dbo].[Villages](VillageId),
	CONSTRAINT CampusId_in_Campus FOREIGN KEY(CampusId) REFERENCES Campuses(CampusId),

	CONSTRAINT createdUser_in_User2 FOREIGN KEY(CreatedBy) REFERENCES [dbo].[Users](UserId),
	CONSTRAINT updatedUser_in_User2 FOREIGN KEY(LastModifiedBy) REFERENCES [dbo].[Users](UserId),
) ON [PRIMARY]
GO

INSERT INTO [dbo].[Houses] VALUES (N'Trọ Tâm Lê', N'Gần Bún bò Huế', N'someStringGeneratedByGoogleMap', N'Rất đẹp', 3, N'LA000001', 1,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');

-------------------------------------------------------------------------------------------------------------------------------------------

--Trạng thái của 1 phòng (dùng cho Room)
CREATE TABLE [dbo].[Statuses] (
	StatusId int NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	StatusName nvarchar(300),

	CreatedDate datetime,
) ON [PRIMARY]
GO

INSERT INTO [dbo].[Statuses] VALUES (N'Available', GETDATE());	--có thể thuê	-> hiển thị khi search
INSERT INTO [dbo].[Statuses] VALUES (N'Occupied', GETDATE());	--đã có ng thuê	-> ko hiển thị khi search
INSERT INTO [dbo].[Statuses] VALUES (N'Disabled', GETDATE());	--ko dùng dc vì lý do nào đó	-> ko hiển thị khi search

-------------------------------------------------------------------------------------------------------------------------------------------

--Loại phòng (dùng cho Room)
CREATE TABLE [dbo].[RoomTypes] (
	RoomTypeId int NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	RoomTypeName nvarchar(300),

	CreatedDate datetime,
) ON [PRIMARY]
GO

INSERT INTO [dbo].[RoomTypes] VALUES (N'Open', GETDATE());
INSERT INTO [dbo].[RoomTypes] VALUES (N'Closed', GETDATE());
INSERT INTO [dbo].[RoomTypes] VALUES (N'Mini flat', GETDATE());

-------------------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE [dbo].[Rooms] (
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

	--Dành cho những Table CRUD dc -> History
	CreatedDate datetime,
	LastModifiedDate datetime,
	CreatedBy nchar(30),
	LastModifiedBy nchar(30),

	CONSTRAINT StatusId_in_Status FOREIGN KEY(StatusId) REFERENCES [dbo].[Statuses](StatusId),
	CONSTRAINT RoomTypeId_in_RoomType FOREIGN KEY(RoomTypeId) REFERENCES [dbo].[RoomTypes](RoomTypeId),
	CONSTRAINT HouseId_in_House FOREIGN KEY(HouseId) REFERENCES [dbo].[Houses](HouseId),

	CONSTRAINT createdUser_in_User3 FOREIGN KEY(CreatedBy) REFERENCES [dbo].[Users](UserId),
	CONSTRAINT updatedUser_in_User3 FOREIGN KEY(LastModifiedBy) REFERENCES [dbo].[Users](UserId),
) ON [PRIMARY]
GO

INSERT INTO [dbo].[Rooms] VALUES (N'101', 3000000, N'Gạch sàn nhà có họa tiết hình con cá', 5, 2, 1, 1, 1, 1, 2, 1, 
GETDATE(), GETDATE(), N'LA000001', N'LA000001');

-------------------------------------------------------------------------------------------------------------------------------------------

--Đánh giá & Comment của 1 người dùng
CREATE TABLE [dbo].[Rates] (
	RateId int NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	Star int,							--Số sao Rate
	Comment nvarchar(MAX),				--Nội dung Comment
	LandlordReply nvarchar(MAX),		--Phản hồi của chủ nhà

	HouseId int,						--Cái nhà dc Comment
	StudentId nchar(30),				--Người viết Comment

	--Dành cho những Table CRUD dc -> History
	CreatedDate datetime,
	LastModifiedDate datetime,
	CreatedBy nchar(30),
	LastModifiedBy nchar(30),

	CONSTRAINT HouseId_in_House2 FOREIGN KEY(HouseId) REFERENCES [dbo].[Houses](HouseId),
	CONSTRAINT StudentId_in_User FOREIGN KEY(StudentId) REFERENCES [dbo].[Users](UserId),

	CONSTRAINT createdUser_in_User4 FOREIGN KEY(CreatedBy) REFERENCES [dbo].[Users](UserId),
	CONSTRAINT updatedUser_in_User4 FOREIGN KEY(LastModifiedBy) REFERENCES [dbo].[Users](UserId),
) ON [PRIMARY]
GO

INSERT INTO [dbo].[Rates] VALUES (5, N'Rất tuyệt vời, gần trường nữa', N'Cảm ơn bạn', 1, N'HE153046', 
GETDATE(), GETDATE(), N'HE153046', N'HE153046');

-------------------------------------------------------------------------------------------------------------------------------------------

--Ảnh nhà
CREATE TABLE [dbo].[ImagesOfHouse] (
	ImageId int NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	ImageLink nvarchar(500),

	HouseId int,

	--Dành cho những Table CRUD dc -> History
	CreatedDate datetime,
	LastModifiedDate datetime,
	CreatedBy nchar(30),
	LastModifiedBy nchar(30),

	CONSTRAINT HouseId_in_House3 FOREIGN KEY(HouseId) REFERENCES [dbo].[Houses](HouseId),

	CONSTRAINT createdUser_in_User5 FOREIGN KEY(CreatedBy) REFERENCES [dbo].[Users](UserId),
	CONSTRAINT updatedUser_in_User5 FOREIGN KEY(LastModifiedBy) REFERENCES [dbo].[Users](UserId),
) ON [PRIMARY]
GO

INSERT INTO [dbo].[ImagesOfHouse] VALUES (N'link_of_image.jpg', 1,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');

-------------------------------------------------------------------------------------------------------------------------------------------

--Ảnh phòng
CREATE TABLE [dbo].[ImagesOfRoom] (
	ImageId int NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	ImageLink nvarchar(500),

	RoomId int,

	--Dành cho những Table CRUD dc -> History
	CreatedDate datetime,
	LastModifiedDate datetime,
	CreatedBy nchar(30),
	LastModifiedBy nchar(30),

	CONSTRAINT RoomId_in_Room FOREIGN KEY(RoomId) REFERENCES [dbo].[Rooms](RoomId),

	CONSTRAINT createdUser_in_User6 FOREIGN KEY(CreatedBy) REFERENCES [dbo].[Users](UserId),
	CONSTRAINT updatedUser_in_User6 FOREIGN KEY(LastModifiedBy) REFERENCES [dbo].[Users](UserId),
) ON [PRIMARY]
GO

INSERT INTO [dbo].[ImagesOfRoom] VALUES (N'link_of_image2.jpg', 1,
GETDATE(), GETDATE(), N'LA000001', N'LA000001');

-------------------------------------------------------------------------------------------------------------------------------------------

--Report của sinh viên đối với nhà trọ
CREATE TABLE [dbo].[Reports] (
	ReportId int NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	ReportContent nvarchar(MAX),

	StudentId nchar(30),
	HouseId int,

	--Dành cho những Table CRUD dc -> History
	CreatedDate datetime,
	LastModifiedDate datetime,
	CreatedBy nchar(30),
	LastModifiedBy nchar(30),

	CONSTRAINT HouseId_in_House4 FOREIGN KEY(HouseId) REFERENCES [dbo].[Houses](HouseId),
	CONSTRAINT StudentId_in_User3 FOREIGN KEY(StudentId) REFERENCES [dbo].[Users](UserId),

	CONSTRAINT createdUser_in_User7 FOREIGN KEY(CreatedBy) REFERENCES [dbo].[Users](UserId),
	CONSTRAINT updatedUser_in_User7 FOREIGN KEY(LastModifiedBy) REFERENCES [dbo].[Users](UserId),
) ON [PRIMARY]
GO

INSERT INTO [dbo].[Reports] VALUES ('Chủ trọ tăng giá phòng trái với hợp đồng', N'HE153046', 1,
GETDATE(), GETDATE(), N'HE153046', N'HE153046');