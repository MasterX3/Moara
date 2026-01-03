using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moara
{
    public class PlayerRed1
    {
        public List<Piece> PieseRosii { get; set; }

        public PlayerRed1() 
        { 
            PieseRosii = new List<Piece>();

            int x = 20;
            int y = -7;
            int space = 3;

            for(int i = 0; i < 9; i++)
            {
                RedPiece piece = new RedPiece(i, new System.Drawing.Point(x + i * (60 + space), y));
                PieseRosii.Add(piece);
            }
        }

        public void RemoveRed(Piece piece)
        {
            if(PieseRosii.Contains(piece))
                PieseRosii.Remove(piece);
        }
    }
}
