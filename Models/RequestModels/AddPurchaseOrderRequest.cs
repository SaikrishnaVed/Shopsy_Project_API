namespace Shopsy_Project.Models.RequestModels
{
    public class AddPurchaseOrderRequest
    {
        public int userId { get; set; }
        public DateTime? DateCreated { get; set; } = DateTime.UtcNow;
    }
}