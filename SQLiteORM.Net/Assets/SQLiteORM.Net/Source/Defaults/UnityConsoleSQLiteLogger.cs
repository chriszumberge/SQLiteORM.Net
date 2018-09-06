using System;
using UnityEngine;

public class UnityConsoleSQLiteLogger : ISQLiteLogger {
    public void Error(string logMessage)
    {
        Debug.LogError(logMessage);
    }

    public void Fatal(string logMessage)
    {
        Debug.LogError(String.Concat("FATAL", Environment.NewLine, logMessage));
    }

    public void Log(string logMessage)
    {
        Debug.Log(logMessage);
    }

    public void Warn(string logMessage)
    {
        Debug.LogWarning(logMessage);
    }
}
