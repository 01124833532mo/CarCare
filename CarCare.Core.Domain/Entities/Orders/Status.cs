using System.Runtime.Serialization;

namespace CarCare.Core.Domain.Entities.Orders
{
    public enum Status
    {
        Pending = 1,
        InProgress,
        Completed,
        Canceled,
        [EnumMember(Value = "Payment Received")]
        PaymentReceived,
        [EnumMember(Value = "Payment Failed")]
        PaymentFailed,
    }
}
