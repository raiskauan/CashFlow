using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Repositories.Expanses;

public interface IExpansesRepository
{ 
    Task Add(Expanse expanse);
    Task<List<Expanse>> GetAll();
    Task<Expanse?> GetById(long id);
}