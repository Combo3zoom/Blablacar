using Blabalacar.Database;
using Blabalacar.Models;
using Microsoft.EntityFrameworkCore;

namespace Blabalacar.Repository;

public class TripRepository:IRepository<Trip,Guid>
{
    private readonly BlalacarContext _context;

    public TripRepository(BlalacarContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Trip>> GetAll()
    {
        return await _context.Trip.Include("UserTrips").Include("Route").ToListAsync().ConfigureAwait(false);
    }

    public async Task<Trip> GetById(Guid id)
    {
        return (await _context.Trip.Include("UserTrips").Include("Route").SingleOrDefaultAsync(trip => trip.Id == id).ConfigureAwait(false))!;
    }

    public async Task<Trip> Insert(Trip trip)
    {
        await _context.Trip.AddAsync(trip).ConfigureAwait(false);
        return trip;
    }

    public Task DeleteAt(Guid id)
    {
        var trip = _context.Trip.SingleOrDefaultAsync(currentTrip => currentTrip.Id == id); 
        return trip;
    }

    public Task Delete(Trip trip)
    {
        _context.Remove(trip);
        return Task.CompletedTask;
    }

    public async Task Save()
    {
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }
}