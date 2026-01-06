using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Moara
{
    public class GameLogic
    {
        public PlayerRed1 JucatorR1;
        public PlayerBlue2 JucatorB2;

        public Piece piesaDeMutat = null;
        Piece piesaApasata = null;
        public Piece rezerva = null;

        bool gameOn = true;
        public bool moara = false;

        int pieseDePusRosii = 9;
        int pieseDePusAlbastre = 9;
        int winner = 0;


        public int[] boardState = new int[24];

        public Point[] PozitiiTabla;
        

        private List<int[]> mills;
        private List<int[]> vecini;

        public int turn { get; set; } = 1;

        public int GetBoardState(int index) => boardState[index];

        public GameLogic() 
        {
            JucatorR1 = new PlayerRed1();
            JucatorB2 = new PlayerBlue2();

            

            PozitiiTabla = new Point[24] {

            // primul patrat (exterior)

            new Point(76, 75),   // sus-stanga
            new Point(301, 75),  // sus-mijloc
            new Point(525, 75),  // sus-dreapta
            new Point(76, 300),  // mijloc-stanga
            new Point(76, 525),  // jos-stanga
            new Point(301, 525), // jos-mijloc
            new Point(525, 525), // jos-dreapta
            new Point(525, 300), // mijloc-dreapta

            // al doilea patrat (mijloc)

            new Point (151, 150),  // sus-stanga
            new Point(301, 150),   // sus-mijloc
            new Point(450, 150),   // sus-dreapta
            new Point(151, 300),   // mijloc-stanga
            new Point(151, 450),   // jos-stanga
            new Point(301, 450),   // jos-mijloc
            new Point(450, 450),   // jos-dreapta
            new Point(450, 300),   // mijloc-dreapta

            // al treilea patrat (interior)

            new Point(226, 226),   // sus-stanga
            new Point(301, 226),   // sus-mijloc
            new Point(375, 226),   // sus-dreapta
            new Point(226, 300),   // mijloc-stanga
            new Point(226, 375),   // jos-stanga
            new Point(300, 375),   // jos-mijloc
            new Point(375, 375),   // jos-dreapta
            new Point(375, 300)    // mijloc-dreapta

        };

        mills = new List<int[]>
            {
                // patratul exterior (0-7)
                new int[] {0, 1, 2}, new int[] {2, 7, 6}, new int[] {6, 5, 4}, new int[] {4, 3, 0},
                //patratul mijlociu (8-15)
                new int[] {8, 9, 10}, new int[] {10, 15, 14}, new int[] {14, 13, 12}, new int[] {12, 11, 8},
                //patratul interior (16-23)
                new int[] {16, 17, 18}, new int[] {18, 23, 22}, new int[] {22, 21, 20}, new int[] {20, 19, 16},
                //linii de mijloc
                new int[] {1, 9, 17}, new int[] {5, 13, 21}, new int[] {3, 11, 19}, new int[] {7, 15, 23}
            };

            vecini = new List<int[]> {
                //patratul exterior
                new[] {1, 3},
                new[] {0, 2, 9},
                new[] {1, 7 },
                new[] {0, 4, 11},
                new[] {3, 5},
                new[] {4, 6, 13},
                new[] {5, 7},
                new[] {2, 6, 15},

                //patratul mijlociu
                new[] {9, 11},
                new[] {8, 10, 1, 17},
                new[] {9, 15},
                new[] {8, 12, 3, 19},
                new[] {11, 13},
                new[] {12, 14, 5, 21},
                new[] {13, 15},
                new[] {10, 14, 7, 23},

                //patratul interior
                new[] {17, 19},
                new[] {16, 18, 9},
                new[] {17, 23},
                new[] {16, 20, 11},
                new[] {19, 21},
                new[] {20, 22, 13},
                new[] {21, 23},
                new[] {18, 22, 15}
            };
        }

        private bool PiesaBlocata(int pozitieCurenta)
        {
            int[] veciniPiesa = vecini[pozitieCurenta];

            foreach(int vecin in  veciniPiesa)
            {
                if (boardState[vecin] == 0)
                {
                    return false;
                }
            }

            return true;
        }

        private bool ExistaMutariValide()
        {
            if((turn == 1 && pieseDePusRosii > 0) || (turn == 2 && pieseDePusAlbastre > 0))
            {
                return true;
            }

            if((turn == 1 && JucatorR1.PieseRosii.Count() <= 3) || (turn == 2 && JucatorB2.PieseAlbastre.Count() <= 3))
            {
                return true;
            }

            for(int i = 0; i < 24; i++)
            {
                if (boardState[i] == turn)
                {
                    if (!PiesaBlocata(i))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool EsteMutareValida(int pozitieCurenta, int pozitieNoua)
        {
            return Array.IndexOf(vecini[pozitieCurenta], pozitieNoua) != -1;
        }

        private bool TryPlacePiece(int positionIndex)
        {

            boardState[positionIndex] = turn;

            bool newMill = CheckNewMill(positionIndex);

            if (!newMill)
                SwitchTurn();

            return newMill;
        }

        private bool CheckNewMill(int positionIndex)
        {
            int piesaCurenta = boardState[positionIndex];

            if(piesaCurenta == 0) return false;

            foreach(var line in mills)
            {
                if(Array.IndexOf(line, positionIndex) != -1)
                {
                    if (boardState[line[0]] == piesaCurenta && boardState[line[1]] == piesaCurenta && boardState[line[2]] == piesaCurenta)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void SwitchTurn()
        {
            if (turn == 1)
                turn = 2;
            else
                turn = 1;

            if (!ExistaMutariValide())
            {
                if (turn == 1)
                {
                    winner = 2;
                }
                else
                    winner = 1;
                gameOn = false;
            }
        }

        private bool AllPiecesAreInMill(int opponentColor)
        {
            for(int i = 0; i < 24; i++)
            {
                if(boardState[i] == opponentColor)
                {
                    if(!CheckNewMill(i)) return false;
                }
            }
            return true;
        }

        public bool RemovePiece(int pieceIndex)
        {
            if (pieceIndex != -1)
            {
                int piesaDeScos = boardState[pieceIndex];
                int adversar = (turn == 1) ? 2 : 1;

                if (!AllPiecesAreInMill(adversar))
                {
                    if (piesaDeScos == adversar && !CheckNewMill(pieceIndex))
                    {
                        boardState[pieceIndex] = 0;

                        SwitchTurn();
                        return true;
                    }
                }
                else
                {
                    if (piesaDeScos == adversar)
                    {
                        boardState[pieceIndex] = 0;

                        SwitchTurn();
                        return true;
                    }
                }
                
            }
            return false;
        }

        public string GetMessage()
        {
            if (winner == 1)
            {
                if (JucatorB2.PieseAlbastre.Count() >= 3)
                {
                    return ("Piese \nblocate!\nJucătorul \nroșu \na câștigat!");
                }
                else {
                    return ("Jucătorul roșu a câștigat!");
                }
            }
            else
            {
                if (winner == 2)
                {
                    if(JucatorR1.PieseRosii.Count() >= 3)
                    {
                        return ("Piese \nblocate!\nJucătorul \nalbastru \na câștigat!");
                    }
                    else
                    {
                        return ("Jucătorul albastru a câștigat!");
                    }
                    
                }
            }

            if (moara)
            {
                return ("Moara formată!\nSelectează o piesă a adversarului!");
            }

            if (turn == 1)
            {
                return ("Rândul jucătorului roșu");
            }
            else
            {
                return ("Rândul jucătorului albastru");
            }
        }

        public Color GetColor()
        {
            if(winner != 0)
            {
                return Color.Yellow;
            }
            else
            {
                if(moara)
                {
                    return Color.DarkGreen;
                }
                else
                {
                    if(turn == 1)
                    {
                        return Color.Red;
                    }
                    else
                    {
                        return Color.Blue;
                    }
                }
            }
        }

        public void UpdateNumberOfPieces(Piece piece)
        {
            if (piece is BluePiece)

            {
                JucatorB2.RemoveBlue(piece);
                
                if (JucatorB2.PieseAlbastre.Count() < 3)
                {
                    winner = 1;
                    
                    gameOn = false;

                }
                //piece.Plasat = false;
            }
            else
            {
                JucatorR1.RemoveRed(piece);
                
                if (JucatorR1.PieseRosii.Count() < 3)
                {
                    winner = 2;
                    
                    gameOn = false;

                }
                //piece.Plasat = false;

            }
            return;
        }

        public void SelectPiece()
        {
            if (gameOn)
            {
                if ((turn == 1 && piesaApasata is RedPiece) ||
                        (turn == 2 && piesaApasata is BluePiece))
                {

                    if (piesaApasata.Plasat == true)
                    {
                        if (pieseDePusRosii > 0 || pieseDePusAlbastre > 0)
                            return;

                        int pieseRamase = (turn == 1) ? JucatorR1.PieseRosii.Count() : JucatorB2.PieseAlbastre.Count();

                        if (pieseRamase > 3)
                        {
                            if (PiesaBlocata(piesaApasata.Pozitie))
                            {
                                return;
                            }
                        }
                    }

                    if (piesaDeMutat != null)
                    {
                        piesaDeMutat.PictureBox.BackColor = Color.Transparent;
                        if (rezerva != null)
                            rezerva.PictureBox.BackColor = Color.Transparent;
                    }

                    piesaDeMutat = piesaApasata;

                    if (rezerva != null)
                        rezerva.PictureBox.BackColor = Color.Transparent;
                    piesaDeMutat.HighLight();

                    if (piesaDeMutat.PictureBox.InvokeRequired)
                    {
                        piesaDeMutat.PictureBox.Invoke(new Action(() => piesaDeMutat.PictureBox.BringToFront()));
                    }
                    else
                    {
                        piesaDeMutat.PictureBox.BringToFront();
                    }
                }
            }
        }

        public Piece GetPiece(PictureBox pb)
        {
            if (piesaDeMutat != null)
                rezerva = piesaDeMutat;

            
            piesaApasata = GasestePiesaDupaPictureBox(pb);
            return piesaApasata;
        }

        public Piece GetPiece(int id, int player)
        {
            if (piesaDeMutat != null)
                rezerva = piesaDeMutat;

            if (player == 1)
            {
                foreach (Piece item in JucatorR1.PieseRosii)
                {
                    if (item.id == id)
                    {
                        piesaApasata = item;
                    }
                }
            }
            else
            {
                foreach (Piece item in JucatorB2.PieseAlbastre)
                {
                    if (item.id == id)
                    {
                        piesaApasata = item;
                    }
                }
            }

            return piesaApasata;
        }

        public bool MovePiece(int pozitieNouaIndex)
        {
            if (piesaDeMutat != null && gameOn && !moara)
            {

                if (pozitieNouaIndex != -1)
                {

                    if (GetBoardState(pozitieNouaIndex) == 0)
                    {

                        if (piesaDeMutat.Plasat)
                        {
                            boardState[piesaDeMutat.Pozitie] = 0;

                            bool mutareValida = EsteMutareValida(piesaDeMutat.Pozitie, pozitieNouaIndex);

                            int pieseRamase = (turn == 1) ? JucatorR1.PieseRosii.Count() : JucatorB2.PieseAlbastre.Count();

                            if (pieseRamase > 3 && !mutareValida)
                            {
                                return false;
                            }
                        }
                        else
                        {
                            if (piesaDeMutat is RedPiece)
                            {
                                pieseDePusRosii--;
                            }
                            else
                            {
                                pieseDePusAlbastre--;
                            }
                        }

                        if (piesaDeMutat.PictureBox.InvokeRequired)
                        {
                            piesaDeMutat.PictureBox.Invoke(new Action(() =>
                            {
                                piesaDeMutat.PictureBox.Location = new Point(
                                    PozitiiTabla[pozitieNouaIndex].X - (piesaDeMutat.PictureBox.Width / 2),
                                    PozitiiTabla[pozitieNouaIndex].Y - (piesaDeMutat.PictureBox.Height / 2));

                                piesaDeMutat.PictureBox.BackColor = Color.Transparent;
                            }));
                        }
                        else
                        {
                            piesaDeMutat.PictureBox.Location = new Point(
                                PozitiiTabla[pozitieNouaIndex].X - (piesaDeMutat.PictureBox.Width / 2),
                                PozitiiTabla[pozitieNouaIndex].Y - (piesaDeMutat.PictureBox.Height / 2));

                            piesaDeMutat.PictureBox.BackColor = Color.Transparent;
                        }

                        piesaDeMutat.Pozitie = pozitieNouaIndex;
                        piesaDeMutat.Plasat = true;

                        moara = TryPlacePiece(pozitieNouaIndex);


                        piesaDeMutat = null;

                        return true;
                    }


                }
                else
                {

                    piesaDeMutat.PictureBox.BackColor = Color.Transparent;
                    piesaDeMutat = null;
                }
            }

            return false;
        }

        

        public int CeaMaiApropiataPozitie(Point p)
        {
            int index = -1;

            for (int i = 0; i < 24; i++)
            {
                double distanta = Math.Sqrt(Math.Pow(p.X - PozitiiTabla[i].X, 2) + Math.Pow(p.Y - PozitiiTabla[i].Y, 2));

                if (distanta < 50)
                {
                    index = i;
                }
            }

            return index;
        }

        private Piece GasestePiesaDupaPictureBox(PictureBox pb)
        {
            foreach (Piece item in JucatorR1.PieseRosii)
            {
                if (item.PictureBox == pb)
                { return item; }
            }

            foreach (Piece item in JucatorB2.PieseAlbastre)
            {
                if (item.PictureBox == pb)
                { return item; }
            }

            return null;
        }

    }
}
