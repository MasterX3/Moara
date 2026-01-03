using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moara
{
    public class RedPiece : Piece
    {
        public RedPiece(int id, Point locatie) : base(id, Properties.Resources.piesa_rosie_png, locatie)
        {
            //PictureBox = new System.Windows.Forms.PictureBox();
            //PictureBox.Image = Properties.Resources.piesa_rosie_png;
        }

        public override void HighLight()
        {
            this.PictureBox.BackColor = Color.LightPink;
        }
    }
}
