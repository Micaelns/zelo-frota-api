using Application.DTO;
using MediatR;

namespace Application.UseCases.Travels.MonthReport;

public class MonthReportCommand : IRequest<Result<string>>
{
    public Guid? VehicleId { get; set; }
    public Guid? DestinationId { get; set; }
    public DateTime? MonthYearTravel { get; set; }
}
