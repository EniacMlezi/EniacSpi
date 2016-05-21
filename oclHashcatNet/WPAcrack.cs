using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace oclHashcatNet.Objects
{
    public class WPAcrack
    {
        private object locker = new object();
        private static StringBuilder hcOutput = null;
        private ManualResetEvent started = new ManualResetEvent(false);
        private Process hcBruteForceProcess;
        private WPACrackStatus status;

        public WPACrackStatus Status
        {
            get
            {
                lock (locker)
                {
                    return status;
                }
            }
            private set
            {
                lock (locker)
                {
                    this.status = value;
                }
            }
        }

        private void parseStatus(string statusOutput)
        {
            WPACrackStatus tempStatus = new WPACrackStatus();
            string statusBlock;

            if (!statusOutput.Contains("STATUS") || !statusOutput.Contains("ENDSTATUS"))
                return;

            statusBlock = statusOutput.Remove(0, statusOutput.IndexOf("STATUS")); //remove leading garbage
            statusBlock = statusBlock.Remove(statusBlock.IndexOf("ENDSTATUS")); // remove trailing garbage
            hcOutput.Clear();
            foreach (string line in new LineReader(() => new StringReader(statusBlock)))
            {
                if (line.Contains("Status."))
                {
                    var tmp = line.Split(':');
                    tempStatus.Condition = (WPAcrackCondition)Enum.Parse(typeof(WPAcrackCondition), tmp[1]);
                }
                else if (line.Contains("Progress."))
                {
                    var tmp = line.Split(':');
                    var progress = tmp[1].Remove(0, tmp[1].IndexOf("(") + 1); // remove leading garbage
                    progress = progress.Remove(progress.IndexOf("%")); // remove trailing garbarge
                    tempStatus.Progress = float.Parse(progress);
                }
                else if (line.Contains("HWMon.GPU."))
                {
                    var tmp = line.Split(':');
                    var id = int.Parse(tmp[0].Split('#')[1].TrimEnd('.', ':'));
                    var values = tmp[1].Split(',');
                    var load = int.Parse(values[0].Split('%')[0]);
                    var temperature = int.Parse(values[1].Split('c')[0]);
                    var speed = int.Parse(values[2].Split('%')[0]);

                    tempStatus.GPUs.Add(new GPUStatus { Id = id, Load = load, Temperature = temperature, Speed = speed });
                }
            }
            this.Status = tempStatus;       
        }


        public WPAcrack()
        {
            status = new WPACrackStatus();
            hcOutput = new StringBuilder("");

            //prepare hashcat bruteforce process
            hcBruteForceProcess = new Process();
            hcBruteForceProcess.StartInfo.UseShellExecute = false;
            hcBruteForceProcess.StartInfo.RedirectStandardOutput = true;
            hcBruteForceProcess.StartInfo.RedirectStandardError = true;
            hcBruteForceProcess.StartInfo.FileName = "C:/Hashcat/cudaHashcat/cudaHashcat.exe";
            hcBruteForceProcess.StartInfo.WorkingDirectory = "C:/Hashcat/cudaHashCat";
            hcBruteForceProcess.StartInfo.Arguments = "--status -m 2500 -a 3 -o cracked.txt capture.hccap";
            hcBruteForceProcess.StartInfo.CreateNoWindow = true;
            hcBruteForceProcess.OutputDataReceived += HCOutputHandler;
            hcBruteForceProcess.ErrorDataReceived += HCErrorHandler;
            hcBruteForceProcess.Exited += HCExited;
        }

        public void Start()
        {
            hcBruteForceProcess.Start();
            hcBruteForceProcess.BeginOutputReadLine();
            hcBruteForceProcess.BeginErrorReadLine();

            started.WaitOne();
        }

        public void Stop()
        {
            if (this.Status.Condition == WPAcrackCondition.Stopped)
                return;

            hcBruteForceProcess.Kill();
            hcBruteForceProcess.WaitForExit();
            hcBruteForceProcess.Close();

            this.Status = new WPACrackStatus();
        }

        private void HCOutputHandler(object sender, DataReceivedEventArgs e)
        {
            // Collect the hashcat command output.
            if (!String.IsNullOrEmpty(e.Data))
            {
                // Add the text to the collected output.
                hcOutput.Append(Environment.NewLine + e.Data);

                if (e.Data.Contains("ENDSTATUS"))
                {
                    Task.Run(() => parseStatus(hcOutput.ToString())); // start parsing
                }
                else if (e.Data.Contains("STARTED"))
                {
                    started.Set(); // signal started
                }

                Console.WriteLine(e.Data);
            }
        }

        private void HCErrorHandler(object sender, DataReceivedEventArgs e)
        {
            // Collect the hashcat command output.
            if (!String.IsNullOrEmpty(e.Data))
            {
                // Add the text to the collected output.
                hcOutput.Append(Environment.NewLine + e.Data);
                Console.WriteLine(e.Data);
            }
        }

        private void HCExited(object sender, EventArgs e)
        {
            Console.WriteLine("oclHashcat Exited");
        }
    }
}