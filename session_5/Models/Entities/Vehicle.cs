namespace Models.Entities;
using Models.Enums;
public abstract class Vehicle
{
    public int Id { get; set; }
    public string LicensePlate { get; set; }
    public VehicleType Type { get; set; }
    public ICollection<ParkingTicket> ParkingTickets { get; set; }
}