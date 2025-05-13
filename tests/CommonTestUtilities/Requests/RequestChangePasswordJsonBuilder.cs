using Bogus;
using CashFlow.Communication.Requests;

namespace CommonTestUtilities.Requests;

public static class RequestChangePasswordJsonBuilder
{
    public static RequestChangePasswordJson Build()
    {
        return new Faker<RequestChangePasswordJson>()
            .RuleFor(user => user.Password, f => f.Internet.Password())
            .RuleFor(user => user.NewPassword, f => f.Internet.Password(prefix: "!Aa1"));
    }
}