using Blabalacar.Models;

namespace Blabalacar.Repository;

public interface IUserRepository<TUnionModel, TId> : IRepository<User,Guid>
{
    Task<TUnionModel> GetUserTrips(TId firstId, TId secondId, CancellationToken cancellationToken=default);
    Task ConnectionBetweenUserAndTripDelete(TUnionModel entity, CancellationToken cancellationToken=default);
    Task<string> GetRefreshToken(Guid id, CancellationToken cancellationToken = default);
    Task<User> GetByName(string name, CancellationToken cancellationToken=default);
}