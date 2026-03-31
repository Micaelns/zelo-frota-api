using Domain.ObjectValues;

namespace Domain.Entities;

public class Vehicle(VehicleType type, Plate plate,
                int mileage, List<Travel> travels) : Base()
{
    public VehicleType Type { get; set; } = type;
    public string Plate { get; set; } = plate.Value;
    public int Mileage { get; private set; } = mileage > 0 ? mileage : 0;
    public List<Travel> Travels { get; set; } = travels;

    public void NewMileage(int mileage)
    {
        if (mileage < 0 || mileage < Mileage)
        {
            throw new ArgumentException("Quilometragem inválida");
        }
        Mileage = mileage;
    }

}
