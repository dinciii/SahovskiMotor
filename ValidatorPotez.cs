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

        public List<(int, int)> PotezeKmeta(int x, int y, Barva barva)
        {
            List<(int, int)> poteze = new List<(int, int)>();

            int smer = (barva == Barva.Bela) ? 1 : -1; // Smer premika (beli gor, črni dol)
            int zacetnaVrstica = (barva == Barva.Bela) ? 1 : 6; // Kje začne kmet

            //Naprej za eno polje
            if (Sahovnica.ZnotrajSahovnice(x, y + smer) && Sahovnica.Polja[x, y + smer] == null)
            {
                poteze.Add((x, y + smer));

                //Naprej za dve polji (samo če je na začetnem mestu in pred njim prazno)
                if (y == zacetnaVrstica && Sahovnica.ZnotrajSahovnice(x, y + 2 * smer) && Sahovnica.Polja[x, y + 2 * smer] == null)
                {
                    poteze.Add((x, y + 2 * smer));
                }
            }

            //Jemanje levo (preveri meje in nasprotnika)
            if (Sahovnica.ZnotrajSahovnice(x - 1, y + smer))
            {
                Figura poljeLevo = Sahovnica.Polja[x - 1, y + smer];
                if (poljeLevo != null && poljeLevo.Barva != barva) // Lahko jemlje samo nasprotnika
                {
                    poteze.Add((x - 1, y + smer));
                }
            }

            //Jemanje desno (preveri meje in nasprotnika)
            if (Sahovnica.ZnotrajSahovnice(x + 1, y + smer))
            {
                Figura poljeDesno = Sahovnica.Polja[x + 1, y + smer];
                if (poljeDesno != null && poljeDesno.Barva != barva) // Lahko jemlje samo nasprotnika
                {
                    poteze.Add((x + 1, y + smer));
                }
            }

            return poteze;
        }


        public List<(int, int)> PotezeKonja(int x, int y, Barva barva)
        {
            List<(int, int)> legalnePoteze = new List<(int, int)>();
            List<(int, int)> smer = new List<(int, int)> 
            { 
                (2, 1), (1, 2), (-1, 2), (-2, 1), (-2, -1), (-1, -2), (1, -2), (2, -1)
            };

            foreach ((int dx, int dy) in smer)
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

        public List<(int, int)> PotezeKraljice(int x, int y, Barva barva)
        {
            List<(int, int)> legalnePoteze = new List<(int, int)>();
            List<(int, int)> smer = new List<(int, int)>
            {
                //Smeri Topa
                (1, 0), (0, 1), (-1, 0), (0, -1),
                //In smeri Lovca
                (1, 1), (1, -1), (-1, 1), (-1,-1)
            };

            legalnePoteze.AddRange(PotezeLovca(x, y, barva));
            legalnePoteze.AddRange(PotezeTopa(x, y, barva));
            return legalnePoteze;
        }

        public List<(int, int)> PotezeKralja(int x, int y, Barva barva)
        {
            List<(int, int)> legalnePoteze = new List<(int, int)>();
            List<(int, int)> smer = new List<(int, int)>
            {
                (1, 0), (1, 1), (0, 1), (-1, 1), (-1, 0), (-1, -1), (0, -1), (1, -1)
            };

            foreach ((int dx, int dy) in smer)
            {
                int novX = x + dx;
                int novY = y + dy;


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
