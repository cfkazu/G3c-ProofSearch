using System;
using System.Collections.Generic;
using System.IO;

namespace G3c
{
    class Program
    {
        static StreamWriter writer;
        static void Main(string[] args)
        {
            TansakuTest();
            //CodeTest();
        }
        static void CodeTest()
        {
            while (true)
            {
                string r = Console.ReadLine();
                if (r == "") { break; }
                string str = Formula.Coding(r);
                Console.WriteLine(str);
            }
        }
        static void DecodeTest()
        {
            while (true)
            {
                string r = Console.ReadLine();
                if (r == "") { break; }
                string str = Formula.Decoding(r);
                Console.WriteLine(str);
            }
        }
        static void DoubleTest()
        {
            while (true)
            {
                string r = Console.ReadLine();
                if (r == "") { break; }
                string str = Formula.Coding(r);
                Console.WriteLine(str);
                Console.WriteLine(Formula.Decoding(str));
            }
        }

        static void TansakuTest()
        {
            while (true)
            {
           //     Console.WriteLine("やめるなら y");
           //    if (Console.ReadLine() == "y") { break; }
                List<string> maestrings = new List<string>(MakeMaeStrings());
                List<string> atostrings = new List<string>(MakeAtoStrings());
                Formula form = new Formula(maestrings, atostrings);
                Figure fig = form.Simplimize();

                //   Console.ReadLine();
                writer = new StreamWriter("result.txt");
                fig.OutPut(writer,0);
                fig.OutPut(0);
                writer.Close();

            }
        }

        static List<string> MakeMaeStrings()
        {
            List<string> maestrings = new List<string>();
            while (true)
            {
                Console.WriteLine("前提を入力");
                string r = Console.ReadLine();
                if (r == "") { break; }
                string str = Formula.Coding(r);
                maestrings.Add(str);
            }
            return maestrings;
        }

        static List<string> MakeAtoStrings()
        {
            List<string> atostrings = new List<string>();
            while (true)
            {
                Console.WriteLine("結論を入力");
                string r = Console.ReadLine();
                if (r == "") { break; }
                string str = Formula.Coding(r);
                atostrings.Add(str);
            }
            return atostrings;
        }

    }
}
