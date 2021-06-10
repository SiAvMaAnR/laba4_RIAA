using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace laba5_RIAA
{
    class Program
    {
        static double LeftRectangle(Func<double, double> f, double a, double b, double eps)
        {
            double I1 = 0, I2;
            for (var h = (Math.Abs(a) + Math.Abs(b)) / 2.0; ; h /= 2.0)
            {
                I2 = 0.0;
                for (var x = a; x < b; x += h)
                {
                    I2 += f(x) * h;
                }

                if (Math.Abs(I2 - I1) <= eps)
                {
                    return (double)I2;
                }
                I1 = I2;
            }
        }

        static double RightRectangle(Func<double, double> f, double a, double b, double eps)
        {
            double I1 = 0, I2;
            for (var h = (Math.Abs(a) + Math.Abs(b)) / 2.0; ; h /= 2.0)
            {
                I2 = 0.0;
                for (var x = a + h; x <= b; x += h)
                {
                    I2 += f(x) * h;
                }

                if (Math.Abs(I2 - I1) <= eps)
                {
                    return (double)I2;
                }
                I1 = I2;
            }
        }

        static double CentralRectangle(Func<double, double> f, double a, double b, double eps)
        {
            double I1 = 0, I2;
            for (var h = (Math.Abs(a) + Math.Abs(b)) / 2.0; ; h /= 2.0)
            {
                I2 = 0.0;
                for (var x = a; x < b; x += h)
                {
                    I2 += f(x + h / 2) * h;
                }

                if (Math.Abs(I2 - I1) <= eps)
                {
                    return (double)I2;
                }
                I1 = I2;
            }
        }

        private static double Simpson(Func<double, double> f, double a, double b, double eps)
        {
            double I1 = 0, I2;
            for (var h = (Math.Abs(a) + Math.Abs(b)) / 2.0; ; h /= 2.0)
            {
                I2 = 0.0;
                for (var x = a+h; x <= b; x += h)
                {
                    I2 += (f(x - h) + 4 * f(x - h / 2) + f(x)) * h / 6;
                }

                if (Math.Abs(I2 - I1) <= eps)
                {
                    return (double)I2;
                }
                I1 = I2;
            }
        }

        private static double Trapezoid(Func<double, double> f, double a, double b, double eps)
        {
            double I1 = 0, I2;
            for (var h = (Math.Abs(a) + Math.Abs(b)) / 2.0; ; h /= 2.0)
            {
                I2 = 0.0;
                for (var x = a; x < b; x += h)
                {
                    I2 += (f(x) + f(x + h)) / 2 * h;
                }

                if (Math.Abs(I2 - I1) <= eps)
                {
                    return (double)I2;
                }
                I1 = I2;
            }
        }

        static void Main(string[] args)
        {
            Run();
            Task.Delay(100);
            Console.ReadKey();
        }


        static void Run()
        {
            //локальная функция
            double f(double x) => Math.Cos(2 * Math.PI * x);

            double a = 0;
            double b = 1.0 / 4;
            double eps = 0.00001;

            List<Task<double>> tasks = new List<Task<double>>();
            tasks.Add(new Task<double>(() => LeftRectangle(f, a, b, eps)));
            tasks.Add(new Task<double>(() => RightRectangle(f, a, b, eps)));
            tasks.Add(new Task<double>(() => CentralRectangle(f, a, b, eps)));
            tasks.Add(new Task<double>(() => Trapezoid(f, a, b, eps)));
            tasks.Add(new Task<double>(() => Simpson(f, a, b, eps)));

            tasks.ForEach(x => x.Start());

            Task.WaitAll(tasks.ToArray());

            Console.WriteLine($"Формула левых прямоугольников: {tasks[0].Result}");
            Console.WriteLine($"Формула правых прямоугольников: {tasks[1].Result}");
            Console.WriteLine($"Формула средних прямоугольников: {tasks[2].Result}");
            Console.WriteLine($"Формула трапеции: {tasks[3].Result}");
            Console.WriteLine($"Формула Симпсона: {tasks[4].Result}");
        }
    }
}
