using System.Runtime.Serialization;

namespace CarCare.Core.Domain.Entities.Orders.ServicesDetails
{
    public enum TypeOfFuel
    {
        [EnumMember(Value = "P 80")]

        P80 = 1,
        [EnumMember(Value = "P 92")]

        P92,
        Gaz
    }
}
