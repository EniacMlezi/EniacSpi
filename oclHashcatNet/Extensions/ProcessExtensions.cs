using System;
using System.Diagnostics;

namespace oclHashcatNet.Extensions
{
    public static class ProcessExtensions
    {
        public static bool IsRunning(this Process process)
        {
            if (process == null)
                throw new ArgumentNullException("process");

            try
            {
                Process.GetProcessById(process.Id);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
    }
}