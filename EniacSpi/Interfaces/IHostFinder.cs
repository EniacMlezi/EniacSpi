using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EniacSpi.Interfaces
{
    public interface IHostFinder
    {
        void StartListening();

        void StopListening();

        bool IsListening { get; }
    }
}