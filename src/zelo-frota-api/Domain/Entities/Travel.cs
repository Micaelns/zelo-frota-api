namespace Domain.Entities;

public class Travel(int id,
                    Vehicle vehicle,
                    int startedMileage,
                    int finishedMileage,
                    float autonomy) : Base(id)
{
    public Vehicle Vehicle { get; set; } = vehicle;
    public int StartedMileage { get; set; } = startedMileage;
    public int FinishedMileage { get; set; } = finishedMileage;
    public float Autonomy { get; private set; } = autonomy;
}
