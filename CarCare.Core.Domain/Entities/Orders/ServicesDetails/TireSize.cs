using System.Runtime.Serialization;

namespace CarCare.Core.Domain.Entities.Orders.ServicesDetails
{
    public enum TireSize
    {
        [EnumMember(Value = "Small Cars")]

        SmallCars = 1,
        [EnumMember(Value = "Sedans And Hatchbacks")]

        SedansAndHatchbacks,
        [EnumMember(Value = "Sports Cars")]

        SportsCars,
        [EnumMember(Value = "SUVs And Trucks")]

        SUVsAndTrucks,
    }
}
