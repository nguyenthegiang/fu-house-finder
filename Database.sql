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

	--Dành cho người Login = Facebook/Google
	FacebookUserId nchar(300) NULL,
	GoogleUserId nchar(300) NULL,

	Username nvarchar(100),
	Password nvarchar(100),
	Email nvarchar(100),
	Active bit,		--chuyển thành false nếu User bị Disable

	--Dành cho Staff & Landlord
	FullName nvarchar(500) NULL,

	--Những thông tin riêng của Landlord
	PhoneNumber nvarchar(50) NULL,
	FacebookURL nvarchar(300) NULL,
	IdentityCardFrontSideImageLink nvarchar(500) NULL,	--Link ảnh Căn cước công dân, mặt trước
	IdentityCardBackSideImageLink nvarchar(500) NULL,	--Link ảnh Căn cước công dân, mặt sau

	RoleId int,

	--Dành cho những Table CRUD dc -> History
	createdDate datetime,
	updatedDate datetime,
	createdUser nchar(30),
	updatedUser nchar(30),

	CONSTRAINT RoleId_in_UserRole FOREIGN KEY(RoleId) REFERENCES UserRole(RoleId),
	CONSTRAINT createdUser_in_User FOREIGN KEY(createdUser) REFERENCES [dbo].[User](UserId),
	CONSTRAINT updatedUser_in_User FOREIGN KEY(updatedUser) REFERENCES [dbo].[User](UserId),
) ON [PRIMARY]
GO

--Students
INSERT INTO [dbo].[User] VALUES (N'HE153046', null, null, N'nguyenthegiang', N'nguyenthegiang', N'giangnthe153046@fpt.edu.vn', 1, null , null, null, null, null, 1, 
'2022-09-28', '2022-09-28', N'HE153046', N'HE153046');
--Staffs
INSERT INTO [dbo].[User] VALUES (N'SA000001', null, null, N'thanhle', N'thanhle', N'thanhle@gmail.com', 1, 'Lê Thành', null, null, null, null, 3, 
'2022-09-28', '2022-09-28', N'SA000001', N'SA000001');
--Landlords
INSERT INTO [dbo].[User] VALUES (N'LA000001', null, null, N'tamle', N'tamle', N'tamle@gmail.com', 1, 'Tâm Lê', '0987654321', 'facebook.com/tamle12', 'identity_card_front.jpg', 'identity_card_back.jpg', 2, 
'2022-09-28', '2022-09-28', N'SA000001', N'SA000001');

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
	CommuneName nvarchar(100),
	DistrictId int,
	CONSTRAINT DistrictId_in_District FOREIGN KEY(DistrictId) REFERENCES District(DistrictId),
) ON [PRIMARY]
GO

INSERT INTO [dbo].[Commune] VALUES (N'Thị trấn Liên Quan', 1);
INSERT INTO [dbo].[Commune] VALUES (N'Xã Bình Phú', 1);
INSERT INTO [dbo].[Commune] VALUES (N'Xã Bình Yên', 1);
INSERT INTO [dbo].[Commune] VALUES (N'Xã Canh Nậu', 1);
INSERT INTO [dbo].[Commune] VALUES (N'Xã Cẩm Yên', 1);
INSERT INTO [dbo].[Commune] VALUES (N'Xã Cần Kiệm', 1);
INSERT INTO [dbo].[Commune] VALUES (N'Xã Chàng Sơn', 1);
INSERT INTO [dbo].[Commune] VALUES (N'Xã Dị Nậu', 1);
INSERT INTO [dbo].[Commune] VALUES (N'Xã Đại Đồng', 1);
INSERT INTO [dbo].[Commune] VALUES (N'Xã Đồng Trúc', 1);
INSERT INTO [dbo].[Commune] VALUES (N'Xã Hạ Bằng', 1);
INSERT INTO [dbo].[Commune] VALUES (N'Xã Hương Ngải', 1);
INSERT INTO [dbo].[Commune] VALUES (N'Xã Hữu Bằng', 1);
INSERT INTO [dbo].[Commune] VALUES (N'Xã Kim Quan', 1);
INSERT INTO [dbo].[Commune] VALUES (N'Xã Lại Thượng', 1);
INSERT INTO [dbo].[Commune] VALUES (N'Xã Phú Kim', 1);
INSERT INTO [dbo].[Commune] VALUES (N'Xã Phùng Xá', 1);
INSERT INTO [dbo].[Commune] VALUES (N'Xã Tân Xã', 1);
INSERT INTO [dbo].[Commune] VALUES (N'Xã Thạch Hòa', 1);
INSERT INTO [dbo].[Commune] VALUES (N'Xã Thạch Xá', 1);
INSERT INTO [dbo].[Commune] VALUES (N'Xã Tiến Xuân', 1);
INSERT INTO [dbo].[Commune] VALUES (N'Xã Yên Bình', 1);
INSERT INTO [dbo].[Commune] VALUES (N'Xã Yên Trung', 1);

INSERT INTO [dbo].[Commune] VALUES (N'Thị trấn Quốc Oai', 2);
INSERT INTO [dbo].[Commune] VALUES (N'Xã Cấn Hữu', 2);
INSERT INTO [dbo].[Commune] VALUES (N'Xã Cộng Hòa', 2);
INSERT INTO [dbo].[Commune] VALUES (N'Xã Đại Thành', 2);
INSERT INTO [dbo].[Commune] VALUES (N'Xã Đồng Quang', 2);
INSERT INTO [dbo].[Commune] VALUES (N'Xã Đông Xuân', 2);
INSERT INTO [dbo].[Commune] VALUES (N'Xã Đông Yên', 2);
INSERT INTO [dbo].[Commune] VALUES (N'Xã Hòa Thạch', 2);
INSERT INTO [dbo].[Commune] VALUES (N'Xã Liệp Tuyết', 2);
INSERT INTO [dbo].[Commune] VALUES (N'Xã Nghĩa Hương', 2);
INSERT INTO [dbo].[Commune] VALUES (N'Xã Ngọc Liệp', 2);
INSERT INTO [dbo].[Commune] VALUES (N'Xã Ngọc Mỹ', 2);
INSERT INTO [dbo].[Commune] VALUES (N'Xã Phú Cát', 2);
INSERT INTO [dbo].[Commune] VALUES (N'Xã Phú Mãn', 2);
INSERT INTO [dbo].[Commune] VALUES (N'Xã Phượng Cách', 2);
INSERT INTO [dbo].[Commune] VALUES (N'Xã Sài Sơn', 2);
INSERT INTO [dbo].[Commune] VALUES (N'Xã Tân Hòa', 2);
INSERT INTO [dbo].[Commune] VALUES (N'Xã Tân Phú', 2);
INSERT INTO [dbo].[Commune] VALUES (N'Xã Thạch Thán', 2);
INSERT INTO [dbo].[Commune] VALUES (N'Xã Tuyết Nghĩa', 2);
INSERT INTO [dbo].[Commune] VALUES (N'Xã Yên Sơn', 2);

