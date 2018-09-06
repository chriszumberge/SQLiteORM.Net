using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public static class Extensions
{
    public static object GetValue(this PropertyInfo property, object obj)
    {
        return property.GetValue(obj, null);
    }

    public static void SetValue(this PropertyInfo property, object obj, object value)
    {
        property.SetValue(obj, value, null);
    }

    public static object GetDefault(this Type type)
    {
        if (type.IsValueType)
        {
            return Activator.CreateInstance(type);
        }
        return null;
    }
}
