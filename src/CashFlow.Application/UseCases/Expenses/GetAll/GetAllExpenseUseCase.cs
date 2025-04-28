using AutoMapper;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories.Expanses;
using CashFlow.Domain.Services.ILoggedUser;

namespace CashFlow.Application.UseCases.Expenses.GetAll;

public class GetAllExpenseUseCase  : IGetAllExpanseUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IExpansesReadOnlyRepository _repository;
    private readonly IMapper _mapper;
    public GetAllExpenseUseCase(IExpansesReadOnlyRepository repository, IMapper mapper, ILoggedUser loggedUser)
    {
        _loggedUser = loggedUser;
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ResponseExpansesJson> Execute()
    {
        var loggedUser = await _loggedUser.Get();
        
        var result = await _repository.GetAll(loggedUser);

        return new ResponseExpansesJson
        {
            Expenses = _mapper.Map<List<ResponseShortExpanseJson>>(result)
        };
    }
}