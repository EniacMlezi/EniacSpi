using EniacSpi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using oclHashcatNet.Extensions;
using System.IO;
using System.Web;
using Dropbox.Api;
using Dropbox.Api.Babel;
using Dropbox.Api.Files;

namespace EniacSpi.Objects
{
    public class Host : IHost
    {
        public Host(Socket socket, string name)
        {
            this.Socket = socket;
            this.Name = name;
            this.WPAcrack = new WPAcrack();

            AvailableNetworks = new List<INetworkInformation>();
            AvailableTargetHosts = new List<IHostInformation>();
            AvailableNetworks.Add(new NetworkInformation() { SSID = "testSSID", MAC = "TEST-AABBCC-DD1234", Security = "WPA/WPA2(test)", Signal = 10, CrackProgressStatus = 0, CrackProgressEnd = 1, IsCracking = false });
            this.SelectedNetwork = AvailableNetworks.FirstOrDefault();
            AvailableTargetHosts.Add(new HostInformation { MAC = "TEST-AABBCC-DD1235" });
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

        public IList<INetworkInformation> AvailableNetworks { get; }

        public IList<IHostInformation> AvailableTargetHosts { get; }

        public IHostInformation SelectedTargetHost { get; set; }

        public INetworkInformation SelectedNetwork { get; set; }

        public async Task<string> StartCracking()
        {
            if (this.IsCracking)
                StopCracking();
            
            // download /this.Name/this.SelectedNetwork.MAC/capture.extension as tempCapture.extension to C:/Hashcat/
            var dropboxClient = HttpContext.Current.Application["DropboxClient"] as DropboxClient;
            try
            {
                var response = await dropboxClient.Files.DownloadAsync(new Dropbox.Api.Files.DownloadArg(String.Format("/{0}/{1}/capture.hccap", this.Name, this.SelectedNetwork.MAC)));

                var contentBytes = await response.GetContentAsByteArrayAsync();
                FileStream fileStream = System.IO.File.Create(@"c:\Hashcat\capture.hccap", (int)contentBytes.Length);
                fileStream.Write(contentBytes, 0, contentBytes.Length);
                fileStream.Close();
            }
            catch (ApiException<DownloadError.Path> ex)
            {
                return String.Format("{0}: The hccap file does not exist in the dropbox archive. Try again later.", ex.HResult);
            }
            catch (Exception ex)
            {
                return String.Format("{0}: {1}", ex.HResult, ex.Message);
            }

            // start cracking
            this.WPAcrack.Start();
            this.IsCracking = true;

            return "Success";
        }

        public void StopCracking()
        {
            this.WPAcrack.Stop();
            this.IsCracking = false;
        }

        public bool IsCracking { get; set; }

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