INSERT INTO [dbo].[Commune] VALUES (N'Phường Lê Lợi', 3);
INSERT INTO [dbo].[Commune] VALUES (N'Phường Ngô Quyền', 3);
INSERT INTO [dbo].[Commune] VALUES (N'Phường Phú Thịnh', 3);
INSERT INTO [dbo].[Commune] VALUES (N'Phường Quang Trung', 3);
INSERT INTO [dbo].[Commune] VALUES (N'Phường Sơn Lộc', 3);
INSERT INTO [dbo].[Commune] VALUES (N'Phường Trung Hưng', 3);
INSERT INTO [dbo].[Commune] VALUES (N'Phường Trung Sơn Trầm', 3);
INSERT INTO [dbo].[Commune] VALUES (N'Phường Viên Sơn', 3);
INSERT INTO [dbo].[Commune] VALUES (N'Phường Xuân Khanh', 3);
INSERT INTO [dbo].[Commune] VALUES (N'Xã Cổ Đông', 3);
INSERT INTO [dbo].[Commune] VALUES (N'Xã Đường Lâm', 3);
INSERT INTO [dbo].[Commune] VALUES (N'Xã Kim Sơn', 3);
INSERT INTO [dbo].[Commune] VALUES (N'Xã Sơn Đông', 3);
INSERT INTO [dbo].[Commune] VALUES (N'Xã Thanh Mỹ', 3);
INSERT INTO [dbo].[Commune] VALUES (N'Xã Xuân Sơn', 3);

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
INSERT INTO [dbo].[Village] VALUES (N'Phú Thứ', 1);
INSERT INTO [dbo].[Village] VALUES (N'Đụn Dương', 1);
INSERT INTO [dbo].[Village] VALUES (N'Hà Tân', 1);
INSERT INTO [dbo].[Village] VALUES (N'Khu Phố', 1);

INSERT INTO [dbo].[Village] VALUES (N'Bình Xá', 2);
INSERT INTO [dbo].[Village] VALUES (N'Bình Xá', 2);
INSERT INTO [dbo].[Village] VALUES (N'Phú Hòa', 2);
INSERT INTO [dbo].[Village] VALUES (N'Phú Ổ 1', 2);
INSERT INTO [dbo].[Village] VALUES (N'Phú Ổ 2', 2);
INSERT INTO [dbo].[Village] VALUES (N'Phú Ổ 3', 2);
INSERT INTO [dbo].[Village] VALUES (N'Phú Ổ 4', 2);
INSERT INTO [dbo].[Village] VALUES (N'Phú Ổ 5', 2);
INSERT INTO [dbo].[Village] VALUES (N'Phú Ổ 6', 2);

INSERT INTO [dbo].[Village] VALUES (N'Yên Mỹ', 3);
INSERT INTO [dbo].[Village] VALUES (N'Phúc Tiến', 3);
INSERT INTO [dbo].[Village] VALUES (N'Đồi Sen', 3);
INSERT INTO [dbo].[Village] VALUES (N'Sen Trì', 3);
INSERT INTO [dbo].[Village] VALUES (N'Cánh Chủ', 3);
INSERT INTO [dbo].[Village] VALUES (N'Vân Lôi', 3);
INSERT INTO [dbo].[Village] VALUES (N'Thái Bình', 3);
INSERT INTO [dbo].[Village] VALUES (N'Hòa Lạc', 3);
INSERT INTO [dbo].[Village] VALUES (N'Linh Sơn', 3);

INSERT INTO [dbo].[Village] VALUES (N'1', 4);
INSERT INTO [dbo].[Village] VALUES (N'2', 4);
INSERT INTO [dbo].[Village] VALUES (N'3', 4);
INSERT INTO [dbo].[Village] VALUES (N'4', 4);
INSERT INTO [dbo].[Village] VALUES (N'5', 4);
INSERT INTO [dbo].[Village] VALUES (N'6', 4);
INSERT INTO [dbo].[Village] VALUES (N'7', 4);
INSERT INTO [dbo].[Village] VALUES (N'8', 4);
INSERT INTO [dbo].[Village] VALUES (N'9', 4);
INSERT INTO [dbo].[Village] VALUES (N'10', 4);
INSERT INTO [dbo].[Village] VALUES (N'11', 4);

INSERT INTO [dbo].[Village] VALUES (N'Cẩm Bào', 5);
INSERT INTO [dbo].[Village] VALUES (N'Kinh Đạ', 5);
INSERT INTO [dbo].[Village] VALUES (N'Yên Lỗ', 5);

INSERT INTO [dbo].[Village] VALUES (N'Phú Đa 1', 6);
INSERT INTO [dbo].[Village] VALUES (N'Phú Đa 2', 6);
INSERT INTO [dbo].[Village] VALUES (N'Phú Lễ', 6);
INSERT INTO [dbo].[Village] VALUES (N'Yên Lạc 1', 6);
INSERT INTO [dbo].[Village] VALUES (N'Yên Lạc 2', 6);
INSERT INTO [dbo].[Village] VALUES (N'Yên Lạc 3', 6);

INSERT INTO [dbo].[Village] VALUES (N'1', 7);
INSERT INTO [dbo].[Village] VALUES (N'2', 7);
INSERT INTO [dbo].[Village] VALUES (N'3', 7);
INSERT INTO [dbo].[Village] VALUES (N'4', 7);
INSERT INTO [dbo].[Village] VALUES (N'5', 7);
INSERT INTO [dbo].[Village] VALUES (N'6', 7);
INSERT INTO [dbo].[Village] VALUES (N'7', 7);

INSERT INTO [dbo].[Village] VALUES (N'Tam Nông 1', 8);
INSERT INTO [dbo].[Village] VALUES (N'Tam Nông 2', 8);
INSERT INTO [dbo].[Village] VALUES (N'Hòa Bình 1', 8);
INSERT INTO [dbo].[Village] VALUES (N'Hòa Bình 2', 8);
INSERT INTO [dbo].[Village] VALUES (N'Đoàn Kết 1', 8);
INSERT INTO [dbo].[Village] VALUES (N'Đoàn Kết 2', 8);

