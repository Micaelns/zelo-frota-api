namespace Domain.Entities;

public class Travel(Guid vehicleId,
                    Destination destination) : Base()
{
    public Guid VehicleId { get; private set; } = vehicleId;
    public Destination Destination { get; set; } = destination;
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
        Start = whenTravel??DateTime.Now;
    }

    public void Ends(int finishMileage, float fuelQTD, DateTime? whenArrived)
    {
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
        End = whenArrived ?? DateTime.Now;
        var distance = FinishedMileage - StartedMileage;
        Autonomy = distance / fuelQTD;
    }
}
