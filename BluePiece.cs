using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moara
{
    public class BluePiece : Piece
    {
        public BluePiece(int id, Point locatie) : base(id, Properties.Resources.piesa_albastra_png, locatie) 
        {
            //PictureBox = new System.Windows.Forms.PictureBox();
            //PictureBox.Image = Properties.Resources.piesa_albastra_png;

        }

        public override void HighLight()
        {
            this.PictureBox.BackColor = Color.LightBlue;
        }
    }
}
