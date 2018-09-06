using System;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class FieldNameAttribute : Attribute
{
    public string FieldName { get; private set; }

    public FieldNameAttribute(string name)
    {
        FieldName = name;
    }
}
