# Лабораторная работа №3 — Задача 5

Вычисление геометрического центра множества точек в 3D.

Две версии:
- loop/ — распараллеливание цикла (Parallel.For).
- func/ — функциональная декомпозиция (Parallel.Invoke).

## Генерация входного файла (PowerShell)

    $rnd = New-Object System.Random 42
    $lines = 1..1000000 | ForEach-Object {
        "{0:F6} {1:F6} {2:F6}" -f ($rnd.NextDouble()*100), ($rnd.NextDouble()*100), ($rnd.NextDouble()*100)
    }
    $lines | Set-Content -Path points.txt -Encoding utf8

## Сборка и запуск

    cd loop
    dotnet build -c Release
    dotnet run -c Release -- ..\points.txt <число_потоков>

    cd ..\func
    dotnet build -c Release
    dotnet run -c Release -- ..\points.txt <число_потоков>

число_потоков = 0 — последовательная версия.

## Примеры

Версия с распараллеливанием цикла:

    cd loop
    dotnet run -c Release -- ..\points.txt 0
    dotnet run -c Release -- ..\points.txt 4

Версия с функциональной декомпозицией:

    cd ..\func
    dotnet run -c Release -- ..\points.txt 0
    dotnet run -c Release -- ..\points.txt 3
