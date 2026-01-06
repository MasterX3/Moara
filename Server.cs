using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Moara
{
    public class Server : NetworkModule
    {
        public ServerState State { get; set; }
        public TcpListener Listener { get; set; }
   
        public Server(Form1 form) : base(form)
        { 
            Listener = new TcpListener(System.Net.IPAddress.Any, 7071);
            Listener.Start();
            State = ServerState.WaitingForClient;

            Thread = new Thread(new ThreadStart(StartListening));
            ThreadAlive = true;

            Thread.Start();
        }

        private void StartListening()
        {
            while (ThreadAlive)
            {
                try
                {
                    Socket socket = Listener.AcceptSocket();
                    NetStream = new NetworkStream(socket);
                    Reader = new StreamReader(NetStream);
                    Writer = new StreamWriter(NetStream);
                    Writer.AutoFlush = true; // Trimite pe stream dupa fiecare scriere, nu asteapta sa se umple buffer-ul

                    ListenLoop();
                }
                catch (SocketException)
                {
                    MessageBox.Show("Socket was closed - expected during shutdown");
                    break;
                }
                catch (Exception e)
                {
                    if (ThreadAlive) // Only show error if not shutting down
                    {
                        MessageBox.Show(e.Message, "A crăpat!");
                    }
                    break;
                }
            }
        }
        
        protected override string GetNetworkType()
        {
            return "server";
        }

        public new void Stop()
        {
            ThreadAlive = false;
            
            try
            {
                Listener.Stop();
            }
            catch
            {
                // Ignore errors during cleanup
            }
            
            base.Stop();
        }
    }
}
