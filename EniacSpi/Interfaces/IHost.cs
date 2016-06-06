using EniacSpi.Interfaces;
using EniacSpi.Objects;
using oclHashcatNet.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EniacSpi.Interfaces
{
    public interface IHost
    {
        WPAcrack WPAcrack { get; }
        ConcurrentQueue<PacketData> TrafficQueue { get; }
        ConcurrentQueue<WPACrackStatus> NetworkInfiltrationStatusQueue { get; }

        string Name { get; }
        string Address { get; }

        IList<INetworkInformation> AvailableNetworks { get; }
        IList<IHostInformation> AvailableTargetHosts { get; }
        INetworkInformation SelectedNetwork { get; set; }
        IHostInformation SelectedTargetHost { get; set; }

        Task<string> StartCracking();
        void StopCracking();
        bool IsCracking { get; }

        bool IsConnected { get; }
    }
}