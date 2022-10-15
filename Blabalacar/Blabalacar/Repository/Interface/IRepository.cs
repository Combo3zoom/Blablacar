using Blabalacar.Models;

namespace Blabalacar.Repository;

public interface IRepository<TModel,TId> where TModel:class
{
    Task<IEnumerable<TModel?>> GetAll(CancellationToken cancellationToken=default);
    Task<TModel?> GetById(TId? id, CancellationToken cancellationToken=default);
    Task<TModel?> Insert(TModel? entity, CancellationToken cancellationToken=default);
    Task DeleteAt(TId? id, CancellationToken cancellationToken=default);
    Task Delete(TModel entity, CancellationToken cancellationToken=default);
    Task Save(CancellationToken cancellationToken=default);
}