namespace DataAccess.Model.Product
{
    public class ProductGetView
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Pack { get; set; }
        public string Size { get; set; }
        public string Codebar { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
