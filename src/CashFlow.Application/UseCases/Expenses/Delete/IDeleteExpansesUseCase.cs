namespace CashFlow.Application.UseCases.Expenses.Delete;

public interface IDeleteExpansesUseCase
{
    Task Execute(long id);
}