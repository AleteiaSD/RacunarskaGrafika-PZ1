using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PredmetniZadatak1.Models
{
    public class LinijaNaGridu
    {
        private long pocetniCvor;
        private long krajnjiCvor;
        private double x1;
        private double x2;
        private double y1;
        private double y2;


        public long PocetniCvor { get { return pocetniCvor; } set { pocetniCvor = value; } }
        public long KrajnjiCvor { get { return krajnjiCvor; } set { krajnjiCvor = value; } }

        public double X1 { get { return x1; } set { x1 = value; } }
        public double X2 { get { return x2; } set { x2 = value; } }
        public double Y1 { get { return y1; } set { y1 = value; } }
        public double Y2 { get { return y2; } set { y2 = value; } }

        public LinijaNaGridu(double x1, double y1, double x2, double y2, long pocetni, long krajnji)
        {
            this.X1 = x1;
            this.Y1 = y1;
            this.X2 = x2;
            this.Y2 = y2;
            this.PocetniCvor = pocetni;
            this.KrajnjiCvor = krajnji;
        }
    }
}
