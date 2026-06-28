namespace Services;
using Data;
using DTOs;
using Exceptions;
using Interfaces;
using Mappers;
using Models.Entities;
using Models.Enums;
using Microsoft.EntityFrameworkCore;

public class ParkingService : IParkingService
{
    private readonly AppDbContext _db;

    public ParkingService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<TicketResponseDto> ParkVehicleAsync(ParkRequestDto request)
    {
        var vehicleType = Enum.Parse<VehicleType>(request.VehicleType, true);
        var feeStrategy = Enum.Parse<FeeStrategy>(request.FeeStrategy, true);

        Vehicle vehicle = await _db.Vehicles.FirstOrDefaultAsync(v => v.LicensePlate == request.LicensePlate);
        if (vehicle == null)
        {
            if (vehicleType == VehicleType.Car)
                vehicle = new Car { LicensePlate = request.LicensePlate };
            else if (vehicleType == VehicleType.Motorcycle)
                vehicle = new Motorcycle { LicensePlate = request.LicensePlate };
            else if (vehicleType == VehicleType.Truck)
                vehicle = new Truck { LicensePlate = request.LicensePlate };
            else
                throw new ArgumentException("Invalid vehicle type");

            _db.Vehicles.Add(vehicle);
        }

        List<ParkingSpot> freeSpots = await _db.ParkingSpots
    .Where(s => s.IsOccupied == false)
    .ToListAsync();

ParkingSpot spot = freeSpots.FirstOrDefault(s => s.CanFitVehicle(vehicleType));

        if (spot == null)
            throw new NoAvailableSpotException($"No available spot for vehicle type {vehicleType}");

        ParkingTicket ticket = new ParkingTicket
        {
            Vehicle = vehicle,
            ParkingSpot = spot,
            EntryTime = DateTime.UtcNow,
            FeeStrategy = feeStrategy
        };

        spot.OccupySpot();
        _db.ParkingTickets.Add(ticket);
        await _db.SaveChangesAsync();

        return ParkingMapper.ToTicketResponseDto(ticket);
    }

    public async Task<ExitResponseDto> ExitVehicleAsync(int ticketId)
    {
        ParkingTicket ticket = await _db.ParkingTickets
            .Include(t => t.Vehicle)
            .Include(t => t.ParkingSpot)
            .FirstOrDefaultAsync(t => t.Id == ticketId);

        if (ticket == null)
            throw new TicketNotFoundException($"Ticket with ID {ticketId} not found.");

        IFeeStrategy strategy;
        if (ticket.FeeStrategy == FeeStrategy.Hourly)
            strategy = new HourlyFeeStrategy();
        else if (ticket.FeeStrategy == FeeStrategy.Daily)
            strategy = new DailyFeeStrategy();
        else if (ticket.FeeStrategy == FeeStrategy.Flat)
            strategy = new FlatFeeStrategy();
        else
            throw new ArgumentException("Invalid fee strategy");

        ticket.Fee = strategy.Calculate(ticket.EntryTime, DateTime.UtcNow);
        ticket.ExitTime = DateTime.UtcNow;
        ticket.ParkingSpot.Free();

        await _db.SaveChangesAsync();

        return ParkingMapper.ToExitResponseDto(ticket);
    }

    public async Task<IEnumerable<ActiveTicketDto>> GetActiveTicketsAsync()
    {
        return await _db.ParkingTickets
            .Where(t => t.ExitTime == null)
            .Include(t => t.Vehicle)
            .Include(t => t.ParkingSpot)
            .Select(t => ParkingMapper.ToActiveTicketDto(t))
            .ToListAsync();
    }

    public async Task<TicketResponseDto> GetTicketByIdAsync(int ticketId)
    {
        ParkingTicket ticket = await _db.ParkingTickets
            .Include(t => t.Vehicle)
            .Include(t => t.ParkingSpot)
            .FirstOrDefaultAsync(t => t.Id == ticketId);

        if (ticket == null)
            throw new TicketNotFoundException($"Ticket with ID {ticketId} not found.");

        return ParkingMapper.ToTicketResponseDto(ticket);
    }
}