using Application.DTO;
using Domain.Entities;
using MediatR;

namespace Application.UseCases.Drivers.ListDriver;

public class ListDriverQuery : IRequest<Result<List<Driver>>>
{
    public int Page { get; set; } = 1;
    public int Take { get; set; } = 10;
}
