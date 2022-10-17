using Blabalacar.Database;
using Blabalacar.Models;
using Microsoft.EntityFrameworkCore;

namespace Blabalacar.Repository;

public class TripRepository:IRepository<Trip,Guid>
{
    private readonly BlablacarContext _context;

    public TripRepository(BlablacarContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Trip?>> GetAll(CancellationToken cancellationToken=default)
    {
        return await _context.Trip.Include(trip=>trip.UserTrips)
            .Include(trip=>trip.Route).ToListAsync(cancellationToken: cancellationToken);
    }
    
    public async Task<Trip?> GetById(Guid id, CancellationToken cancellationToken=default)
    {
        return (await _context.Trip.Include(trip=>trip.UserTrips)
            .Include(trip=>trip.Route)
            .SingleOrDefaultAsync(trip => trip.Id == id, cancellationToken: cancellationToken))!;
    }

    public async Task<Trip?> Insert(Trip trip, CancellationToken cancellationToken=default)
    {
        await _context.Trip.AddAsync(trip, cancellationToken);
        return trip;
    }
    

    public async Task DeleteAt(Guid id, CancellationToken cancellationToken=default)
    {
        await _context.Trip.SingleOrDefaultAsync(currentTrip => currentTrip.Id == id, cancellationToken: cancellationToken);
    }

    public Task Delete(Trip trip, CancellationToken cancellationToken=default)
    {
        _context.Remove(trip);
        return Task.CompletedTask;
    }

    public async Task Save(CancellationToken cancellationToken=default)
    {
        await  _context.SaveChangesAsync(cancellationToken);
    }
}