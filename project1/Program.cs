using System;
using System.Collections.Generic;
using System.Linq;

namespace project1
{
    class Program
    {
        public class Fraction
        {
            public int p { get; set; }
            public int q { get; set; }
            public decimal err1 { get; set; }
            public decimal err2 { get; set; }
            public string type { get; set; }
        }
        //const decimal CONST = 3.1415926m;
        //const decimal CONST = 0.584962500721156m; //Salem number σ1
        const decimal CONST = 3.3598856m; //Reciprocal Fibonacci constant
        //const decimal CONST = 1.7320508m; //Theodorus' constant sqrt(3)
        static int K = 100;
        static List<decimal> xlist = new List<decimal>(1);
        static List<int> alist = new List<int>(1);
        static List<decimal> dlist = new List<decimal>(1);
        static List<Fraction> sortedByErrors = new List<Fraction>();

        static void Main(string[] args)
        {
            string cont = "y";
            while ((cont == "y" || cont == "Y" || cont.ToLower() == "yes") && cont != null)
            {
                Console.WriteLine("Enter n:");
                int n = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Enter m:");
                int m = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("You entered n:" + n + ", m:" + m);
                VMVapprox(n, m);
                Console.WriteLine("Constant is: " + CONST);
                Vdecimals(CONST);
                Console.WriteLine("\n---------------------");
                sortByErrors();
                sortedByErrors.Clear();
                Console.WriteLine("\nContinue? (y/n)");
                cont = Console.ReadLine();
            }
        }

        static void VMVapprox(int n, int m)
        {
            for (int i = n; i <= m; i++)
            {
                int q = i;
                int p = Decimal.ToInt32(Decimal.Round(Decimal.Multiply(CONST, q)));
                sortedByErrors.Add(new Fraction
                {
                    p = p,
                    q = q,
                    err1 = Decimal.MaxValue,
                    err2 = Decimal.MaxValue,
                    type = "N"
                });
            }

            Console.WriteLine("---------------------");
            clearArrays();
        }

        static void Vdecimals(decimal constant)
        {
            xlist[0] = constant;
            alist[0] = Decimal.ToInt32(Decimal.Floor(xlist[0]));
            dlist[0] = xlist[0] - alist[0];
            for (int i = 1; i < K; i++)
            {
                if (dlist[i - 1] < 0.00000001m)
                {
                    break;
                }
                xlist.Add(Decimal.Divide(1, dlist[i - 1]));
                alist.Add(Decimal.ToInt32(Decimal.Floor(xlist[i])));
                dlist.Add(xlist[i] - alist[i]);
            }
            if (alist[alist.Count - 1] == 1 && alist.Count != 1)
            {
                alist.RemoveAt(alist.Count - 1);
                alist[alist.Count - 1]++;
            }
            printVeriz();
        }

        static void printVeriz()
        {
            Console.Write("[");
            for (int i = 0; i < alist.Count; i++)
            {
                if (alist.Count == 1)
                {
                    Console.Write(alist[i]);
                }
                else if (i == 0)
                {
                    Console.Write(alist[0] + ";");
                }
                else if (i == alist.Count - 1)
                {
                    Console.Write(alist[i]);
                }
                else
                {
                    Console.Write(alist[i] + ",");
                }
            }
            Console.Write("]");
        }

        static void clearArrays()
        {
            xlist.Clear();
            alist.Clear();
            dlist.Clear();
            xlist.Add(0);
            alist.Add(0);
            dlist.Add(0);
        }

        static void sortByErrors()
        {
            decimal minError = Decimal.MaxValue;
            decimal minError2 = Decimal.MaxValue;
            foreach (var el in sortedByErrors)
            {
                int gcd = GCD(el.p, el.q);
                el.err1 = CONST - Decimal.Divide(el.p, el.q);
                el.err2 = Decimal.Multiply(el.q/gcd, CONST) - el.p/gcd;
                if (el.err1 <= minError)
                {
                    el.type = "I";
                }
                if (el.err2 <= minError2)
                {
                    el.type = "II";
                }
                if (Math.Abs(el.err1) < Math.Abs(minError))
                    minError = el.err1;
                if (Math.Abs(el.err2) < Math.Abs(minError2))
                    minError2 = el.err2;
            }
            sortedByErrors = sortedByErrors.OrderBy(o => Math.Abs(o.err1)).ToList();

            foreach (var v in sortedByErrors)
            {
                clearArrays();
                Console.Write("p/q = " + v.p + "/" + v.q + " ");
                Vdecimals(Decimal.Divide(v.p, v.q));
                Console.WriteLine("  error: " + v.err1 + "  type: " + v.type);
            }
        }

        static int GCD(int a, int b)
        {
            while (b > 0)
            {
                int rem = a % b;
                a = b;
                b = rem;
            }
            return a;
        }
    }
}
