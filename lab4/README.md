# Лабораторная работа №4 — Задача 6

Вычисление (Σx + Σy + Σz) / 3N с использованием функциональной декомпозиции,
глобальной суммы и критических секций.

## Генерация входного файла (PowerShell)

    $rnd = New-Object System.Random 42
    $lines = 1..1000000 | ForEach-Object {
        "{0:F6} {1:F6} {2:F6}" -f ($rnd.NextDouble()*100), ($rnd.NextDouble()*100), ($rnd.NextDouble()*100)
    }
    $lines | Set-Content -Path points.txt -Encoding utf8

## Сборка и запуск

    dotnet build -c Release
    dotnet run -c Release -- points.txt <число_потоков>

число_потоков = 0 — последовательная версия.

## Примеры

    dotnet run -c Release -- points.txt 0
    dotnet run -c Release -- points.txt 2
    dotnet run -c Release -- points.txt 3
    dotnet run -c Release -- points.txt 8