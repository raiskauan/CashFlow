using AutoMapper;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories.Expanses;

namespace CashFlow.Application.UseCases.Expenses.GetAll;

public class GetAllExpenseUseCase  : IGetAllExpanseUseCase
{
    private readonly IExpansesReadOnlyRepository _repository;
    private readonly IMapper _mapper;
    
    public GetAllExpenseUseCase(IExpansesReadOnlyRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ResponseExpansesJson> Execute()
    {
        var result = await _repository.GetAll();

        return new ResponseExpansesJson
        {
            Expanses = _mapper.Map<List<ResponseShortExpanseJson>>(result)
        };
    }
}