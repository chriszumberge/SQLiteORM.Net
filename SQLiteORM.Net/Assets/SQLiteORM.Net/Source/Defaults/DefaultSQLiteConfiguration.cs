using UnityEngine;

public class DefaultSQLiteConfiguration : SQLiteConfiguration
{
    public override string DatabaseLocation
    {
        get
        {
            return Application.persistentDataPath;
        }
    }
}
