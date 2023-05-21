namespace irods_Csharp;

public static class PathUtil
{
    public static string First(string path) => path[..path.LastIndexOf('/')];

    public static string Last(string path) => path[(path.LastIndexOf('/') + 1)..];

    public static string PathCombine(string l, string r)
    {
        while (r.StartsWith("../"))
        {
            r = r[3..];
            l = l[..l.LastIndexOf('/')];
        }

        return r == string.Empty ? l : $"{l}/{r}";
    }
}