namespace Shopsy_Project.Models.RequestModels
{
    public class UpdateUserRequest
    {
        public int userId { get; set; }
        public string role { get; set; }
        public bool isActive { get; set; }
    }
}