using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ŠahovskiMotor
{
    public class ValidatorPotez
    {
        private Sahovnica Sahovnica;

        public ValidatorPotez(Sahovnica sahovnica)
        {
            Sahovnica = sahovnica;
        }

        public List<(int, int)>  PotezeKmeta(int x, int y, Barva barva)
        {
            List<(int, int)> poteze = new List<(int, int)>();

            int smer = 0;
            if (KmetNaZacMestu(x, y) && barva == Barva.Bela) { smer = 2; poteze.Add((x, y+1)); } 
            else if (KmetNaZacMestu(x, y) && barva == Barva.Črna) { smer = -2; poteze.Add((x, y-1)); }
            else if (!KmetNaZacMestu(x, y)) { smer = barva == Barva.Bela ? 1 : -1; }

            if (Sahovnica.ZnotrajSahovnice(x, y + smer) && Sahovnica.Polja[x, y + smer] == null)
                poteze.Add((x, y + smer));

            return poteze;
        }

        public List<(int, int)> PotezeKonja(int x, int y, Barva barva)
        {
            List<(int, int)> legalnePoteze = new List<(int, int)>();
            List<(int, int)> moznePoteze = new List<(int, int)> 
            { 
                (2, 1), (1, 2), (-1, 2), (-2, 1), (-2, -1), (-1, -2), (1, -2), (2, -1)
            };

            foreach ((int dx, int dy) in moznePoteze)
            {
                int novX = x + dx;
                int novY = y + dy;

                if (novX >= 0 && novX < 8 && novY >= 0 && novY < 8)
                {
                    Figura ciljnaFigura = Sahovnica.Polja[novX, novY];
                    if (ciljnaFigura == null || ciljnaFigura.Barva != barva)
                        legalnePoteze.Add((novX, novY));
                }
            }

            return legalnePoteze;
        }

        public List<(int, int)> PotezeLovca(int x, int y, Barva barva)
        {
            List<(int, int)> legalnePoteze = new List<(int, int)>();

            List<(int, int)> smer = new List<(int, int)>
            {
                //diagonale
                (1, 1), (1, -1), (-1, 1), (-1,-1)
            };

            foreach ((int dx, int dy) in smer)
            {
                int novX = x + dx;
                int novY = y + dy;

                while (Sahovnica.ZnotrajSahovnice(novX, novY))
                {
                    Figura ciljnaFigura = Sahovnica.Polja[novX, novY];

                    if (ciljnaFigura == null)
                    {
                        legalnePoteze.Add((novX, novY));
                    }
                    else
                    {
                        if (ciljnaFigura.Barva != barva)
                        {
                            legalnePoteze.Add((novX, novY));
                        }
                        //ustavi se ko pride do prijateljske ali nasprotnikove figure
                        break;
                    }
                    novX += dx;
                    novY += dy;
                }
            }
            return legalnePoteze;
        }

        public List<(int, int)> PotezeTopa(int x, int y, Barva barva)
        {
            List<(int, int)> legalnePoteze = new List<(int, int)>();
            List<(int, int)> smer = new List<(int, int)>
            {
                (1, 0), (0, 1), (-1, 0), (0, -1)
            };

            foreach ((int dx, int dy) in smer)
            {
                int novX = x + dx;
                int novY = y + dy;

                while (Sahovnica.ZnotrajSahovnice(novX, novY))
                {
                    Figura ciljnaFigura = Sahovnica.Polja[novX, novY];

                    if (ciljnaFigura == null)
                    {
                        legalnePoteze.Add((novX, novY));
                    }
                    else
                    {
                        if (ciljnaFigura.Barva != barva)
                        {
                            legalnePoteze.Add((novX, novY));
                        }
                        //ustavi se ko pride do prijateljske ali nasprotnikove figure
                        break;
                    }
                    novX += dx;
                    novY += dy;
                }
            }
            return legalnePoteze;
        }

        private bool KmetNaZacMestu(int x, int y)
        {
            if (y == 1 || y == 6)
                return true;
            return false;
        }
    }
}
