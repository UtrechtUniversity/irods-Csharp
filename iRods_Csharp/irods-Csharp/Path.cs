namespace irods_Csharp;

public class Path
{
    private readonly string _path;

    public Path(string path)
    {
        _path = path;
    }

    public static string First(string path) => path[..path.LastIndexOf('/')];

    public static string Last(string path) => path[(path.LastIndexOf('/') + 1)..];

    public override string ToString() => _path;

    public static Path operator +(Path l, Path r) => new (l + r.ToString());

    public static implicit operator string(Path path) => path.ToString();

    public static string operator +(Path l, string r)
    {
        string left = l.ToString();
        if (!r.StartsWith("/") && r != "") r = "/" + r;
        while (r.StartsWith("/.."))
        {
            r = r[3..];
            left = l._path[..left.LastIndexOf('/')];
        }

        return left + r;
    }
}