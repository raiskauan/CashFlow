using CashFlow.Domain.Entities;
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
    public ExpenseIdentityManager Expense { get; private set; }
    public UserIdentityManager User_Team_Member { get; private set; }

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
        var user = AddUserTeamMember(dbContext, passwordEncripter, accessTokenGeneration);
        AddExpenses(dbContext, user);
        
        dbContext.SaveChanges();
    }

    private User AddUserTeamMember(CashFlowDbContext dbContext, IPasswordEncripter passwordEncripter, IAcessTokenGeneration accessTokenGeneration)
    {
        var user = UserBuilder.Build();
        var password = user.Password;
        
        user.Password = passwordEncripter.Encrypt(user.Password);
        
        dbContext.User.Add(user);
        var token = accessTokenGeneration.Generate(user);
        
        User_Team_Member = new UserIdentityManager(user, password, token);

        return user;
    }

    private void AddExpenses(CashFlowDbContext dbContext, User user)
    {
        var expense = ExpenseBuilder.Build(user);
        
        dbContext.Expanses.Add(expense);
        
        Expense = new ExpenseIdentityManager(expense);
    }
}