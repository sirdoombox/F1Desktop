namespace F1Desktop.Misc;

public static class PathHelper
{
    public static string MoveUp(string path, int moves)
    {
        var span = path.AsSpan();
        for (int i = path.Length - 1, j = 0; i >= 0; i--)
        {
            if (span[i] is '\\')
                j++;
            if (j == moves)
                return span[..i].ToString();
        }
        return null;
    }
}