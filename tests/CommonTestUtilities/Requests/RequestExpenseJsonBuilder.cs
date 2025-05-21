using Bogus;
using CashFlow.Communication.Enums;
using CashFlow.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestExpenseJsonBuilder
{
    public static RequestExpenseJson Build()
    {
        var faker = new Faker("en");

        return new Faker<RequestExpenseJson>()
            .RuleFor(r => r.Title, faker.Commerce.Product())
            .RuleFor(r => r.Description, faker.Commerce.ProductDescription)
            .RuleFor(r => r.Date, faker.Date.Past())
            .RuleFor(r => r.PaymentType, faker.PickRandom<PaymentType>())
            .RuleFor(r => r.Amount, faker.Random.Decimal(min: 1, max: 1000))
            .RuleFor(r => r.Tags, faker => faker.Make(1, () => faker.PickRandom<Tag>()));
    }
}