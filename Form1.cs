using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Reflection;
//using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Moara
{
    public partial class Form1 : Form
    {

        GameLogic gameLogic;
        private Label label;
        



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
            label.Text = gameLogic.GetMessage();
            label.ForeColor = gameLogic.GetColor();
            //label.BringToFront();
        }

        private void Piesa_Click(object sender, EventArgs e)
        {
            PictureBox pb = sender as PictureBox;
            Piece piesa = gameLogic.GetPiece(pb);


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
                }
                else
                    gameLogic.moara = true;
            }
            else
            {
                gameLogic.SelectPiece();
                UpdateLabel();
            }
            UpdateLabel();
        }

        private void Board_MouseClick(object sender, MouseEventArgs e)
        {

            int pozitieNouaIndex = gameLogic.CeaMaiApropiataPozitie(e.Location);
            gameLogic.MovePiece(pozitieNouaIndex);
            UpdateLabel();
        }



        //private void Form1_MouseMove_Coordonate(object sender, MouseEventArgs e)
        //{
        //    // Asta va scrie coordonatele mouse-ului în bara de titlu a ferestrei
        //    this.Text = $"X: {e.X}, Y: {e.Y}";
        //}

    }
}
