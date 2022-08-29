using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Blabalacar.Validations;

[AttributeUsage(AttributeTargets.Property | 
                AttributeTargets.Field, AllowMultiple = false)]
public class NameValidationAttribute: ValidationAttribute
{
    private readonly string _regex;

    public NameValidationAttribute(string regex)
    {
        _regex = regex;
    }

    public override bool IsValid(object? value)
    {
        var a1 = value.ToString()!;
        var b = Regex.IsMatch(a1, _regex);
        var a = value != null && b;
        return a;
    }
}