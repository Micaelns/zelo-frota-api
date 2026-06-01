using Application.DTO;
using MediatR;

namespace Application.UseCases.Destinations.CreateDestination
{
    public class CreateDestinationCommand : IRequest<Result<Guid>>
    {
        public string ZipCode { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? Neighborhood { get; set; }
        public string? Locality { get; set; }
        public required string City { get; set; } = string.Empty;
        public required string Uf { get; set; } = string.Empty;
    }
}
