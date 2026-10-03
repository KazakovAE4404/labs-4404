using System;
using System.Threading.Tasks;

public static class Task2
{
    public static void Run(int numThreads, string schedule)
    {
        const int n = 16000;

        Console.WriteLine($"=== Задача 2: n = {n}, потоков = {numThreads}, распределение = {schedule} ===");

        // Исходный массив: a[i] = i
        var a = new double[n];
        for (int i = 0; i < n; i++) a[i] = i;

        // Результирующий массив
        var b = new double[n];
        b[0] = a[0];
        b[n - 1] = a[n - 1];

        var options = new ParallelOptions { MaxDegreeOfParallelism = numThreads };

        switch (schedule.ToLower())
        {
            case "static":
                // Блочное статическое распределение: непрерывные диапазоны
                Parallel.For(0, numThreads, options, t =>
                {
                    int chunk = (n - 2 + numThreads - 1) / numThreads;
                    int start = 1 + t * chunk;
                    int end = Math.Min(1 + (t + 1) * chunk, n - 1);
                    for (int i = start; i < end; i++)
                        b[i] = (a[i - 1] + a[i] + a[i + 1]) / 3.0;
                });
                break;

            case "dynamic":
                // Динамическое распределение с блоком 1
                Parallel.For(1, n - 1, options, i =>
                {
                    b[i] = (a[i - 1] + a[i] + a[i + 1]) / 3.0;
                });
                break;

            case "guided":
                // Guided: размер блока динамически уменьшается
                int nextIndex = 1;
                object lockObj = new object();

                Parallel.For(0, numThreads, options, t =>
                {
                    while (true)
                    {
                        int start, count;

                        lock (lockObj)
                        {
                            if (nextIndex >= n - 1) return;

                            int remaining = (n - 1) - nextIndex;
                            count = Math.Max(1, remaining / (2 * numThreads));
                            start = nextIndex;
                            nextIndex += count;
                        }

                        int end = Math.Min(start + count, n - 1);
                        for (int i = start; i < end; i++)
                            b[i] = (a[i - 1] + a[i] + a[i + 1]) / 3.0;
                    }
                });
                break;

            default:
                Console.WriteLine($"Неизвестный тип распределения: {schedule}");
                Console.WriteLine("Допустимые значения: static, dynamic, guided");
                return;
        }

        // Вывод нескольких элементов для проверки
        Console.Write("b[0..4]     = ");
        for (int i = 0; i < 5; i++) Console.Write($"{b[i]:F3} ");
        Console.WriteLine();

        Console.Write("b[n-5..n-1] = ");
        for (int i = n - 5; i < n; i++) Console.Write($"{b[i]:F3} ");
        Console.WriteLine();
    }
}