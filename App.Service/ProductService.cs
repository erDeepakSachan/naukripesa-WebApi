using App.Dto;
using App.Entity;
using App.Repository;

namespace App.Service;

public class ProductService : GenericService<Product>,IDdlOption
{
    public ProductService(ProductRepository repository) : base(repository)
    {
    }
    
    public List<DdlOption> GetAllDdlOptions()
    {
        return GetAll().Select(P => new DdlOption()
        {
            Text = P.Pname,
            Value = Convert.ToString(P.ProductId)
        }).ToList();
    }
}