namespace CarCare.Core.Domain.Contracts.Common
{
    public interface IBaseAuditableEntity
    {
        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public string LastModifiedBy { get; set; }


        public DateTime LastModifiedOn { get; set; }
    }
}
