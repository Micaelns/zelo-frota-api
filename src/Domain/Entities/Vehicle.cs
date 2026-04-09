using Domain.ObjectValues;

namespace Domain.Entities;

public class Vehicle : Base
{
    protected Vehicle() {
        Plate = string.Empty;
    }

    public Vehicle(Guid vehicleTypeId, Plate plate, int mileage)
    {
        VehicleTypeId = vehicleTypeId;
        Plate = plate.Value;
        Mileage = mileage > 0 ? mileage : 0;
    }
    public Guid VehicleTypeId { get; set; }
    public string Plate { get; set; }
    public int Mileage { get; private set; }
    private readonly List<Travel> _travels = [];
    public IReadOnlyCollection<Travel> Travels => _travels;

    public void NewMileage(int mileage)
    {
        if (mileage < 0 || mileage < Mileage)
        {
            throw new ArgumentException("Quilometragem inválida");
        }
        Mileage = mileage;
    }

}
