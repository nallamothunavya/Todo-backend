namespace Todo.Settings;

public class PostgresSettings
{

    public string Host { get; set; }

    public int Port { get; set; }

    public string Username { get; set; }

    public string Password { get; set; }

    public string Database { get; set; }

    // Use semicolon ; not colon :
    public string ConnectionString { get => $"Host={Host};Port={Port};Username={Username};Password={Password};Database={Database};Include Error Detail=true"; }
}