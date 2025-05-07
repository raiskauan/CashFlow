using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expanses;
using Moq;

namespace CommonTestUtilities.Repositories;

public class ExpensesUpdateOnlyRepositoryBuilder
{
    private readonly Mock<IExpansesUpdateOnlyRepository> _repository;

    public ExpensesUpdateOnlyRepositoryBuilder()
    {
        _repository = new Mock<IExpansesUpdateOnlyRepository>();
    }

    public ExpensesUpdateOnlyRepositoryBuilder GetById(User user, Expanse? expanse)
    {
        if (expanse is not null)
        {
            _repository.Setup(rep => rep.GetById(user,expanse.Id)).ReturnsAsync(expanse);
        }
        
        return this;
    }
    
    public IExpansesUpdateOnlyRepository Build() => _repository.Object;
}