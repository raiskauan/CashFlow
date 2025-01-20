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
    }

    private void EntityToResponse()
    {
        CreateMap<Expanse, ResponseRegisteredExpenseJson>();
        CreateMap<Expanse, ResponseShortExpanseJson>();
        CreateMap<Expanse, ResponseExpenseJson>();
    }
}