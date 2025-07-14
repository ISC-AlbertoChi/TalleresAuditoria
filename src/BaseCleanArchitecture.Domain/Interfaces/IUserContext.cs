namespace BaseCleanArchitecture.Domain.Interfaces
{
    public interface IUserContext
    {
        int? GetCurrentUserId();
        string? GetCurrentUserEmail();
        int? GetCurrentUserEmpresaId();
    }
} 