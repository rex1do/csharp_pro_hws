using System.Diagnostics;

int[] sizes = { 100_000, 1_000_000, 10_000_000 };
Console.WriteLine($".NET {Environment.Version}, логических процессоров: {Environment.ProcessorCount}");
Console.WriteLine("Элементов | Сумма | Цикл, мс | Thread, мс | PLINQ, мс");

foreach (int size in sizes)
{
    int[] numbers = Enumerable.Range(1, size).ToArray();
    long expected = (long)size * (size + 1) / 2;

    double sequential = Measure(CalculateSequential, numbers, expected);
    double threads = Measure(CalculateWithThreads, numbers, expected);
    double plinq = Measure(CalculateWithPlinq, numbers, expected);
    Console.WriteLine($"{size} | {expected} | {sequential:F3} | {threads:F3} | {plinq:F3}");
}

static long CalculateSequential(int[] numbers)
{
    long sum = 0;
    foreach (int number in numbers)
        sum += number;

    return sum;
}

static long CalculateWithThreads(int[] numbers)
{
    int threadCount = Math.Min(Environment.ProcessorCount, numbers.Length);
    var threads = new List<Thread>(threadCount);
    object sumLock = new();
    long total = 0;

    for (int i = 0; i < threadCount; i++)
    {
        int start = (int)((long)i * numbers.Length / threadCount);
        int end = (int)((i + 1L) * numbers.Length / threadCount);
        var thread = new Thread(() =>
        {
            long localSum = 0;
            for (int j = start; j < end; j++)
                localSum += numbers[j];

            lock (sumLock)
                total += localSum;
        });

        threads.Add(thread);
        thread.Start();
    }

    foreach (Thread thread in threads)
        thread.Join();

    return total;
}

static long CalculateWithPlinq(int[] numbers)
{
    return numbers.AsParallel().Sum(number => (long)number);
}

static double Measure(Func<int[], long> calculate, int[] numbers, long expected)
{
    calculate(numbers); //прогрев перед замерами
    var times = new double[11];
    var stopwatch = new Stopwatch();
    for (int i = 0; i < times.Length; i++)
    {
        stopwatch.Restart();
        long sum = calculate(numbers);
        stopwatch.Stop();
        if (sum != expected)
            throw new InvalidOperationException($"Ожидалась сумма {expected}, получена {sum}");

        times[i] = stopwatch.Elapsed.TotalMilliseconds;
    }

    Array.Sort(times);
    return times[times.Length / 2];
}
