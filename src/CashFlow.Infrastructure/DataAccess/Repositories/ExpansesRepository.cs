using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expanses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace CashFlow.Infrastructure.DataAccess.Repositories;

internal class ExpansesRepository : IExpansesReadOnlyRepository, IExpansesWriteOnlyRepository, IExpansesUpdateOnlyRepository
{
    private readonly CashFlowDbContext _dbContext;
    
    public ExpansesRepository(CashFlowDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task Add(Expanse expanse)
    {
        await _dbContext.Expanses.AddAsync(expanse);
    }

    public async Task Delete(long id)
    {
        var result = await _dbContext.Expanses.FindAsync(id);
        
        _dbContext.Expanses.Remove(result!);
    }
    
    public async Task<List<Expanse>> GetAll(User user)
    {
        return await _dbContext.Expanses.AsNoTracking().Where(expanse => expanse.UserId == user.Id).ToListAsync();
    }

    async Task<Expanse?> IExpansesReadOnlyRepository.GetById(User user,long id)
    {
        return await GetFullExpense()
            .AsNoTracking()
            .FirstOrDefaultAsync(expanse => expanse.Id == id && expanse.UserId == user.Id);
    }

    public async Task<List<Expanse>> FilterByMonth(User user,DateOnly date)
    {
        var startDate = new DateTime(date.Year, date.Month, 1, 0, 0, 0);
        var daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);
        var endDate = new DateTime(date.Year, date.Month, daysInMonth, 23, 59, 59);
        
        return await _dbContext
            .Expanses
            .AsNoTracking()
            .Where(expanse => expanse.UserId == user.Id && expanse.Date >= startDate && expanse.Date <= endDate)
            .OrderBy(expanse => expanse.Date)
            .ThenBy(expanse => expanse.Title)
            .ToListAsync();
    }

    async Task<Expanse?> IExpansesUpdateOnlyRepository.GetById(User user,long id)
    {
        return await GetFullExpense()
            .FirstOrDefaultAsync(expanse => expanse.Id == id && expanse.UserId == user.Id);
    }
    

    public void Update(Expanse expanse)
    {
        _dbContext.Expanses.Update(expanse);
    }

    private IIncludableQueryable<Expanse, ICollection<Tag>> GetFullExpense()
    {
        return _dbContext.Expanses
            .Include(expanse => expanse.Tags);
    }
}