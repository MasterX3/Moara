using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moara
{
    public class PlayerBlue2
    {
        public List<Piece> PieseAlbastre { get; set; }

        public PlayerBlue2()
        {
            PieseAlbastre = new List<Piece>();

            int x = 20;
            int y = 550;
            int space = 3;

            for (int i = 0; i < 9; i++)
            {
                BluePiece piece = new BluePiece(i, new System.Drawing.Point(x + i * (60 + space), y));
                PieseAlbastre.Add(piece);
            }
        }

        public void RemoveBlue(Piece piece)
        {
            if(PieseAlbastre.Contains(piece))
                PieseAlbastre.Remove(piece);
        }
    }
}
