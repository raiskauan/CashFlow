using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Security.Tokens;

public interface IAcessTokenGeneration
{
    string Generate(User user);
}