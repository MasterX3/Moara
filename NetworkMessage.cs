using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moara
{
    public class NetworkMessage
    {
        public MoveType Type { get; set; }
        public int IdPiesa { get; set; }
        public int Player { get; set; }

        public int? PozitieNouaIndex { get; set; }

    }
}
