USE EcommerceSystem
GO

IF OBJECT_ID('dbo.GetProducts', 'P') IS NOT NULL
	DROP PROCEDURE dbo.GetProducts
GO

CREATE PROCEDURE dbo.GetProducts
@Search NVARCHAR(MAX)
AS
	SELECT p.Id, p.[Name] AS ProductName, p.[Description], p.Price
	FROM Products p
	WHERE (p.[Name] like '%' + @Search + '%')
GO