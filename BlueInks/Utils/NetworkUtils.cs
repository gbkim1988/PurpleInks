using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BlueInks.Utils
{
    public class NetworkUtils
    {

        public static String GetValidIPAddress() {
            IPAddress LocalIPAddress = null;
            // 인터넷이 가능한 경우 
            IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Parse("93.184.216.34"), 80);

            System.Net.Sockets.Socket s;
            s = new System.Net.Sockets.Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                s.Connect(remoteEndPoint);
                LocalIPAddress = (s.LocalEndPoint as IPEndPoint).Address;
            }
            catch (Exception e){ }
            if (LocalIPAddress == null)
            {
                // 인터넷이 되지 않는 경우 gw.yes24.com 으로 연결 시도하여 유효한 아이피를 획득
                IPEndPoint remoteEndPoint2 = new IPEndPoint(IPAddress.Parse("192.168.50.167"), 80);

                Socket s2;
                s2 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                try
                {
                    s2.Connect(remoteEndPoint2);
                    LocalIPAddress = (s2.LocalEndPoint as IPEndPoint).Address;
                }
                catch (Exception e)
                { }

            }

            if (LocalIPAddress != null)
                return LocalIPAddress.ToString();
            else
            {
                // 만약 유효한 아이피를 찾아내지 못하는 경우 아이피 중 가장 첫번째 아이피를 추출한다.
                var host = Dns.GetHostEntry(Dns.GetHostName());
                return host.AddressList.First<IPAddress>().ToString();

            }
        }
    }
}
