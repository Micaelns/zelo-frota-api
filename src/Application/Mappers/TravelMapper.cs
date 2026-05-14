using Application.DTO.Travel;
using Application.Helpers;
using Domain.Entities;

namespace Application.Mappers;

public class TravelMapper
{
    public static TravelDTO ToTravelDTO(Travel travel)
    {
        return new TravelDTO()
        {
            IdTravel = travel.Id,
            VehiclePlate = travel.Vehicle.Plate,
            VehicleId = travel.Vehicle.Id,
            Vehicle = travel.Vehicle.VehicleType.Name,
            DestinationId = travel.Destination.Id,
            Destination = travel.Destination.ToString(),
            Autonomy = travel.Autonomy,
            StartedMileage = travel.StartedMileage,
            Start = TimeZoneHelper.ToSaoPaulo(travel.Start),
            FinishedMileage = travel.FinishedMileage,
            End = TimeZoneHelper.ToSaoPaulo(travel.End)
        };
    }

    public static List<TravelDTO> ToListTravelDTO(List<Travel> travels)
    { 
        var list = new List<TravelDTO>();
        foreach(Travel travel in travels)
        {
            list.Add(ToTravelDTO(travel));
        }
        return list;
    }
}
