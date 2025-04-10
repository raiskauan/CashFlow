using AutoMapper;
using CashFlow.Application.UseCases.Expenses.Register;
using CashFlow.Communication.Requests;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expanses;
using CashFlow.Domain.Services.ILoggedUser;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Expenses.Update;

public class UpdateExpanseUseCase : IUpdateExpanseUseCase
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IExpansesUpdateOnlyRepository _repository;
    private readonly ILoggedUser _loggedUser;
    
    public UpdateExpanseUseCase(IMapper mapper, IUnitOfWork unitOfWork, IExpansesUpdateOnlyRepository repository, ILoggedUser loggedUser)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _repository = repository;
        _loggedUser = loggedUser;
    }
    public async Task Execute(long id, RequestExpenseJson request)
    {
        Validate(request);

        var loggedUser = await _loggedUser.Get();
        
        var expense = await _repository.GetById(loggedUser,id);

        if (expense == null)
        {
            throw new NotFoundException(ResourcesErrorMessage.EXPENSE_NOT_FOUND);
        }
        
        _mapper.Map(request, expense);
        
        _repository.Update(expense);

        await _unitOfWork.Commit();
    }

    private static void Validate(RequestExpenseJson request)
    {
        var validator = new ExpanseValidator();
        
        var result = validator.Validate(request);

        if (result.IsValid == false)
        {
            var errorMessages = result.Errors.Select(x => x.ErrorMessage).ToList();
            
            throw new ErrorOnValidationException(errorMessages);
        }
    }

   
}