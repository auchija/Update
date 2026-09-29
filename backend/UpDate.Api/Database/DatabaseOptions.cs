namespace UpDate.Api.Database;

public sealed class DatabaseOptions
{
    public const string ConnectionStringName = "Database";

    public string ConnectionString { get; set; } = string.Empty;
}
