using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.User;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAccess.Repositories;

public class UserRepository : IUserReadOnlyRepository, IUserWriteOnlyRepository
{
    
    private readonly CashFlowDbContext _dbcontext;

    public UserRepository(CashFlowDbContext dbcontext)
    {
        _dbcontext = dbcontext;
    }
    
    public async Task<bool> ExistActiveUserWithEmail(string email)
    {
         return await _dbcontext.User.AnyAsync(user  => user.Email.Equals(email));
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        return await _dbcontext.User.AsNoTracking().FirstOrDefaultAsync(user  => user.Email.Equals(email));
    }

    public async Task Add(User user)
    {
        await _dbcontext.User.AddAsync(user);
    }
}