INSERT INTO [dbo].[Village] VALUES (N'Minh Nghĩa', 9);
INSERT INTO [dbo].[Village] VALUES (N'Minh Đức', 9);
INSERT INTO [dbo].[Village] VALUES (N'Ngọc Lâu', 9);
INSERT INTO [dbo].[Village] VALUES (N'Hương Lam', 9);
INSERT INTO [dbo].[Village] VALUES (N'Rộc Đoài', 9);
INSERT INTO [dbo].[Village] VALUES (N'Tây Trong', 9);
INSERT INTO [dbo].[Village] VALUES (N'Hàn Chùa', 9);
INSERT INTO [dbo].[Village] VALUES (N'Đình Rối', 9);
INSERT INTO [dbo].[Village] VALUES (N'Lươn Trong', 9);
INSERT INTO [dbo].[Village] VALUES (N'Lươn Ngoài', 9);
INSERT INTO [dbo].[Village] VALUES (N'Đồng Cầu', 9);

INSERT INTO [dbo].[Village] VALUES (N'Chầm Muộn', 10);
INSERT INTO [dbo].[Village] VALUES (N'Trúc Voi', 10);
INSERT INTO [dbo].[Village] VALUES (N'Đồng Táng', 10);
INSERT INTO [dbo].[Village] VALUES (N'Đồng Kho', 10);
INSERT INTO [dbo].[Village] VALUES (N'Hòa Bình', 10);
INSERT INTO [dbo].[Village] VALUES (N'Chiến Thắng', 10);
INSERT INTO [dbo].[Village] VALUES (N'Khu Ba', 10);
INSERT INTO [dbo].[Village] VALUES (N'Xóm Đông', 10);
INSERT INTO [dbo].[Village] VALUES (N'Khoang Mè', 10);

INSERT INTO [dbo].[Village] VALUES (N'Khoang Mè 1', 11);
INSERT INTO [dbo].[Village] VALUES (N'Khoang Mè 2', 11);
INSERT INTO [dbo].[Village] VALUES (N'Đầm Cầu', 11);
INSERT INTO [dbo].[Village] VALUES (N'Đầm Quán', 11);
INSERT INTO [dbo].[Village] VALUES (N'Giếng Cốc', 11);
INSERT INTO [dbo].[Village] VALUES (N'Mương Ốc', 11);
INSERT INTO [dbo].[Village] VALUES (N'Vực Giang', 11);
INSERT INTO [dbo].[Village] VALUES (N'Gò Mận', 11);
INSERT INTO [dbo].[Village] VALUES (N'Giang Nu', 11);

INSERT INTO [dbo].[Village] VALUES (N'1', 12);
INSERT INTO [dbo].[Village] VALUES (N'2', 12);
INSERT INTO [dbo].[Village] VALUES (N'3', 12);
INSERT INTO [dbo].[Village] VALUES (N'4', 12);
INSERT INTO [dbo].[Village] VALUES (N'5', 12);
INSERT INTO [dbo].[Village] VALUES (N'6', 12);
INSERT INTO [dbo].[Village] VALUES (N'7', 12);
INSERT INTO [dbo].[Village] VALUES (N'8', 12);
INSERT INTO [dbo].[Village] VALUES (N'9', 12);

INSERT INTO [dbo].[Village] VALUES (N'Si Chợ', 13);
INSERT INTO [dbo].[Village] VALUES (N'Bò', 13);
INSERT INTO [dbo].[Village] VALUES (N'Sen Trì', 13);
INSERT INTO [dbo].[Village] VALUES (N'Bàn Giữa', 13);
INSERT INTO [dbo].[Village] VALUES (N'Đình', 13);
INSERT INTO [dbo].[Village] VALUES (N'Đông', 13);
INSERT INTO [dbo].[Village] VALUES (N'Miễu', 13);
INSERT INTO [dbo].[Village] VALUES (N'Ba Mát', 13);
INSERT INTO [dbo].[Village] VALUES (N'Giếng Cốc', 13);

INSERT INTO [dbo].[Village] VALUES (N'1', 14);
INSERT INTO [dbo].[Village] VALUES (N'2', 14);
INSERT INTO [dbo].[Village] VALUES (N'3', 14);
INSERT INTO [dbo].[Village] VALUES (N'4', 14);
INSERT INTO [dbo].[Village] VALUES (N'Mơ', 14);
INSERT INTO [dbo].[Village] VALUES (N'6', 14);
INSERT INTO [dbo].[Village] VALUES (N'7', 14);
INSERT INTO [dbo].[Village] VALUES (N'8', 14);
INSERT INTO [dbo].[Village] VALUES (N'9', 14);
INSERT INTO [dbo].[Village] VALUES (N'10', 14);
INSERT INTO [dbo].[Village] VALUES (N'84', 14);

INSERT INTO [dbo].[Village] VALUES (N'Ngũ Sơn', 15);
INSERT INTO [dbo].[Village] VALUES (N'Lại Khánh', 15);
INSERT INTO [dbo].[Village] VALUES (N'Lại Thượng', 15);
INSERT INTO [dbo].[Village] VALUES (N'Phú Thụ', 15);
INSERT INTO [dbo].[Village] VALUES (N'Thanh Câu', 15);
INSERT INTO [dbo].[Village] VALUES (N'Hoàng Xá', 15);

INSERT INTO [dbo].[Village] VALUES (N'Thủy Lai', 16);
INSERT INTO [dbo].[Village] VALUES (N'Phú Nghĩa', 16);
INSERT INTO [dbo].[Village] VALUES (N'Bách Kim', 16);
INSERT INTO [dbo].[Village] VALUES (N'Nội Thôn', 16);
INSERT INTO [dbo].[Village] VALUES (N'Ngoại Thôn', 16);

INSERT INTO [dbo].[Village] VALUES (N'1', 17);
INSERT INTO [dbo].[Village] VALUES (N'2', 17);
INSERT INTO [dbo].[Village] VALUES (N'3', 17);
INSERT INTO [dbo].[Village] VALUES (N'4', 17);
INSERT INTO [dbo].[Village] VALUES (N'5', 17);
INSERT INTO [dbo].[Village] VALUES (N'6', 17);
INSERT INTO [dbo].[Village] VALUES (N'7', 17);
INSERT INTO [dbo].[Village] VALUES (N'8', 17);
INSERT INTO [dbo].[Village] VALUES (N'9', 17);

INSERT INTO [dbo].[Village] VALUES (N'Phú Hữu', 18);
INSERT INTO [dbo].[Village] VALUES (N'Cừ Viên', 18);
INSERT INTO [dbo].[Village] VALUES (N'Cầu Giáo', 18);
INSERT INTO [dbo].[Village] VALUES (N'Hương Trung', 18);
INSERT INTO [dbo].[Village] VALUES (N'Cầu Sông', 18);
INSERT INTO [dbo].[Village] VALUES (N'Xóm Mới', 18);
INSERT INTO [dbo].[Village] VALUES (N'Xóm Quán', 18);
INSERT INTO [dbo].[Village] VALUES (N'Xóm Hiệp', 18);
INSERT INTO [dbo].[Village] VALUES (N'Xóm Than', 18);

