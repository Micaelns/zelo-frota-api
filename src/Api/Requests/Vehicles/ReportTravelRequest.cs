namespace Api.Requests.Vehicles;
public class ReportTravelRequest
{
    public Guid? DestinationId { get; set; }
    public DateTime? MonthYearTravel { get; set; }
}
