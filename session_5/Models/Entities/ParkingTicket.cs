using Models.Enums;
namespace Models.Entities;
public class ParkingTicket
{
    public int Id { get; set; }
    public int VehicleId { get; set; }
    public Vehicle Vehicle { get; set; }
    public int ParkingSpotId { get; set; }
    public ParkingSpot ParkingSpot { get; set; }
    public DateTime EntryTime { get; set; }
    public DateTime? ExitTime { get; set; }
    public decimal? Fee { get; set; }
    public FeeStrategy FeeStrategy { get; set; }

}