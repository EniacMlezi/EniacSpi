using EniacSpi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using oclHashcatNet.Objects;

namespace EniacSpi.Objects
{
    public class Host : IHost
    {
        public Host(Socket socket, string name)
        {
            this.Socket = socket;
            this.Name = name;
        }

        public WPAcrack WPAcrack { get; }

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
                availableNetworks.Add(new NetworkInformation() { SSID = "testSSID", MAC = "TEST-AABBCC-DD1234", Security = "WPA/WPA2(test)", Signal = 10, CrackProgressStatus = 3, CrackProgressEnd = 10, IsCracking = true });
                this.SelectedNetwork = availableNetworks.FirstOrDefault();
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

        public IHostInformation SelectedTargetHost { get; set; }

        public INetworkInformation SelectedNetwork
        {
            get
            {
                return new NetworkInformation() { SSID = "testSSID", MAC = "TEST-AABBCC-DD1234", Security = "WPA/WPA2(test)", Signal = 10, CrackProgressEnd = 100, CrackProgressStatus = 32, IsCracking = true  };
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
