using UnityEngine;

public class DefaultSQLiteConfiguration : SQLiteConfiguration
{
    public override string DatabaseLocation { get { return Application.persistentDataPath;  } }

    public override LoggingLevel LoggingLevel { get { return LoggingLevel.WARN; } }

    public override ISQLiteLogger SQLiteLogger { get { return new UnityConsoleSQLiteLogger(); } }
}
