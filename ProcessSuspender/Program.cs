using System;
using System.Threading;

namespace ProcessSuspender
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("GTA 5 Online Process Suspender");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Suspending process...");
            ProcessManager.SuspendProcess(1);

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Waiting 10 seconds to go offline...");
            Thread.Sleep(10000);

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Resuming process...");
            ProcessManager.ResumeProcess(1);

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Closing application...");
            Thread.Sleep(3000);
        }
    }
}
