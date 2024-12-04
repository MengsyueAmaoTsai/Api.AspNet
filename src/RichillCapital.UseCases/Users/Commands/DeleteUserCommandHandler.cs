using RichillCapital.Domain;
using RichillCapital.Domain.Abstractions.Repositories;
using RichillCapital.UseCases.Abstractions;

using RichillCapital.SharedKernel.Monads;
namespace RichillCapital.UseCases.Users.Commands;

internal sealed class DeleteUserCommandHandler(
    IRepository<User> _userRepository,
    IUnitOfWork _unitOfWork) : 
    ICommandHandler<DeleteUserCommand, Result>
{
    public async Task<Result> Handle(
        DeleteUserCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = UserId.From(command.UserId);

        if (validationResult.IsFailure)
        {
            return Result.Failure(validationResult.Error);
        }

        var id = validationResult.Value;

        var maybeUser = await _userRepository.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

        if (maybeUser.IsNull)
        {
            return Result.Failure(UserErrors.NotFound(id));
        }

        var user = maybeUser.Value;

        _userRepository.Remove(user);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success;
    }
}