using System;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            PrintUsage();
            return;
        }

        string task = args[0].ToLower();
        int numThreads = args.Length > 1 ? int.Parse(args[1]) : 8;

        switch (task)
        {
            case "task1":
                Task1.Run(numThreads);
                break;

            case "task2":
                string schedule = args.Length > 2 ? args[2] : "static";
                Task2.Run(numThreads, schedule);
                break;

            case "task3":
                Task3.Run(numThreads);
                break;

            default:
                Console.WriteLine($"Неизвестная задача: {task}");
                PrintUsage();
                break;
        }
    }

    static void PrintUsage()
    {
        Console.WriteLine("Использование:");
        Console.WriteLine("  dotnet run -c Release -- task1 <numThreads>");
        Console.WriteLine("  dotnet run -c Release -- task2 <numThreads> <schedule>");
        Console.WriteLine("  dotnet run -c Release -- task3 <numThreads>");
        Console.WriteLine();
        Console.WriteLine("Примеры:");
        Console.WriteLine("  dotnet run -c Release -- task1 8");
        Console.WriteLine("  dotnet run -c Release -- task2 8 static");
        Console.WriteLine("  dotnet run -c Release -- task2 8 dynamic");
        Console.WriteLine("  dotnet run -c Release -- task2 8 guided");
        Console.WriteLine("  dotnet run -c Release -- task3 8");
    }
}