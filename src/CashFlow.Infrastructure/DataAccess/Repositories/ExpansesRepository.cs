using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expanses;
using Microsoft.EntityFrameworkCore;

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

    public async Task<bool> Delete(long id)
    {
        var result = await _dbContext.Expanses.FirstOrDefaultAsync(expanse => expanse.Id == id);
        if (result is null)
        {
            return false;
        }
        
        _dbContext.Expanses.Remove(result);
        
        return true;
    }
    
    public async Task<List<Expanse>> GetAll()
    {
        return await _dbContext.Expanses.AsNoTracking().ToListAsync();
    }

    async Task<Expanse?> IExpansesReadOnlyRepository.GetById(long id)
    {
        return await _dbContext.Expanses.AsNoTracking().FirstOrDefaultAsync(expanse => expanse.Id == id);
    }

    public async Task<List<Expanse>> FilterByMonth(DateOnly date)
    {
        var startDate = new DateTime(date.Year, date.Month, 1, 0, 0, 0);
        var daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);
        var endDate = new DateTime(date.Year, date.Month, daysInMonth, 23, 59, 59);
        
        return await _dbContext.Expanses.AsNoTracking()
            .Where(expanse => expanse.Date >= startDate && expanse.Date <= endDate)
            .OrderBy(expanse => expanse.Date)
            .ThenBy(expanse => expanse.Title).ToListAsync();
    }

    async Task<Expanse?> IExpansesUpdateOnlyRepository.GetById(long id)
    {
        return await _dbContext.Expanses.FirstOrDefaultAsync(expanse => expanse.Id == id);
    }
    

    public void Update(Expanse expanse)
    {
        _dbContext.Expanses.Update(expanse);
    }
}