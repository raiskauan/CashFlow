using CashFlow.Communication.Responses;

namespace CashFlow.Application.UseCases.Expenses.GetAll;

public interface IGetAllExpanseUseCase
{
    public Task<ResponseExpansesJson> Execute();
}
