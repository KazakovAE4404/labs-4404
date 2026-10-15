using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

class Program
{
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

        var sw = Stopwatch.StartNew();

        double sumX, sumY, sumZ;

        if (threads == 0)
            SumSequential(xs, ys, zs, n, out sumX, out sumY, out sumZ);
        else
            SumFunctional(xs, ys, zs, n, threads, out sumX, out sumY, out sumZ);

        sw.Stop();

        Console.WriteLine($"Время выполнения: {sw.ElapsedMilliseconds} мс");
        Console.WriteLine($"Геометрический центр: ({sumX / n:F6}, {sumY / n:F6}, {sumZ / n:F6})");
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

    static void SumSequential(double[] xs, double[] ys, double[] zs, int n,
                              out double sx, out double sy, out double sz)
    {
        sx = 0; sy = 0; sz = 0;
        for (int i = 0; i < n; i++)
        {
            sx += xs[i];
            sy += ys[i];
            sz += zs[i];
        }
    }
    static void SumFunctional(double[] xs, double[] ys, double[] zs, int n, int threads,
                              out double sx, out double sy, out double sz)
    {
        double rX = 0, rY = 0, rZ = 0;

        var options = new ParallelOptions { MaxDegreeOfParallelism = threads };

        Parallel.Invoke(options,
            () => { for (int i = 0; i < n; i++) rX += xs[i]; },
            () => { for (int i = 0; i < n; i++) rY += ys[i]; },
            () => { for (int i = 0; i < n; i++) rZ += zs[i]; }
        );

        sx = rX;
        sy = rY;
        sz = rZ;
    }
}