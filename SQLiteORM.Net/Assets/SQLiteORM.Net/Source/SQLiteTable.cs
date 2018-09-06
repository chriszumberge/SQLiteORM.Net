using Newtonsoft.Json;
using SQLiteDatabase;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SQLiteTable<T> where T : new()
{
    readonly SQLiteDB _db;
    readonly string _tableIdentifier;

    readonly System.Reflection.PropertyInfo[] _itemProperties;
    readonly System.Reflection.PropertyInfo _primaryKeyProperty;

    public SQLiteTable(SQLiteDB db, string tableIdentifier)
    {
        _db = db;
        _tableIdentifier = tableIdentifier;

        _itemProperties = typeof(T).GetProperties();

        foreach (var property in _itemProperties)
        {
            if (PropertyAttribute.GetCustomAttributes(typeof(PrimaryKeyAttribute), false).Length != 0)
            {
                _primaryKeyProperty = property;
                break;
            }
        }
    }

    public IEnumerable<T> GetItems()
    {
        return AggregateDbReaderItems(_db.GetAllData(_tableIdentifier));
    }

    //public T GetItem(string primaryKeyValue)
    //{

    //}

    public IEnumerable<T> Query(string query)
    {
        return AggregateDbReaderItems(_db.Select(query));
    }

    //public IEnumerable<T> Query(Expression<Func<T, bool>> queryPredicate)
    //{

    //}

    public bool Insert(T item)
    {
        bool successful = false;

        List<SQLiteDB.DB_DataPair> dataPairList = new List<SQLiteDB.DB_DataPair>();
        SQLiteDB.DB_DataPair data = new SQLiteDB.DB_DataPair();

        foreach (var property in _itemProperties)
        {
            if (property.GetCustomAttributes(typeof(ColumnIgnoreAttribute), false).Length == 0)
            {
                data.fieldName = property.Name;
                data.value = ConvertValueToString(item, property);
                dataPairList.Add(data);
            }
        }

        int changeCount = _db.Insert(_tableIdentifier, dataPairList);

        if (changeCount > 0)
        {
            successful = true;
        }

        return successful;
    }

    public bool Update(T item)
    {
        bool successful = false;

        List<SQLiteDB.DB_DataPair> dataPairList = new List<SQLiteDB.DB_DataPair>();
        SQLiteDB.DB_DataPair data = new SQLiteDB.DB_DataPair();
        SQLiteDB.DB_ConditionPair condition = new SQLiteDB.DB_ConditionPair();

        foreach (var property in _itemProperties)
        {
            // if the property isn't being ignored
            if (property.GetCustomAttributes(typeof(ColumnIgnoreAttribute), false).Length == 0)
            {
                // if isn't primary key
                if (property.GetCustomAttributes(typeof(PrimaryKeyAttribute), false).Length == 0)
                {
                    data.fieldName = property.Name;
                    data.value = ConvertValueToString(item, property);
                    dataPairList.Add(data);
                }
                // else is, it can't be updated but instead used for finding record to update
                else
                {
                    condition.fieldName = property.Name;
                    condition.value = ConvertValueToString(item, property);
                    condition.condition = SQLiteDB.DB_Condition.EQUAL_TO;
                }
            }
        }

        if (_db.Update(_tableIdentifier, dataPairList, condition) > 0)
        {
            successful = true;
        }

        return successful;
    }

    public bool Delete(string primaryKeyValue)
    {
        bool successful = false;

        SQLiteDB.DB_ConditionPair condition = new SQLiteDB.DB_ConditionPair();

        condition.fieldName = _primaryKeyProperty.Name;
        condition.value = primaryKeyValue;
        condition.condition = SQLiteDB.DB_Condition.EQUAL_TO;

        if (_db.DeleteRow(_tableIdentifier, condition) > 0)
        {
            successful = true;
        }

        return successful;
    }

    public void Truncate()
    {
        _db.ClearTable(_tableIdentifier);
    }

    private static string ConvertValueToString(T item, System.Reflection.PropertyInfo property)
    {
        string strValue;
        if (property.PropertyType.Equals(typeof(double)) ||
            property.PropertyType.Equals(typeof(float)) ||
            property.PropertyType.Equals(typeof(int)) ||
            property.PropertyType.Equals(typeof(long)) ||
            property.PropertyType.Equals(typeof(short)) ||
            property.PropertyType.Equals(typeof(string)))
        {
            strValue = property.GetValue(item).ToString();
        }
        else
        {
            strValue = JsonConvert.SerializeObject(property.GetValue(item));
        }

        return strValue;
    }

    private List<T> AggregateDbReaderItems(DBReader reader)
    {
        List<T> items = new List<T>();
        while (reader != null && reader.Read())
        {
            T item = new T();

            foreach (var property in _itemProperties)
            {
                string propertyName = property.Name;

                var fieldNameAttribute = property.GetCustomAttributes(typeof(FieldNameAttribute), false).FirstOrDefault() as FieldNameAttribute;
                if (fieldNameAttribute != null)
                {
                    propertyName = fieldNameAttribute.FieldName;
                }

                if (property.PropertyType.Equals(typeof(double)))
                {
                    property.SetValue(item, reader.GetDoubleValue(property.Name));
                }
                else if (property.PropertyType.Equals(typeof(float)))
                {
                    property.SetValue(item, reader.GetFloatValue(property.Name));
                }
                else if (property.PropertyType.Equals(typeof(int)))
                {
                    property.SetValue(item, reader.GetIntValue(property.Name));
                }
                else if (property.PropertyType.Equals(typeof(long)))
                {
                    property.SetValue(item, reader.GetLongValue(property.Name));
                }
                else if (property.PropertyType.Equals(typeof(short)))
                {
                    property.SetValue(item, reader.GetShortValue(property.Name));
                }
                else if (property.PropertyType.Equals(typeof(string)))
                {
                    property.SetValue(item, reader.GetStringValue(property.Name));
                }
                else
                {
                    // was serialized
                    object deserializedValue = JsonConvert.DeserializeObject(reader.GetStringValue(property.Name), property.PropertyType);
                    property.SetValue(item, deserializedValue);
                }
            }

            items.Add(item);
        }

        return items;
    }
}
