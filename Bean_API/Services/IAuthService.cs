namespace Bean_API.Services
{
    public interface IAuthService
    {
        string GenerateToken(string username);
    }
}