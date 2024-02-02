namespace JetStreamApiMongoDb.Interfaces
{
    /// <summary>
    /// Interface for base model
    /// </summary>
    public interface IBaseModel
    {
        static List<(string, string)> foreignKeys;
    }
}