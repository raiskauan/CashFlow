using AutoMapper;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories.Expanses;
using CashFlow.Domain.Services.ILoggedUser;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Expenses.GetById;

public class GetByIdUseCase : IGetByIdUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IExpansesReadOnlyRepository _repository;
    private readonly IMapper _mapper;
    
    public GetByIdUseCase(IExpansesReadOnlyRepository repository, IMapper mapper, ILoggedUser loggedUser)
    {
        _loggedUser = loggedUser;
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ResponseExpenseJson> Execute(long id)
    {
        var loggedUser = await _loggedUser.Get();
        
       var result = await _repository.GetById(loggedUser,id);

       if (result is null)
       {
           throw new NotFoundException(ResourcesErrorMessage.EXPENSE_NOT_FOUND);
       }
       
       return _mapper.Map<ResponseExpenseJson>(result);
    }
}