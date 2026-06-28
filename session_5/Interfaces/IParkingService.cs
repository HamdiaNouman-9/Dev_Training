namespace Interfaces;
using DTOs;

public interface IParkingService
{
    Task<TicketResponseDto> ParkVehicleAsync(ParkRequestDto request);
    Task<ExitResponseDto> ExitVehicleAsync(int ticketId);
    Task<IEnumerable<ActiveTicketDto>> GetActiveTicketsAsync();
    Task<TicketResponseDto> GetTicketByIdAsync(int ticketId);
}