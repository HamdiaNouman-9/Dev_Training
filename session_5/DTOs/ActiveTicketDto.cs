namespace DTOs;
public class ActiveTicketDto
{
    public int TicketId { get; set; }
    public string LicensePlate { get; set; }
    public int SpotId { get; set; }
    public DateTime EntryTime { get; set; }
}