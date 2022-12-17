using System;

namespace DataAccess.Model
{
    public class SaleSaleDetailsCreditClientSeller
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public DateTime SaleDate { get; set; }
        public string Description { get; set; }
        public int StatusId { get; set; }
        public int SaleDetailId { get; set; }
        public int StockId { get; set; }
        public string ProductName { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitaryPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
