namespace Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Interfaces;
using DTOs;

[ApiController]
[Route("api/parking")]
public class ParkingController : ControllerBase
{
    private readonly IParkingService _parkingService;

    public ParkingController(IParkingService parkingService)
    {
        _parkingService = parkingService;
    }

    [HttpPost("park")]
    [Authorize]
    public async Task<IActionResult> Park([FromBody] ParkRequestDto request)
    {
        var ticket = await _parkingService.ParkVehicleAsync(request);
        return CreatedAtAction(nameof(GetTicketById), new { ticketId = ticket.TicketId }, ticket);
    }

    [HttpPut("exit/{ticketId}")]
    [Authorize]
    public async Task<IActionResult> Exit(int ticketId)
    {
        var result = await _parkingService.ExitVehicleAsync(ticketId);
        return Ok(result);
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetActiveTickets()
    {
        var tickets = await _parkingService.GetActiveTicketsAsync();
        return Ok(tickets);
    }

    [HttpGet("ticket/{ticketId}")]
    public async Task<IActionResult> GetTicketById(int ticketId)
    {
        var ticket = await _parkingService.GetTicketByIdAsync(ticketId);
        return Ok(ticket);
    }
}