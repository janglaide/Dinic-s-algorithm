using DinicsAlgorithm;
using FordFulkersonAlgorithm;
using FordFulkersonAlgorithm.Auxiliary;
using PathSearching;
using System;
using System.Collections.Generic;

namespace ConsoleApp
{
    class Program
    {
        static void Main()
        {
            //IOConsole iOConsole = new IOConsole("input.txt"); //file input
            //iOConsole.ReadMatrix();

            IOConsole iOConsole = new IOConsole(); //console input
            iOConsole.ConsoleInputMatrix();
            //iOConsole.RandomGenerate();

            iOConsole.WriteMatrix(iOConsole.CMatrix, iOConsole.CMatrix); //output

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

            //Greedy search
            Console.WriteLine("\nGreedy solution:");
            var greedy = new Greedy();
            (int cost, List<int> path) = greedy.GreedyAlgorithm(iOConsole.CMatrix, iOConsole.From - 1, iOConsole.To - 1);
            iOConsole.WriteList(path, cost);

            //DFS
            Console.WriteLine("\nDFS solution:");
            var dfs = new DFS(iOConsole.CMatrix, iOConsole.N, iOConsole.From - 1, iOConsole.To - 1);
            (cost, path) = dfs.Run();
            iOConsole.WriteList(path, cost);

            Console.ReadKey();
        }
    }
}
