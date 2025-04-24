using CashFlow.Domain.Repositories.Expanses;
using Moq;

namespace CommonTestUtilities.Repositories;

public static class ExpansesWriteOnlyRepositoryBuilder
{
    public static IExpansesWriteOnlyRepository Build()
    {
        var mock = new Mock<IExpansesWriteOnlyRepository>();
        
        return mock.Object;
    }

}