namespace App.Service;

public interface IGenericService<T> where T : class
{
    IQueryable<T> GetAll();

    Task<T?> Get<TPk>(TPk id);

    Task<bool> Insert(T obj);

    Task<bool> Update(T obj);

    Task<bool> Delete(T obj);
}