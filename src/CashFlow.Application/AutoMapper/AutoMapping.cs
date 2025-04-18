using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Entities;

namespace CashFlow.Application.AutoMapper;

public class AutoMapping : Profile
{
    public AutoMapping()
    {
        RequestToEntity();
        EntityToResponse();
    }

    private void RequestToEntity()
    {
        CreateMap<RequestExpenseJson, Expanse>();
        CreateMap<RequestRegisterUserJson, User>()
            .ForMember(dest => dest.Password,  config => config.Ignore());
    }

    private void EntityToResponse()
    {
        CreateMap<Expanse, ResponseRegisteredExpenseJson>();
        CreateMap<Expanse, ResponseShortExpanseJson>();
        CreateMap<Expanse, ResponseExpenseJson>();
    }
}