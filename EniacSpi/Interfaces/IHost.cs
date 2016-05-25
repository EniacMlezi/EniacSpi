using EniacSpi.Interfaces;
using oclHashcatNet.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EniacSpi.Interfaces
{
    public interface IHost
    {
        WPAcrack WPAcrack { get; }

        string Name { get; }
        string Address { get; }

        IList<INetworkInformation> AvailableNetworks { get; }
        IList<IHostInformation> AvailableTargetHosts { get; }
        INetworkInformation SelectedNetwork { get; set; }
        IHostInformation SelectedTargetHost { get; set; }

        Task<string> StartCracking();
        void StopCracking();
        bool IsCracking { get; set; }

        bool IsConnected { get; }
    }
}