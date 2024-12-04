using RichillCapital.Domain;
using RichillCapital.Domain.Abstractions.Clock;
using RichillCapital.Domain.Abstractions.Repositories;
using RichillCapital.UseCases.Abstractions;

using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.UseCases.Users.Commands;

internal sealed class CreateUserCommandHandler(
    IDateTimeProvider _dateTimeProvider,
    IRepository<User> _userRepository,
    IUnitOfWork _unitOfWork) :
    ICommandHandler<CreateUserCommand, ErrorOr<UserId>>
{
    public async Task<ErrorOr<UserId>> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        var validationResult = Result<(Email, UserName)>.Combine(
            Email.From(command.Email),
            UserName.From(command.Name));

        if (validationResult.IsFailure)
        {
            return ErrorOr<UserId>.WithError(validationResult.Error);
        }

        var (email, name) = validationResult.Value;

        var duplicateEmail = await _userRepository.AnyAsync(
            u => u.Email == email,
            cancellationToken);

        if (duplicateEmail)
        {
            return ErrorOr<UserId>.WithError(UserErrors.EmailTaken(email));
        }

        var errorOrUser = User.Create(
            UserId.NewUserId(),
            email,
            name,
            command.Password,
            _dateTimeProvider.UtcNow);

        if (errorOrUser.HasError)
        {
            return ErrorOr<UserId>.WithError(errorOrUser.Errors);
        }

        var user = errorOrUser.Value;

        _userRepository.Add(user);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ErrorOr<UserId>.With(user.Id);
    }
}
