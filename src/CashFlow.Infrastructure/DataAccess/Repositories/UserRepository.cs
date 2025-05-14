using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.User;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAccess.Repositories;

public class UserRepository : IUserReadOnlyRepository, IUserWriteOnlyRepository, IUserUpdateOnlyRepository
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

    public async Task Delete(User user)
    {
        var userToRemove = await _dbcontext.User.FindAsync(user.Id);
        _dbcontext.User.Remove(userToRemove!);
    }

    public Task<User> GetById(long id)
    {
        return _dbcontext.User.FirstAsync(user  => user.Id == id);
    }

    public void Update(User user)
    {
        _dbcontext.User.Update(user);
    }
}