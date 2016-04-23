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
            Modules = new List<IHost>();
            Thread connectedListener = new Thread(CheckConnected);
            connectedListener.Start();
        }

        private static void CheckConnected()
        {
            while (true)
            {
                System.Threading.Thread.Sleep(3000);
                foreach (var module in Current.Modules.ToList())
                {
                    if (!module.IsConnected)
                    {
                        File.AppendAllText(@"C:\log.txt", System.DateTime.Now.ToString() + " -> Disconnected Module: " + module.Name + Environment.NewLine);
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

        internal List<IHost> Modules { get; set; }

        public IEnumerable<IHost> GetModules()
        {
            return Modules;
        }

        public IHost GetModule(string name)
        {
            return GetModules().Where(m => m.Name == name).FirstOrDefault();
        }

        public void Add(IHost module)
        {
            if (HostManager.Current.GetModule(module.Name) == null)
            {
                Modules.Add(module);
                File.AppendAllText(@"C:\log.txt", System.DateTime.Now.ToString() + " -> [ModuleManager] Added Module: " + module.Name + Environment.NewLine);
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
            Delete(GetModule(name));
        }

        public void Delete(IHost module)
        {
            Modules.Remove(module);
        }
    }
}