using EniacSpi.Interfaces;
using oclHashcatNet.Objects;
using System.Collections.Generic;

namespace EniacSpi.Interfaces
{
    public interface IHost
    {
        WPAcrack WPAcrack { get; }

        string Name { get; }
        string Address { get; }

        IEnumerable<INetworkInformation> AvailableNetworks { get; }
        IEnumerable<IHostInformation> AvailableTargetHosts { get; }
        INetworkInformation SelectedNetwork { get; set; }
        IHostInformation SelectedTargetHost { get; set; }

        void StartCracking();
        void StopCracking();

        bool IsConnected { get; }
    }
}