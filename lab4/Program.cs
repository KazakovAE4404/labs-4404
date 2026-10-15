using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

class Program
{
    // Глобальная сумма всех координат
    static double globalSum = 0;

    // Объект для критической секции
    static readonly object sumLock = new object();

    static void Main(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Использование: dotnet run -c Release -- <файл> <число_потоков>");
            Console.WriteLine("  число_потоков = 0 -> последовательно");
            return;
        }

        string path = args[0];
        int threads = int.Parse(args[1]);

        double[] xs, ys, zs;
        ReadPoints(path, out xs, out ys, out zs);
        int n = xs.Length;

        Console.WriteLine($"Файл: {path}");
        Console.WriteLine($"Точек: {n}");
        Console.WriteLine($"Потоков: {(threads == 0 ? "последовательно" : threads.ToString())}");

        globalSum = 0;

        var sw = Stopwatch.StartNew();

        if (threads == 0)
            ComputeSequential(xs, ys, zs, n);
        else
            ComputeFunctional(xs, ys, zs, n, threads);

        double result = globalSum / (3.0 * n);

        sw.Stop();

        Console.WriteLine($"Время выполнения: {sw.ElapsedMilliseconds} мс");
        Console.WriteLine($"Значение: {result:F6}");
    }

    static void ReadPoints(string path, out double[] xs, out double[] ys, out double[] zs)
    {
        var lines = File.ReadAllLines(path);
        xs = new double[lines.Length];
        ys = new double[lines.Length];
        zs = new double[lines.Length];

        for (int i = 0; i < lines.Length; i++)
        {
            var p = lines[i].Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            xs[i] = double.Parse(p[0], CultureInfo.InvariantCulture);
            ys[i] = double.Parse(p[1], CultureInfo.InvariantCulture);
            zs[i] = double.Parse(p[2], CultureInfo.InvariantCulture);
        }
    }

    // Последовательная версия
    static void ComputeSequential(double[] xs, double[] ys, double[] zs, int n)
    {
        for (int i = 0; i < n; i++)
        {
            globalSum += xs[i];
            globalSum += ys[i];
            globalSum += zs[i];
        }
    }

    // Функциональная декомпозиция с критическими секциями
    static void ComputeFunctional(double[] xs, double[] ys, double[] zs, int n, int threads)
    {
        var options = new ParallelOptions { MaxDegreeOfParallelism = threads };

        Parallel.Invoke(options,
            () =>
            {
                double localSum = 0;
                for (int i = 0; i < n; i++) localSum += xs[i];
                lock (sumLock) { globalSum += localSum; }
            },
            () =>
            {
                double localSum = 0;
                for (int i = 0; i < n; i++) localSum += ys[i];
                lock (sumLock) { globalSum += localSum; }
            },
            () =>
            {
                double localSum = 0;
                for (int i = 0; i < n; i++) localSum += zs[i];
                lock (sumLock) { globalSum += localSum; }
            }
        );
    }
}