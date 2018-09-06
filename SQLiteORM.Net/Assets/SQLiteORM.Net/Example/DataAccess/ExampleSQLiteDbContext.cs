public class ExampleSQLiteDbContext : SQLiteDbContext {

    public ExampleSQLiteDbContext() : base("ExampleDatabase.db", new ExampleSQLiteConfiguration()) { }

    public SQLiteTable<TestUser> Users;
    public SQLiteTable<TestData> Data;
}
