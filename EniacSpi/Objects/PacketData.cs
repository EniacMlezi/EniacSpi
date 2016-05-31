using PacketDotNet;
using System;
using System.Net;

namespace EniacSpi.Objects
{
    public class PacketData
    {
        public DateTime Time { get; set; }
        public int Length { get; set; }
        public IPAddress SourceIp { get; set; }
        public IPAddress DestinationIp { get; set; }
        public IPProtocolType Protocol { get; set; }
        public int SourcePort { get; set; }
        public int DestinationPort { get; set; }
    }
}