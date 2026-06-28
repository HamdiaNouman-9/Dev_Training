namespace DTOs;
public class ExitResponseDto
{
    public int TicketId { get; set; }
    public string LicensePlate { get; set; }
    public DateTime EntryTime { get; set; }
    public DateTime ExitTime { get; set; }
    public decimal Fee { get; set; }
}