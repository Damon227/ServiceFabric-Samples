/*
Source Server         : kolibresqlserver-dev.database.chinacloudapi.cn
Source Server Version : 120000
Source Host           : kolibresqlserver-dev.database.chinacloudapi.cn:1433
Source Database       : kolibresql-dev
Source Schema         : dbo

Target Server Type    : SQL Server
Target Server Version : 120000
File Encoding         : 65001
*/


-- ----------------------------
-- Table structure for Credit.Kolibre.Identity.RoleClaims
-- ----------------------------
DROP TABLE [dbo].[Credit.Kolibre.Identity.RoleClaims]
GO
CREATE TABLE [dbo].[Credit.Kolibre.Identity.RoleClaims] (
[Id] int NOT NULL IDENTITY(1,1) ,
[RoleId] char(32) NOT NULL ,
[ClaimType] nvarchar(MAX) NULL ,
[ClaimValue] nvarchar(MAX) NULL ,
PRIMARY KEY ([Id])
)


GO

-- ----------------------------
-- Indexes structure for table Credit.Kolibre.Identity.RoleClaims
-- ----------------------------
CREATE INDEX [IN_dbo.Credit.Kolibre.Identity.RoleClaims_RoleId] ON [dbo].[Credit.Kolibre.Identity.RoleClaims]
([RoleId] ASC) 
GO



-- ----------------------------
-- Table structure for Credit.Kolibre.Identity.UserClaims
-- ----------------------------
DROP TABLE [dbo].[Credit.Kolibre.Identity.UserClaims]
GO
CREATE TABLE [dbo].[Credit.Kolibre.Identity.UserClaims] (
[Id] int NOT NULL IDENTITY(1,1) ,
[UserId] char(32) NOT NULL ,
[ClaimType] nvarchar(MAX) NULL ,
[ClaimValue] nvarchar(MAX) NULL ,
PRIMARY KEY ([Id])
)


GO

-- ----------------------------
-- Indexes structure for table Credit.Kolibre.Identity.UserClaims
-- ----------------------------
CREATE INDEX [IN_dbo.Credit.Kolibre.Identity.UserClaims_UserId] ON [dbo].[Credit.Kolibre.Identity.UserClaims]
([UserId] ASC) 
GO



-- ----------------------------
-- Table structure for Credit.Kolibre.Identity.UserRoles
-- ----------------------------
DROP TABLE [dbo].[Credit.Kolibre.Identity.UserRoles]
GO
CREATE TABLE [dbo].[Credit.Kolibre.Identity.UserRoles] (
[Id] int NOT NULL IDENTITY(1,1) ,
[UserId] char(32) NOT NULL ,
[RoleId] char(32) NOT NULL ,
PRIMARY KEY ([UserId], [RoleId])
)


GO

-- ----------------------------
-- Indexes structure for table Credit.Kolibre.Identity.UserRoles
-- ----------------------------
CREATE INDEX [IN_dbo.Credit.Kolibre.Identity.UserRoles_UserId_RoleId] ON [dbo].[Credit.Kolibre.Identity.UserRoles]
([UserId] ASC, [RoleId] ASC) 
GO
CREATE INDEX [IN_dbo.Credit.Kolibre.Identity.UserRoles_UserId] ON [dbo].[Credit.Kolibre.Identity.UserRoles]
([UserId] ASC) 
GO
CREATE INDEX [IN_dbo.Credit.Kolibre.Identity.UserRoles_RoleId] ON [dbo].[Credit.Kolibre.Identity.UserRoles]
([RoleId] ASC) 
GO


-- ----------------------------
-- Table structure for Credit.Kolibre.Identity.Roles
-- ----------------------------
DROP TABLE [dbo].[Credit.Kolibre.Identity.Roles]
GO
CREATE TABLE [dbo].[Credit.Kolibre.Identity.Roles] (
[Id] int NOT NULL IDENTITY(1,1) ,
[RoleId] char(32) NOT NULL ,
[Name] nvarchar(255) NOT NULL ,
[NormalizedName] nvarchar(255) NOT NULL ,
[ConcurrencyStamp] nvarchar(255) NOT NULL ,
PRIMARY KEY ([Id])
)


GO

-- ----------------------------
-- Indexes structure for table Credit.Kolibre.Identity.Roles
-- ----------------------------
CREATE UNIQUE INDEX [UQ_dbo.Credit.Kolibre.Identity.Roles_RoleId] ON [dbo].[Credit.Kolibre.Identity.Roles]
([RoleId] ASC) 
WITH (IGNORE_DUP_KEY = ON)
GO
CREATE UNIQUE INDEX [UQ_dbo.Credit.Kolibre.Identity.Roles_NormalizedName] ON [dbo].[Credit.Kolibre.Identity.Roles]
([NormalizedName] ASC)
WITH (IGNORE_DUP_KEY = ON)
GO
CREATE INDEX [IN_dbo.Credit.Kolibre.Identity.Roles_Name] ON [dbo].[Credit.Kolibre.Identity.Roles]
([Name] ASC) 
GO




