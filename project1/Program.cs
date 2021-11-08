using System;
using System.Collections.Generic;
using System.Linq;

namespace project1
{
    class Program
    {
        public class Element
        {
            public int p { get; set; }
            public int q { get; set; }
            public decimal err { get; set; }
            public string type { get; set; }
        }
        const decimal CONST = 3.1415926m;
        //const decimal CONST = 0.584962500721156m;
        static int K = 100;
        static List<int> plist = new List<int>();
        static List<int> qlist = new List<int>();
        static List<decimal> xlist = new List<decimal>(1);
        static List<int> alist = new List<int>(1);
        static List<decimal> dlist = new List<decimal>(1);
        static List<Element> sortedByErrors = new List<Element>();

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
                Vdecimals(CONST);
                Console.WriteLine("---------------------");
                sortByErrors();
                plist.Clear();
                qlist.Clear();
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
                plist.Add(p);
                qlist.Add(q);
            }

            var pandqs = plist.Zip(qlist, (p, q) => (p, q));
            foreach (var pq in pandqs)
            {
                Console.Write("p/q = " + pq.Item1 + "/" + pq.Item2);
                sortedByErrors.Add(new Element
                {
                    p = pq.Item1,
                    q = pq.Item2,
                    err = Decimal.MaxValue,
                    type = "N"
                });
                clearArrays();
                Vdecimals(Decimal.Divide(pq.Item1, pq.Item2));
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
                if (dlist[i - 1] < 0.000001m)
                {
                    break;
                }
                xlist.Add(Decimal.Divide(1, dlist[i - 1]));
                alist.Add(Decimal.ToInt32(Decimal.Floor(xlist[i])));
                dlist.Add(xlist[i] - alist[i]);
            }
            if (alist[alist.Count - 1] == 1)
            {
                alist.RemoveAt(alist.Count - 1);
                alist[alist.Count - 1]++;
            }
            printVerig();
        }

        static void printVerig()
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
            Console.WriteLine("]");
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
            decimal lastError = Decimal.MaxValue;
            foreach (var el in sortedByErrors)
            {
                el.err = Math.Abs(CONST - Decimal.Divide(el.p, el.q));
                if (el.err <= lastError)
                {
                    el.type = "I";
                }
                lastError = el.err;
            }
            sortedByErrors = sortedByErrors.OrderBy(o => o.err).ToList();

            foreach (var v in sortedByErrors)
            {
                Console.WriteLine("p/q = " + v.p + "/" + v.q + "  e1: " + v.err + "  type: " + v.type);
            }
        }
    }
}
