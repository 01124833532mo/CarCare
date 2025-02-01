namespace CareCare.Core.Application.Abstraction.Models.FeedBack
{
    public class ReturnFeedBackDto
    {
        public DateTime Date { get; set; }

        public string? Comment { get; set; }
        public int Id { get; set; }

        public decimal Rating { get; set; }

        public required string UserId { get; set; }
    }
}
