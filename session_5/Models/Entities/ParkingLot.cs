namespace Models.Entities;
public class ParkingLot
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public ICollection<Floor> Floors { get; set; }
}