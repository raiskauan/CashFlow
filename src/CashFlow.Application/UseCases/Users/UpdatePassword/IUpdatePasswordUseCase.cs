using CashFlow.Communication.Requests;

namespace CashFlow.Application.UseCases.Users.UpdatePassword;

public interface IUpdatePasswordUseCase
{
    Task Execute(RequestChangePasswordJson request);
}