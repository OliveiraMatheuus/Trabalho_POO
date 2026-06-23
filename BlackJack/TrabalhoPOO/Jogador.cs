using System;
using System.Collections.Generic;

namespace TrabalhoPOO
{
    internal class Jogador
    {
        private List<Carta> _mao = new List<Carta>();

        public List<Carta> Mao { get => _mao; set => _mao = value; }

        public void ReceberCarta(Carta carta)
        {
            Mao.Add(carta);
        }

        public void LimparMao()
        {
            Mao.Clear();
        }

        public int CalcularPontos()
        {
            int total = 0;
            int quantidadeAses = 0;

            foreach (Carta carta in _mao)
            {
                total += carta.Peso;
                if (carta.Valor == "A") quantidadeAses++;
            }

            // Se estourou 21 e tem As valendo 11, transforma em 1
            while (total > 21 && quantidadeAses > 0)
            {
                total -= 10;
                quantidadeAses--;
            }

            return total;
        }
    }
}
