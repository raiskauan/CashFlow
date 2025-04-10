using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Repositories.Expanses;

public interface IExpansesUpdateOnlyRepository
{
    Task<Expanse?> GetById(Entities.User user,long id);
    void Update(Expanse expanse);
}