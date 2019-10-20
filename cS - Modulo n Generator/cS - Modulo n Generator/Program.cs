using System;

namespace cS___Modulo_n_Generator
{
    class Program
    {
        static void PrintColour(string str, ConsoleColor clr)
        {
            Console.ForegroundColor = clr;
            Console.Write(str);
            Console.ForegroundColor = ConsoleColor.White;
        }
        static void PrintInstr()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            Console.WriteLine("This program determines the sets that each element of the multiplicative group (Z/nZ)^x generate.");

            Console.Write("The generators of these sets will be coloured in ");
            PrintColour("green", ConsoleColor.Green);
            Console.WriteLine(".");

            Console.Write("The generators of (Z/nZ)^x and its elements are highlighted in ");
            PrintColour("red", ConsoleColor.Red);
            Console.WriteLine(".");

            Console.WriteLine("Format: \"g: g g^2 g^3 ...\", where g is in (Z/nZ)^x.");
            Console.WriteLine();
        }
        static int GCD(int a, int b)
        {  
            if (a < 1)
            {
                return b;
            }

            //Assume b > a. If not, swap b and a   
            if (b < a)
            {
                int k = b;
                b = a;
                a = k;
            }

            return GCD(b - a, a);
        }
        static int PrintZnZ(int n)
        {
            Console.Write("(Z/" + n + "Z)^x contains: ");
            int totient = 0;

            for (int i = 1; i < n; i++)
            {
                if (GCD(i, n) == 1)
                {
                    Console.Write(i + " ");
                    totient++;
                }
            }
            Console.WriteLine("\n");
            return totient;
        }
        static bool IsGeneratorOfn(string line, int n, int totient)
        {
            string[] arr = line.Split(" ");
            
            for (int i = 1; i < totient; i++)
            {
                if (GCD(int.Parse(arr[i]), n) != 1)
                {
                    return false;
                }

                //Check for repeating patterns
                for (int j = 0; j < i; j++)
                {
                    //Repeated element found means NOT a generator
                    if (arr[i] == arr[j])
                    {
                        return false;
                    }
                }   
            }

            return true;
        }
        static void OutputGeneratedSet(string line, int g, int n, int totient)
        {
            if (IsGeneratorOfn(line, n, totient))
            {

                PrintColour(g.ToString() + " : " + line + "\n", ConsoleColor.Red);
            }
            else
            {
                PrintColour(g.ToString(), ConsoleColor.Green);
                Console.Write(" : ");
                Console.WriteLine(line);
            }
        }
        static void GenerateSet(int g, int n, int totient)
        {
            string line = "";
            int x = 1;
            
            //Using x in place of g^k to avoid NaN that appears for large n
            for (int k = 1; k <= totient; k++)
            {
                x = (x * g) % n;
                line += x;
                if (k != n)
                {
                    line += " ";
                }     
            }

            //Output generated set in appropriate colour
            OutputGeneratedSet(line, g, n, totient);
        }
        static void ComputeSets(int n, int totient)
        {
            for (int g = 1; g < n; g++)
            {
                if (GCD(g, n) == 1)
                {
                    GenerateSet(g, n, totient);
                }    
            }

            Console.WriteLine();
        }
        static void Main(string[] args)
        {
            PrintInstr();

            while (true)
            {
                Console.Write("Enter n: ");
                string input = Console.ReadLine();

                if (int.TryParse(input, out int n))
                {
                    Console.WriteLine();

                    //Print (Z/nZ)^x and obtain |(Z/nZ)^x|
                    int totient = PrintZnZ(n);

                    //Outputs the set that each element of (Z/nZ)^x generates
                    ComputeSets(n, totient);
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("Error: Input must be a number");
                    Console.WriteLine();
                }
            }
        }
    }
}
