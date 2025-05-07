using CashFlow.Application.UseCases.Expenses.Update;
using CashFlow.Communication.Enums;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expanses;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;
using PaymentType = CashFlow.Domain.Enums.PaymentType;

namespace UseCases.Test.Expanses.Update;

public class UpdateExpenseUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build();
        var request = RequestExpenseJsonBuilder.Build();
        var expense = ExpenseBuilder.Build(loggedUser);
        
        var useCase = CreateUseCase(loggedUser, expense);
        
        var act = async () => await useCase.Execute(expense.Id, request);
        
        await act.Should().NotThrowAsync();
        
        expense.Title.Should().Be(request.Title);
        expense.Description.Should().Be(request.Description);
        expense.Amount.Should().Be(request.Amount);
        expense.Date.Should().Be(request.Date);
      //  expense.PaymentType.Should().Be((PaymentType)request.PaymentType);
    }

    [Fact]
    public async Task Error_Title_Empty()
    {
        var loggedUser = UserBuilder.Build();
        
        var request = RequestExpenseJsonBuilder.Build();
        request.Title = string.Empty;
        
        var useCase = CreateUseCase(loggedUser);
        
        var act = async () => await useCase.Execute(loggedUser.Id, request);

        var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourcesErrorMessage.TITLE_REQUIRED));
    }
    
    [Fact]
    public async Task Error_Expense_Not_Found()
    {
        var loggedUser = UserBuilder.Build();
        
        var request = RequestExpenseJsonBuilder.Build();
        
        var useCase = CreateUseCase(loggedUser);

        var act = async () => await useCase.Execute(id: 1000, request);

        var result = await act.Should().ThrowAsync<NotFoundException>();

        result.Where(ex =>
            ex.GetErrors().Count() == 1 && ex.GetErrors().Contains(ResourcesErrorMessage.EXPENSE_NOT_FOUND));
    }
    
    private UpdateExpanseUseCase CreateUseCase(User user, Expanse? expanse = null)
    {
        var repository = new ExpensesUpdateOnlyRepositoryBuilder().GetById(user, expanse).Build();
        var mapper = MapperBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new UpdateExpanseUseCase(mapper, unitOfWork, repository, loggedUser);
    }
}