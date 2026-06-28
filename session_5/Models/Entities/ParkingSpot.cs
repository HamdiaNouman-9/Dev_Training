namespace Models.Entities;
using Models.Enums;
public abstract class ParkingSpot
{
    public int Id { get; set; }
    public int FloorId { get; set; }
    public Floor Floor { get; set; }
    public SpotType Type { get; set; }
    public abstract bool CanFitVehicle(VehicleType vehicleType);
public bool IsOccupied { get; set; }

public void OccupySpot()
{
    IsOccupied = true;
}

public void Free()
{
    IsOccupied = false;
}
}