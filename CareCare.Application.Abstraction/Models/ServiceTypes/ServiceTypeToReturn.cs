namespace CareCare.Core.Application.Abstraction.Models.ServiceTypes
{
    public class ServiceTypeToReturn
    {
        public required int Id { get; set; }

        public required string Name { get; set; }


        public required string Description { get; set; }

        //public required decimal Price { get; set; }

        public string? PictureUrl { get; set; }


    }
}
