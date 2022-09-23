using Blabalacar.Models;

namespace Blabalacar.Repository;

public interface IUserRepository<TUnionModel,TId> : IRepository<User,Guid>
{
    Task<TUnionModel> GetFirstAndSecondModel(TId firstId, TId secondId);
    Task ConnectionBetweenUserAndTripDelete(TUnionModel entity);
    
    //for auth
    Task<User> GetByName(string name);
}