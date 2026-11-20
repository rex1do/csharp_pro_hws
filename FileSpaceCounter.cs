public sealed class FileSpaceCounter
{
    public FileSpaces Count(string path)
    {
        long spaces = 0;
        using var reader = new StreamReader(path);
        char[] buffer = new char[16 * 1024];
        int length;

        while ((length = reader.Read(buffer, 0, buffer.Length)) > 0)
        {
            for (int i = 0; i < length; i++)
                if (buffer[i] == ' ')
                    spaces++;
        }

        return new FileSpaces(path, spaces);
    }
}

public sealed record FileSpaces(string Path, long Count);
