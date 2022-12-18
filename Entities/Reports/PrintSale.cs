namespace Entities.Reports
{
    public class PrintSale
    {
        public string Code { get; set; }

        public string ProductName { get; set; }

        public string Unit { get; set; }

        public decimal Quantity { get; set; }

        public decimal UnitaryPrice { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
