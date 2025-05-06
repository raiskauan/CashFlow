using CashFlow.Communication.Requests;
using CashFlow.Domain.Enums;

namespace CashFlow.Domain.Entities;

public class Expanse
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public PaymentType PaymentType { get; set; }
    
    public long UserId { get; set; }
    public User User { get; set; } = default!;

    public static explicit operator Expanse(RequestExpenseJson request)
    {
        return new Expanse()
        {
            Title = request.Title,
            Amount = request.Amount,
            Date = request.Date,
            Description = request.Description,
            Id = 0,
            User = new User()
        };
    }
}