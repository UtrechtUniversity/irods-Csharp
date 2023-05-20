namespace irods_Csharp;

public struct Metadata
{
    public string Name;
    public string Value;
    public int? Units;

    /// <summary>
    /// Constructor for metadata
    /// </summary>
    /// <param name="name">Metadata name</param>
    /// <param name="value">Metadata value</param>
    /// <param name="units">Metadata units, these are optional</param>
    public Metadata(string name, string value, int? units)
    {
        Name = name;
        Value = value;
        Units = units;
    }
}