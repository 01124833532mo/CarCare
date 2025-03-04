using System.Runtime.Serialization;

namespace CarCare.Core.Domain.Entities.Orders
{
    public enum PaymentStatus
    {



        [EnumMember(Value = "Pending")]
        Pending,
        [EnumMember(Value = "Payment Received")]

        PaymentReceived,
        [EnumMember(Value = "Payment Failed")]

        PaymentFailed,
    }
}
