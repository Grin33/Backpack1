/*
Задача о рюкзаке
Вместимость: 100кг  Даны предметы с разным весом и ценностью (в Файле Loot.txt)
Укомплектовать рюкзак с максимальной ценностью
Использование удельной ценности = использование жадного алгоритма, что запрещено при выполнении
*/


using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Channels;

namespace testtttt
{
    class Program
    {
        static void AnsPrint(Backpack back, TimeSpan sw)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            if (back.Most_Valuable == null)
            {
                Console.WriteLine("Нет Ответа");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Время выполнения программы {sw}");
            }
            else
            {
                Console.WriteLine("Список лучших вещей");
                PrintLoots(back.Most_Valuable);
                Console.WriteLine($"Суммарная ценность вещей {back.best_value}");
                Console.WriteLine($"Суммарный вес вещей {back.final_weight}");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Время выполнения программы {sw}");
            }
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine();
        }
        static void PrintLoots(List<Loot> loots)
        {
            foreach (Loot loot in loots)
                Console.WriteLine(loot);
            Console.WriteLine();
        }

        static List<Loot> StandartLoots()
        {
            var loots = new List<Loot>
        {
            new Loot("1", 2000m, 30m)
            ,new Loot("2",2500m,10m)
            ,new Loot("3",4000m,40m)
            ,new Loot("4",2100m,35m)
            ,new Loot("5",4000m,45m)
            ,new Loot("6",3000m,40m)
            ,new Loot("7",2500m,40m)
            ,new Loot("8",2900m,15m)
            ,new Loot("9",3000m,16m)
            ,new Loot("10",3000m,15.5m)
            ,new Loot("11",2700m,11m)
            ,new Loot("12",3500m,39m)
        };
            return loots;
        }
        static void Main()
        {
            var cap = new BackpackCap("Default");
            var loots = StandartLoots();
            PrintLoots(loots);

            Console.WriteLine("Последовательный алгоритм");
            var sw = Stopwatch.StartNew();
            sw.Start();
            var back = new Backpack(cap.Capacity);
            back.shuffle(ref loots);
            var straight = sw.Elapsed;
            AnsPrint(back, straight);
            sw.Stop();

            Console.WriteLine("Параллельный алгоритм");

            var sw1 = Stopwatch.StartNew();
            sw1.Start();
            var back1 = new Backpack(cap.Capacity);
            back1.parallel_shuffle(loots);
            var parallel = sw1.Elapsed;
            AnsPrint(back1, parallel);
            sw1.Stop();

            Console.WriteLine($"Время Для Параллельного выполнения:\t {parallel}");
            Console.WriteLine($"Время Для Последовательного Выполнения:\t {straight}");
        }
    }
}
