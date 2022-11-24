using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PredmetniZadatak1.Models
{
    public class TackaNaGridu
    {
        private bool posecen = false;
        private bool sadrziElement = false;
        private List<long> posecenId = new List<long>();

        public bool Posecen { get { return posecen; } set { posecen = value; } }
        public bool SadrziElement { get { return sadrziElement; } set { sadrziElement = value; } }
        public List<long> PosecenId { get { return posecenId; } set { posecenId = value; } }

        public TackaNaGridu() { }
    }
}
