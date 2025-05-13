using CashFlow.Domain.Entities;
using CashFlow.Domain.Services.ILoggedUser;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace UseCases.Test.Users.UpdatePassword;

public class UpdatePasswordUseCase
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        
        var request = RequestChangePasswordJsonBuilder.Build();
        
        var useCase = CreateUseCase(user, request.Password);

        var act = async () => await useCase.Execute(request);
        
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task Error_NewPassword_Empty()
    {
        var user = UserBuilder.Build();

        var request = RequestChangePasswordJsonBuilder.Build();
        request.Password = String.Empty;

        var useCase = CreateUseCase(user, request.Password);

        var act = async () => { await useCase.Execute(request); };

        var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

        result.Where(e => e.GetErrors().Count == 1 );
    }
    
    [Fact]
    public async Task Error_CurrentPassword_Different()
    {
        var user = UserBuilder.Build();

        var request = RequestChangePasswordJsonBuilder.Build();

        var useCase = CreateUseCase(user);

        var act = async () => { await useCase.Execute(request); };

        var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

        result.Where(e => e.GetErrors().Count == 1 && e.GetErrors().Contains(ResourcesErrorMessage.PASSWORD_DIFFERENT_CURRENT_PASSWORD));
    }

    private CashFlow.Application.UseCases.Users.UpdatePassword.UpdatePasswordUseCase CreateUseCase(User user, string? password = null)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var repository = UserUpdateOnlyRepositoryBuilder.Build(user);
        var unitOfWork = UnitOfWorkBuilder.Build();
        var passwordEncripter = new BCryptBuilder().Verify(password).Build();
        
        return new CashFlow.Application.UseCases.Users.UpdatePassword.UpdatePasswordUseCase(loggedUser, repository, unitOfWork, passwordEncripter);
    }
}