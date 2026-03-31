// See https://aka.ms/new-console-template for more information
using Domain.Entities;
using Domain.ObjectValues;

var type = new VehicleType(1, "Cavalinho Scanna");
var plate = new Plate("ASD-1W56");

var vehicle = new Vehicle(1, type, plate, 0, []);


Console.WriteLine($"Hello, World! {vehicle.Plate} {vehicle.Mileage}", vehicle);
