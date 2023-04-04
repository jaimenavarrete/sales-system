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

-- Clients Table
CREATE TABLE Clients (
	Id INT IDENTITY(1,1),
	FirstName VARCHAR(25) NOT NULL,
	LastName VARCHAR(25) NOT NULL,
	Dui CHAR(10) NOT NULL,
	Email VARCHAR(50),
	Phone CHAR(8) NOT NULL,
	Address VARCHAR(200) NOT NULL,
	Debt DECIMAL(7,2) DEFAULT 0.0,
	Created DATETIME NOT NULL,
	CreatedBy NVARCHAR(36) NOT NULL,
	Modified DATETIME,
	ModifiedBy NVARCHAR(36),

	PRIMARY KEY (Id)
);
GO

-- Sale States Table
CREATE TABLE DeliveryStates (
	Id INT IDENTITY(1,1),
	Name NVARCHAR(50) NOT NULL,

	PRIMARY KEY (Id)
);
GO

-- Sales Table
CREATE TABLE Sales (
	Id INT IDENTITY(1,1),
	ClientId INT NOT NULL,
	HomeDelivery BIT NOT NULL,
	DeliveryStateId INT,
	Observation VARCHAR(1000),
	SaleDate DATETIME,
	DeliveryDate DATETIME,
	PaymentCompleted BIT NOT NULL,
	Completed BIT NOT NULL,
	Created DATETIME NOT NULL,
	CreatedBy NVARCHAR(36) NOT NULL,
	Modified DATETIME,
	ModifiedBy NVARCHAR(36),

	PRIMARY KEY (Id),
	FOREIGN KEY (ClientId) REFERENCES Clients(Id),
	FOREIGN KEY (DeliveryStateId) REFERENCES DeliveryStates(Id)
);
GO

-- Sale Details Table
CREATE TABLE SaleDetails (
	Id INT IDENTITY(1,1),
	SaleId INT NOT NULL,
	ProductId INT NOT NULL,
	CurrentPrice DECIMAL(7,2),
	Quantity DECIMAL(7,2),
	Discount DECIMAL(7,2)

	PRIMARY KEY (Id),
	FOREIGN KEY (SaleId) REFERENCES Sales(Id) ON DELETE CASCADE,
	FOREIGN KEY (ProductId) REFERENCES Products(Id)
);
GO