using System;
using System.Collections.Generic;

namespace TrabalhoPOO
{
    internal class Baralho
    {
        public List<Carta> Cartas { get; private set; }

        public Baralho()
        {
            Cartas = new List<Carta>();

            string[] naipes = { "Paus", "Copas", "Espadas", "Ouros" };
            string[] valores = { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };

            foreach (string naipe in naipes)
            {
                foreach (string valor in valores)
                {
                    Carta novaCarta = new Carta(valor, naipe);
                    Cartas.Add(novaCarta);
                }
            }

            Embaralhar();
        }

        public void Embaralhar()
        {
            Random rnd = new Random();
            int n = Cartas.Count;

            while (n > 1)
            {
                n--;
                int k = rnd.Next(n + 1);
                Carta value = Cartas[k];
                Cartas[k] = Cartas[n];
                Cartas[n] = value;
            }
        }

        public Carta ComprarCarta()
        {
            if (Cartas.Count > 0)
            {
                Carta cartaPuxada = Cartas[0];
                Cartas.RemoveAt(0);
                return cartaPuxada;
            }

            return null;
        }
    }
}
