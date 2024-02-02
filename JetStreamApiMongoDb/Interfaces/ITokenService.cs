namespace JetStreamApiMongoDb.Interfaces
{
    /// <summary>
    /// Interface for token service
    /// </summary>
    public interface ITokenService
    {
        string GenerateToken(string username, string role);
    }
}