using System;
using System.IO;

namespace TARSAgent
{
    public class MissionConfig
    {
        public string CooperUrl { get; set; }
        public string AgentName { get; set; }
        public int TransmissionIntervalMs { get; set; }

        public static MissionConfig Load()
        {
            MissionConfig cfg = new MissionConfig();

            string file = "mission.config";

            if (!File.Exists(file))
            {
                cfg.CooperUrl = "http://192.168.10.10:5000/api/docking";
                cfg.TransmissionIntervalMs = 5000;
                cfg.AgentName = "TARS-" + Environment.MachineName;

                File.WriteAllLines(file, new[]
                {
        "CooperUrl=" + cfg.CooperUrl,
        "TransmissionIntervalMs=" + cfg.TransmissionIntervalMs,
        "AgentName=" + cfg.AgentName
    });

                Console.WriteLine("mission.config created automatically.");
                Console.WriteLine("Program will continue using default config.");

                return cfg;
            }
            foreach (string line in File.ReadAllLines(file))
            {
                string[] p = line.Split('=');

                if (p.Length < 2)
                    continue;

                switch (p[0])
                {
                    case "CooperUrl":
                        cfg.CooperUrl = p[1];
                        break;

                    case "AgentName":
                        cfg.AgentName = p[1];
                        break;

                    case "TransmissionIntervalMs":
                        cfg.TransmissionIntervalMs = int.Parse(p[1]);
                        break;
                }
            }

            return cfg;
        }
    }
}