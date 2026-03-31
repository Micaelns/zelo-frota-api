using System.Text.RegularExpressions;

namespace Domain.ObjectValues;

public class Plate
{
    private const string Pattern = @"^[A-Z]{3}-?[0-9][A-Z0-9][0-9]{2}$";

    public string Value { get; set; }

    public Plate(string value) { 
        if ( string.IsNullOrEmpty(value)  ||  !Regex.IsMatch(value, Pattern) ) {
            throw new ArgumentException("Formato de placa inválido");
        }

        Value = value.Replace("-", "").ToUpper();
    }
}
