using DinicsAlgorithm;
using System;

namespace ConsoleApp
{
    class Program
    {
        static void Main()
        {
            var dinics = new Dinics("1.txt");
            dinics.FlowMatrix.ConsoleOutput();
            dinics.Run(1, 10);
            Console.ReadKey();
        }
    }
}
