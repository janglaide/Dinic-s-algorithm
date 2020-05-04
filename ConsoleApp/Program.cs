using DinicsAlgorithm;
using FordFulkersonAlgorithm;
using FordFulkersonAlgorithm.Auxiliary;
using System;

namespace ConsoleApp
{
    class Program
    {
        static void Main()
        {
            //Ford-Fulkerson
            IOStream stream = new IOStream();
            Solver solve = new Solver();

            int from = 0, to = 0;
            (int[,] cMartix, int[,] eMartix) = stream.FromFile(ref from, ref to);
            Console.WriteLine($"from: {from}\n to: {to}");
            Console.WriteLine("Ford-Fulkerson solved:");
            //stream.ToConsole(cMartix);
            stream.ConsoleOutput(stream.NullMatrix);
            //Console.WriteLine("-");
            //stream.ToConsole(eMartix);
            Flow result = solve.FordFulkerson(cMartix, eMartix, from - 1, to - 1);
            Console.WriteLine($"Result: Max F = {result.Cost}");
            //stream.ToConsole(result.Vertexes);
            stream.ConsoleOutput(result.Vertexes);
            //Console.ReadKey();

            //Dinics
            Console.WriteLine("\nDinics solved:");
            var dinics = new Dinics("input.txt");
            dinics.FlowMatrix.ConsoleOutput();
            dinics.Run();
            Console.ReadKey();
        }
    }
}
