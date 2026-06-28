namespace Mappers;
using DTOs;
using Models.Entities;
public static class ParkingMapper
{
    public static ActiveTicketDto ToActiveTicketDto(ParkingTicket ticket)
    {
        return new ActiveTicketDto
        {
            TicketId = ticket.Id,
            LicensePlate = ticket.Vehicle.LicensePlate,
            SpotId = ticket.ParkingSpot.Id,
            EntryTime = ticket.EntryTime
        };
    }

    public static TicketResponseDto ToTicketResponseDto(ParkingTicket ticket)
    {
        return new TicketResponseDto
        {
            TicketId = ticket.Id,
            LicensePlate = ticket.Vehicle.LicensePlate,
            EntryTime = ticket.EntryTime,
            ExitTime = ticket.ExitTime,
            Fee = ticket.Fee,
            FeeStrategy = ticket.FeeStrategy.ToString()
        };
    }
    public static ExitResponseDto ToExitResponseDto(ParkingTicket ticket)
    {
        return new ExitResponseDto
        {
            TicketId = ticket.Id,
            LicensePlate = ticket.Vehicle.LicensePlate,
            EntryTime = ticket.EntryTime,
            ExitTime = ticket.ExitTime ?? DateTime.Now,
            Fee = ticket.Fee ?? 0
        };
    }
}