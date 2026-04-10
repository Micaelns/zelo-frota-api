using System.Text.RegularExpressions;

namespace Domain.ObjectValues;

public class ZipCode
{
    private const string Pattern = @"^[0-9]{5}-?[0-9]{3}$";
    public string Value { get; set; }

    public ZipCode(string value)
    {
        if ( string.IsNullOrEmpty( value ) || !Regex.IsMatch(value, Pattern))
        {
            throw new ArgumentException("CEP inválido");
        }
        Value = value.Replace("-", "");
    }
}
