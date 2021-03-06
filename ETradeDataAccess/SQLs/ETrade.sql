delete from Products
delete from Categories
delete from Users
delete from Roles

SET IDENTITY_INSERT [dbo].[Categories] ON 

INSERT [dbo].[Categories] ([Id], [Name]) VALUES (1, N'Computer')
INSERT [dbo].[Categories] ([Id], [Name]) VALUES (2, N'Home Entertainment')
SET IDENTITY_INSERT [dbo].[Categories] OFF
GO
SET IDENTITY_INSERT [dbo].[Products] ON 

INSERT [dbo].[Products] ([Id], [Name], [UnitPrice], [StockAmount], [CategoryId], CreateDate, UpdateDate, IsDeleted) VALUES (1, N'Laptop', 3000.0000, 10, 1, GETDATE(), null, 0)
INSERT [dbo].[Products] ([Id], [Name], [UnitPrice], [StockAmount], [CategoryId], CreateDate, UpdateDate, IsDeleted) VALUES (2, N'Mouse', 20.0000, 20, 1, GETDATE(), null, 0)
INSERT [dbo].[Products] ([Id], [Name], [UnitPrice], [StockAmount], [CategoryId], CreateDate, UpdateDate, IsDeleted) VALUES (3, N'Keyboard', 40.0000, 21, 1, GETDATE(), null, 0)
INSERT [dbo].[Products] ([Id], [Name], [UnitPrice], [StockAmount], [CategoryId], CreateDate, UpdateDate, IsDeleted) VALUES (18, N'Speaker', 2500.0000, 5, 2, GETDATE(), null, 0)
INSERT [dbo].[Products] ([Id], [Name], [UnitPrice], [StockAmount], [CategoryId], CreateDate, UpdateDate, IsDeleted) VALUES (19, N'Receiver', 5000.0000, 9, 2, GETDATE(), null, 0)
INSERT [dbo].[Products] ([Id], [Name], [UnitPrice], [StockAmount], [CategoryId], CreateDate, UpdateDate, IsDeleted) VALUES (23, N'Monitor', 2500.0000, 27, 1, GETDATE(), null, 0)
INSERT [dbo].[Products] ([Id], [Name], [UnitPrice], [StockAmount], [CategoryId], CreateDate, UpdateDate, IsDeleted) VALUES (24, N'Equalizer', 1000.0000, 11, 2, GETDATE(), null, 0)
SET IDENTITY_INSERT [dbo].[Products] OFF
GO

set identity_insert Roles on
insert into roles (Id, Name) values (1, 'Admin')
insert into roles (Id, Name) values (2, 'User')
set identity_insert Roles off
go
set identity_insert Users on
insert into users (Id, UserName, Password, EMail, BirthDay, Active, RoleId) values (1, 'cagil', 'cagil', 'cagil@alsac.com', '1980-05-05', 1, 1)
insert into users (Id, UserName, Password, EMail, BirthDay, Active, RoleId) values (2, 'leo', 'leo', 'leo@alsac.com', '2014-05-06', 1, 2)
set identity_insert Users off
go