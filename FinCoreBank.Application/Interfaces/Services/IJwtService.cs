using FinCoreBank.Domain.Entities;

namespace FinCoreBank.Application.Interfaces.Services;

public interface IJwtService
{
    string GenerateToken(
        User user);
}