namespace CareCare.Core.Application.Abstraction.Models.Contacts
{
    public class ReturnContactDto
    {
        public int Id { get; set; }
        public required string Message { get; set; }
        public string CreatedBy { get; set; } = null!;

        public DateTime CreatedOn { get; set; }

        public string LastModifiedBy { get; set; } = null!;

        public DateTime LastModifiedOn { get; set; }
        public required string UserId { get; set; }
        public required string FullName { get; set; }
    }
}
