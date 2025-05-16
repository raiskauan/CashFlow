using CashFlow.Communication.Requests;
using CashFlow.Exception;
using FluentValidation;

namespace CashFlow.Application.UseCases.Expenses;

public class ExpanseValidator : AbstractValidator<RequestExpenseJson>
{
    public ExpanseValidator()
    {
        RuleFor(x => x.Title).NotEmpty().WithMessage(ResourcesErrorMessage.TITLE_REQUIRED);
        RuleFor(x => x.Amount).GreaterThan(0).WithMessage(ResourcesErrorMessage.AMOUNT_MUST_BE_GREATER_THAN_ZERO);
        RuleFor(x => x.Date).LessThanOrEqualTo(DateTime.UtcNow).WithMessage(ResourcesErrorMessage.EXPENSES_CANNOT_FOR_THE_FUTURE);
        RuleFor(x => x.PaymentType).IsInEnum().WithMessage(ResourcesErrorMessage.PAYMENT_TYPE_INVALID);
        RuleFor(x => x.Tags).ForEach(rule =>
        {
            rule.IsInEnum().WithMessage(ResourcesErrorMessage.TAG_TYPE_NOT_SUPPORTED);
        });
    }
}