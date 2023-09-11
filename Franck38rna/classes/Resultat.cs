using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franck38rna.classes
{
    class Resultat
    {
        public int IdPrototype { get; set; }
        public double ValeurPrediteX { get; set; }
        public double ValeurAttendueX { get; set; }
        public double ErreurX { get; set; }
        public Resultat(int id, double vaX, double vpX)
        {
            IdPrototype = id;
            ValeurAttendueX = vaX;
            ValeurPrediteX = vpX;
            ErreurX = Math.Round(Math.Abs(vpX - vaX), 8);
        }
    }
}
