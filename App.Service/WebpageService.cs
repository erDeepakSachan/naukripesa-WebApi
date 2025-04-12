using App.Dto;
using App.Entity;
using App.Repository;

namespace App.Service;

public class WebpageService : GenericService<Webpage>,IDdlOption
{
    private WebpageRepository repository;
    public WebpageService(WebpageRepository repository) : base(repository)
    {
        this.repository = repository;
    }
    
    public List<DdlOption> GetAllDdlOptions()
    {
        return repository.GetAll().Where(p => p.WebpageId > 0).Select(P => new DdlOption()
        {
            Text = P.Name,
            Value = Convert.ToString(P.WebpageId)
        }).ToList();
    }
    
    public new IQueryable<Webpage> GetAll()
    {
        return repository.GetAll();
    }
}