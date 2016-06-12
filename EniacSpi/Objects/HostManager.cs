using EniacSpi.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EniacSpi.Objects
{
    public class HostManager
    {
        public HostManager()
        {
            Hosts = new List<IHost>();
            new Thread(CheckConnected).Start();
        }

        private static void CheckConnected()
        {
            while (true)
            {
                System.Threading.Thread.Sleep(3000);
                foreach (var module in Current.Hosts.ToList())
                {
                    if (!module.IsConnected)
                    {
                        Current.Delete(module);
                    }
                }
            }
        }

        private static HostManager _current;
        public static HostManager Current
        {
            get { return _current ?? (_current = new HostManager()); }
        }

        internal List<IHost> Hosts { get; set; }

        public IEnumerable<IHost> GetHosts()
        {
            return Hosts;
        }

        public IHost GetHost(string name)
        {
            return GetHosts().Where(m => m.Name == name).FirstOrDefault();
        }

        public void Add(IHost host)
        {
            if (HostManager.Current.GetHost(host.Name) == null)
            {
                Hosts.Add(host);
                File.AppendAllText(@"C:\log.txt", System.DateTime.Now.ToString() + " -> [HostManager] Added Host: " + host.Name + Environment.NewLine);
            }
        }

        public void Delete(IEnumerable<IHost> modules)
        {
            foreach (var module in modules.ToList())
            {
                Delete(module);
            }
        }

        public void Delete(string name)
        {
            Delete(GetHost(name));
        }

        public void Delete(IHost module)
        {
            Hosts.Remove(module);
            File.AppendAllText(@"C:\log.txt", System.DateTime.Now.ToString() + " -> [HostManager] Deleted Host: " + module.Name + Environment.NewLine);
        }
    }
}