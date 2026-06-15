using Application.DTO;
using Application.DTO.Travel;
using Domain.Entities;
using MediatR;

namespace Application.UseCases.Travels.ShowTravel;

public class ShowTravelQuery : IRequest<Result<TravelDTO>>
{
    public Guid Id { get; set; }
}
