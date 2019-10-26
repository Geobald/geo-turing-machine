using System;
using System.Threading;

namespace GeoTuringMachine
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("-- BEGIN --");
            Console.WriteLine("...");
            Console.WriteLine("...");
            var sut = new Machine(
       0,
       new Head(new[] { '1', '1', '1', Head.Blank, '1', '1' }, 0),
       TransitionTableGenerator.Addition());

            var result = sut.Run();

            WriteResult("STOPPED AT HEAD", result.Head);
            WriteResult("STOPPED AT STATE", result.State);
            Console.WriteLine("...");
            Console.WriteLine("...");
            Console.WriteLine("-- HALT --");
        }


        public static void WriteResult(string str, object obj = null) {
            Thread.Sleep(1000);
            Console.WriteLine($"{str}: {obj}");
        }
    }

}
