using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ŠahovskiMotor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Sahovnica s = new Sahovnica();
            ValidatorPotez vp = new ValidatorPotez(s);

            //foreach ((int rx, int ry) in vp.PotezeLovca(3, 2, Barva.Bela))
            //{
            //    Console.WriteLine(Sahovnica.PretvoriKoordinate(rx, ry));
            //}
        }
    }

    public enum TipFigure { Kmet, Konj, Lovec, Top, Kraljica, Kralj }
    public enum Barva { Bela, Črna, Nič }

    public class Figura
    {
        public TipFigure TipFigure { get; set; }
        public Barva Barva { get; set; }


        public Figura(TipFigure tipFigure, Barva barva) 
        { 
            TipFigure = tipFigure;
            Barva = barva;
        }
    }

    public class Sahovnica
    {
        public Figura[,] Polja { get; private set; }

        public Sahovnica()
        {
            Polja = new Figura[8, 8];
            PripraviSahovnico();
        }

        private void PripraviSahovnico()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Polja[i, j] = null;
                }
            }
            //Postavitev kmetov
            for (int i = 0; i < 8; i++)
            {
                Polja[i, 1] = new Figura(TipFigure.Kmet, Barva.Bela);
                Polja[i, 6] = new Figura(TipFigure.Kmet, Barva.Črna);
            }

            TipFigure[] vrstniRed = 
            { 
                TipFigure.Top, TipFigure.Konj, TipFigure.Lovec, 
                TipFigure.Kraljica, TipFigure.Kralj,
                TipFigure.Lovec, TipFigure.Konj, TipFigure.Top 
            };

            //Postavitev figur z uporabo simetrije - vrstnega reda zgoraj
            for (int i = 0; i < 8; i++)
            {
                Polja[i, 0] = new Figura(vrstniRed[i], Barva.Bela);
                Polja[i, 7] = new Figura(vrstniRed[i], Barva.Črna);
            }
        }

        public bool ZnotrajSahovnice(int x, int y)
        {
            if (x >= 0 && x <= 7 && y >= 0 && y <= 7)
                return true;
            return false;
        }

        public static string PretvoriKoordinate(int x, int y) 
        {
            char stolpec = (char)('a' + x);
            char vrstica = (char)('1' + y);

            return $"{stolpec}{vrstica}";
        }

        public bool MakeMove((int, int) iz, (int, int) v)
        {
            if (!ZnotrajSahovnice(iz.Item1, iz.Item2) || !ZnotrajSahovnice(v.Item1, v.Item2))
                return false;

            //Pregledamo stanje figure
            Figura getFigura = Polja[iz.Item1, iz.Item2];

            if (getFigura == null)
                return false;

            if (Polja[v.Item1, v.Item2] != null && Polja[v.Item1, v.Item2].Barva == getFigura.Barva)
                return false; // Ne moremo premikati na lastno figuro

            //Premik
            Polja[iz.Item1, iz.Item2] = null;
            Polja[v.Item1, v.Item2] = getFigura;

            return true;
        }
    }
}