INSERT INTO [dbo].[Village] VALUES (N'1', 19);
INSERT INTO [dbo].[Village] VALUES (N'2', 19);
INSERT INTO [dbo].[Village] VALUES (N'3', 19);
INSERT INTO [dbo].[Village] VALUES (N'4', 19);
INSERT INTO [dbo].[Village] VALUES (N'5', 19);
INSERT INTO [dbo].[Village] VALUES (N'6', 19);
INSERT INTO [dbo].[Village] VALUES (N'7', 19);
INSERT INTO [dbo].[Village] VALUES (N'8', 19);
INSERT INTO [dbo].[Village] VALUES (N'9', 19);
INSERT INTO [dbo].[Village] VALUES (N'10', 19);
INSERT INTO [dbo].[Village] VALUES (N'11', 19);

INSERT INTO [dbo].[Village] VALUES (N'1', 20);
INSERT INTO [dbo].[Village] VALUES (N'2', 20);
INSERT INTO [dbo].[Village] VALUES (N'3', 20);
INSERT INTO [dbo].[Village] VALUES (N'4', 20);
INSERT INTO [dbo].[Village] VALUES (N'5', 20);
INSERT INTO [dbo].[Village] VALUES (N'6', 20);
INSERT INTO [dbo].[Village] VALUES (N'7', 20);
INSERT INTO [dbo].[Village] VALUES (N'8', 20);
INSERT INTO [dbo].[Village] VALUES (N'9', 20);

INSERT INTO [dbo].[Village] VALUES (N'Chùa 1', 21);
INSERT INTO [dbo].[Village] VALUES (N'Chùa 2', 21);
INSERT INTO [dbo].[Village] VALUES (N'Đồng Dâu', 21);
INSERT INTO [dbo].[Village] VALUES (N'Đồng Cao', 21);
INSERT INTO [dbo].[Village] VALUES (N'Miễu 1', 21);
INSERT INTO [dbo].[Village] VALUES (N'Miễu 2', 21);
INSERT INTO [dbo].[Village] VALUES (N'Gò Chói 1', 21);
INSERT INTO [dbo].[Village] VALUES (N'Gò Chói 2', 21);
INSERT INTO [dbo].[Village] VALUES (N'Gò Mè', 21);
INSERT INTO [dbo].[Village] VALUES (N'Bình Sơn', 21);
INSERT INTO [dbo].[Village] VALUES (N'Bãi Dài', 21);
INSERT INTO [dbo].[Village] VALUES (N'Trại Mới 1', 21);
INSERT INTO [dbo].[Village] VALUES (N'Trại Mới 2', 21);
INSERT INTO [dbo].[Village] VALUES (N'Cố Đụng 1', 21);
INSERT INTO [dbo].[Village] VALUES (N'Cố Đụng 2', 21);
INSERT INTO [dbo].[Village] VALUES (N'Quê Vải', 21);
INSERT INTO [dbo].[Village] VALUES (N'Gò Chè', 21);
INSERT INTO [dbo].[Village] VALUES (N'Nhòn', 21);

INSERT INTO [dbo].[Village] VALUES (N'Tân Bình', 22);
INSERT INTO [dbo].[Village] VALUES (N'Lụa', 22);
INSERT INTO [dbo].[Village] VALUES (N'Vao', 22);
INSERT INTO [dbo].[Village] VALUES (N'Thung Mộ', 22);
INSERT INTO [dbo].[Village] VALUES (N'Thạch Bình', 22);
INSERT INTO [dbo].[Village] VALUES (N'Thuống', 22);
INSERT INTO [dbo].[Village] VALUES (N'Dục', 22);
INSERT INTO [dbo].[Village] VALUES (N'Đình', 22);
INSERT INTO [dbo].[Village] VALUES (N'Cò', 22);
INSERT INTO [dbo].[Village] VALUES (N'Dân Lập', 22);

INSERT INTO [dbo].[Village] VALUES (N'Bối', 23);
INSERT INTO [dbo].[Village] VALUES (N'Luồng', 23);
INSERT INTO [dbo].[Village] VALUES (N'Số', 23);
INSERT INTO [dbo].[Village] VALUES (N'Tơi', 23);
INSERT INTO [dbo].[Village] VALUES (N'Hương', 23);
INSERT INTO [dbo].[Village] VALUES (N'Lặt', 23);
INSERT INTO [dbo].[Village] VALUES (N'Hội', 23);

INSERT INTO [dbo].[Village] VALUES (N'Đình Tổ', 24);
INSERT INTO [dbo].[Village] VALUES (N'Du Nghệ', 24);
INSERT INTO [dbo].[Village] VALUES (N'Hoa Vôi', 24);
INSERT INTO [dbo].[Village] VALUES (N'TDP Ngô Sài', 24);
INSERT INTO [dbo].[Village] VALUES (N'TDP Phố Huyện', 24);

INSERT INTO [dbo].[Village] VALUES (N'Cấn Hạ', 25);
INSERT INTO [dbo].[Village] VALUES (N'Cấn Thượng', 25);
INSERT INTO [dbo].[Village] VALUES (N'Cây Chay', 25);
INSERT INTO [dbo].[Village] VALUES (N'Đĩnh Tú', 25);
INSERT INTO [dbo].[Village] VALUES (N'Thái Khê', 25);
INSERT INTO [dbo].[Village] VALUES (N'Thượng Khê', 25);

INSERT INTO [dbo].[Village] VALUES (N'1', 26);
INSERT INTO [dbo].[Village] VALUES (N'2', 26);
INSERT INTO [dbo].[Village] VALUES (N'3', 26);
INSERT INTO [dbo].[Village] VALUES (N'4', 26);
INSERT INTO [dbo].[Village] VALUES (N'5', 26);
INSERT INTO [dbo].[Village] VALUES (N'6', 26);

INSERT INTO [dbo].[Village] VALUES (N'Đại Tảo', 27);
INSERT INTO [dbo].[Village] VALUES (N'Độ Tràng', 27);
INSERT INTO [dbo].[Village] VALUES (N'Tình Lam', 27);

INSERT INTO [dbo].[Village] VALUES (N'Đồng Lư', 28);
INSERT INTO [dbo].[Village] VALUES (N'Dương Cốc', 28);
INSERT INTO [dbo].[Village] VALUES (N'Yên Nội', 28);

