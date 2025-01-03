namespace Shopsy_Project.Models
{
    public class Conversation
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserMessage { get; set; }
        public string BotResponse { get; set; }
        public DateTime Timestamp { get; set; }
    }
}