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
            IOConsole iOConsole = new IOConsole("input.txt");
            iOConsole.ReadMatrix();
            Console.WriteLine($"From: {iOConsole.From.ToString()}\n To: {iOConsole.To.ToString()}");

            //Ford-Fulkerson
            Console.WriteLine("Ford-Fulkerson solution:");

            Solver solve = new Solver(iOConsole.CMatrix, iOConsole.N);
            iOConsole.WriteMatrix(solve.StartMatrix, iOConsole.GetNullMatrix());
            Flow result = solve.FordFulkerson(iOConsole.From - 1, iOConsole.To - 1);

            Console.WriteLine($"Result: Max F = {result.Cost}");
            iOConsole.WriteMatrix(solve.StartMatrix, result.Vertexes);

            //Dinics
            Console.WriteLine("\nDinics solution:");

            var dinics = new Dinics(iOConsole.From, iOConsole.To, iOConsole.N, iOConsole.CMatrix);
            iOConsole.WriteMatrix(iOConsole.CMatrix, iOConsole.GetNullMatrix());
            dinics.Run();

            Console.WriteLine("\nResult: Max F = " + dinics.F.ToString());
            iOConsole.WriteMatrix(iOConsole.CMatrix, dinics.FlowMatrix.FlowToIntMatrix());

            Console.ReadKey();
        }
    }
}
