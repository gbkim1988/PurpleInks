using Mono.Posix;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BlueInksTest
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            var host = Dns.GetHostEntry(Dns.GetHostName());
            Console.WriteLine(host.HostName);
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    Console.WriteLine(ip.ToString());
                }
            }

            foreach (NetworkInterface netif in NetworkInterface.GetAllNetworkInterfaces())
            {
                Console.WriteLine("Network Interface: {0}", netif.Name);
                IPInterfaceProperties properties = netif.GetIPProperties();
                foreach (IPAddress dns in properties.DnsAddresses)
                    Console.WriteLine("\tDNS: {0}", dns);
                foreach (IPAddressInformation anycast in properties.AnycastAddresses)
                    Console.WriteLine("\tAnyCast: {0}", anycast.Address);
                foreach (IPAddressInformation multicast in properties.MulticastAddresses)
                    Console.WriteLine("\tMultiCast: {0}", multicast.Address);
                foreach (IPAddressInformation unicast in properties.UnicastAddresses)
                    Console.WriteLine("\tUniCast: {0}", unicast.Address);
            }*/
            //String timeStamp = GetTimestamp(DateTime.Now);

            // IF You couldn't able to connect to internet, You will receive System.Net.Sockets.SocketException : 알려진 호스트가 없습니다. 
            // So, You need try catch statements to check whether it is active or deactive internet connections
            /*
            TcpClient client = null;
            try
            {
                client = new TcpClient("www.example.com", 80);
                Console.WriteLine(((IPEndPoint)client.Client.LocalEndPoint).Address.ToString());
            }
            catch(System.Net.Sockets.SocketException sError)
            {
                if (client != null)
                    Console.WriteLine(((IPEndPoint)client.Client.LocalEndPoint).Address.ToString());
            }
            */

            IPAddress LocalIPAddress = null;
            {
                //IPAddress [] ipAddress = Dns.GetHostAddresses("www.example.com");
                // Toward external network
                IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Parse("93.184.216.34"), 80);

                System.Net.Sockets.Socket s;
                s = new System.Net.Sockets.Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //s.ReceiveTimeout = 0;
                //s.SendTimeout = 0;
                //s.Bind(remoteEndPoint);
                try
                {
                    s.Connect(remoteEndPoint);
                    LocalIPAddress = (s.LocalEndPoint as IPEndPoint).Address;
                    Console.WriteLine((s.LocalEndPoint as IPEndPoint).Address.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine("Intranet LocalEndPoint");
                }
                finally
                {

                }
            }
            /*
            {
                IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Parse("192.168.50.167"), 80);
                
                Socket s;
                s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                s.ReceiveTimeout = 0;
                s.SendTimeout = 0;

                //s.Bind(remoteEndPoint);
                try
                {
                    s.Connect(remoteEndPoint);
                    Console.WriteLine((s.LocalEndPoint as IPEndPoint).Address.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine("Internet LocalEndPoint");
                }
                finally
                {

                }

            }*/

            {
                foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                    {
                        //Console.WriteLine(ni.Name);
                        foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                        {
                            if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                            {
                                if (LocalIPAddress.ToString() == ip.Address.ToString())
                                    Console.WriteLine(ni.GetPhysicalAddress());
                            }
                        }
                    }
                }

            }



            /*
            IPEndPoint remoteIpEndPoint = s.RemoteEndPoint as IPEndPoint;
            s.Connect(remoteEndPoint);

            IPEndPoint localIpEndPoint = s.LocalEndPoint as IPEndPoint;

            if (remoteIpEndPoint != null)
            {
                // Using the RemoteEndPoint property.
                Console.WriteLine("I am connected to " + remoteIpEndPoint.Address + "on port number " + remoteIpEndPoint.Port);
            }

            if (localIpEndPoint != null)
            {
                // Using the LocalEndPoint property.
                Console.WriteLine("My local IpAddress is :" + localIpEndPoint.Address + "I am connected on port number " + localIpEndPoint.Port);
            }
            //Console.WriteLine(Program.getInternetIPAddress());
            */
        }

        static IPAddress getInternetIPAddress()
        {
            try
            {
                IPAddress[] addresses = Dns.GetHostAddresses(Dns.GetHostName());
                IPAddress gateway = IPAddress.Parse(getInternetGateway());
                return findMatch(addresses, gateway);
            }
            catch (FormatException e) {
                return null;
            }
        }

        static string getInternetGateway()
        {
            using (Process tracert = new Process())
            {
                ProcessStartInfo startInfo = tracert.StartInfo;
                startInfo.FileName = "tracert.exe";
                startInfo.Arguments = "-h 1 www.example.com";
                startInfo.UseShellExecute = false;
                startInfo.RedirectStandardOutput = true;
                tracert.Start();

                using (StreamReader reader = tracert.StandardOutput)
                {
                    string line = "";
                    for (int i = 0; i < 5; ++i)
                        line = reader.ReadLine();
                    line = line.Trim();
                    return line.Substring(line.LastIndexOf(' ') + 1);
                }
            }
        }

        static IPAddress findMatch(IPAddress[] addresses, IPAddress gateway)
        {
            byte[] gatewayBytes = gateway.GetAddressBytes();
            foreach (IPAddress ip in addresses)
            {
                byte[] ipBytes = ip.GetAddressBytes();
                if (ipBytes[0] == gatewayBytes[0]
                    && ipBytes[1] == gatewayBytes[1]
                    && ipBytes[2] == gatewayBytes[2])
                {
                    return ip;
                }
            }
            return null;
        }
    }
}
