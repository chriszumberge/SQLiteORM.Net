public static class LogHelper {
    public static void Log(LoggingLevel level, SQLiteConfiguration config, string message)
    {
        if (((int)level) >= (int)config.LoggingLevel)
        {
            switch(level)
            {
                case LoggingLevel.TRACE:
                case LoggingLevel.LOG:
                    config.SQLiteLogger.Log(message);
                    break;
                case LoggingLevel.WARN:
                    config.SQLiteLogger.Warn(message);
                    break;
                case LoggingLevel.ERROR:
                    config.SQLiteLogger.Error(message);
                    break;
                case LoggingLevel.FATAL:
                    config.SQLiteLogger.Fatal(message);
                    break;
                default:
                    break;
            }
        }
    }
}
