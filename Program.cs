using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ŠahovskiMotor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Sahovnica s = new Sahovnica();
            ValidatorPotez vp = new ValidatorPotez(s);
            AI aI = new AI();

            foreach ((int rx1, int ry1, int rx2, int ry2) in aI.VsePoteze(s, true))
            {
                Console.WriteLine(s.Notacija((rx1, ry1), (rx2, ry2)));
            }
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

            //Polja[0, 2] = new Figura(TipFigure.Kraljica, Barva.Bela);
            //Polja[2, 2] = new Figura(TipFigure.Lovec, Barva.Bela);
        }

        public bool ZnotrajSahovnice(int x, int y)
        {
            if (x >= 0 && x <= 7 && y >= 0 && y <= 7)
                return true;
            return false;
        }

        public string PretvoriKoordinate(int x, int y) 
        {
            char stolpec = (char)('a' + x);
            char vrstica = (char)('1' + y);

            return $"{stolpec}{vrstica}";
        }

        public string Notacija((int, int) iz, (int, int) v, bool enPassant = false, TipFigure promocija = TipFigure.Kraljica)
        {
            Figura figura = Polja[iz.Item1, iz.Item2];
            Figura ciljnaFigura = Polja[v.Item1, v.Item2];

            string notacija = "";

            
            if (figura.TipFigure == TipFigure.Kralj && Math.Abs(iz.Item1 - v.Item1) == 2)
            {
                return (v.Item1 == 6) ? "O-O" : "O-O-O";
            }

            if (figura.TipFigure != TipFigure.Kmet)
                notacija += DobiOznakoFigure(figura.TipFigure);

            if (figura.TipFigure == TipFigure.Kmet && (ciljnaFigura != null || enPassant))
                notacija += (char)('a' + iz.Item1);

            if (ciljnaFigura != null || enPassant)
                notacija += "x";

            notacija += PretvoriKoordinate(v.Item1, v.Item2);

            if (figura.TipFigure == TipFigure.Kmet && (v.Item2 == 0 || v.Item2 == 7))
                notacija += "=" + DobiOznakoFigure(promocija);

            return notacija;
        }

        private string DobiOznakoFigure(TipFigure tip)
        {
            switch (tip)
            {
                case TipFigure.Konj: return "S";
                case TipFigure.Lovec: return "L";
                case TipFigure.Top: return "T";
                case TipFigure.Kraljica: return "D";
                case TipFigure.Kralj: return "K";
                default: return "";
            }
        }

        public (int, int) NajdiKralja(Barva barvaKralja)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Figura figura = Polja[i, j];
                    if (figura == null) continue;

                    if (figura != null && figura.TipFigure == TipFigure.Kralj && figura.Barva == barvaKralja)
                        return (i, j);
                }
            }
            throw new Exception("Ni kralja!");
        }

        //MakeMove returna novo šahovnico, da lažje preverjamo poteze za vnaprej
        public Sahovnica MakeMove((int, int) iz, (int, int) v, Sahovnica trenutnoStanje)
        {
            if (!ZnotrajSahovnice(iz.Item1, iz.Item2) || !ZnotrajSahovnice(v.Item1, v.Item2))
                return trenutnoStanje;

            //Pregledamo stanje figure
            Figura getFigura = Polja[iz.Item1, iz.Item2];

            if (getFigura == null)
                return trenutnoStanje;

            if (Polja[v.Item1, v.Item2] != null && Polja[v.Item1, v.Item2].Barva == getFigura.Barva)
                return trenutnoStanje; // Ne moremo premikati na lastno figuro

            //Šele tukaj se premaknemo v alternativni šahovnici, ko so vsi pogoji izpolnjeni
            trenutnoStanje.Polja[iz.Item1, iz.Item2] = null;
            trenutnoStanje.Polja[v.Item1, v.Item2] = getFigura;

            return trenutnoStanje;
        }

    }
}