INSERT INTO [dbo].[Village] VALUES (N'Cửu Khâu', 29);
INSERT INTO [dbo].[Village] VALUES (N'Đà Thâm', 29);
INSERT INTO [dbo].[Village] VALUES (N'Đồng Âm', 29);
INSERT INTO [dbo].[Village] VALUES (N'Đồng Bèn 1', 29);
INSERT INTO [dbo].[Village] VALUES (N'Đồng Bèn 2', 29);
INSERT INTO [dbo].[Village] VALUES (N'Đồng Bồ', 29);
INSERT INTO [dbo].[Village] VALUES (N'Đồng Chằn', 29);
INSERT INTO [dbo].[Village] VALUES (N'Đồng Rằng', 29);
INSERT INTO [dbo].[Village] VALUES (N'Lập Thành', 29);

INSERT INTO [dbo].[Village] VALUES (N'Đông Hạ', 30);
INSERT INTO [dbo].[Village] VALUES (N'Đông Thượng', 30);
INSERT INTO [dbo].[Village] VALUES (N'Việt Yên', 30);
INSERT INTO [dbo].[Village] VALUES (N'Yên Thái', 30);

INSERT INTO [dbo].[Village] VALUES (N'Bạch Thạch', 31);
INSERT INTO [dbo].[Village] VALUES (N'Hòa Phú', 31);
INSERT INTO [dbo].[Village] VALUES (N'Hòa Trúc', 31);
INSERT INTO [dbo].[Village] VALUES (N'Long Phú', 31);
INSERT INTO [dbo].[Village] VALUES (N'Thắng Đầu', 31);

INSERT INTO [dbo].[Village] VALUES (N'Bái Ngoại', 32);
INSERT INTO [dbo].[Village] VALUES (N'Bái Nội', 32);
INSERT INTO [dbo].[Village] VALUES (N'Đại Phu', 32);
INSERT INTO [dbo].[Village] VALUES (N'Thông Đạt', 32);
INSERT INTO [dbo].[Village] VALUES (N'Vĩnh Phúc', 32);

INSERT INTO [dbo].[Village] VALUES (N'Thế Trụ', 33);
INSERT INTO [dbo].[Village] VALUES (N'Văn Khê', 33);
INSERT INTO [dbo].[Village] VALUES (N'Văn Quang', 33);

INSERT INTO [dbo].[Village] VALUES (N'Đồng Bụt', 34);
INSERT INTO [dbo].[Village] VALUES (N'Liệp Mai', 34);
INSERT INTO [dbo].[Village] VALUES (N'Ngọc Bài', 34);
INSERT INTO [dbo].[Village] VALUES (N'Ngọc Phúc', 34);

INSERT INTO [dbo].[Village] VALUES (N'Ngọc Than', 35);
INSERT INTO [dbo].[Village] VALUES (N'Phúc Mỹ', 35);

INSERT INTO [dbo].[Village] VALUES (N'1', 36);
INSERT INTO [dbo].[Village] VALUES (N'2', 36);
INSERT INTO [dbo].[Village] VALUES (N'3', 36);
INSERT INTO [dbo].[Village] VALUES (N'4', 36);
INSERT INTO [dbo].[Village] VALUES (N'5', 36);
INSERT INTO [dbo].[Village] VALUES (N'6', 36);
INSERT INTO [dbo].[Village] VALUES (N'7', 36);

INSERT INTO [dbo].[Village] VALUES (N'1 (Đồng Vàng)', 37);
INSERT INTO [dbo].[Village] VALUES (N'2 (Cổ Rùa)', 37);
INSERT INTO [dbo].[Village] VALUES (N'3 (Đồng Âm)', 37);
INSERT INTO [dbo].[Village] VALUES (N'4 (Trán Voi)', 37);
INSERT INTO [dbo].[Village] VALUES (N'5 (Đồng Vỡ)', 37);
INSERT INTO [dbo].[Village] VALUES (N'(Làng Trên)', 37);

INSERT INTO [dbo].[Village] VALUES (N'1', 38);
INSERT INTO [dbo].[Village] VALUES (N'2', 38);
INSERT INTO [dbo].[Village] VALUES (N'3', 38);
INSERT INTO [dbo].[Village] VALUES (N'4', 38);

INSERT INTO [dbo].[Village] VALUES (N'Đa Phúc', 39);
INSERT INTO [dbo].[Village] VALUES (N'Khánh Tân', 39);
INSERT INTO [dbo].[Village] VALUES (N'Năm Trại', 39);
INSERT INTO [dbo].[Village] VALUES (N'Phúc Đức', 39);
INSERT INTO [dbo].[Village] VALUES (N'Sài Khê', 39);
INSERT INTO [dbo].[Village] VALUES (N'Thụy Khê', 39);

INSERT INTO [dbo].[Village] VALUES (N'An Ninh', 40);
INSERT INTO [dbo].[Village] VALUES (N'Bờ Hồ', 40);
INSERT INTO [dbo].[Village] VALUES (N'Đồng Găng', 40);
INSERT INTO [dbo].[Village] VALUES (N'Thị Ngoại', 40);
INSERT INTO [dbo].[Village] VALUES (N'Thổ Ngõa', 40);
INSERT INTO [dbo].[Village] VALUES (N'Yên Mã', 40);

INSERT INTO [dbo].[Village] VALUES (N'Hạ Hòa', 41);
INSERT INTO [dbo].[Village] VALUES (N'Phú Hạng', 41);
INSERT INTO [dbo].[Village] VALUES (N'Yên Quán', 41);
INSERT INTO [dbo].[Village] VALUES (N'Yên Quang', 41);

INSERT INTO [dbo].[Village] VALUES (N'1', 42);
INSERT INTO [dbo].[Village] VALUES (N'2', 42);
INSERT INTO [dbo].[Village] VALUES (N'3', 42);
INSERT INTO [dbo].[Village] VALUES (N'4', 42);

INSERT INTO [dbo].[Village] VALUES (N'Cổ Hiền', 43);
INSERT INTO [dbo].[Village] VALUES (N'Đại Đồng', 43);
INSERT INTO [dbo].[Village] VALUES (N'Độ Lân', 43);
INSERT INTO [dbo].[Village] VALUES (N'Đông Sơn', 43);
INSERT INTO [dbo].[Village] VALUES (N'Liên Trì', 43);
INSERT INTO [dbo].[Village] VALUES (N'Muôn', 43);
INSERT INTO [dbo].[Village] VALUES (N'Ro', 43);

INSERT INTO [dbo].[Village] VALUES (N'Ba Nhà', 44);
INSERT INTO [dbo].[Village] VALUES (N'Quảng Yên', 44);
INSERT INTO [dbo].[Village] VALUES (N'Sơn Trung', 44);

