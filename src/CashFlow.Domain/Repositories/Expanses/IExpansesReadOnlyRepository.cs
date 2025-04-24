using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Repositories.Expanses;

public interface IExpansesReadOnlyRepository
{
    Task<List<Expanse>> GetAll(Entities.User user);
    Task<Expanse?> GetById(Entities.User user,long id);
    
    Task<List<Expanse>> FilterByMonth(Entities.User user,DateOnly date);
}