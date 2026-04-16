namespace Domain.Entities;

public class Travel : Base
{
    protected Travel() { }

    public Travel(Guid vehicleId,Guid destinationId){
        VehicleId = vehicleId;
        DestinationId = destinationId;
    }

    public Guid VehicleId { get; private set; } 
    public Guid DestinationId { get; private set; }
    public int? StartedMileage { get; private set; }
    public int? FinishedMileage { get; private set; }
    public float? Autonomy { get; private set; }
    public DateTime? Start { get; private set; }
    public DateTime? End { get; private set; }

    public void Starts(int currentMileage, DateTime? whenTravel)
    {
        if (Start != null)
            throw new InvalidOperationException("Viagem já iniciada");

        StartedMileage = currentMileage;
        Start = whenTravel??DateTime.UtcNow;
    }

    public void Ends(int finishMileage, float fuelQTD, DateTime? whenArrived)
    {
        if (FinishedMileage is not null)
        {
            throw new InvalidOperationException("A viagem já foi finalizada");
        }

        if ( finishMileage < 0)
        {
            throw new ArgumentException("Quilometragem informada é inválida");
        }

        if (fuelQTD <= 0)
        {
            throw new ArgumentException("A quantidade de combustível informada é inválida");
        }

        if (StartedMileage is null)
        {
            throw new InvalidOperationException("A viagem não foi iniciada");
        }

        if ( StartedMileage > finishMileage )
        {
            throw new ArgumentException("A viagem não pode finalizar antes de iniciar");
        }

        FinishedMileage = finishMileage;
        End = whenArrived ?? DateTime.UtcNow;
        var distance = FinishedMileage - StartedMileage;
        Autonomy = distance / fuelQTD;
    }
}
