namespace Api.Requests.Vehicles;
public class EndsTravelRequest
{
    public int FinishMileage { get; set; }
    public float FuelQTD { get; set; }
    public DateTime? WhenArrived { get; set; }
}
