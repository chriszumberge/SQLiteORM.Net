using System;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class TableNameAttribute : Attribute
{
    public string TableName { get; private set; }

    public TableNameAttribute(string name)
    {
        TableName = name;
    }
}
