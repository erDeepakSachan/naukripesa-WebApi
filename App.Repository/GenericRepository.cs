using System.Data;
using App.Entity;
using Microsoft.EntityFrameworkCore;

namespace App.Repository;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly DbContext db;
    protected readonly DbSet<T> table;

    public GenericRepository(AppDbContext db)
    {
        this.db = db;
        this.table = db.Set<T>();
    }

    public IQueryable<T> GetAll()
    {
        return table.AsQueryable();
    }

    public async Task<T?> Get<TPk>(TPk id)
    {
        return await table.FindAsync(id);
    }

    public async Task<bool> Insert(T obj)
    {
        await table.AddAsync(obj);
        return true;
    }

    public bool Update(T obj)
    {
        table.Update(obj);
        return true;
    }

    public bool Delete(T obj)
    {
        table.Remove(obj);
        return true;
    }

    public async Task<bool> Commit()
    {
        return await db.SaveChangesAsync() > 0;
    }

    protected IDbConnection GetUnderlineConnection()
    {
        var connection = db.Database.GetDbConnection();
        // Ensure the connection is open before executing raw SQL
        // if (connection.State == ConnectionState.Closed)
        // {
        //     await connection.OpenAsync();
        // }

        return connection;
    }
}