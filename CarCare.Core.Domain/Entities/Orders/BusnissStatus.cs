using System.Runtime.Serialization;

namespace CarCare.Core.Domain.Entities.Orders
{
    public enum BusnissStatus
    {
        Pending = 1,
        [EnumMember(Value = "In Progress")]

        InProgress,
        Completed,
        Canceled,
    }
}