INSERT INTO [dbo].[Village] VALUES (N'TDP Hậu An', 45);
INSERT INTO [dbo].[Village] VALUES (N'TDP Hậu Bình', 45);
INSERT INTO [dbo].[Village] VALUES (N'TDP Hậu Ninh', 45);
INSERT INTO [dbo].[Village] VALUES (N'TDP Hậu Thái', 45);
INSERT INTO [dbo].[Village] VALUES (N'TDP Hậu Tĩnh', 45);
INSERT INTO [dbo].[Village] VALUES (N'TDP Hồng Hà', 45);
INSERT INTO [dbo].[Village] VALUES (N'TDP Lạc Sơn', 45);
INSERT INTO [dbo].[Village] VALUES (N'TDP Trạng Trình', 45);
INSERT INTO [dbo].[Village] VALUES (N'TDP Trưng Vương', 45);

INSERT INTO [dbo].[Village] VALUES (N'1', 46);
INSERT INTO [dbo].[Village] VALUES (N'2', 46);
INSERT INTO [dbo].[Village] VALUES (N'3', 46);
INSERT INTO [dbo].[Village] VALUES (N'4', 46);
INSERT INTO [dbo].[Village] VALUES (N'5', 46);
INSERT INTO [dbo].[Village] VALUES (N'6', 46);
INSERT INTO [dbo].[Village] VALUES (N'7', 46);

INSERT INTO [dbo].[Village] VALUES (N'Hồng Hậu', 47);
INSERT INTO [dbo].[Village] VALUES (N'Phố Hàng', 47);
INSERT INTO [dbo].[Village] VALUES (N'Phú Mai', 47);
INSERT INTO [dbo].[Village] VALUES (N'Phú Nhi 1', 47);
INSERT INTO [dbo].[Village] VALUES (N'Phú Nhi 2', 47);
INSERT INTO [dbo].[Village] VALUES (N'Phú Nhi 3', 47);
INSERT INTO [dbo].[Village] VALUES (N'Yên Thịnh', 47);

INSERT INTO [dbo].[Village] VALUES (N'Tổ 1', 48);
INSERT INTO [dbo].[Village] VALUES (N'Tổ 2', 48);
INSERT INTO [dbo].[Village] VALUES (N'Tổ 3', 48);
INSERT INTO [dbo].[Village] VALUES (N'Tổ 4', 48);
INSERT INTO [dbo].[Village] VALUES (N'Tổ 5', 48);
INSERT INTO [dbo].[Village] VALUES (N'Tổ 6', 48);
INSERT INTO [dbo].[Village] VALUES (N'Tổ 7', 48);
INSERT INTO [dbo].[Village] VALUES (N'Tổ 8', 48);
INSERT INTO [dbo].[Village] VALUES (N'Tổ 9', 48);

INSERT INTO [dbo].[Village] VALUES (N'Tổ 1', 49);
INSERT INTO [dbo].[Village] VALUES (N'Tổ 2', 49);
INSERT INTO [dbo].[Village] VALUES (N'Tổ 3', 49);
INSERT INTO [dbo].[Village] VALUES (N'Tổ 4', 49);
INSERT INTO [dbo].[Village] VALUES (N'Tổ 5', 49);
INSERT INTO [dbo].[Village] VALUES (N'Tổ 6', 49);
INSERT INTO [dbo].[Village] VALUES (N'Tổ 7', 49);
INSERT INTO [dbo].[Village] VALUES (N'Tổ 8', 49);

INSERT INTO [dbo].[Village] VALUES (N'TDP 1', 50);
INSERT INTO [dbo].[Village] VALUES (N'TDP 2', 50);
INSERT INTO [dbo].[Village] VALUES (N'TDP 3', 50);
INSERT INTO [dbo].[Village] VALUES (N'TDP 4', 50);
INSERT INTO [dbo].[Village] VALUES (N'TDP 5', 50);
INSERT INTO [dbo].[Village] VALUES (N'TDP 6', 50);
INSERT INTO [dbo].[Village] VALUES (N'TDP 7', 50);
INSERT INTO [dbo].[Village] VALUES (N'TDP 8', 50);
INSERT INTO [dbo].[Village] VALUES (N'TDP 9', 50);

INSERT INTO [dbo].[Village] VALUES (N'Khu phố 1', 51);
INSERT INTO [dbo].[Village] VALUES (N'Khu phố 2', 51);
INSERT INTO [dbo].[Village] VALUES (N'Khu phố 3', 51);
INSERT INTO [dbo].[Village] VALUES (N'Khu phố 4', 51);
INSERT INTO [dbo].[Village] VALUES (N'Khu phố 5', 51);
INSERT INTO [dbo].[Village] VALUES (N'Khu phố 6', 51);
INSERT INTO [dbo].[Village] VALUES (N'Khu phố 7', 51);
INSERT INTO [dbo].[Village] VALUES (N'Khu phố 8', 51);
INSERT INTO [dbo].[Village] VALUES (N'Khu phố 9', 51);

INSERT INTO [dbo].[Village] VALUES (N'TDP La Thành', 52);
INSERT INTO [dbo].[Village] VALUES (N'TDP Thiều Xuân', 52);
INSERT INTO [dbo].[Village] VALUES (N'TDP1 Phù Sa', 52);
INSERT INTO [dbo].[Village] VALUES (N'TDP1 Tiền Huân', 52);
INSERT INTO [dbo].[Village] VALUES (N'TDP2 Phù Xa', 52);
INSERT INTO [dbo].[Village] VALUES (N'TDP2 Tiền Huân', 52);
INSERT INTO [dbo].[Village] VALUES (N'TDP3 Phù Sa', 52);
INSERT INTO [dbo].[Village] VALUES (N'TDP3 Tiền Huân', 52);
INSERT INTO [dbo].[Village] VALUES (N'TDP4 Tiền Huân', 52);

INSERT INTO [dbo].[Village] VALUES (N'TDP 1', 53);
INSERT INTO [dbo].[Village] VALUES (N'TDP 2', 53);
INSERT INTO [dbo].[Village] VALUES (N'TDP 3', 53);
INSERT INTO [dbo].[Village] VALUES (N'TDP 4', 53);
INSERT INTO [dbo].[Village] VALUES (N'TDP 5', 53);
INSERT INTO [dbo].[Village] VALUES (N'TDP 6', 53);
INSERT INTO [dbo].[Village] VALUES (N'TDP 7', 53);
INSERT INTO [dbo].[Village] VALUES (N'TDP 8', 53);

