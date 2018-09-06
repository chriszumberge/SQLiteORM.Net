using UnityEngine;

public class SQLiteDbContext<T> : MonoBehaviour where T : SQLiteConnection, new()
{
    //private static readonly Lazy<T> lazy = new System.Lazy<T>(() => new T());
    private static readonly T instance = new T();

    public static T Database { get { return instance; } }

    private void OnApplicationQuit()
    {
        Database.Dispose();
    }
}
