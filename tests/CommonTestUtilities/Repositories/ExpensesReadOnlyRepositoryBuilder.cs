using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expanses;
using Moq;

namespace CommonTestUtilities.Repositories;

public class ExpensesReadOnlyRepositoryBuilder
{
    private readonly Mock<IExpansesReadOnlyRepository> _repository;

    public ExpensesReadOnlyRepositoryBuilder()
    {
        _repository = new Mock<IExpansesReadOnlyRepository>();
    }
    
    public ExpensesReadOnlyRepositoryBuilder GetAll(User user, List<Expanse> expenses)
    {
        _repository.Setup(repository => repository.GetAll(user)).ReturnsAsync(expenses);
        
        return this;
    }

    public ExpensesReadOnlyRepositoryBuilder GetById(User user, Expanse? expense)
    {
        if(expense is not null)
            _repository.Setup(repository => repository.GetById(user, expense.Id)).ReturnsAsync(expense);
        
        return this;
    }

    public ExpensesReadOnlyRepositoryBuilder FilterByMonth(User user,List<Expanse> expenses)
    {
        _repository.Setup(repository => repository.FilterByMonth(user, It.IsAny<DateOnly>())).ReturnsAsync(expenses);
        
        return this;
    }
    
    public IExpansesReadOnlyRepository Build() => _repository.Object;
}