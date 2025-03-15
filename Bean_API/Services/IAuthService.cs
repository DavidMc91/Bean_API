namespace Bean_API.Services
{
    public interface IAuthService
    {
        public string GenerateToken(string username);
    }
}