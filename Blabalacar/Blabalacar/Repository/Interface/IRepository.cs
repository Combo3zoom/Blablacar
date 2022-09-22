namespace Blabalacar.Repository;

public interface IRepository<TModel,TId> where TModel:class
{
    Task<IEnumerable<TModel>> GetAll();
    Task<TModel> GetById(TId id);
    Task<TModel> Insert(TModel entity);
    Task DeleteAt(TId id);
    Task Delete(TModel entity);
    Task Save();
}