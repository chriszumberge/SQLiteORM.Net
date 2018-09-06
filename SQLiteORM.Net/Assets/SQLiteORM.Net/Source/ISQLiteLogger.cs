using System;

public interface ISQLiteLogger
{
    void Log(string logMessage);

    void Warn(string logMessage);

    void Error(string logMessage);

    void Fatal(string logMessage);
}
