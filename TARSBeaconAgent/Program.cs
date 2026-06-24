using System;
using System.Threading;

namespace TARSAgent
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.Title = "TARS Agent";
            }
            catch
            {
                // Ignore if console handle is invalid
            }

            Console.WriteLine("================================");
            Console.WriteLine(" TARS AGENT INITIALIZED");
            Console.WriteLine("================================");

            MissionConfig config = MissionConfig.Load();

            Console.WriteLine($"Agent Name : {config.AgentName}");
            Console.WriteLine($"Cooper URL : {config.CooperUrl}");
            Console.WriteLine();

            DockingSignalSender sender =
                new DockingSignalSender(config);

            while (true)
            {
                sender.Transmit();

                Thread.Sleep(config.TransmissionIntervalMs);
            }
        }
    }
}