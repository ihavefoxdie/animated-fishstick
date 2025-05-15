using System.Text.RegularExpressions;

namespace UserManagement.Utils;

public static partial class RegularExpressions
{
    [GeneratedRegex("^[A-Za-z0-9]+$")]
    public static partial Regex GetLatinAndNumbers();
    [GeneratedRegex("^[A-Za-zА-Яа-я]")]
    public static partial Regex GetLatinAndCyrillic();
}
