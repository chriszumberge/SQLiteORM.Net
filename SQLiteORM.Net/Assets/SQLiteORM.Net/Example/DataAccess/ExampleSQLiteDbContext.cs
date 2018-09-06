public class ExampleSQLiteDbContext : SQLiteDbContext {

    public ExampleSQLiteDbContext() : base("ExampleDatabase2.db", new ExampleSQLiteConfiguration()) { }

    public SQLiteTable<TestUser> Users;
    public SQLiteTable<TestData> Data;
}
