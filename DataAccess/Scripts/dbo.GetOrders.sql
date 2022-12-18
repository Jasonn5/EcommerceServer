USE EcommerceSystem
GO

IF OBJECT_ID('dbo.GetOrders', 'P') IS NOT NULL
	DROP PROCEDURE dbo.GetOrders
GO

CREATE PROCEDURE dbo.GetOrders
@StartDate DATETIME,
@EndDate DATETIME
AS
	SELECT ord.Id, ord.Code, ord.OrderDate, ord.StatusId, 
	od.Id AS OrderDetailId, od.StockId, od.Quantity, od.UnitaryPrice, od.TotalPrice
	FROM Orders ord
	LEFT JOIN OrderDetails od ON od.OrderId = ord.Id
	WHERE ord.OrderDate >= @StartDate and ord.OrderDate <= @EndDate
GO
