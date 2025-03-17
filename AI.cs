using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ŠahovskiMotor
{
    internal class AI
    {
        //MINIMAX TUKAJ
        

        // **


        public List<(int, int, int, int)> VsePoteze(Sahovnica trenutnoStanje, bool whiteTurn)
        {
            List<(int, int, int, int)> moznePoteze = new List<(int, int, int, int)>();
            ValidatorPotez vp = new ValidatorPotez(trenutnoStanje);
            Barva trenutnaBarva = whiteTurn ? Barva.Bela : Barva.Črna;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Figura polje = trenutnoStanje.Polja[i, j];

                    if (polje == null || polje.Barva != trenutnaBarva)
                        continue;

                    List<(int, int)> poteze = new List<(int, int)>();

                    switch (polje.TipFigure)
                    {
                        case TipFigure.Kmet:
                            poteze = vp.PotezeKmeta(i, j, trenutnaBarva);
                            break;
                        case TipFigure.Konj:
                            poteze = vp.PotezeKonja(i, j, trenutnaBarva);
                            break;
                        case TipFigure.Lovec:
                            poteze = vp.PotezeLovca(i, j, trenutnaBarva);
                            break;
                        case TipFigure.Top:
                            poteze = vp.PotezeTopa(i, j, trenutnaBarva);
                            break;
                        case TipFigure.Kraljica:
                            poteze = vp.PotezeKraljice(i, j, trenutnaBarva);
                            break;
                        case TipFigure.Kralj:
                            poteze = vp.PotezeKralja(i, j, trenutnaBarva);
                            break;
                        default:
                            break;
                    }

                    foreach (var p in poteze)
                    {
                        moznePoteze.Add((i, j, p.Item1, p.Item2));
                    }
                }
            }

            return moznePoteze;
        }

    }
}
