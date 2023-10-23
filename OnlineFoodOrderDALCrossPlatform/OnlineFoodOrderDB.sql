USE [master]
GO

IF (EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE ('[' + name + ']' = N'OnlineFoodOrderDB'OR name = N'OnlineFoodOrderDB')))
DROP DATABASE OnlineFoodOrderDB
GO

CREATE DATABASE OnlineFoodOrderDB
GO

USE OnlineFoodOrderDB
GO

-- DROP Scripts
-------------------------------------------------
IF OBJECT_ID('Category') IS NOT NULL
	DROP TABLE Category
GO 

IF OBJECT_ID('Items') IS NOT NULL
	DROP TABLE Items
GO

IF OBJECT_ID('Customers') IS NOT NULL
	DROP TABLE Customers
GO

IF OBJECT_ID('Orders') IS NOT NULL
	DROP TABLE Orders
GO

IF OBJECT_ID('usp_AddOrderDetails') IS NOT NULL
	DROP PROCEDURE usp_AddOrderDetails
GO

IF OBJECT_ID('ufn_FetchItemPrice') IS NOT NULL
	DROP FUNCTION ufn_FetchItemPrice
GO

IF OBJECT_ID ('ufn_GetAllOrderDetails') IS NOT NULL
	DROP FUNCTION ufn_GetAllOrderDetails
GO

IF OBJECT_ID ('ufn_FetchItemDetails') IS NOT NULL
	DROP FUNCTION ufn_FetchItemDetails
GO

IF OBJECT_ID ('ufn_CheckDeliveryStatus') IS NOT NULL
	DROP FUNCTION ufn_CheckDeliveryStatus
GO

IF OBJECT_ID ('ufn_GetOrderDetails') IS NOT NULL
	DROP FUNCTION ufn_GetOrderDetails
GO

--CREATE SCRIPTS FOR Category TABLE
--------------------------------------------------
CREATE TABLE Category
(
	[CategoryId] INT CONSTRAINT [pk_CategoryId] PRIMARY KEY IDENTITY,
	[CategoryName] VARCHAR(50) NOT NULL
)
GO

-- INSERTION SCRIPTS FOR Category TABLE
----------------------------------------------------------------
INSERT INTO Category VALUES ('Pizza');
INSERT INTO Category VALUES ('Burger');
GO

-- SELECT Query
---------------
SELECT * FROM Category
GO


--CREATE SCRIPTS FOR Items TABLE
--------------------------------------------------
CREATE TABLE Items
(
	[ItemId] CHAR(3) CONSTRAINT [pk_ItemId] PRIMARY KEY,
	[ItemName] VARCHAR(50) NOT NULL,
	[CategoryId] INT CONSTRAINT [fk_CategoryId] REFERENCES Category(CategoryId) NOT NULL,
	[Price] MONEY CONSTRAINT [chk_Price] CHECK ([Price] > 0) NOT NULL,
)
GO

-- INSERTION SCRIPTS FOR Items TABLE
----------------------------------------------------------------
INSERT INTO Items VALUES ('MAR', 'Margherita', 1, 85);
INSERT INTO Items VALUES ('VDL', 'Veg Delight', 1, 145);
INSERT INTO Items VALUES ('CFA', 'Chicken Fiesta', 1, 195);
INSERT INTO Items VALUES ('CBR', 'Cheese Burger', 2, 79);
INSERT INTO Items VALUES ('CRS', 'Chicken Rockings', 2, 99);
INSERT INTO Items VALUES ('ZPS', 'Zinger Plus', 2, 119);
GO

-- SELECT Query 
---------------
SELECT * FROM Items
GO

--CREATE SCRIPTS FOR Customers TABLE
--------------------------------------------------
CREATE TABLE Customers
(	
	[CustomerId] INT CONSTRAINT [pk_CustomerId] PRIMARY KEY IDENTITY(1001, 1),
	[CustomerName] VARCHAR(50) NOT NULL,	
	[Address] VARCHAR(50) NOT NULL,
	[EmailId] VARCHAR(50) CONSTRAINT [chk_EmailId] CHECK ([EmailId] LIKE '%_@__%.__%') NOT NULL,
	[PhoneNumber] VARCHAR(10) NOT NULL
)
GO

