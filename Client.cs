using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Moara
{
    public class Client : NetworkModule
    {
        public TcpClient TcpClient { get; set; }

        public Client(string ipAddress, int port, Form1 form) : base(form)
        {
            TcpClient = new TcpClient(ipAddress, port);
            ThreadAlive = true;

            Thread = new Thread(new ThreadStart(StartListening));
            Thread.Start();
            NetStream = TcpClient.GetStream();
        }

        private void StartListening()
        {
            Reader = new StreamReader(NetStream);
            Writer = new StreamWriter(NetStream);
            Writer.AutoFlush = true;

            ListenLoop();
        }

        protected override string GetNetworkType()
        {
            return "client";
        }
        
        public new void Stop()
        {
            ThreadAlive = false;
            
            try
            {
                TcpClient.Close();
            }
            catch
            {
                // Ignore errors during cleanup
            }
            
            base.Stop();
        }
    }
}