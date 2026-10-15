using Application.DTO;
using Domain.Entities;
using MediatR;

namespace Application.UseCases.Drivers.ShowDriver;

public class ShowDriverQuery : IRequest<Result<Driver>>
{
    public Guid Id { get; set; }
}
