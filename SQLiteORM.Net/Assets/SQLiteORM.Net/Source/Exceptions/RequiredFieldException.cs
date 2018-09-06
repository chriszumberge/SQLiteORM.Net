using System;

public class RequiredFieldException : Exception {
    public RequiredFieldException(string fieldName) : base(String.Concat("No value was provided for Required field '" + fieldName + "'"))
    {
    }
}
