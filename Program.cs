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
                for (var x = a+h; x <= b; x += h)
                {
                    I2 += f(x - h) * h;
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

        static double CentralRectangle(Func<double, double> f, double a, double b, double eps)
        {
            double I1 = 0, I2;
            for (var h = (Math.Abs(a) + Math.Abs(b)) / 2.0; ; h /= 2.0)
            {
                I2 = 0.0;
                for (var x = a; x <= b; x += h)
                {
                    I2 += f(x - h/2) * h;
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
                for (var x = a; x <= b; x += h)
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
                for (var x = a; x <= b; x += h)
                {
                    I2 += (f(x - h) + f(x)) / 2 * h;
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


        static async void Run()
        {
            //локальная функция
            double f(double x) => Math.Sin(2 * Math.PI * x);

            double a = 0;
            double b = 1.0/4;
            double eps = 0.001;

            Console.WriteLine("");
            Task<double> t1 = Task.Run(() => LeftRectangle(f, a, b, eps));
            Task<double> t2 = Task.Run(() => RightRectangle(f, a, b, eps));
            Task<double> t3 = Task.Run(() => CentralRectangle(f, a, b, eps));
            Task<double> t4 = Task.Run(() => Trapezoid(f, a, b, eps));
            Task<double> t5 = Task.Run(() => Simpson(f, a, b, eps));


            await Task.WhenAll(new[] { t1, t2, t3, t4, t5 });

            Console.WriteLine("Формула левых прямоугольников: {0}", t1.Result);
            Console.WriteLine("Формула правых прямоугольников: {0}", t2.Result);
            Console.WriteLine("Формула средних прямоугольников: {0}", t3.Result);
            Console.WriteLine("Формула трапеции: {0}", t4.Result);
            Console.WriteLine("Формула Симпсона: {0}", t5.Result);
        }
    }
}