INSERT INTO [dbo].[Village] VALUES (N'Cổ Liễn', 54);
INSERT INTO [dbo].[Village] VALUES (N'Đại Trung', 54);
INSERT INTO [dbo].[Village] VALUES (N'Đoàn Kết', 54);
INSERT INTO [dbo].[Village] VALUES (N'Đồng Trạng', 54);
INSERT INTO [dbo].[Village] VALUES (N'La Gián', 54);
INSERT INTO [dbo].[Village] VALUES (N'Ngõ Bắc', 54);
INSERT INTO [dbo].[Village] VALUES (N'Ngọc Kiên', 54);
INSERT INTO [dbo].[Village] VALUES (N'Phúc Lộc', 54);
INSERT INTO [dbo].[Village] VALUES (N'Thiên Mã', 54);
INSERT INTO [dbo].[Village] VALUES (N'Trại Hồ', 54);
INSERT INTO [dbo].[Village] VALUES (N'Trại Láng', 54);
INSERT INTO [dbo].[Village] VALUES (N'Triều Đông', 54);
INSERT INTO [dbo].[Village] VALUES (N'Trung Lạc', 54);
INSERT INTO [dbo].[Village] VALUES (N'Vĩnh Lộc', 54);

INSERT INTO [dbo].[Village] VALUES (N'Cam Lâm', 55);
INSERT INTO [dbo].[Village] VALUES (N'Cam Thịnh', 55);
INSERT INTO [dbo].[Village] VALUES (N'Đoài Giáp', 55);
INSERT INTO [dbo].[Village] VALUES (N'Đông Sàng', 55);
INSERT INTO [dbo].[Village] VALUES (N'Hà tân', 55);
INSERT INTO [dbo].[Village] VALUES (N'Hưng Thịnh', 55);
INSERT INTO [dbo].[Village] VALUES (N'Mông Phụ', 55);
INSERT INTO [dbo].[Village] VALUES (N'Phụ Khang', 55);
INSERT INTO [dbo].[Village] VALUES (N'Văn Miếu', 55);

INSERT INTO [dbo].[Village] VALUES (N'Kim Chung', 56);
INSERT INTO [dbo].[Village] VALUES (N'Kim Đái 1', 56);
INSERT INTO [dbo].[Village] VALUES (N'Kim Đái 2', 56);
INSERT INTO [dbo].[Village] VALUES (N'Kim Tân', 56);
INSERT INTO [dbo].[Village] VALUES (N'Lòng Hồ', 56);
INSERT INTO [dbo].[Village] VALUES (N'Ngải Sơn', 56);
INSERT INTO [dbo].[Village] VALUES (N'Nhà Thờ', 56);

INSERT INTO [dbo].[Village] VALUES (N'Bắc', 57);
INSERT INTO [dbo].[Village] VALUES (N'Bình Sơn', 57);
INSERT INTO [dbo].[Village] VALUES (N'Bồng', 57);
INSERT INTO [dbo].[Village] VALUES (N'Cao Sơn', 57);
INSERT INTO [dbo].[Village] VALUES (N'Đa AB', 57);
INSERT INTO [dbo].[Village] VALUES (N'Đại Quang', 57);
INSERT INTO [dbo].[Village] VALUES (N'Đậu', 57);
INSERT INTO [dbo].[Village] VALUES (N'Điếm Ba', 57);
INSERT INTO [dbo].[Village] VALUES (N'Đình', 57);
INSERT INTO [dbo].[Village] VALUES (N'Đồi Chợ', 57);
INSERT INTO [dbo].[Village] VALUES (N'Đồi Vua', 57);
INSERT INTO [dbo].[Village] VALUES (N'Đông A', 57);
INSERT INTO [dbo].[Village] VALUES (N'Khoang Sau', 57);
INSERT INTO [dbo].[Village] VALUES (N'Năn', 57);
INSERT INTO [dbo].[Village] VALUES (N'Tân An', 57);
INSERT INTO [dbo].[Village] VALUES (N'Tân Phú', 57);
INSERT INTO [dbo].[Village] VALUES (N'Tân Phúc', 57);
INSERT INTO [dbo].[Village] VALUES (N'Vạn An', 57);

INSERT INTO [dbo].[Village] VALUES (N'400', 58);
INSERT INTO [dbo].[Village] VALUES (N'Đồng Đổi', 58);
INSERT INTO [dbo].[Village] VALUES (N'Phố Vị Thủy', 58);
INSERT INTO [dbo].[Village] VALUES (N'Quảng Đại', 58);
INSERT INTO [dbo].[Village] VALUES (N'Tây Vị', 58);
INSERT INTO [dbo].[Village] VALUES (N'Thanh Tiến', 58);
INSERT INTO [dbo].[Village] VALUES (N'Thanh Vị', 58);
INSERT INTO [dbo].[Village] VALUES (N'Thủ Trung', 58);
INSERT INTO [dbo].[Village] VALUES (N'TDP Vị Thủy', 58);
INSERT INTO [dbo].[Village] VALUES (N'TDP Z155', 58);
INSERT INTO [dbo].[Village] VALUES (N'Yên Mỹ', 58);

INSERT INTO [dbo].[Village] VALUES (N'An Sơn', 59);
INSERT INTO [dbo].[Village] VALUES (N'Kỳ Sơn', 59);
INSERT INTO [dbo].[Village] VALUES (N'Lễ Khê', 59);
INSERT INTO [dbo].[Village] VALUES (N'Nhân Lý', 59);
INSERT INTO [dbo].[Village] VALUES (N'Tam Sơn', 59);
INSERT INTO [dbo].[Village] VALUES (N'Văn Khê', 59);
INSERT INTO [dbo].[Village] VALUES (N'Xuân Khanh', 59);
INSERT INTO [dbo].[Village] VALUES (N'Z 151', 59);
INSERT INTO [dbo].[Village] VALUES (N'Z 175', 59);
INSERT INTO [dbo].[Village] VALUES (N'Xóm Bướm', 59);
INSERT INTO [dbo].[Village] VALUES (N'Xóm Chằm', 59);

-------------------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE [dbo].[House] (
	HouseId int NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	HouseName nvarchar(100),
	Address nvarchar(500),				--địa chỉ cụ thể 
	GoogleMapLocation nvarchar(MAX),	--địa chỉ theo Google Map -> Sử dụng khi hiển thị Map & search khoảng cách
	Information nvarchar(MAX),			--thông tin thêm

	VillageId int,						--thôn/xóm -> phường/xã -> quận/huyện
	LandlordId nchar(30),				--chủ nhà
	CampusId int,						--Campus mà nhà này thuộc về

	--Dành cho những Table CRUD dc -> History
	createdDate datetime,
	updatedDate datetime,
	createdUser nchar(30),
	updatedUser nchar(30),

	CONSTRAINT LandlordId_in_User FOREIGN KEY(LandlordId) REFERENCES [dbo].[User](UserId),
	CONSTRAINT VillageId_in_Village FOREIGN KEY(VillageId) REFERENCES [dbo].[Village](VillageId),
	CONSTRAINT CampusId_in_Campus FOREIGN KEY(CampusId) REFERENCES Campus(CampusId),

	CONSTRAINT createdUser_in_User2 FOREIGN KEY(createdUser) REFERENCES [dbo].[User](UserId),
	CONSTRAINT updatedUser_in_User2 FOREIGN KEY(updatedUser) REFERENCES [dbo].[User](UserId),
) ON [PRIMARY]
GO

