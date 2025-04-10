using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Domain.Services.ILoggedUser;
using CashFlow.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.Services.LoggedUser;

public class LoggedUser : ILoggedUser
{
    private readonly CashFlowDbContext _dbcontext;
    private readonly ITokenProvider _tokenProvider;

    public LoggedUser(CashFlowDbContext dbcontext, ITokenProvider tokenProvider)
    {
        _dbcontext = dbcontext;
        _tokenProvider = tokenProvider;
    }
    
    public async Task<User> Get()
    {
        var token = _tokenProvider.TokenOnRequest();

        var tokenHandler = new JwtSecurityTokenHandler();
        
        var jwtSecurityToken = tokenHandler.ReadJwtToken(token);

        var identifier = jwtSecurityToken.Claims.First(claim => claim.Type == ClaimTypes.Sid).Value;
        
        return (await _dbcontext.User.AsNoTracking().FirstOrDefaultAsync(user => user.UserIdentifier == Guid.Parse(identifier)))!;
    }
}