using MediatR;
using Microsoft.EntityFrameworkCore;
using PDH.Shared.Infrastructure;
using PDH.Shared.Kernel.Interfaces;
using BCrypt.Net;

namespace PDH.Application.Queries.Login;

public class LoginQueryHandler : IRequestHandler<LoginQuery, string?>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public LoginQueryHandler(ApplicationDbContext dbContext, IJwtTokenGenerator jwtTokenGenerator)
    {
        _dbContext = dbContext;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<string?> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return null;

        user.RecordLogin();
        await _dbContext.SaveChangesAsync(cancellationToken);

        return _jwtTokenGenerator.GenerateToken(user.Id, user.Email);
    }
}
