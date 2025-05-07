using CashFlow.Domain.Entities;

namespace WebApi.Test.Resources;

public class ExpenseIdentityManager
{
    private readonly Expanse _expense;

    public ExpenseIdentityManager(Expanse expense)
    {
        _expense = expense;
    }
    
    public long GetExpenseId() => _expense.Id;
}