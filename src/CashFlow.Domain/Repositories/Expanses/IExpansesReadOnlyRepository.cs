using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Repositories.Expanses;

public interface IExpansesReadOnlyRepository
{
    Task<List<Expanse>> GetAll();
    Task<Expanse?> GetById(long id);
    
    Task<List<Expanse>> FilterByMonth(DateOnly date);
}