using EniacSpi.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EniacSpi.Objects
{
    public class StateObject
    {
        // Client  socket.
        public Socket workSocket = null;
        // Size of receive buffer.
        public const int BufferSize = 1024;
        // Receive buffer.
        public byte[] buffer = new byte[BufferSize];
        // Received data string.
        public StringBuilder sb = new StringBuilder();
    }
    public static class HostFinder
    {
        public static bool IsListening { get { return pauseEvent.WaitOne(0); } }

        private static Thread listener = new Thread(new ThreadStart(StartListening));

        public static void ResumeListening()
        {
            pauseEvent.Set();
        }

        public static void PauseListening()
        {
            pauseEvent.Reset();
        }

        private static ManualResetEvent pauseEvent = new ManualResetEvent(false);
        private static ManualResetEvent allDone = new ManualResetEvent(false);

        public static void StartListening()
        {
            // Establish the local endpoint for the socket.
            // The DNS name of the computer
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 11000);

            // Create a TCP/IP socket.
            Socket listener = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);

            //Bind the socket to the local endpoint and listen for incoming connections.
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(100);

                pauseEvent.Reset();
                while (true)
                {
                    pauseEvent.WaitOne();
                    // Set the event to nonsignaled state.
                    allDone.Reset();

                    // Start an asynchronous socket to listen for connections.
                    listener.BeginAccept(
                        new AsyncCallback(AcceptCallback),
                        listener);

                    // Wait until a connection is made before continuing.
                    allDone.WaitOne();
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText(@"C:\log.txt", System.DateTime.Now.ToString() + " -> [TcpModuleFinder]: " + Environment.NewLine + Environment.NewLine + ex.ToString() + Environment.NewLine + Environment.NewLine);
            }
        }

        private static void AcceptCallback(IAsyncResult ar)
        {
            // Signal the main thread to continue.
            allDone.Set();

            // Get the socket that handles the client request.
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            StateObject state = new StateObject();
            state.workSocket = handler;
            // Get values in order to create the module
            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReadCallback), state);
        }

        private static void ReadCallback(IAsyncResult ar)
        {
            String content = String.Empty;

            // Retrieve the state object and the handler socket
            // from the asynchronous state object.
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;

            // Read data from the client socket. 
            int bytesRead = handler.EndReceive(ar);

            if (bytesRead > 0)
            {
                // There  might be more data, so store the data received so far.
                state.sb.Append(Encoding.ASCII.GetString(
                    state.buffer, 0, bytesRead));

                // Check for end-of-file tag. If it is not there, read 
                // more data.
                content = state.sb.ToString();
                if (!(content.IndexOf("<EOF>") > -1))
                {
                    // Not all data received. Get more.
                    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReadCallback), state);
                }
                else
                {
                    AddHost(state);
                }
            }
        }

        private static void AddHost(StateObject state)
        {
            string[] ModuleInfo = state.sb.ToString().Split('|');

            // Create the host object.
            IHost host = new Host(state.workSocket, ModuleInfo[0]);

            HostManager.Current.Add(host);
        }
    }
}
