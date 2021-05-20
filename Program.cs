using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace laba5_RIAA
{
    class Program
    {
        static double LeftRectangle(Func<double, double> f, double a, double b, int n)
        {
            var h = (b - a) / n;
            var sum = 0d;
            for (var i = 0; i < n; i++)
            {
                var x = a + i * h;
                sum += f(x);
            }

            return h * sum;
        }

        static double RightRectangle(Func<double, double> f, double a, double b, int n)
        {
            var h = (b - a) / n;
            var sum = 0d;
            for (var i = 1; i <= n; i++)
            {
                var x = a + i * h;
                sum += f(x);
            }

            return h * sum; 
        }

        static double CentralRectangle(Func<double, double> f, double a, double b, int n)
        {
            var h = (b - a) / n;
            var sum = (f(a) + f(b)) / 2;
            for (var i = 1; i < n; i++)
            {
                var x = a + i * h;
                sum += f(x);
            }

            return h * sum;
        }

        private static double Simpson(Func<double, double> f, double a, double b, int n)
        {
            var h = (b - a) / n;
            var sum1 = 0d;
            var sum2 = 0d;
            for (var k = 1; k <= n; k++)
            {
                var xk = a + k * h;
                if (k <= n - 1)
                {
                    sum1 += f(xk);
                }

                var xk_1 = a + (k - 1) * h;
                sum2 += f((xk + xk_1) / 2);
            }

            return h / 3d * (1d / 2d * f(a) + sum1 + 2 * sum2 + 1d / 2d * f(b));
        }

        private static double Trapezoid(Func<double, double> f, double a, double b, int n)
        {
            var h = (b - a) / n;
            var sum = 0d;
            for (var k = 0; k < n; k++)
            {
                var x = a + k * h;
                sum += f(x);
            }
            return h / 2 * (f(a + 0 * h) + 2 * sum + f(a + n * b)); 
        }


        static void Main(string[] args)
        {
            Run();
            Console.ReadKey();
        }


        static async void Run()
        {
            //локальная функция
            double f(double x) => Math.Cos(Math.PI * x / 2);

            int n = 10000;
            double a = 0;
            double b = -1;


            Task<double> t1 = Task.Run(() => LeftRectangle(f, a, b, n));
            Task<double> t2 = Task.Run(() => RightRectangle(f, a , b, n));
            Task<double> t3 = Task.Run(() => CentralRectangle(f, a , b, n));
            Task<double> t4 = Task.Run(() => Simpson(f, a, b, n));
            Task<double> t5 = Task.Run(() => Trapezoid(f, a, b, n));

            await Task.WhenAll(new[] { t1, t2, t3, t4, t5 });

            Console.WriteLine("Формула левых прямоугольников: {0}", t1.Result);
            Console.WriteLine("Формула правых прямоугольников: {0}", t2.Result);
            Console.WriteLine("Формула средних прямоугольников: {0}", t3.Result);
            Console.WriteLine("Формула Симпсона: {0}", t4.Result);
            Console.WriteLine("Формула трапеции: {0}", t5.Result);


        }


    }
}
