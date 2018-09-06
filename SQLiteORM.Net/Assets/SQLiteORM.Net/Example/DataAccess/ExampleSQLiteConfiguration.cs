using UnityEngine;

public class ExampleSQLiteConfiguration : SQLiteConfiguration
{
    public override string DatabaseLocation { get { return Application.persistentDataPath; } }

    public override LoggingLevel LoggingLevel { get { return LoggingLevel.TRACE; } }

    public override ISQLiteLogger SQLiteLogger { get { return new UnityConsoleSQLiteLogger(); } }
}

public class PickySQLiteConfiguration : DefaultSQLiteConfiguration
{
    public override LoggingLevel LoggingLevel { get { return LoggingLevel.TRACE;  } }
}