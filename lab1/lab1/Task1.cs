using System;
using System.Threading;
using System.Threading.Tasks;

public static class Task1
{
    public static void Run(int numThreads)
    {
        Console.WriteLine($"=== Задача 1: потоков = {numThreads} ===");
        Console.WriteLine($"Количество процессоров: {Environment.ProcessorCount}");

        var options = new ParallelOptions { MaxDegreeOfParallelism = numThreads };

        Parallel.For(0, numThreads, options, i =>
        {
            int tid = Thread.CurrentThread.ManagedThreadId;
            int total = numThreads;
            Console.WriteLine($"Поток {tid}: всего потоков = {total}, Hello World");
        });
    }
}