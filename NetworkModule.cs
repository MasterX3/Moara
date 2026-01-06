using Newtonsoft.Json;
using System;
using System.Drawing;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace Moara
{
    public abstract class NetworkModule
    {
        protected NetworkStream NetStream { get; set; }
        public StreamReader Reader { get; set; }
        public StreamWriter Writer { get; set; }
        public Thread Thread { get; set; }
        public bool ThreadAlive { get; set; }

        private Form1 form;

        public NetworkModule(Form1 form) {
            this.form = form;
        }

        protected abstract string GetNetworkType();

        protected void ListenLoop()
        {
            try
            {
                while (ThreadAlive)
                {
                    string dateServer = Reader.ReadLine();

                    if (dateServer == null)
                    {
                        // Clientul s-a deconectat
                        form.SetLabel("Adversarul s-a deconectat!", Color.Yellow);
                        break;
                    }

                    NetworkMessage message = JsonConvert.DeserializeObject<NetworkMessage>(dateServer);
                    form.MessageReceived(message);

                    // MessageBox.Show("Date primite de la " + GetNetworkType() + ": " + dateServer); // pentru debug
                }
            }
            catch (IOException e)
            {
                MessageBox.Show("" + GetNetworkType() + " - exceptie");
                // Connection closed or interrupted - this is expected during shutdown
            }
            catch (ObjectDisposedException)
            {
                // Stream was disposed - this is expected during shutdown
            }
            Stop();
        }

        public void Send(NetworkMessage message)
        {
            string msgString = JsonConvert.SerializeObject(message);
            Writer.WriteLine(msgString);
        }

        public void Stop()
        {
            ThreadAlive = false;
            
            try
            {
                NetStream.Close();
            }
            catch
            {
                // Ignore any errors during cleanup
            }
        }

    }
}
