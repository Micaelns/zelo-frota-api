namespace Domain.Entities;

public class Travel(Vehicle vehicle,
                    Destination destination) : Base()
{
    public Vehicle Vehicle { get; set; } = vehicle;
    public Destination Destination { get; set; } = destination;
    public int StartedMileage { get; set; } = int.MinValue;
    public int FinishedMileage { get; set; } = int.MinValue;
    public float Autonomy { get; private set; } = float.MinValue;
    public DateTime? Start { get; private set; }
    public DateTime? End { get; private set; }

    public void Starts(DateTime? whenTravel)
    {
        StartedMileage = Vehicle.Mileage;
        Start = whenTravel??DateTime.Now;
    }

    public void Ends(int finishMileage, DateTime? whenArrived)
    {
        if ( finishMileage < 0)
        {
            throw new ArgumentException("Quilometragem informada é inválida");
        }

        if (StartedMileage < 0 )
        {
            throw new ArgumentException("A viagem não foi iniciada");
        }

        if ( StartedMileage > finishMileage )
        {
            throw new ArgumentException("A viagem não pode finalizar antes de iniciar");
        }

        FinishedMileage = finishMileage;
        Autonomy = (FinishedMileage - StartedMileage) / FinishedMileage;
        End = whenArrived ?? DateTime.Now;
    }
}
