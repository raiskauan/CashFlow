using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Repositories.Expanses;

public interface IExpansesWriteOnlyRepository
{
    Task Add(Expanse expanse);
    
    Task Delete(long id);
}