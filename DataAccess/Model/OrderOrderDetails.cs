using System;

namespace DataAccess.Model
{
    public class OrderOrderDetails
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int StatusId { get; set; }
        public int StockId { get; set; }
        public string ClientName { get; set; }
        public int OrderDetailId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitaryPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
