namespace Shopsy_Project.Models.RequestModels
{
    public class AddBulkPurchaseOrderRequest
    {
        public int Id { get; set; }
        public int Product_Id { get; set; }
        public int Quantity { get; set; }
        public int userId { get; set; }
        public decimal price { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    }
}