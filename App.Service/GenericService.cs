using App.Repository;

namespace App.Service;

public class GenericService<T> : BaseService, IGenericService<T> where T : class
{
    private readonly IGenericRepository<T> repository;

    public GenericService(IGenericRepository<T> repository)
    {
        this.repository = repository;
    }

    public IQueryable<T> GetAll()
    {
        return repository.GetAll();
    }

    public async Task<T?> Get<TPk>(TPk id)
    {
        return await repository.Get(id);
    }

    public async Task<bool> Insert(T obj)
    {
        await repository.Insert(obj);
        return await repository.Commit();
    }

    public async Task<bool> Update(T obj)
    {
        repository.Update(obj);
        return await repository.Commit();
    }

    public async Task<bool> Delete(T obj)
    {
        repository.Delete(obj);
        return await repository.Commit();
    }
}