INSERT INTO [dbo].[House] VALUES (N'Trọ Tâm Lê', N'Gần Bún bò Huế', N'someStringGeneratedByGoogleMap', N'Rất đẹp', 3, N'LA000001', 1,
'2022-09-28', '2022-09-28', N'LA000001', N'LA000001');

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

	--Dành cho những Table CRUD dc -> History
	createdDate datetime,
	updatedDate datetime,
	createdUser nchar(30),
	updatedUser nchar(30),

	CONSTRAINT StatusId_in_Status FOREIGN KEY(StatusId) REFERENCES [dbo].[Status](StatusId),
	CONSTRAINT RoomTypeId_in_RoomType FOREIGN KEY(RoomTypeId) REFERENCES [dbo].[RoomType](RoomTypeId),
	CONSTRAINT HouseId_in_House FOREIGN KEY(HouseId) REFERENCES [dbo].[House](HouseId),

	CONSTRAINT createdUser_in_User3 FOREIGN KEY(createdUser) REFERENCES [dbo].[User](UserId),
	CONSTRAINT updatedUser_in_User3 FOREIGN KEY(updatedUser) REFERENCES [dbo].[User](UserId),
) ON [PRIMARY]
GO

INSERT INTO [dbo].[Room] VALUES (N'101', 3000000, N'Gạch sàn nhà có họa tiết hình con cá', 5, 2, 1, 1, 1, 1, 2, 1, 
'2022-09-28', '2022-09-28', N'LA000001', N'LA000001');

-------------------------------------------------------------------------------------------------------------------------------------------

--Đánh giá & Comment của 1 người dùng
CREATE TABLE [dbo].[Rate] (
	RateId int NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	Star int,							--Số sao Rate
	Comment nvarchar(MAX),				--Nội dung Comment
	LandlordReply nvarchar(MAX),		--Phản hồi của chủ nhà

	HouseId int,						--Cái nhà dc Comment
	StudentId nchar(30),				--Người viết Comment

	--Dành cho những Table CRUD dc -> History
	createdDate datetime,
	updatedDate datetime,
	createdUser nchar(30),
	updatedUser nchar(30),

	CONSTRAINT HouseId_in_House2 FOREIGN KEY(HouseId) REFERENCES [dbo].[House](HouseId),
	CONSTRAINT StudentId_in_User FOREIGN KEY(StudentId) REFERENCES [dbo].[User](UserId),

	CONSTRAINT createdUser_in_User4 FOREIGN KEY(createdUser) REFERENCES [dbo].[User](UserId),
	CONSTRAINT updatedUser_in_User4 FOREIGN KEY(updatedUser) REFERENCES [dbo].[User](UserId),
) ON [PRIMARY]
GO

INSERT INTO [dbo].[Rate] VALUES (5, N'Rất tuyệt vời, gần trường nữa', N'Cảm ơn bạn', 1, N'HE153046', 
'2022-09-28', '2022-09-28', N'HE153046', N'HE153046');

-------------------------------------------------------------------------------------------------------------------------------------------

--Ảnh nhà
CREATE TABLE [dbo].[ImageOfHouse] (
	ImageId int NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	ImageLink nvarchar(500),

	HouseId int,

	--Dành cho những Table CRUD dc -> History
	createdDate datetime,
	updatedDate datetime,
	createdUser nchar(30),
	updatedUser nchar(30),

	CONSTRAINT HouseId_in_House3 FOREIGN KEY(HouseId) REFERENCES [dbo].[House](HouseId),

	CONSTRAINT createdUser_in_User5 FOREIGN KEY(createdUser) REFERENCES [dbo].[User](UserId),
	CONSTRAINT updatedUser_in_User5 FOREIGN KEY(updatedUser) REFERENCES [dbo].[User](UserId),
) ON [PRIMARY]
GO

INSERT INTO [dbo].[ImageOfHouse] VALUES (N'link_of_image.jpg', 1,
'2022-09-28', '2022-09-28', N'LA000001', N'LA000001');

-------------------------------------------------------------------------------------------------------------------------------------------

--Ảnh phòng
CREATE TABLE [dbo].[ImageOfRoom] (
	ImageId int NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	ImageLink nvarchar(500),

	RoomId int,

	--Dành cho những Table CRUD dc -> History
	createdDate datetime,
	updatedDate datetime,
	createdUser nchar(30),
	updatedUser nchar(30),

	CONSTRAINT RoomId_in_Room FOREIGN KEY(RoomId) REFERENCES [dbo].[Room](RoomId),

	CONSTRAINT createdUser_in_User6 FOREIGN KEY(createdUser) REFERENCES [dbo].[User](UserId),
	CONSTRAINT updatedUser_in_User6 FOREIGN KEY(updatedUser) REFERENCES [dbo].[User](UserId),
) ON [PRIMARY]
GO

INSERT INTO [dbo].[ImageOfRoom] VALUES (N'link_of_image2.jpg', 1,
'2022-09-28', '2022-09-28', N'LA000001', N'LA000001');

-------------------------------------------------------------------------------------------------------------------------------------------

--Report của sinh viên đối với nhà trọ
CREATE TABLE [dbo].[Report] (
	ReportId int NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	ReportContent nvarchar(MAX),

	StudentId nchar(30),
	HouseId int,

	--Dành cho những Table CRUD dc -> History
	createdDate datetime,
	updatedDate datetime,
	createdUser nchar(30),
	updatedUser nchar(30),

	CONSTRAINT HouseId_in_House4 FOREIGN KEY(HouseId) REFERENCES [dbo].[House](HouseId),
	CONSTRAINT StudentId_in_User3 FOREIGN KEY(StudentId) REFERENCES [dbo].[User](UserId),

	CONSTRAINT createdUser_in_User7 FOREIGN KEY(createdUser) REFERENCES [dbo].[User](UserId),
	CONSTRAINT updatedUser_in_User7 FOREIGN KEY(updatedUser) REFERENCES [dbo].[User](UserId),
) ON [PRIMARY]
GO

INSERT INTO [dbo].[Report] VALUES ('Chủ trọ tăng giá phòng trái với hợp đồng', N'HE153046', 1,
'2022-09-28', '2022-09-28', N'HE153046', N'HE153046');