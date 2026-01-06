using System;
using System.Drawing;
//using System.Reflection.Emit;
using System.Windows.Forms;

namespace Moara
{
    public partial class Form1 : Form
    {
        NetworkModule networkModule;
        GameLogic gameLogic;
        private Label label;
        private int me;
        
        public Form1()
        {
            InitializeComponent();

            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            this.UpdateStyles();
            

            this.MouseClick += Board_MouseClick;

            //this.MouseMove += Form1_MouseMove_Coordonate;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //this.BackgroundImage = Properties.Resources.moara_png;
            //this.BackgroundImageLayout = ImageLayout.Stretch;

        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            this.BackgroundImage = Properties.Resources.moara_png;
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.ClientSize = new Size(600, 600);

            gameLogic = new GameLogic();


            foreach (Piece p in gameLogic.JucatorR1.PieseRosii)
            {
                
                p.PictureBox.MouseClick += Piesa_Click;

                this.Controls.Add(p.PictureBox);

            }

            foreach (Piece p in gameLogic.JucatorB2.PieseAlbastre)
            {
                
                p.PictureBox.MouseClick += Piesa_Click;

                this.Controls.Add(p.PictureBox);
            }

            label = new Label();
            label.AutoSize = false;
            label.Size = new Size(110, 100);

            label.TextAlign = ContentAlignment.MiddleCenter;
            label.Font = new Font("Arial", 10, FontStyle.Bold);
            label.BackColor = Color.Transparent;
            label.ForeColor = Color.Black;
            label.Text = "Start joc: Randul jucatorului rosu";

            label.BringToFront();
            label.Location = new Point((this.ClientSize.Width - label.Width) / 2, 250);
            this.Controls.Add(label);
        }

        private void UpdateLabel()
        {
            if (label.InvokeRequired)
            {
                label.Invoke(new Action(() =>
                {
                    label.Text = gameLogic.GetMessage();
                    label.ForeColor = gameLogic.GetColor();
                }));
            }
            else
            {
                label.Text = gameLogic.GetMessage();
                label.ForeColor = gameLogic.GetColor();
            }
            //label.BringToFront();
        }

        private void Piesa_Click(object sender, EventArgs e)
        {
            if(gameLogic.turn != me)
            {
                return;
            }

            PictureBox pb = sender as PictureBox;
            Piece piesa = gameLogic.GetPiece(pb);

            int idPiesa = piesa.id;
            int turn = gameLogic.turn % 2 + 1; // de la celalalt jucator

            if (gameLogic.moara)
            {
                gameLogic.moara = false;
                bool eliminat = gameLogic.RemovePiece(piesa.Pozitie);

                if (eliminat)
                {
                    gameLogic.boardState[piesa.Pozitie] = 0;
                    piesa.Pozitie = -1;
                    this.Controls.Remove(pb);
                    UpdateLabel();

                    gameLogic.UpdateNumberOfPieces(piesa);
                    UpdateLabel();

                    NetworkMessage message = new NetworkMessage
                    {
                        Type = MoveType.RemovePiece,
                        IdPiesa = idPiesa,
                        Player = turn
                    };
                    networkModule.Send(message);
                }
                else
                    gameLogic.moara = true;
            }
            else
            {
                gameLogic.SelectPiece();
            }
            UpdateLabel();
        }

        private void Board_MouseClick(object sender, MouseEventArgs e)
        {
            if (gameLogic.turn != me)
            {
                return;
            }

            int pozitieNouaIndex = gameLogic.CeaMaiApropiataPozitie(e.Location);
            int idPiesa = gameLogic.piesaDeMutat != null ? gameLogic.piesaDeMutat.id : -1;
            int turn = gameLogic.turn;
            bool moved = gameLogic.MovePiece(pozitieNouaIndex);
            if(moved)
            {
                NetworkMessage message = new NetworkMessage
                {
                    Type = MoveType.MovePiece,
                    IdPiesa = idPiesa,
                    Player = turn,
                    PozitieNouaIndex = pozitieNouaIndex
                };
                networkModule.Send(message);
            }
            UpdateLabel();
        }

        private void startServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            networkModule = new Server(this);
            me = 1;
        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            networkModule = new Client("127.0.0.1", 7071, this);
            me = 2;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (networkModule != null)
            {
                networkModule.Stop();
            }
        }

        internal void MessageReceived(NetworkMessage message)
        {
            if (message.Type == MoveType.MovePiece)
            {
                gameLogic.GetPiece(message.IdPiesa, message.Player);
                gameLogic.SelectPiece();

                gameLogic.MovePiece((int)message.PozitieNouaIndex);
            }
            if (message.Type == MoveType.RemovePiece)
            {
                if (gameLogic.moara)
                {
                    gameLogic.moara = false;
                    Piece piesa = gameLogic.GetPiece(message.IdPiesa, message.Player);
                    gameLogic.RemovePiece(piesa.Pozitie);
                    gameLogic.boardState[piesa.Pozitie] = 0;
                    piesa.Pozitie = -1;
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new Action(() => this.Controls.Remove(piesa.PictureBox)));
                    }
                    else
                    {
                        this.Controls.Remove(piesa.PictureBox);
                    }
                    gameLogic.UpdateNumberOfPieces(piesa);
                }
                else
                {
                    MessageBox.Show("Am primit mutare de remove când nu e moară");
                }
            }

            UpdateLabel();

        }
    }
}
