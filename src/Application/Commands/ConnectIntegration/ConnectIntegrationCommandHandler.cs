using MediatR;
using PDH.Application.Interfaces;
using PDH.Shared.Infrastructure.Repositories;
using PDH.Shared.Infrastructure.Persistence;
using PDH.Modules.Identity;
using IOAuthTokenEncryptionService = PDH.Application.Interfaces.IOAuthTokenEncryptionService;

namespace PDH.Application.Commands.ConnectIntegration;

public class ConnectIntegrationCommandHandler : IRequestHandler<ConnectIntegrationCommand>
{
    private readonly IRepository<User> _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IOAuthTokenEncryptionService _encryptionService;

    public ConnectIntegrationCommandHandler(
        IRepository<User> userRepository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        IOAuthTokenEncryptionService encryptionService)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _encryptionService = encryptionService;
    }

    public async Task Handle(ConnectIntegrationCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user == null) throw new UnauthorizedAccessException("User not found");

        var encryptedAccess = _encryptionService.Encrypt(request.AccessToken);
        var encryptedRefresh = _encryptionService.Encrypt(request.RefreshToken);

        user.AddOAuthIntegration(request.Provider, encryptedAccess, encryptedRefresh, request.ExpiresAt);
        _userRepository.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
