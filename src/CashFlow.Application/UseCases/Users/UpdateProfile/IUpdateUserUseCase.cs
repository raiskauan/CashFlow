using CashFlow.Communication.Requests;

namespace CashFlow.Application.UseCases.Users.UpdateProfile;

public interface IUpdateUserUseCase
{
    public Task Execute(RequestUpdateUserJson request);
}