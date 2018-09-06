using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[TableName("TestDataCustomeTableName")]
public class TestData
{
    [PrimaryKey]
    public int Id { get; set; }
    public bool IsSomething { get; set; }
    public TestNestedData NestedData { get; set; }
}
