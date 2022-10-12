using Blabalacar.Database;
using Blabalacar.Models;
using Microsoft.EntityFrameworkCore;

namespace Blabalacar.Repository;

public class UserRepository:IUserRepository<UserTrip, Guid>
{
    private readonly BlablacarContext _context;

    public UserRepository(BlablacarContext context)
    {
        _context = context;
    }


    public async Task<UserTrip> GetUserTrips(Guid firstId, Guid secondId, CancellationToken cancellationToken=default)
    {
        return (await _context.UserTrips.SingleOrDefaultAsync(stageUserTrip =>
            stageUserTrip!.UserId == firstId && stageUserTrip.TripId == secondId, cancellationToken: cancellationToken))!;
    }

    public Task ConnectionBetweenUserAndTripDelete(UserTrip userTrip, CancellationToken cancellationToken=default)
    {
        _context.UserTrips.Remove(userTrip);
        return Task.CompletedTask;
    }

    public async Task<User> GetByName(string name, CancellationToken cancellationToken=default)
    {
        var a = await _context.User.Include(user => user.UserTrips)
            .SingleOrDefaultAsync(user => user.Name == name, cancellationToken);
        return a;
    }

    public async Task<IEnumerable<User>> GetAll(CancellationToken cancellationToken=default)
    {
        return await _context.User.Include(user=>user.UserTrips)
            .ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<User> GetById(Guid id, CancellationToken cancellationToken=default)
    {
        return (await _context.User.Include(a=>a.UserTrips)
            .SingleOrDefaultAsync(user => user.Id == id, cancellationToken: cancellationToken))!;
    }

    public async Task<User> Insert(User entity, CancellationToken cancellationToken=default)
    {
        await _context.User.AddAsync(entity, cancellationToken);
        return entity;
    }

    public async Task DeleteAt(Guid id, CancellationToken cancellationToken=default)
    {
        var user = await _context.User.
            FirstOrDefaultAsync(currentUser => currentUser.Id == id, cancellationToken: cancellationToken);
        if (user != null)
            _context.Remove(user);
    }

    public Task Delete(User user, CancellationToken cancellationToken=default)
    {
        _context.Remove(user);
        return Task.CompletedTask;
    }

    public async Task Save(CancellationToken cancellationToken=default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<string> GetRefreshToken(Guid id, CancellationToken cancellationToken=default)
    {
        var currentUser = await _context.User.SingleOrDefaultAsync(user => user.Id == id,
            cancellationToken: cancellationToken);
        return currentUser.RefreshToken;
    }
}