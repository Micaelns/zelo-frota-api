using Application.DTO;
using Domain.ObjectValues;
using MediatR;

namespace Application.UseCases.Drivers.CreateDriver;

public class CreateDriverCommand : IRequest<Result<Guid>>
{
    public string Name { get; set; } = string.Empty;
    public string? Description {  get; set; }
    public required DriverLicense DriverLicense { get; set; }
}
