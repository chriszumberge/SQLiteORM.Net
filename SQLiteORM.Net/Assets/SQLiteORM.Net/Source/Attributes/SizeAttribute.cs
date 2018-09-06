using System;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class SizeAttribute : Attribute
{
    public int FieldSize { get; private set; }

    public SizeAttribute(int size)
    {
        FieldSize = size;
    }
}
