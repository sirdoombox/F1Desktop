using System.IO;
using System.Reflection;

namespace F1Desktop.Misc;

public static class PathHelper
{
    public static string MoveUp(string path, int moves = 1)
    {
        for (var i = 0; i < moves; i++)
            path = Path.GetDirectoryName(path);
        return path;
    }
}