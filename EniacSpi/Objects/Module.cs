using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EniacSpi.Objects
{
    public class Module
    {
        public Module(Socket socket)
        {
            this.Socket = socket;
        }

        public string Address
        {
            get
            {
                return Socket.RemoteEndPoint.ToString();
            }

            set
            {
                throw new NotImplementedException();
            }
        }      

        public string Name { get; set; }

        public bool IsConnected { get { return isConnected(this.Socket); } }

        public static bool isConnected(Socket socket)
        {
            try
            {
                socket.Send(new byte[] { 0 });
            }
            catch
            {
                return false;
            }
            return true;
        }
        

        public Socket Socket { get; set; }
    }
}