--INSERTION SCRIPTS FOR Customers TABLE
----------------------------------------------------------------
INSERT INTO Customers VALUES ('Albert', 'No.123, 14th Cross, Mysore', 'Albert@gmail.com', 9876543210);
INSERT INTO Customers VALUES ('Rosie', 'No. 11, Street. No. 8, Bangalore', 'Rosie@gmail.com', 8765432109);
INSERT INTO Customers VALUES ('Sam', 'No. 43, Block No. 10, Kolkata', 'Sam@gmail.com', 7654321098);
INSERT INTO Customers VALUES ('Rani', 'No. 78, 12th Main, Chennai', 'Rani@gmail.com', 6543210987);
GO

-- SELECT Query
---------------
SELECT * FROM Customers
GO

--CREATE SCRIPTS FOR Orders TABLE
--------------------------------------------------
CREATE TABLE Orders
(
	[OrderId] INT CONSTRAINT [pk_OrderId] PRIMARY KEY IDENTITY,
	[CustomerId] INT CONSTRAINT [fk_CustomerId] REFERENCES Customers (CustomerId) NOT NULL,
	[ItemId] CHAR(3) CONSTRAINT [fk_ItemId] REFERENCES Items(ItemId) NOT NULL,
	[Quantity] INT CONSTRAINT [chk_Quantity] CHECK ([Quantity] > 0) NOT NULL,	
	[TotalPrice] MONEY CONSTRAINT [chk_TotalPrice] CHECK ([TotalPrice] > 0) NOT NULL,
	[DeliveryAddress] VARCHAR(50) NOT NULL,
	[OrderDate] DATE DEFAULT GETDATE() CONSTRAINT [chk_OrderDate] CHECK([OrderDate] >= CONVERT(DATE,GETDATE())) NOT NULL,
	[DeliveryStatus] CHAR(3) DEFAULT 'NDL' CONSTRAINT [chk_DeliveryStatus] CHECK ([DeliveryStatus]='DL' OR [DeliveryStatus]='NDL') NOT NULL
)
GO

SELECT 1 WHERE '2023-08-17' <= CONVERT(DATE,GETDATE())

--INSERTION SCRIPTS FOR Orders TABLE
----------------------------------------------------------------
INSERT INTO Orders VALUES (1004, 'MAR', 3, 255.00, 'Infy, Mysore. 8102121254', CONVERT(DATE, GETDATE()), 'DL');
INSERT INTO Orders VALUES (1003, 'CFA', 6, 1170.00, 'No. 350, Hebbal. 9447123254', DATEADD(d, 5, CONVERT(DATE, GETDATE())), 'NDL');
INSERT INTO Orders VALUES (1002, 'VDL', 5, 725.00, 'Kavery Circle. 8654485425', CONVERT(DATE, GETDATE()), 'NDL');
INSERT INTO Orders VALUES (1001, 'CRS', 2, 198.00, 'Water Tank Jn. 9000547893', DATEADD(d, 10, CONVERT(DATE, GETDATE())), 'NDL');
INSERT INTO Orders VALUES (1001, 'CRS', 2, 198.00, 'Water Tank Jn. 9000547893', DATEADD(d, 10, CONVERT(DATE, GETDATE())), 'NDL');
INSERT INTO Orders VALUES (1001, 'CRS', 2, 198.00, 'Water Tank Jn. 9000547893', DATEADD(d, 10, CONVERT(DATE, GETDATE())), 'NDL');
GO

-- SELECT Query
---------------
SELECT * FROM Orders
GO

--STORED PROCEDURE - usp_AddOrderDetails
--------------------------------------------------------------------
CREATE PROCEDURE usp_AddOrderDetails
(	
	@CustomerId INT,
	@ItemId CHAR(3),
	@Quantity INT,
	@DeliveryAddress VARCHAR(50),
	@OrderDate DATE,
	@TotalPrice MONEY OUT,
	@OrderId INT OUT
)
AS 
BEGIN
	DECLARE @ReturnValue INT, @Price MONEY
	BEGIN TRY
		IF NOT EXISTS (SELECT CustomerId FROM Customers WHERE CustomerId = @CustomerId)
			SET @ReturnValue = -1
		ELSE IF NOT EXISTS (SELECT ItemId FROM Items WHERE ItemId = @ItemId)
			SET @ReturnValue = -2
		ELSE IF (@Quantity <= 0)
			SET @ReturnValue = -3
		ELSE IF (@DeliveryAddress IS NULL)
			SET @ReturnValue = -4
		ELSE IF (@OrderDate < CONVERT(DATE, GETDATE()))
			SET @ReturnValue = -5
		ELSE 
		BEGIN
			SELECT @Price = Price FROM Items WHERE ItemId = @ItemId
			SELECT @TotalPrice = @Quantity * @Price
			INSERT INTO Orders VALUES (@CustomerId, @ItemId, @Quantity, 
										@TotalPrice, @DeliveryAddress, 
										@OrderDate, DEFAULT)
			SET @OrderId = IDENT_CURRENT('Orders')
			SET @ReturnValue = 1
		END
		RETURN @ReturnValue
	END TRY
	BEGIN CATCH
		SET @ReturnValue = -99
		RETURN @ReturnValue
	END CATCH
