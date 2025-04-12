namespace App.Repository;

public interface IGenericRepository<T> where T : class
{
    IQueryable<T>
        GetAll();

    Task<T?> Get<TPk>(TPk id);

    Task<bool> Insert(T obj);

    bool Update(T obj);

    bool Delete(T obj);

    Task<bool> Commit();
}