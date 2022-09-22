using Blabalacar.Database;
using Blabalacar.Models;
using Microsoft.EntityFrameworkCore;

namespace Blabalacar.Repository;

public class UserRepository:IUserRepository<UserTrip, Guid>
{
    private readonly BlalacarContext _context;

    public UserRepository(BlalacarContext context)
    {
        _context = context;
    }


    public async Task<UserTrip> GetFirstAndSecondModel(Guid firstId, Guid secondId)
    {
        return await _context.UserTrips.SingleOrDefaultAsync(stageUserTrip =>
             stageUserTrip!.UserId == firstId && stageUserTrip.TripId == secondId);
    }

    public Task ConnectionBetweenUserAndTripDelete(UserTrip userTrip)
    {
        _context.UserTrips.Remove(userTrip);
        return Task.CompletedTask;
    }

    public async Task<User> GetByName(string name)
    {
        return await _context.User.Include("UserTrips").FirstOrDefaultAsync(user => user.Name == name).ConfigureAwait(false);
    }

    public async Task<IEnumerable<User>> GetAll()
    {
        return await _context.User.Include("UserTrips").ToListAsync().ConfigureAwait(false);
    }

    public async Task<User> GetById(Guid id)
    {
        return await _context.User.Include("UserTrips").SingleOrDefaultAsync(user =>user.Id==id).ConfigureAwait(false);
    }

    public async Task<User> Insert(User entity)
    {
        await _context.User.AddAsync(entity).ConfigureAwait(false);
        return entity;
    }

    public async Task DeleteAt(Guid id)
    {
        var user = await _context.User.FirstOrDefaultAsync(currentUser => currentUser.Id == id).ConfigureAwait(false);
        if (user != null)
            _context.Remove(user);
    }

    public Task Delete(User user)
    {
        _context.Remove(user);
        return Task.CompletedTask;
    }

    public async Task Save()
    {
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }
}