using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using System.Configuration;
using System.Globalization;

namespace ItaliaTreni.Console;

public sealed class InputFileMap: ClassMap<InputFileCsvRow>
{
    public InputFileMap()
    {
        AutoMap(CultureInfo.GetCultureInfo(ConfigurationManager.AppSettings["CultureInfo"]));
    }
}
public sealed class InputFileCsvRow
{
    [Name("mm")]
    public string MM { get; set; }
    [Name("p1")]
    public string P1 { get; set; }
    [Name("p2")]
    public string P2 { get; set; }
    [Name("p3")]
    public string P3 { get; set; }
    [Name("p4")]
    public string P4 { get; set; }
}