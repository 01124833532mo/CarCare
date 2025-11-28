using System.Runtime.Serialization;

namespace CarCare.Core.Domain.Entities.Orders.ServicesDetails
{
    public enum TypeOfOil
    {
        [EnumMember(Value = "Engine Oil")]
        EngineOil = 1,

        [EnumMember(Value = "Synthetic Oil")]
        SyntheticOil,

        [EnumMember(Value = "Synthetic Blend Oil")]
        SyntheticBlendOil,

        [EnumMember(Value = "High Mileage Oil")]
        HighMileageOil
    }
}
