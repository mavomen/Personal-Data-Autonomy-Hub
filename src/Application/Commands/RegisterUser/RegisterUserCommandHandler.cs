using MediatR;
using PDH.Shared.Infrastructure.Repositories;
using PDH.Shared.Infrastructure.Persistence;
using PDH.Modules.Identity;

namespace PDH.Application.Commands.RegisterUser;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Guid>
{
    private readonly IRepository<User> _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterUserCommandHandler(IRepository<User> userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        // Hash the password (using BCrypt)
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        var user = new User(Guid.NewGuid(), request.Email, passwordHash);

        await _userRepository.AddAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}
