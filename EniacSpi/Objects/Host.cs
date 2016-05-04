using EniacSpi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EniacSpi.Objects
{
    public class Host : IHost
    {
        public Host(Socket socket, string name)
        {
            this.Socket = socket;
            this.Name = name;
        }

        public string Name { get; }
        public string Address
        {
            get
            {
                //return Socket.RemoteEndPoint.ToString();
                return "test";
            }
        }

        public IEnumerable<INetworkInformation> AvailableNetworks
        {
            get
            {
                var availableNetworks = new List<INetworkInformation>();
                availableNetworks.Add(new NetworkInformation() { SSID = "testSSID", MAC = "TEST-AABBCC-DD1234", Security = "WPA/WPA2(test)", Signal = 10 });
                return availableNetworks;
            }
        }
        public IEnumerable<IHostInformation> AvailableTargetHosts
        {
            get
            {
                var availableTargetHosts = new List<IHostInformation>();
                availableTargetHosts.Add(new HostInformation { MAC= "TEST-AABBCC-DD1235" });
                return availableTargetHosts;
            }
        }

        public INetworkInformation SelectedNetwork
        {
            get
            {
                return new NetworkInformation() { SSID = "testSSID", MAC = "TEST-AABBCC-DD1234", Security = "WPA/WPA2(test)", Signal = 10 };
            }

            set
            {
                
            }
        }

        public bool IsConnected { get { return isConnected(this.Socket); } }

        public static bool isConnected(Socket socket)
        {
            try
            {
                socket.Send(new byte[] { 0 });
            }
            catch
            {
                return false;
            }
            return true;
        }


        public Socket Socket { get; set; }     
    }
}
