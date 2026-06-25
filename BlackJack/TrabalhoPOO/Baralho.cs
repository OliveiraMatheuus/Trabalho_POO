using System;
using System.Collections.Generic;

namespace TrabalhoPOO
{
    internal class Baralho
    {
        private static readonly Random _rnd = new Random();

        public List<Carta> Cartas { get; private set; }

        public Baralho(int quantidadeDeBaralhos = 1)
        {
            if (quantidadeDeBaralhos < 1)
                throw new ArgumentException("É necessário ao menos um baralho.");

            Cartas = new List<Carta>();

            for (int i = 0; i < quantidadeDeBaralhos; i++)
            {
                foreach (string naipe in Carta.NaipesValidos)
                {
                    foreach (string valor in Carta.ValoresValidos)
                    {
                        Cartas.Add(new Carta(valor, naipe));
                    }
                }
            }

            Embaralhar();
        }

        public void Embaralhar()
        {
            int n = Cartas.Count;

            while (n > 1)
            {
                n--;
                int k = _rnd.Next(n + 1);
                Carta temp = Cartas[k];
                Cartas[k] = Cartas[n];
                Cartas[n] = temp;
            }
        }

        public Carta ComprarCarta()
        {
            if (Cartas.Count == 0)
                return null;

            Carta cartaPuxada = Cartas[0];
            Cartas.RemoveAt(0);
            return cartaPuxada;
        }

        public int CartasRestantes => Cartas.Count;
    }
}