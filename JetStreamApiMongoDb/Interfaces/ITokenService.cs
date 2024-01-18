namespace JetStreamApiMongoDb.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(string username, string role);
    }
}