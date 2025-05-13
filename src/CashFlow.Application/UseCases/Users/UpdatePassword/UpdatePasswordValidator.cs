using CashFlow.Communication.Requests;
using FluentValidation;

namespace CashFlow.Application.UseCases.Users.UpdatePassword;

public class UpdatePasswordValidator : AbstractValidator<RequestChangePasswordJson>
{
    public UpdatePasswordValidator()
    {
        RuleFor(x => x.NewPassword).SetValidator(new PasswordValidator<RequestChangePasswordJson>());
    }
}