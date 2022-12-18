USE EcommerceSystem
GO

IF OBJECT_ID('dbo.GetSales', 'P') IS NOT NULL
	DROP PROCEDURE dbo.GetSales
GO

CREATE PROCEDURE dbo.GetSales
@StartDate DATETIME,
@EndDate DATETIME,
@Search NVARCHAR(MAX)
AS
	SELECT sa.Id, sa.Code, sa.[Date] AS SaleDate, sa.[Description], sa.StatusId, 
	sd.Id AS SaleDetailId, sd.TotalPrice
	FROM Sales sa
	LEFT JOIN SaleDetails sd ON sd.SaleId = sa.Id
	WHERE (sa.[Date] BETWEEN @StartDate and @EndDate)
	AND (sa.Code like '%' + @Search + '%')
GO