-- ----------------------------
-- Table structure for Credit.Kolibre.Identity.Users
-- ----------------------------
DROP TABLE [dbo].[Credit.Kolibre.Identity.Users]
GO
CREATE TABLE [dbo].[Credit.Kolibre.Identity.Users] (
[Id] int NOT NULL IDENTITY(1,1) ,
[UserId] char(32) NOT NULL ,
[UserName] nvarchar(255) NOT NULL ,
[NormalizedUserName] nvarchar(255) NOT NULL ,
[Email] nvarchar(255) NULL ,
[NormalizedEmail] nvarchar(255) NULL ,
[EmailConfirmed] bit NOT NULL ,
[PasswordHash] nvarchar(MAX) NOT NULL ,
[Cellphone] nvarchar(255) NULL ,
[CellphoneConfirmed] bit NOT NULL ,
[LockoutEnd] datetimeoffset(7) NULL ,
[LockoutEnabled] bit NOT NULL ,
[AccessFailedCount] int NOT NULL ,
[ConcurrencyStamp] nvarchar(255) NOT NULL ,
PRIMARY KEY ([Id])
)


GO

-- ----------------------------
-- Indexes structure for table Credit.Kolibre.Identity.Users
-- ----------------------------
CREATE UNIQUE INDEX [UQ_dbo.Credit.Kolibre.Identity.Users_UserId] ON [dbo].[Credit.Kolibre.Identity.Users]
([UserId] ASC) 
WITH (IGNORE_DUP_KEY = ON)
GO
CREATE UNIQUE INDEX [UQ_dbo.Credit.Kolibre.Identity.Users_NormalizedUserName] ON [dbo].[Credit.Kolibre.Identity.Users]
([NormalizedUserName] ASC) 
WITH (IGNORE_DUP_KEY = ON)
GO
CREATE INDEX [IN_dbo.Credit.Kolibre.Identity.Users_Email] ON [dbo].[Credit.Kolibre.Identity.Users]
([Email] ASC) 
GO
CREATE INDEX [IN_dbo.Credit.Kolibre.Identity.Users_NormalizedEmail] ON [dbo].[Credit.Kolibre.Identity.Users]
([NormalizedEmail] ASC) 
GO
CREATE INDEX [IN_dbo.Credit.Kolibre.Identity.Users_Cellphone] ON [dbo].[Credit.Kolibre.Identity.Users]
([Cellphone] ASC) 
GO



-- ----------------------------
-- Foreign Key structure for table [dbo].[Credit.Kolibre.Identity.RoleClaims]
-- ----------------------------
ALTER TABLE [dbo].[Credit.Kolibre.Identity.RoleClaims] ADD CONSTRAINT [FK_dbo.Credit.Kolibre.Identity.RoleClaims_dbo.Credit.Kolibre.Identity.Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Credit.Kolibre.Identity.Roles] ([RoleId]) ON DELETE CASCADE ON UPDATE NO ACTION
GO

-- ----------------------------
-- Foreign Key structure for table [dbo].[Credit.Kolibre.Identity.UserClaims]
-- ----------------------------
ALTER TABLE [dbo].[Credit.Kolibre.Identity.UserClaims] ADD CONSTRAINT [FK_dbo.Credit.Kolibre.Identity.UserClaims_dbo.Credit.Kolibre.Identity.Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Credit.Kolibre.Identity.Users] ([UserId]) ON DELETE CASCADE ON UPDATE NO ACTION
GO

-- ----------------------------
-- Foreign Key structure for table [dbo].[Credit.Kolibre.Identity.UserRoles]
-- ----------------------------
ALTER TABLE [dbo].[Credit.Kolibre.Identity.UserRoles] ADD CONSTRAINT [FK_dbo.Credit.Kolibre.Identity.UserRoles_dbo.Credit.Kolibre.Identity.Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Credit.Kolibre.Identity.Roles] ([RoleId]) ON DELETE CASCADE ON UPDATE NO ACTION
GO
ALTER TABLE [dbo].[Credit.Kolibre.Identity.UserRoles] ADD CONSTRAINT [FK_dbo.Credit.Kolibre.Identity.UserRoles_dbo.Credit.Kolibre.Identity.Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Credit.Kolibre.Identity.Users] ([UserId]) ON DELETE CASCADE ON UPDATE NO ACTION
GO
