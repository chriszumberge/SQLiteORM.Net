# SQLiteORM.Net
SQLiteORM.Net is a Code First SQLite ORM written for .NET for use in Unity projects.

## Getting Started
Download the `SQLiteORM.Net.Core.unitypackage` and import it into your project.

You will also need to download the two dependencies, I unfortunately could not package the SQLite Database
code since it's a paid asset available on the [Unity Asset Store](https://assetstore.unity.com/packages/tools/integration/sqlite-database-40375).

I chose SQLite because it's extremely lightweight and most importantly it supports iOS, Android, PC and Mac.

JsonDotNet is **free** and also available on the [Unity Asset Store](https://assetstore.unity.com/packages/tools/input-management/json-net-for-unity-11347).

The `SQLiteORM.Net.unitypackage` includes the JsonDotNet binaries.

#### Dependencies
- [SQLiteDatabase](https://assetstore.unity.com/packages/tools/integration/sqlite-database-40375) - $10
- [JsonDotNet](https://assetstore.unity.com/packages/tools/input-management/json-net-for-unity-11347) - FREE

#### Roadmap
- Error events
- Foreign keys and Navigations Properties
- Migrations
- Change tracking and SaveChanges()/Rollbacks

## Documentation

### Creating a Database
Databases are represented by the `SQLiteDbContext`{:.language-cs} abstract class.
To create your own database simply create a class inheriting from that base class,

```cs
public class ExampleDbContext : SQLiteDBContext {}
```

Then, in your code, simply create a new instance of your `SQLiteDBContext`.

```cs
using (var dbContext = new ExampleDbContext())
{
    // Db Code Here using the dbContext variable
}
```

### Defining Models to Store your Data
As a code-first solution, your tables are represented by models.
Simply create a class for the data that you want to store in your database,

```cs
public class User
{
    [PrimaryKey]
    public string Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public int Age { get; set; }
}
```

By default, the name of the class will be the name of the resulting table.
Additionally, the names of all the properties will be the names of all the columsn in that table.

### Adding Tables for your Models to your Database
Tables are represented by the `SQLiteTable<>`{:.language-cs} generic class.
Simply add a public `SQLiteTable`{:.language-cs} property whose generic argument is the type
of the model you want to create a table for,

```cs
public class ExampleDbContext : SQLiteDBContext
{
    public SQLiteTable<User> Users;
}
```

### Interacting with Tables in Code
```cs
using (var dbContext = new ExampleDbContext())
{
    string id = Guid.NewGuid().ToString();

    // Insert new data
    bool successful = dbContext.Users.Insert(new User
    {
        Id = id,
        Age = 26,
        FirstName = "Christopher",
        LastName = "Zumberge"
    });

    // Retrieve data
    IEnumerable<User> users = dbContext.Users.GetItems();

    // Update data
    

    // Delete data
    
}
```


### Foreign Keys and Navigation Properties
Coming soon....

### Configuring your Database and DbContext
For most all configuration changes to the database and dbcontext, you need to create a subclass
of the abstract `SQLiteConfiguration` class.
This will allow you to change all the parameters that the DbContext uses at runtime.

```cs
public class ExampleSQLiteConfiguration : SQLiteConfiguration
{
    public override string DatabaseLocation { get { return Application.persistentDataPath; } }

    public override LoggingLevel LoggingLevel { get { return LoggingLevel.TRACE; } }

    public override ISQLiteLogger SQLiteLogger { get { return new UnityConsoleSQLiteLogger(); } }
}
```

If you only wish to change a few of the parameters, inherit the `DefaultSQLiteConfiguration`
class instead, since all of the abstract fields are already given default values.

```cs
public class ExampleSQLiteConfiguration : DefaultSQLiteConfiguration
{
    public override LoggingLevel LoggingLevel { get { return LoggingLevel.TRACE; } }
}
```

#### Specify Database Name
To specifiy the name of your database, simply pass a database name string to the base
constructor of your `SQLiteDBContext` class,

```cs
public class ExampleDbContext : SQLiteDBContext {
    public ExampleDbContext() : base("ExampleDatabase.db"){ }
}
```

### Configuring your Data

#### Specifying a Table Name
To create a table for a model that has a different name than the class, use the `[TableName]` attribute,

```cs
[TableName("User")]
public class TestUser
{
    // ...
    // ...
}
```

This will create a table named "User" instead of "TestUser", as it would do by default.

#### Specifying a Field Name
To create a column in a table that has a different name than the property it maps to, use the `[FieldName]` attribute,

```cs
public class User
{
    // ...
    [FieldName("UserAge")]
    public int Age { get; set; }
    // ...
}
```

#### Ignore a Property on your Model
To **not** map a property to a table column, use the `[ColumnIgnore]` attribute,

```cs
public class User
{
    // ...
    public string FirstName { get; set; }
    public string LastName { get; set; }
    // ...
    [ColumnIgnore]
    public string Name => $"{FirstName} {LastName}"
}
```

### Setting Not Null, Primary Key, Unique, Required Flags
All of the above have attributes that you can decorate the intended properties with
```cs
public class User
{
    [PrimaryKey]
    public string Id { get; set; }
    [Unique]   
    public sting SSN { get; set; }
    [NotNull]
    public string FirstName { get; set; }
    [NotNull]
    public string LastName { get; set; }
}
```

### Restricting Data Size
There is a Size Attribute that allows you to specify the size of a field that will be mapped
back to varchar,

```cs
public class User
{
    // ...
    [Size(16)]
    [Unique]
    public string IdentificationNumber { get; set; }
    // ...
}
```

### Composite Primary Keys
Not yet supported...

### Default Values
Since the ORM is Code-First, provide a default value just by setting the class property 
equal to a value.

```cs
public class User
{
    // ...
    public string FirstName { get; set; } = String.Empty;
    // ...
}
```