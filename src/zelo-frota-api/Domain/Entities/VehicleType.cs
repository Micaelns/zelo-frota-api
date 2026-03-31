namespace Domain.Entities;

public class VehicleType(int id, string name):Base(id)
{
    public string Name { get; set; } = name;
}
