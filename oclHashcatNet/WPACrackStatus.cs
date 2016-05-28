using System.Collections.Generic;
using System.ComponentModel;

namespace oclHashcatNet.Extensions
{
    public enum WPAcrackCondition
    {
        Stopped,
        Paused, 
        Running,
        Aborted
    }

    public struct GPUStatus
    {
        public int Id { get; set; }
        public int Load { get; set; }
        public int Temperature { get; set; }
        public float Speed { get; set; }
    }

    public class WPACrackStatus : INotifyPropertyChanged
    {
        public WPACrackStatus()
        {
            GPUs = new List<GPUStatus>();
        }

        public WPAcrackCondition Condition { get; set; }
        public float Progress{ get; set; }

        public IList<GPUStatus> GPUs { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}