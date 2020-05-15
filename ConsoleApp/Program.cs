using DinicsAlgorithm;
using FordFulkersonAlgorithm;
using FordFulkersonAlgorithm.Auxiliary;
using PathSearching;
using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleApp
{
    class Program
    {
        static void Main()
        {
            try
            {

                IOConsole console = new IOConsole();
                while (true)
                {
                    Console.Clear();
                    if (console.CMatrix == null)
                        Console.WriteLine("*** There is no flow matrix to work with ***\n");
                    else
                        Console.WriteLine("*** The matrix is ready ***\n");
                    Console.WriteLine("1. Read flow matrix from file");
                    Console.WriteLine("2. Console input of matrix");
                    Console.WriteLine("3. Random generating matrix");
                    Console.WriteLine("4. Matrix Output");
                    Console.WriteLine("5. Ford-Fulkerson solution");
                    Console.WriteLine("6. Dinics solution");
                    Console.WriteLine("7. Greedy search of path");
                    Console.WriteLine("8. DFS of path");
                    Console.WriteLine("9. Exit");
                    var key = Console.ReadKey().KeyChar;
                    Console.Clear();
                    switch (key)
                    {
                        case '1':
                            if (console.CMatrix != null)
                            {
                                Console.WriteLine("You have already got a matrix. " +
                                  "Do you really want to rewrite it? [y/n]");
                                Console.WriteLine();
                                char k;
                                do
                                {
                                    k = Console.ReadKey().KeyChar;
                                } while (k != 'n' && k != 'y');
                                if (k == 'n')
                                    break;
                            }

                            Console.WriteLine("Enter the filename: ");
                            var filename = Console.ReadLine();
                            if (!File.Exists(filename))
                            {
                                Console.WriteLine("The file \"" + filename + "\" does not exist");
                                Console.ReadKey();
                                continue;
                            }
                            console = new IOConsole(filename);
                            console.ReadMatrix();
                            console.WriteMatrix(console.CMatrix);
                            Console.ReadKey();
                            break;
                        case '2':
                            if (console.CMatrix != null)
                            {
                                Console.WriteLine("You have already got a matrix. " +
                                  "Do you really want to rewrite it? [y/n]");
                                char k;
                                do
                                {
                                    k = Console.ReadKey().KeyChar;
                                } while (k != 'n' && k != 'y');
                                if (k == 'n')
                                    break;
                            }

                            console = new IOConsole();
                            console.ConsoleInputMatrix();
                            console.WriteMatrix(console.CMatrix);
                            Console.ReadKey();
                            break;
                        case '3':
                            if (console.CMatrix != null)
                            {
                                Console.WriteLine("You have already got a matrix. " +
                                  "Do you really want to rewrite it? [y/n]");
                                char k;
                                do
                                {
                                    k = Console.ReadKey().KeyChar;
                                } while (k != 'n' && k != 'y');
                                if (k == 'n')
                                    break;
                            }

                            console = new IOConsole();
                            console.RandomGenerate();
                            console.WriteMatrix(console.CMatrix);
                            Console.ReadKey();
                            break;
                        case '4':
                            if (console.CMatrix == null)
                                Console.WriteLine("You haven`t got any matrix to use");
                            else
                                console.WriteMatrix(console.CMatrix);
                            Console.ReadKey();
                            break;
                        case '5':
                            if (console.CMatrix == null)
                            {
                                Console.WriteLine("You haven`t got any matrix to use");
                                Console.ReadKey();
                                continue;
                            }

                            Console.WriteLine("Ford-Fulkerson solution:");
                            Solver solve = new Solver(console.CMatrix, console.N);
                            console.WriteFlowMatrix(solve.StartMatrix, console.GetNullMatrix());
                            Flow result = solve.FordFulkerson(console.From - 1, console.To - 1);

                            Console.WriteLine($"Result: Max F = {result.Cost}");
                            console.WriteFlowMatrix(solve.StartMatrix, result.Vertexes);
                            Console.ReadKey();
                            break;
                        case '6':
                            if (console.CMatrix == null)
                            {
                                Console.WriteLine("You haven`t got any matrix to use");
                                Console.ReadKey();
                                continue;
                            }

                            Console.WriteLine("\nDinics solution:");
                            var dinics = new Dinics(console.From, console.To, console.N, console.CMatrix);
                            console.WriteFlowMatrix(console.CMatrix, console.GetNullMatrix());
                            dinics.Run();

                            Console.WriteLine("\nResult: Max F = " + dinics.F.ToString());
                            console.WriteFlowMatrix(console.CMatrix, dinics.FlowMatrix.FlowToIntMatrix());
                            Console.ReadKey();
                            break;
                        case '7':
                            if (console.CMatrix == null)
                            {
                                Console.WriteLine("You haven`t got any matrix to use");
                                Console.ReadKey();
                                continue;
                            }

                            Console.WriteLine("\nGreedy solution:");
                            var greedy = new Greedy();
                            (int costGreedy, List<int> pathGreedy) = greedy.GreedyAlgorithm(console.CMatrix, console.From - 1, console.To - 1);
                            console.WriteList(pathGreedy, costGreedy);
                            Console.ReadKey();
                            break;
                        case '8':
                            if (console.CMatrix == null)
                            {
                                Console.WriteLine("You haven`t got any matrix to use");
                                Console.ReadKey();
                                continue;
                            }

                            Console.WriteLine("\nDFS solution:");
                            var dfs = new DFS(console.CMatrix, console.N, console.From - 1, console.To - 1);
                            (int costDFS, List<int> pathDFS) = dfs.Run();
                            console.WriteList(pathDFS, costDFS);
                            Console.ReadKey();
                            break;
                        case '9':
                            Environment.Exit(0);
                            break;
                        default:
                            Console.WriteLine("Invalid input");
                            Console.ReadKey();
                            break;
                    }
                }
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey();
            }
        }
    }
}
