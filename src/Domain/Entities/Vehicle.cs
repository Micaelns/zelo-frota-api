using Domain.ObjectValues;

namespace Domain.Entities;

public class Vehicle(int id, VehicleType type, Plate plate,
                int mileage, List<Travel> travels) : Base(id)
{
    public VehicleType Type { get; set; } = type;
    public string Plate { get; set; } = plate.Value;
    public int Mileage { get;private set; } = mileage;
    public List<Travel> Travels { get; set; } = travels;

    public void NewMileage(int mileage) {
        if (mileage < 0 || mileage < Mileage) {
            throw new ArgumentException("Quilometragem inválida");
        } 
        Mileage = mileage;
    }

}
