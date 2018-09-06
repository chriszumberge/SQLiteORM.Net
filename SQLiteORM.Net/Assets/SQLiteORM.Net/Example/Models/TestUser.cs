using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUser {
    [PrimaryKey]
    public string Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    [FieldName("User Age")]
    public int Age { get; set; }

    [ColumnIgnore]
    public string Name
    {
        get { return string.Concat(FirstName, " ", LastName); }
    }
}
