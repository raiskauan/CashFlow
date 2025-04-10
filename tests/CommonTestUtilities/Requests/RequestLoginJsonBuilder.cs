using Bogus;
using CashFlow.Communication.Requests;

namespace CommonTestUtilities.Requests;

public static class RequestLoginJsonBuilder
{
    public static RequestLoginJson Build()
    {
        return new Faker<RequestLoginJson>()
            .RuleFor(user => user.Email , faker => faker.Internet.Email())
            .RuleFor(user => user.Password , faker => faker.Internet.Password(prefix: "!aA1"));

    }
}