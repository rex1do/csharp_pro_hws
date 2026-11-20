Console.OutputEncoding = System.Text.Encoding.UTF8;

string directory = args.Length > 0
    ? Path.GetFullPath(args[0])
    : Path.Combine(AppContext.BaseDirectory, "Samples");

if (!Directory.Exists(directory))
{
    Console.WriteLine($"Папка не найдена: {directory}");
    return;
}

string[] files = Directory.GetFiles(directory, "*", SearchOption.TopDirectoryOnly);
Array.Sort(files, StringComparer.OrdinalIgnoreCase);

var runner = new FileTaskRunner(new FileSpaceCounter());

try
{
    Console.WriteLine("Задание 1: три файла параллельно");
    FileRunResult? threeFiles = null;
    if (files.Length >= 3)
    {
        threeFiles = runner.CountParallel(files.Take(3));
        Print(threeFiles);
    }
    else
        Console.WriteLine("Для первого задания нужны минимум три файла.");

    Console.WriteLine("\nЗадание 2: все файлы из папки параллельно");
    FileRunResult folder = runner.CountFolder(directory);
    Print(folder);

    Console.WriteLine("\nЗадание 3: время параллельной обработки");
    if (threeFiles is not null)
        Console.WriteLine($"Первые три файла: {threeFiles.Elapsed.TotalMilliseconds:F3} мс");
    Console.WriteLine($"Вся папка (файлов: {folder.Files.Length}): {folder.Elapsed.TotalMilliseconds:F3} мс");
}
catch (AggregateException exception)
{
    foreach (Exception error in exception.Flatten().InnerExceptions)
        Console.Error.WriteLine($"Ошибка чтения: {error.Message}");
    Environment.ExitCode = 1;
}
catch (IOException exception)
{
    Console.Error.WriteLine($"Ошибка чтения: {exception.Message}");
    Environment.ExitCode = 1;
}

static void Print(FileRunResult result)
{
    foreach (FileSpaces file in result.Files)
        Console.WriteLine($"{Path.GetFileName(file.Path)}: {file.Count} пробелов");

    Console.WriteLine($"Всего: {result.Files.Sum(x => x.Count)} пробелов");
}
