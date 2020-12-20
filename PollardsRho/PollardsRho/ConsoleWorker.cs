using System;
using System.Collections.Generic;
using System.Text;

namespace PollardsRho
{
    public static class ConsoleWorker
    {
        static ConsoleWorker()
        {
            Console.OutputEncoding = Encoding.GetEncoding(65001);
        }

        public static (Point, Point, EllipticCurve) InputValues()
        {
            Console.WriteLine("Задание эллиптической кривой:");

            Console.Write("Введите коэффициент a: ");
            var a = int.Parse(Console.ReadLine());
            Console.Write("Введите коэффициент b: ");
            var b = int.Parse(Console.ReadLine());

            Console.Write("Введите порядок поля F(p): ");
            var fieldOrder = int.Parse(Console.ReadLine());
            Console.Write("Введите порядок подгруппы: ");
            var subgroupOrder = int.Parse(Console.ReadLine());

            Console.Write("Введите координаты точки P, разделяя их пробелом: ");
            var pCoord = Console.ReadLine().Split(' ');
            var p = new Point(int.Parse(pCoord[0]), int.Parse(pCoord[1]));

            Console.Write("Введите координаты точки Q, разделяя их пробелом: ");
            var qCoord = Console.ReadLine().Split(' ');
            var q = new Point(int.Parse(qCoord[0]), int.Parse(qCoord[1]));

            var curve = new EllipticCurve(a, b, fieldOrder, subgroupOrder, p);

            Console.WriteLine($"\ny\u00B2 \u2261 x\u00B3 + {a}x + {b} (mod {fieldOrder})");
            Console.WriteLine($"P = {p} \nQ = {q}\n");

            return (p, q, curve);
        }

        public static void PrintValues(Point p, Point q, EllipticCurve curve)
        {
            Console.WriteLine($"y\u00B2 \u2261 x\u00B3 + {curve.A}x + {curve.B} (mod {curve.FieldOrder})");
            Console.WriteLine($"P = {p} \nQ = {q}");
        }

        public static void PrintAnswer(Stack<(Point, int, int)> tortoiseStack, Stack<(Point, int, int)> hareStack, int logarithm, int subgroupOrder)
        {
            var tortoise = tortoiseStack.ToArray();
            var hare = hareStack.ToArray();

            Array.Reverse(tortoise);
            Array.Reverse(hare);

            Console.WriteLine();

            for (int i = 1; i < tortoise.Length; i++)
            {
                var x1 = tortoise[i].Item1;
                var x2 = hare[i * 2].Item1;

                if (i < tortoise.Length - 1)
                {
                    Console.WriteLine($"Шаг {i}: x1 = {x1}, x2 = {x2}");
                    continue;
                }

                var a1 = tortoise[i].Item2;
                var b1 = tortoise[i].Item3;
                var a2 = hare[i * 2].Item2;
                var b2 = hare[i * 2].Item3;

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Шаг {i}: x1 = {x1}, x2 = {x2}\n");
                Console.ResetColor();

                Console.WriteLine($"x \u2261 ({a1} - {a2})({b2} - {b1})^(-1) (mod {subgroupOrder}) \u2261 {logarithm} (mod {subgroupOrder})");
                Console.WriteLine($"Ответ: {logarithm}");
            }

            Console.ReadLine();
        }
    }
}
