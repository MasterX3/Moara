using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.CodeDom;

namespace Moara
{
    public class Piece
    {
        public int Pozitie { get; set; }

        public Point PozitieInitiala { get; set; }

        public Point PozitieAnterioara { get; set; }
        public int id { get; set; }
        public bool Plasat { get; set; }

        public PictureBox PictureBox { get; set; }

        public Size size { get; set; }

        public Piece(int id, Image img, Point locatie)
        {
            Pozitie = -1;
            Plasat = false;
            this.id = id;
            PozitieInitiala = locatie;
            PozitieAnterioara = locatie;
            PictureBox = new PictureBox();
            PictureBox.Location = locatie;
            PictureBox.Image = img;
            PictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            //PictureBox.Parent = parentForm;
            PictureBox.BackColor = Color.Transparent;
            PictureBox.BringToFront();
            PictureBox.Size = new System.Drawing.Size(60, 60);

            
        }

        public virtual void HighLight()
        {
            this.PictureBox.BackColor = Color.Gold;
        }

    }
}
