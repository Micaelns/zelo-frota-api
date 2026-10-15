using Domain.ObjectValues;

namespace Domain.Entities;

public class Vehicle : Base
{
    protected Vehicle()
    {
        Plate = string.Empty;
    }

    public Vehicle(Guid vehicleTypeId, Plate plate, int mileage)
    {
        VehicleTypeId = vehicleTypeId;
        Plate = plate.Value;
        Mileage = mileage > 0 ? mileage : 0;
    }
    public Guid VehicleTypeId { get; set; }
    public VehicleType VehicleType { get; set; }
    public string Plate { get; set; }
    public int Mileage { get; private set; }
    private readonly List<Travel> _travels = [];
    public IReadOnlyCollection<Travel> Travels => _travels;

    public void NewMileage(int mileage)
    {
        if (mileage < 0 || mileage < Mileage)
        {
            throw new ArgumentException("Quilometragem invalida");
        }
        Mileage = mileage;
    }

    public Travel StartTravel(Guid destinationId, bool hasOpenTravel, Driver? driver = null, DateTime? when = null)
    {
        if (hasOpenTravel)
            throw new InvalidOperationException("Já existe uma viagem em andamento");
 
        if (driver is not null)
        {
            if (VehicleType is null)
            {
                throw new InvalidDataException("VehicleType é necessário para calcular licença do motorista");
            }
            if (!driver.DriverLicense.CanDrive(VehicleType.RequeridLicenseCategory))
            {
                throw new InvalidOperationException("Motorista não possui qualificação necessária. Exigida: '"+VehicleType.RequeridLicenseCategory.ToString()+"', Possui: '"+ driver.DriverLicense.LicenseCategory.ToString()+ "'");
            }
        } 

        var travel = new Travel(this.Id, destinationId, driver?.Id);

        travel.Starts(this.Mileage, when);

        _travels.Add(travel);

        return travel;
    }

    public Travel EndTravel(Travel travel, int finishMileage, float fuelQTD, DateTime? when = null)
    {
        if (finishMileage < this.Mileage)
            throw new ArgumentException("Quilometragem final inválida");

        travel.Ends(finishMileage, fuelQTD, when);

        this.NewMileage(finishMileage);

        return travel;
    }
}
