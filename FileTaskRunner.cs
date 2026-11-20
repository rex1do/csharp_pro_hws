using System.Diagnostics;

public sealed class FileTaskRunner
{
    private readonly FileSpaceCounter _counter;

    public FileTaskRunner(FileSpaceCounter counter) => _counter = counter;

    public FileRunResult CountFolder(string directory)
    {
        if (!Directory.Exists(directory))
            throw new DirectoryNotFoundException(directory);

        string[] files = Directory.GetFiles(directory, "*", SearchOption.TopDirectoryOnly);
        Array.Sort(files, StringComparer.OrdinalIgnoreCase);
        return CountParallel(files);
    }

    public FileRunResult CountParallel(IEnumerable<string> paths)
    {
        var stopwatch = Stopwatch.StartNew();

        Task<FileSpaces>[] tasks = paths
            .Select(path => Task.Run(() => _counter.Count(path)))
            .ToArray();

        Task.WaitAll(tasks);
        stopwatch.Stop();

        return new FileRunResult(tasks.Select(task => task.Result).ToArray(), stopwatch.Elapsed);
    }
}

public sealed record FileRunResult(FileSpaces[] Files, TimeSpan Elapsed);
