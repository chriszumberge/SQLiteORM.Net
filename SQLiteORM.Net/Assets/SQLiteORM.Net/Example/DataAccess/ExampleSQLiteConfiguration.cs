using UnityEngine;

public class ExampleSQLiteConfiguration : SQLiteConfiguration
{
    public override string DatabaseLocation
    {
        get
        {
            return Application.persistentDataPath;
        }
    }
}
