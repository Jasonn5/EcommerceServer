USE EcommerceSystem
GO

IF OBJECT_ID('dbo.GetSale', 'P') IS NOT NULL
	DROP PROCEDURE dbo.GetSale
GO

CREATE PROCEDURE dbo.GetSale
@SaleId INT
AS
	SELECT sa.Id, sa.Code, sa.[Date] AS SaleDate, sa.[Description], sa.StatusId, 
	sd.Id AS SaleDetailId, sd.StockId, sd.ProductName,sd.Quantity, sd.UnitaryPrice, sd.TotalPrice
	FROM Sales sa
	LEFT JOIN SaleDetails sd ON sd.SaleId = sa.Id
	LEFT JOIN Stocks s ON sd.StockId = s.Id
	WHERE sa.Id = @SaleId
GO