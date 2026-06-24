using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TARSAgent
{
    public class DockingSignalSender
    {
        private readonly MissionConfig _config;

        public DockingSignalSender(MissionConfig config)
        {
            _config = config;
        }

        public void Transmit()
        {
            try
            {
                string ipAddress = GetLocalIPAddress();

                string json =
                    "{"
                    + "\"agentName\":\"" + _config.AgentName + "\","
                    + "\"machineName\":\"" + Environment.MachineName + "\","
                    + "\"ipAddress\":\"" + ipAddress + "\","
                    + "\"status\":\"Connected\","
                    + "\"transmissionTime\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\""
                    + "}";

                byte[] data = Encoding.UTF8.GetBytes(json);

                HttpWebRequest req =
                    (HttpWebRequest)WebRequest.Create(_config.CooperUrl);

                req.Method = "POST";
                req.ContentType = "application/json";
                req.ContentLength = data.Length;
                req.Timeout = 5000;

                using (Stream s = req.GetRequestStream())
                {
                    s.Write(data, 0, data.Length);
                }

                using (HttpWebResponse resp =
                    (HttpWebResponse)req.GetResponse())
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(
                        $"[{DateTime.Now:HH:mm:ss}] Transmission sent to COOPER | IP: {ipAddress}");
                    Console.ResetColor();
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(
                    $"[{DateTime.Now:HH:mm:ss}] COOPER unreachable : {ex.Message}");
                Console.ResetColor();
            }
        }

        private string GetLocalIPAddress()
        {
            try
            {
                string hostName = Dns.GetHostName();
                IPAddress[] addresses = Dns.GetHostAddresses(hostName);

                foreach (IPAddress ip in addresses)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return ip.ToString();
                    }
                }
            }
            catch
            {
            }

            return "Unknown";
        }
    }
}