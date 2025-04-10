using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expanses;
using CashFlow.Domain.Services.ILoggedUser;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Expenses.Delete;

public class DeleteExpansesUseCase : IDeleteExpansesUseCase
{
    private readonly IExpansesReadOnlyRepository _expansesReadOnlyRepository;
    private readonly IExpansesWriteOnlyRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILoggedUser _loggedUser;

    public DeleteExpansesUseCase(IExpansesWriteOnlyRepository repository, IUnitOfWork unitOfWork, ILoggedUser loggedUser, IExpansesReadOnlyRepository expansesReadOnlyRepository)
    {
        _expansesReadOnlyRepository = expansesReadOnlyRepository;
        _repository = repository;
        _unitOfWork = unitOfWork;
        _loggedUser = loggedUser;
    }

    public async Task Execute(long id)
    {
        var loggedUser = await _loggedUser.Get();

        var expanses = await _expansesReadOnlyRepository.GetById(loggedUser, id);

        if (expanses is null)
        {
            throw new NotFoundException(ResourcesErrorMessage.EXPENSE_NOT_FOUND);
        }

        await _repository.Delete(id);

        await _unitOfWork.Commit();
    }
}

    