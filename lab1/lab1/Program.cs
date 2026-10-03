using System;
using System.Threading;
using System.Threading.Tasks;

class Lab1
{
    static void Main(string[] args)
    {
        int numThreads = args.Length > 0 ? int.Parse(args[0]) : 4;

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