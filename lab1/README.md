\# Лабораторная работа №1 (Задачи 1–3)



Один проект, три задачи — каждая в отдельном файле.



\## Сборка

dotnet build -c Release



\## Запуск



\### Задача 1

cd lab1

dotnet run -c Release -- task1 8



\### Задача 2

cd lab1

dotnet run -c Release -- task2 8 static

dotnet run -c Release -- task2 8 dynamic

dotnet run -c Release -- task2 8 guided



\### Задача 3

cd lab1

dotnet run -c Release -- task3 8



\## Структура

\- Program.cs — точка входа

\- Task1.cs — задача 1

\- Task2.cs — задача 2

\- Task3.cs — задача 3 (6 способов)

