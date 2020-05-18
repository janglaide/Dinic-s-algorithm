﻿using DinicsAlgorithm;
using FordFulkersonAlgorithm;
using FordFulkersonAlgorithm.Auxiliary;
using PathSearching;
using Experiment.Auxiliary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace ConsoleApp.Experiment
{
    public class Research
    {
        private List<double> FordFulkersonCount;
        private List<double> DinicsCount;
        private List<double> GreedyCount;
        private List<double> DFSCount;
        private IOConsole _console;
        private int bound;
        public Research()
        {
            _console = new IOConsole();
            CounterReset();
        }
        public void CounterReset()
        {
            FordFulkersonCount = new List<double>();
            DinicsCount = new List<double>();
            GreedyCount = new List<double>();
            DFSCount = new List<double>();
        }

        public void Menu()
        {
            bool exit = false;
            while (!exit)
            {
                StreamWriter fileWriter = null;
                char k;
                Console.Clear();
                Console.WriteLine("1. Research by certain matrix size");
                Console.WriteLine("2. Research by time per size");
                Console.WriteLine("3. Exit to main menu");
                var key = Console.ReadLine();
                switch (key)
                {
                    case "1":
                        Console.WriteLine("Write research process and results to files? [y/n]");
                        do
                        {
                            k = Console.ReadKey().KeyChar;
                        } while (k != 'n' && k != 'y');
                        if (k == 'y')
                            fileWriter = _console.GetFileStream();
                        Console.WriteLine();

                        bound = _console.RandomInput();
                        Console.WriteLine("Enter research size:");
                        int.TryParse(Console.ReadLine(), out int size);
                        CounterReset();
                        ResearchForSize(fileWriter, size);
                        Compare(fileWriter);
                        if (fileWriter != null)
                        {
                            _console.WriteTimeToFile(FordFulkersonCount, $"FordFulkerson_{bound}");
                            _console.WriteTimeToFile(DinicsCount, $"Dinic_{bound}");
                            _console.WriteTimeToFile(GreedyCount, $"Greedy_{bound}");
                            _console.WriteTimeToFile(DFSCount, $"DFS_{bound}");
                            Console.WriteLine("\nFiles created!");
                        }
                        Console.ReadKey();
                        break;
                    case "2":
                        Console.WriteLine("Write research process and results to files? [y/n]");
                        do
                        {
                            k = Console.ReadKey().KeyChar;
                        } while (k != 'n' && k != 'y');
                        if (k == 'y')
                            fileWriter = _console.GetFileStream();
                        Console.WriteLine();
                        ResearchByTimePerSize(fileWriter);
                        Console.ReadKey();
                        break;
                    case "3":
                        exit = true;
                        break;
                }
                if (fileWriter != null)
                {
                    fileWriter.Close();
                }
            }
        }
        public void ResearchForSize(StreamWriter fileWriter, int size)
        {
            Solver solve;
            Stopwatch clock = new Stopwatch();

            bool file = false;
            if (fileWriter != null)
                file = true;

            for (int i = 0; i < size; i++)
            {
                _console.RandomGenerate(bound);
                solve = new Solver(_console.From - 1, _console.To - 1, _console.N, _console.CMatrix);

                Console.WriteLine($"№{i + 1}");
                _console.WriteFlowMatrixToConsole(solve.StartMatrix, _console.GetNullMatrix());
                Console.WriteLine("\n\nFord-Fulkerson solution:");
                clock.Start();
                Flow result = solve.FordFulkerson();
                clock.Stop();
                FordFulkersonCount.Add(clock.Elapsed.TotalMilliseconds);

                Console.WriteLine($"\nResult: Max F = {result.Cost}; Time spent for solving: {clock.Elapsed}");
                _console.WriteFlowMatrixToConsole(solve.StartMatrix, result.Vertexes);
                Console.WriteLine();

                if (file)
                {
                    fileWriter.WriteLine($"№{i + 1}");
                    _console.WriteFlowMatrixToFile(solve.StartMatrix, _console.GetNullMatrix(), fileWriter);
                    fileWriter.WriteLine("\n\nFord-Fulkerson solution:");
                    fileWriter.WriteLine($"\nResult: Max F = {result.Cost}; Time spent for solving: {clock.Elapsed}");
                    _console.WriteFlowMatrixToFile(solve.StartMatrix, result.Vertexes, fileWriter);
                    fileWriter.WriteLine();
                }
                clock.Reset();

                Console.WriteLine("Dinics solution:");

                var dinics = new Dinics(_console.From, _console.To, _console.N, _console.CMatrix);
                clock.Start();
                dinics.Run();
                clock.Stop();
                DinicsCount.Add(clock.Elapsed.TotalMilliseconds);

                Console.WriteLine($"\nResult: Max F = {dinics.F.ToString()} ; Time spent for solving: { clock.Elapsed}");
                _console.WriteFlowMatrixToConsole(_console.CMatrix, dinics.FlowMatrix.FlowToIntMatrix());
                Console.WriteLine();
                if (file)
                {
                    fileWriter.WriteLine("Dinics solution:");
                    fileWriter.WriteLine($"\nResult: Max F = {dinics.F.ToString()} ; Time spent for solving: { clock.Elapsed}");
                    _console.WriteFlowMatrixToFile(_console.CMatrix, dinics.FlowMatrix.FlowToIntMatrix(), fileWriter);
                    fileWriter.WriteLine();
                }
                clock.Reset();


                var greedy = new Greedy(_console.CMatrix, _console.N, _console.From - 1, _console.To - 1);
                clock.Start();
                (int costGreedy, List<int> pathGreedy) = greedy.GreedyAlgorithm();
                clock.Stop();
                GreedyCount.Add(clock.Elapsed.TotalMilliseconds);

                Console.WriteLine("\nGreedy solution:");
                _console.WriteListToConsole(pathGreedy, costGreedy);
                Console.WriteLine($"Time spent for solving: { clock.Elapsed}");
                if (file)
                {
                    fileWriter.WriteLine("\nGreedy solution:");
                    _console.WriteListToFile(pathGreedy, costGreedy, fileWriter);
                    fileWriter.WriteLine($"Time spent for solving: { clock.Elapsed}");
                    fileWriter.WriteLine();
                }
                clock.Reset();

                var dfs = new DFS(_console.CMatrix, _console.N, _console.From - 1, _console.To - 1);
                clock.Start();
                (int costDFS, List<int> pathDFS) = dfs.Run();
                clock.Stop();
                DFSCount.Add(clock.Elapsed.TotalMilliseconds);

                Console.WriteLine("\n\nDFS solution:");
                _console.WriteListToConsole(pathDFS, costDFS);
                Console.WriteLine($"Time spent for solving: { clock.Elapsed}");
                if (file)
                {
                    fileWriter.WriteLine("\nDFS solution:");
                    _console.WriteListToFile(pathDFS, costDFS, fileWriter);
                    fileWriter.WriteLine($"Time spent for solving: { clock.Elapsed}");
                    fileWriter.WriteLine("\n\n");
                }
                clock.Reset();

                Console.WriteLine("\n\n");
            }
        }

        public void ResearchByTimePerSize(StreamWriter fileWriter)
        {
            List<Average> time = new List<Average>();
            string result;

            bool file = false;
            if (fileWriter != null)
                file = true;

            Console.WriteLine("Enter start size:");
            var startSize = Convert.ToInt32(Console.ReadLine());
            if (startSize <= 0)
                throw new Exception("Not positive value");
            Console.WriteLine("Enter finish size:");
            var finishSize = Convert.ToInt32(Console.ReadLine());
            if (startSize <= 0 || startSize > finishSize)
                throw new Exception("Invalid value");
            Console.WriteLine("A max value of flow: ");
            bound = Convert.ToInt32(Console.ReadLine());
            if (bound <= 0)
                throw new Exception("Not positive value");
            Console.WriteLine("Enter research size:");
            var researchSize = Convert.ToInt32(Console.ReadLine());
            if (researchSize <= 0)
                throw new Exception("Not positive value");


            for (int i = startSize; i <= finishSize; i++)
            {
                _console.ResearchInput(i, 1, i);
                CounterReset();
                Console.WriteLine($"\t\t___MATRIX SIZE: {i}___\n");
                if (fileWriter != null)
                {
                    fileWriter.WriteLine($"\t\t___MATRIX SIZE: {i}___\n");
                }
                ResearchForSize(fileWriter, researchSize);
                time.Add(new Average(i, Compare(null)));
            }

            result = "RESULTS FOR TIME PER SIZE:\n";
            for (int i = 0; i < time.Count; i++)
            {
                result += $"Size: {time[i].Size}\n";
                result += $"    Ford-Falkerson time: \t{time[i].Time[0]}\n";
                result += $"    Dinics time: \t\t{time[i].Time[1]}\n";
                result += $"    Greedy time: \t\t{time[i].Time[2]}\n";
                result += $"    DFS time: \t\t\t{time[i].Time[3]}\n";
            }
            Console.WriteLine(result);
            if (file)
            {
                fileWriter.WriteLine(result);
                _console.WriteResearch(time);
            }

        }
        public double[] Compare(StreamWriter fileWriter)
        {
            int experimentsCount = FordFulkersonCount.Count;
            if (experimentsCount > 0)
            {
                string conclusion;

                Console.WriteLine($"Result of {experimentsCount} experiments:");
                if (fileWriter != null)
                {
                    fileWriter.WriteLine($"Result of {experimentsCount} experiments:");
                }

                double FordFulkersonAverage = 0, DinicsAverage = 0, GreedyAverage = 0, DFSAverage = 0;
                for (int i = 0; i < experimentsCount; i++)
                {
                    FordFulkersonAverage += FordFulkersonCount[i];
                    DinicsAverage += DinicsCount[i];
                    GreedyAverage += GreedyCount[i];
                    DFSAverage += DFSCount[i];
                }
                FordFulkersonAverage /= experimentsCount;
                DinicsAverage /= experimentsCount;
                GreedyAverage /= experimentsCount;
                DFSAverage /= experimentsCount;
                FordFulkersonAverage = Math.Round(FordFulkersonAverage, 5);
                DinicsAverage = Math.Round(DinicsAverage, 5);
                GreedyAverage = Math.Round(GreedyAverage, 5);
                DFSAverage = Math.Round(DFSAverage, 5);

                Console.WriteLine($"Average running time of the Ford-Fulkersons algorithm = {FordFulkersonAverage} milliseconds");
                Console.WriteLine($"Average running time of the Dinics algorithm = {DinicsAverage} milliseconds");
                Console.WriteLine($"Average running time of the Greedy algorithm = {GreedyAverage} milliseconds");
                Console.WriteLine($"Average running time of the DFS algorithm = {DFSAverage} milliseconds");
                if (fileWriter != null)
                {
                    fileWriter.WriteLine($"Average running time of the Ford-Fulkersons algorithm = {FordFulkersonAverage} milliseconds");
                    fileWriter.WriteLine($"Average running time of the Dinics algorithm = {DinicsAverage} milliseconds");
                    fileWriter.WriteLine($"Average running time of the Greedy algorithm = {GreedyAverage} milliseconds");
                    fileWriter.WriteLine($"Average running time of the DFS algorithm = {DFSAverage} milliseconds");
                }

                conclusion = "So ";
                if (FordFulkersonAverage < DinicsAverage)
                {
                    conclusion += "Ford-Fulkersons algorithm faster than Dinics\n";
                }
                else if (FordFulkersonAverage > DinicsAverage)
                {
                    conclusion += "Dinics algorithm faster than Ford-Fulkersons\n";
                }
                else
                {
                    conclusion += "Flow-search algorithms are equal by time\n";
                }
                conclusion += "   and ";
                if (GreedyAverage < DFSAverage)
                {
                    conclusion += "Greedy algorithm faster than DFS\n";
                }
                else if (GreedyAverage > DFSAverage)
                {
                    conclusion += "DFS algorithm faster than Greedy\n";
                }
                else
                {
                    conclusion += "Path-search algorithms are equal by time\n";
                }

                Console.WriteLine(conclusion);
                if (fileWriter != null)
                {
                    fileWriter.WriteLine(conclusion);
                }

                return new double[] { FordFulkersonAverage, DinicsAverage, GreedyAverage, DFSAverage };
            }
            else
            {
                Console.WriteLine("There were no experiments");
            }
            return null;
        }
    }
}
