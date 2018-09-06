using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UsageExample : MonoBehaviour {

	// Use this for initialization
	void Start () {
        ExampleSQLiteConnection connection = ExampleSQLiteDbContext.Database;

        Debug.Log(connection != null);
        Debug.Log(connection.Data.GetType().FullName);
        Debug.Log(connection.Users.GetType().FullName);

        bool successful = connection.Users.Insert(new TestUser
        {
            Age = 26,
            Id = Guid.NewGuid().ToString(),
            FirstName = "Christopher",
            LastName = "Zumberge"
        });
        Debug.Log("User insert success? " + successful);

        foreach (TestUser user in ExampleSQLiteDbContext.Database.Users.GetItems())
        {
            Debug.Log(String.Concat(user.Id, " | ", user.Name, " | ", user.Age));
        }

        TestData newData = new TestData
        {
            Id = 12,
            IsSomething = true,
            NestedData = new TestNestedData
            {
                Something = "One thing",
                SomethingElse = 32,
                Data = BitConverter.GetBytes(15)
            }
        };
        successful = connection.Data.Insert(newData);
        Debug.Log("Data insert success? " + successful);

        foreach (TestData data in ExampleSQLiteDbContext.Database.Data.GetItems())
        {
            Debug.Log(String.Concat(data.Id, " | ", data.IsSomething, " | ", data.NestedData.Something, " | ", data.NestedData.SomethingElse, " | ", 
                String.Join("", data.NestedData.Data.Select(x => x.ToString()).ToArray())));
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
