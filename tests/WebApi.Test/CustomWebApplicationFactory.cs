using CashFlow.Domain.Entities;
using CashFlow.Domain.Enums;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Infrastructure.DataAccess;
using CommonTestUtilities.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Test.Resources;

namespace WebApi.Test;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public ExpenseIdentityManager Expense_MemberTeam { get; private set; } = default!;
    public ExpenseIdentityManager Expense_Admin { get; private set; } = default!;
    public UserIdentityManager User_Team_Member { get; private set; } = default!;
    
    public UserIdentityManager User_Admin { get; private set; } = default!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {

        builder.UseEnvironment("Test")
            .ConfigureServices(services =>
            {
                var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                services.AddDbContext<CashFlowDbContext>(config =>
                {
                    config.UseInMemoryDatabase("InMemoryDbForTesting");
                    config.UseInternalServiceProvider(provider);
                });

                var scope = services.BuildServiceProvider().CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<CashFlowDbContext>();
                var passwordEncripter = scope.ServiceProvider.GetRequiredService<IPasswordEncripter>();
                var accessTokenGenerator = scope.ServiceProvider.GetRequiredService<IAcessTokenGeneration>();
                
                StartDataBase(dbContext, passwordEncripter, accessTokenGenerator);
            });


    }

    private void StartDataBase(CashFlowDbContext dbContext, IPasswordEncripter passwordEncripter, IAcessTokenGeneration accessTokenGeneration)
    {
        var userTeamMember = AddUserTeamMember(dbContext, passwordEncripter, accessTokenGeneration);
        var expenseTeamMember = AddExpenses(dbContext, userTeamMember, 1, 1);
        Expense_MemberTeam = new ExpenseIdentityManager(expenseTeamMember);
        
        var userAdmin = AddUserAdmin(dbContext, passwordEncripter, accessTokenGeneration);
        var expenseAdmin = AddExpenses(dbContext, userAdmin, 2, 2);
        Expense_Admin = new ExpenseIdentityManager(expenseAdmin);
        
        dbContext.SaveChanges();
    }

    private User AddUserTeamMember(CashFlowDbContext dbContext, IPasswordEncripter passwordEncripter, IAcessTokenGeneration accessTokenGeneration)
    {
        var user = UserBuilder.Build();
        user.Id = 1;
        var password = user.Password;
        
        user.Password = passwordEncripter.Encrypt(user.Password);
        
        dbContext.User.Add(user);
        var token = accessTokenGeneration.Generate(user);
        
        User_Team_Member = new UserIdentityManager(user, password, token);

        return user;
    }
    
    private User AddUserAdmin(CashFlowDbContext dbContext, IPasswordEncripter passwordEncripter, IAcessTokenGeneration accessTokenGeneration)
    {
        var user = UserBuilder.Build(Roles.ADMIN);
        user.Id = 2;
        var password = user.Password;
        
        user.Password = passwordEncripter.Encrypt(user.Password);
        
        dbContext.User.Add(user);
        var token = accessTokenGeneration.Generate(user);
        
        User_Admin = new UserIdentityManager(user, password, token);

        return user;
    }

    private Expanse AddExpenses(CashFlowDbContext dbContext, User user, long expenseId, long tagId)
    {
        var expense = ExpenseBuilder.Build(user);
        expense.Id = expenseId;

        foreach (var tag in expense.Tags)
        {
            tag.Id = tagId;
            tag.ExpenseId = expenseId;
        }
        
        dbContext.Expanses.Add(expense);

        return expense;
    }
}