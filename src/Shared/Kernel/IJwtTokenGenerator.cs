namespace PDH.Shared.Kernel.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(Guid userId, string email);
}
