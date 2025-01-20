using AutoMapper;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories.Expanses;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Expenses.GetById;

public class GetByIdUseCase : IGetByIdUseCase
{
    private readonly IExpansesReadOnlyRepository _repository;
    private readonly IMapper _mapper;
    
    public GetByIdUseCase(IExpansesReadOnlyRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ResponseExpenseJson> Execute(long id)
    {
       var result = await _repository.GetById(id);

       if (result is null)
       {
           throw new NotFoundException(ResourcesErrorMessage.EXPENSE_NOT_FOUND);
       }
       
       return _mapper.Map<ResponseExpenseJson>(result);
    }
}