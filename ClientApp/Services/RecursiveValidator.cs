using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

public interface IValidator<T>
{
    bool IsValid(T obj, out List<ValidationResult> results);
}


public class RecursiveValidator<T> : IValidator<T>
{
    public bool IsValid(T obj, out List<ValidationResult> results)
    {
        return IsValidRecursive(obj, out results);
    }

    private bool IsValidRecursive(object obj, out List<ValidationResult> results)
    {
        results = new List<ValidationResult>();
        var context = new ValidationContext(obj);
        bool isValid = Validator.TryValidateObject(obj, context, results, true);

        foreach (var property in obj.GetType().GetProperties())
        {
            var value = property.GetValue(obj);
            if (value == null || property.PropertyType.IsPrimitive || property.PropertyType == typeof(string))
                continue;

            if (value is IEnumerable<object> collection)
            {
                foreach (var item in collection)
                {
                    if (!IsValidRecursive(item, out var nestedResults))
                    {
                        isValid = false;
                        results.AddRange(nestedResults);
                    }
                }
            }
            else
            {
                if (!IsValidRecursive(value, out var nestedResults))
                {
                    isValid = false;
                    results.AddRange(nestedResults);
                }
            }
        }

        return isValid;
    }
}

