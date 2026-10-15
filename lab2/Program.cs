using System;
using System.Diagnostics;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Использование: dotnet run -c Release -- <размер_матрицы> <число_потоков>");
            Console.WriteLine("  число_потоков = 0 -> последовательная версия");
            return;
        }

        int n = int.Parse(args[0]);
        int threads = int.Parse(args[1]);

        Console.WriteLine($"Размер матрицы: {n}x{n}");
        Console.WriteLine($"Потоков: {(threads == 0 ? "последовательно" : threads.ToString())}");

        double[,] a = new double[n, n];
        double[,] b = new double[n, n];
        double[,] c = new double[n, n];

        var rnd = new Random(42);
        for (int i = 0; i < n; i++)
            for (int j = 0; j < n; j++)
            {
                a[i, j] = rnd.NextDouble();
                b[i, j] = rnd.NextDouble();
            }

        var sw = Stopwatch.StartNew();

        if (threads == 0)
            MultiplySequential(a, b, c, n);
        else
            MultiplyParallel(a, b, c, n, threads);

        sw.Stop();

        Console.WriteLine($"Время выполнения: {sw.ElapsedMilliseconds} мс");

        double sum = 0;
        for (int i = 0; i < n; i++)
            for (int j = 0; j < n; j++)
                sum += c[i, j];
        Console.WriteLine($"Контрольная сумма: {sum:F4}");
    }

    static void MultiplySequential(double[,] a, double[,] b, double[,] c, int n)
    {
        for (int i = 0; i < n; i++)
            for (int j = 0; j < n; j++)
            {
                double s = 0;
                for (int k = 0; k < n; k++)
                    s += a[i, k] * b[k, j];
                c[i, j] = s;
            }
    }

    static void MultiplyParallel(double[,] a, double[,] b, double[,] c, int n, int threads)
    {
        var options = new ParallelOptions { MaxDegreeOfParallelism = threads };

        Parallel.For(0, n, options, i =>
        {
            for (int j = 0; j < n; j++)
            {
                double s = 0;
                for (int k = 0; k < n; k++)
                    s += a[i, k] * b[k, j];
                c[i, j] = s;
            }
        });
    }
}