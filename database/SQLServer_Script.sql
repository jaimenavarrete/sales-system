USE master;
GO

CREATE DATABASE SalesSystem;
GO

USE SalesSystem;
GO

-- Categories Table
CREATE TABLE Categories (
	Id INT IDENTITY(1,1),
	Name NVARCHAR(50) NOT NULL,
	Description NVARCHAR(250),
	Created DATETIME NOT NULL,
	CreatedBy NVARCHAR(36) NOT NULL,
	Modified DATETIME,
	ModifiedBy NVARCHAR(36),
	
	PRIMARY KEY (Id)
);
GO

-- Unit Types Table
CREATE TABLE UnitTypes (
	Id INT IDENTITY(1,1),
	Name NVARCHAR(50) NOT NULL,
	Description NVARCHAR(250),
	Created DATETIME NOT NULL,
	CreatedBy NVARCHAR(36) NOT NULL,
	Modified DATETIME,
	ModifiedBy NVARCHAR(36),
	
	PRIMARY KEY (Id)
);
GO

-- Products Table
CREATE TABLE Products (
	Id INT IDENTITY(1,1),
	Name NVARCHAR(50) NOT NULL,
	Description NVARCHAR(1000),
	Stock DECIMAL(7,2) NOT NULL,
	Price DECIMAL(7,2) NOT NULL,
	UnitTypeId INT NOT NULL,
	CategoryId INT NOT NULL,
	PhotoUrl NVARCHAR(100),
	Created DATETIME NOT NULL,
	CreatedBy NVARCHAR(36) NOT NULL,
	Modified DATETIME,
	ModifiedBy NVARCHAR(36),

	PRIMARY KEY (Id),
	FOREIGN KEY (CategoryId) REFERENCES Categories (Id),
	FOREIGN KEY (UnitTypeId) REFERENCES UnitTypes(Id)
);
GO