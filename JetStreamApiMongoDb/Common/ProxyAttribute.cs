namespace JetStreamApiMongoDb.Common
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ProxyAttribute : Attribute
    {
        public string? ForeignKey { get; set; }

        public string? PluralName { get; set; }

        public ProxyAttribute(string? pluralName = null, string? foreignKey = null)
        {
            PluralName = pluralName;
            ForeignKey = foreignKey;
        }
    }
}
