using System.Runtime.Serialization;

namespace CarCare.Core.Domain.Entities.Orders.ServicesDetails
{
    public enum BettaryType
    {

        Flooded = 1,
        AGM,
        EFB,
        [EnumMember(Value = "Gel Cell")]

        GelCell
    }
}
