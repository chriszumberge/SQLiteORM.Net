public class ExampleSQLiteConnection : SQLiteConnection
{
    public ExampleSQLiteConnection() : base("ExampleDatabase2.db", new ExampleSQLiteConfiguration()) { }

    public SQLiteTable<TestUser> Users;
    public SQLiteTable<TestData> Data;
}
