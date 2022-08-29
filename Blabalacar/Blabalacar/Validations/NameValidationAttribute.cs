using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Blabalacar.Validations;

public class NameValidationAttribute: ValidationAttribute
{
    private string[] _allowdValues;

    public NameValidationAttribute(string[] values)
    {
        _allowdValues = values;
    }

    public override bool IsValid(object? value)
    {
        var specialSymbols = "/^[a-z0-9_-]{3,16}$/";
        return value != null && Regex.IsMatch(value.ToString()!, specialSymbols);
    }
}