END
GO

DECLARE @RetVal INT, @OrderId INT, @TotalPrice MONEY, @Date DATETIME = GETDATE()
EXEC @RetVal = usp_AddOrderDetails 1004, 'CFA', 4, 'No.Infy. 9447123254', 
									@Date, @TotalPrice OUT, @OrderId OUT
SELECT @RetVal AS Result, @OrderId AS OrderId, @TotalPrice AS TotalPrice
GO


--TABLE VALUED FUNCTION - ufn_GetAllOrderDetails
--------------------------------------------------------------------
CREATE FUNCTION ufn_GetAllOrderDetails(@CategoryId INT)
RETURNS TABLE
AS 
	RETURN (SELECT OrderId, o.CustomerId, CT.CustomerName, I.ItemId, I.ItemName, 
					TotalPrice, DeliveryAddress, OrderDate, DeliveryStatus 
			FROM Orders O INNER JOIN Items I 
			ON O.ItemId = I.ItemId 
			INNER JOIN Category C 
			ON I.CategoryId = C.CategoryId 
			INNER JOIN Customers CT
			ON O.CustomerId = CT.CustomerId
			WHERE C.CategoryId = @CategoryId)
GO

SELECT * FROM ufn_GetAllOrderDetails(1)
GO

--TABLE VALUED FUNCTION - ufn_FetchItemDetails
--------------------------------------------------------------------
CREATE FUNCTION ufn_FetchItemDetails(@CategoryName VARCHAR(50))
RETURNS TABLE
AS 
	RETURN (SELECT ItemId, ItemName, Price 
			FROM Items I INNER JOIN Category C 
			ON I.CategoryId = C.CategoryId 
			WHERE C.CategoryName = @CategoryName)
GO

SELECT * FROM ufn_FetchItemDetails('Pizza')
GO

--TABLE VALUED FUNCTION - ufn_GetOrderDetails
--------------------------------------------------------------------
CREATE FUNCTION ufn_GetOrderDetails(@OrderId INT)
RETURNS TABLE
AS 
		RETURN (SELECT OrderId, O.CustomerId, CT.CustomerName, I.ItemName, TotalPrice,
						DeliveryAddress, OrderDate, DeliveryStatus 
				FROM Orders O INNER JOIN Items I 
				ON O.ItemId = I.ItemId 
				INNER JOIN Category C 
				ON I.CategoryId = C.CategoryId 
				INNER JOIN Customers CT
				ON O.CustomerId = CT.CustomerId
				WHERE O.OrderId = @OrderId)
GO

SELECT * FROM ufn_GetOrderDetails(2)
GO

--SCALAR VALUED FUNCTION - ufn_FetchItemPrice
--------------------------------------------------------------------
CREATE FUNCTION ufn_FetchItemPrice
(
	@ItemID CHAR(3)
)
RETURNS MONEY
AS
BEGIN
	DECLARE @ItemPrice MONEY
	SET @ItemPrice = (SELECT Price 
						FROM Items 
						WHERE ItemID = @ItemID)
	RETURN @ItemPrice
END
GO

SELECT [dbo].ufn_FetchItemPrice('CFA') AS ItemPrice
GO

--SCALAR VALUED FUNCTION - ufn_CheckDeliveryStatus
--------------------------------------------------------------------
CREATE FUNCTION ufn_CheckDeliveryStatus
(
	@OrderId INT
)
RETURNS INT
AS
BEGIN
	DECLARE @ReturnValue INT
	SET @ReturnValue = -1
	IF NOT EXISTS (SELECT OrderId 
				FROM Orders 
				WHERE OrderId = @OrderId)
		SET @ReturnValue = -1
	ELSE IF EXISTS (SELECT OrderId 
				FROM Orders 
				WHERE OrderId = @OrderId AND DeliveryStatus = 'DL')
		SET @ReturnValue = 0
	ELSE IF EXISTS (SELECT OrderId 
				FROM Orders 
				WHERE OrderId = @OrderId AND DeliveryStatus = 'NDL')
		SET @ReturnValue = 1
	RETURN @ReturnValue
END
GO

SELECT [dbo].ufn_CheckDeliveryStatus(1) AS RESULT
GO
--------------------------------------------------------------------------
