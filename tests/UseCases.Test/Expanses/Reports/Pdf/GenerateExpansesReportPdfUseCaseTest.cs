using CashFlow.Application.UseCases.Expenses.Reports.PDF;
using CashFlow.Domain.Entities;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using FluentAssertions;

namespace UseCases.Test.Expanses.Reports.Pdf;

public class GenerateExpansesReportPdfUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build();
        var expense = ExpenseBuilder.Collection(loggedUser);
        
        var useCase = CreateUseCase(loggedUser, expense);

        var result = await useCase.Execute(DateOnly.FromDateTime(DateTime.Now));

        result.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Success_Empty()
    {
        var loggedUser = UserBuilder.Build();
        
        var useCase = CreateUseCase(loggedUser, new List<Expanse>());
        
        var result = await useCase.Execute(DateOnly.FromDateTime(DateTime.Now));
        
        result.Should().BeEmpty();
    }

    private GenerateExpansesReportPdfUseCase CreateUseCase(User user, List<Expanse> expanses)
    {
        var repository = new ExpensesReadOnlyRepositoryBuilder().FilterByMonth(user, expanses).Build();
        var loggedUSer = LoggedUserBuilder.Build(user);
        
        return new GenerateExpansesReportPdfUseCase(repository, loggedUSer);
    }
}