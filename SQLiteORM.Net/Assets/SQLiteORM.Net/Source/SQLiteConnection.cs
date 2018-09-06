using SQLiteDatabase;
using System;
using System.Linq;

/// <summary>
/// 
/// </summary>
public abstract class SQLiteConnection : IDisposable
{
    readonly SQLiteConfiguration _config;

    SQLiteDB database = SQLiteDB.Instance;

    public SQLiteConnection(string databaseName = "MyDatabase.db", SQLiteConfiguration config = null)
    {
        _config = config ?? new DefaultSQLiteConfiguration();

        database.DBLocation = _config.DatabaseLocation;
        database.DBName = databaseName.EndsWith(".db") ? databaseName : String.Concat(databaseName, ".db");

        InitializeDatabase();
    }

    void ConnectToDatabase(bool resetDatabase = false)
    {
        database.ConnectToDefaultDatabase(database.DBName, resetDatabase);
    }

    void CreateDatabase(bool forceCreate = true)
    {
        database.CreateDatabase(database.DBName, forceCreate);
    }

    void InitializeDatabase()
    {
        System.Diagnostics.Debug.WriteLine("Initializing Database");

        if (database.Exists)
        {
            System.Diagnostics.Debug.WriteLine("Database exists, connecting");
            ConnectToDatabase();
        }
        else
        {
            System.Diagnostics.Debug.WriteLine("Database does not exist, creating");
            CreateDatabase();
        }

        RunMigrations();
    }

    void RunMigrations()
    {
        // TODO if add/drop migrations or incremental

        System.Diagnostics.Debug.WriteLine("Creating and Migrating");

        foreach (var connectionField in this.GetType().GetFields())
        {
            if (connectionField.FieldType.Name.Equals("SQLiteTable`1"))
            {
                // rename variable for explicitness, now that it represents something else
                var tableField = connectionField;

                // get the generic type argument that the SQLiteTable is using, as that's the type of the table
                Type tableFieldGenericType = tableField.FieldType.GenericTypeArguments[0];
                System.Diagnostics.Debug.WriteLine(tableFieldGenericType.Name);

                // get the name of the field to be the name of the table, unless it has a custom table name attribute, then use that
                string tableName = tableField.Name;
                var tableNameAttribute = tableFieldGenericType.GetCustomAttributes(typeof(TableNameAttribute), false).FirstOrDefault() as TableNameAttribute;
                if (tableNameAttribute != null)
                {
                    // cleanse by replacing all spaces with underscores
                    // TODO rip out all symbols and other invalid characters
                    tableName = tableNameAttribute.TableName.Replace(" ", "_");
                }
                System.Diagnostics.Debug.WriteLine(tableName);

                // Create the schema for the new table to be created
                DBSchema schema = new DBSchema(tableName);

                // Only supporting a single primary key at the moment
                bool schemaHasPrimary = false;

                foreach (var tableProperty in tableFieldGenericType.GetProperties())
                {
                    if (tableProperty.GetCustomAttributes(typeof(ColumnIgnoreAttribute), false).Length == 0)
                    {
                        string columnName = tableProperty.Name;
                        var fieldNameAttribute = tableProperty.GetCustomAttributes(typeof(FieldNameAttribute), false).FirstOrDefault() as FieldNameAttribute;
                        if (fieldNameAttribute != null)
                        {
                            // cleanse by replacing all spaces with underscores
                            // TODO rip out all symbols and other invalid characters
                            columnName = fieldNameAttribute.FieldName.Replace(" ", "_");
                        }

                        SQLiteDB.DB_DataType dataType = 0;
                        int size = 0;
                        // If it's an int type...
                        if (tableProperty.PropertyType.Equals(typeof(Int16)) ||
                            tableProperty.PropertyType.Equals(typeof(Int32)) ||
                            tableProperty.PropertyType.Equals(typeof(Int64)))
                        {
                            dataType = SQLiteDB.DB_DataType.DB_INT;
                        }
                        else
                        {
                            // if not an int, then store as text unless a size was specified, then store as varchar
                            var sizeAttribute = tableProperty.GetCustomAttributes(typeof(SizeAttribute), false).FirstOrDefault() as SizeAttribute;
                            if (sizeAttribute != null)
                            {
                                dataType = SQLiteDB.DB_DataType.DB_VARCHAR;
                                size = sizeAttribute.FieldSize;
                            }
                            else
                            {
                                dataType = SQLiteDB.DB_DataType.DB_TEXT;
                            }
                        }

                        bool isPrimary = false;
                        if (tableProperty.GetCustomAttributes(typeof(PrimaryKeyAttribute), false).Length != 0)
                        {
                            if (schemaHasPrimary)
                            {
                                // TODO custom exception
                                throw new Exception("Multiple primary keys are not supported at this time");
                            }

                            isPrimary = true;
                            schemaHasPrimary = true;
                        }

                        bool isNotNull = false;
                        if (tableProperty.GetCustomAttributes(typeof(NotNullAttribute), false).Length != 0)
                        {
                            isNotNull = true;
                        }

                        bool isUnique = false;
                        if (tableProperty.GetCustomAttributes(typeof(UniqueAttribute), false).Length != 0 || isPrimary)
                        {
                            isUnique = true;
                        }

                        schema.AddField(columnName, dataType, size, isNotNull, isPrimary, isUnique);
                    }
                }

                if (database.IsTableExists(schema.TableName))
                {
                    System.Diagnostics.Debug.WriteLine("The " + schema.TableName + " table exists.");
                    // TODO.. migration update?
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Creating the " + schema.TableName + " table.");
                    database.CreateTable(schema);
                }

                // Set the value of the table back in this calss
                Type typeArgument = tableFieldGenericType;
                Type genericClass = typeof(SQLiteTable<>);
                Type constructedClass = genericClass.MakeGenericType(typeArgument);

                tableField.SetValue(this, Activator.CreateInstance(constructedClass, database, tableName));
            }
        }

        // Depending on type of migration, seed?
    }

    #region IDisposable Support
    public void Dispose()
    {
        database.Dispose();
    }
    #endregion
}
