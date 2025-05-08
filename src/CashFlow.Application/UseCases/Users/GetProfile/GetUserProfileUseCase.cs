using AutoMapper;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Services.ILoggedUser;

namespace CashFlow.Application.UseCases.Users.GetProfile;

public class GetUserProfileUseCase : IGetUserProfileUseCase
{
    private readonly IMapper _mapper;
    private readonly ILoggedUser _user;
    
    public GetUserProfileUseCase(IMapper mapper, ILoggedUser user)
    {
        _mapper = mapper;
        _user = user;
    }

    public async Task<ResponseProfileShortJson> Execute()
    {
        var user = await _user.Get();

        return _mapper.Map<ResponseProfileShortJson>(user);
    }
}