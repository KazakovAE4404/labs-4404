using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public static class Task3
{
    public static void Run(int numThreads)
    {
        Console.WriteLine($"=== Задача 3: потоков = {numThreads} ===\n");

        Console.WriteLine("--- Способ 1: Barrier + массив ---");
        Method1(numThreads);
        Console.WriteLine();

        Console.WriteLine("--- Способ 2: ConcurrentBag + OrderByDescending ---");
        Method2(numThreads);
        Console.WriteLine();

        Console.WriteLine("--- Способ 3: Parallel.For по убыванию ---");
        Method3(numThreads);
        Console.WriteLine();

        Console.WriteLine("--- Способ 4: Task + AttachedToParent ---");
        Method4(numThreads);
        Console.WriteLine();

        Console.WriteLine("--- Способ 5: PLINQ AsOrdered + Reverse ---");
        Method5(numThreads);
        Console.WriteLine();

        Console.WriteLine("--- Способ 6: Interlocked + Barrier ---");
        Method6(numThreads);
        Console.WriteLine();
    }

    // Способ 1: Barrier + массив
    static void Method1(int n)
    {
        var ids = new int[n];
        var barrier = new Barrier(n);
        var threads = new Thread[n];

        for (int i = 0; i < n; i++)
        {
            int idx = i;
            threads[i] = new Thread(() =>
            {
                ids[idx] = idx;
                barrier.SignalAndWait();
                if (idx == 0)
                    for (int k = n - 1; k >= 0; k--)
                        Console.WriteLine($"Поток {ids[k]}");
            });
            threads[i].Start();
        }
        foreach (var t in threads) t.Join();
    }

    // Способ 2: ConcurrentBag + сортировка
    static void Method2(int n)
    {
        var bag = new ConcurrentBag<int>();
        var threads = new Thread[n];
        for (int i = 0; i < n; i++)
        {
            int idx = i;
            threads[i] = new Thread(() => bag.Add(idx));
            threads[i].Start();
        }
        foreach (var t in threads) t.Join();
        foreach (var id in bag.OrderByDescending(x => x))
            Console.WriteLine($"Поток {id}");
    }

    // Способ 3: Parallel.For по убыванию + печать в конце
    static void Method3(int n)
    {
        var ids = new int[n];
        Parallel.For(0, n, i =>
        {
            int reverseIdx = n - 1 - i;
            ids[reverseIdx] = reverseIdx;
        });
        for (int k = n - 1; k >= 0; k--)
            Console.WriteLine($"Поток {ids[k]}");
    }

    // Способ 4: Task + AttachedToParent
    static void Method4(int n)
    {
        var ids = new int[n];
        var parent = Task.Factory.StartNew(() =>
        {
            for (int i = 0; i < n; i++)
            {
                int idx = i;
                Task.Factory.StartNew(() => { ids[idx] = idx; },
                    TaskCreationOptions.AttachedToParent);
            }
        });
        parent.Wait();
        for (int k = n - 1; k >= 0; k--)
            Console.WriteLine($"Поток {ids[k]}");
    }

    // Способ 5: PLINQ + AsOrdered + Reverse
    static void Method5(int n)
    {
        var result = Enumerable.Range(0, n)
                               .AsParallel()
                               .AsOrdered()
                               .Select(i => i)
                               .ToArray();
        foreach (var id in result.Reverse())
            Console.WriteLine($"Поток {id}");
    }

    // Способ 6: Interlocked + Barrier
    static void Method6(int n)
    {
        int counter = -1;
        var ids = new int[n];
        var barrier = new Barrier(n);
        var threads = new Thread[n];

        for (int i = 0; i < n; i++)
        {
            threads[i] = new Thread(() =>
            {
                int myOrder = Interlocked.Increment(ref counter);
                int myReverseNumber = n - 1 - myOrder;
                ids[myReverseNumber] = myReverseNumber;
                barrier.SignalAndWait();
                if (myOrder == 0)
                    for (int k = 0; k < n; k++)
                        Console.WriteLine($"Поток {ids[k]}");
            });
            threads[i].Start();
        }
        foreach (var t in threads) t.Join();
    }
}