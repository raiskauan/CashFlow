using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Repositories.Expanses;

public interface IExpansesWriteOnlyRepository
{
    Task Add(Expanse expanse);

    /// <summary>
    /// This function returns TRUE if the deletion was successful otherwise returns FALSE
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool> Delete(long id);
}