using EniacSpi.Interfaces;
using oclHashcatNet.Objects;
using System.Collections.Generic;

namespace EniacSpi.Interfaces
{
    public interface IHost
    {
        string Name { get; }
        string Address { get; }

        IEnumerable<INetworkInformation> AvailableNetworks { get; }
        IEnumerable<IHostInformation> AvailableTargetHosts { get; }
        INetworkInformation SelectedNetwork { get; set; }
        IHostInformation SelectedTargetHost { get; set; }

        bool IsConnected { get; }
    }
}