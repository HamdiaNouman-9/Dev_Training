namespace Models.Entities;
using Models.Enums; 
public class Floor
{
    public int Id { get; set; }
    public int FloorNumber { get; set; }
    public int ParkingLotId { get; set; }
    public ParkingLot ParkingLot { get; set; }
    public ICollection<ParkingSpot> ParkingSpots { get; set; }
}