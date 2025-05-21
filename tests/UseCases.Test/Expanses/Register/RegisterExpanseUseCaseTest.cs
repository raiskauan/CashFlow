using CashFlow.Application.UseCases.Expenses.Register;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace UseCases.Test.Expanses.Register;

public class RegisterExpanseUseCaseTest
{
    [Fact]
    public async Task Sucess()
    {
        var loggedUser = UserBuilder.Build();
        var request = RequestExpenseJsonBuilder.Build();
        var useCase = CreateUseCase(loggedUser);
        
        var result = await useCase.Execute(request);
        
        result.Should().NotBeNull();
        result.Title.Should().Be(request.Title);
    }
    
    [Fact]
    public async Task Error_Tittle_Empty()
    {
        var loggedUser = UserBuilder.Build();
        
        var request = RequestExpenseJsonBuilder.Build();
        request.Title = string.Empty;
        
        var useCase = CreateUseCase(loggedUser);
        
        var act = async () => await useCase.Execute(request);
        
        var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourcesErrorMessage.TITLE_REQUIRED));
    }

    private RegisterExpenseUseCase CreateUseCase(User user)
    {
        var repository = ExpansesWriteOnlyRepositoryBuilder.Build();
        var mapper = MapperBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new RegisterExpenseUseCase(repository, unitOfWork, mapper, loggedUser);
    }
    
    
}