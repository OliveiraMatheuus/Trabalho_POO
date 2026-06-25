using System;
using System.Collections.Generic;

namespace TrabalhoPOO
{
    internal class Jogador
    {
        private readonly List<Carta> _mao = new List<Carta>();
        public IReadOnlyList<Carta> Mao => _mao;

        public void ReceberCarta(Carta carta)
        {
            if (carta == null) throw new ArgumentNullException(nameof(carta));
            _mao.Add(carta);
        }

        public void LimparMao()
        {
            _mao.Clear();
        }

        public int CalcularPontos()
        {
            int total = 0;
            int quantidadeAses = 0;

            foreach (Carta carta in _mao)
            {
                total += carta.Peso;
                if (carta.Valor == ValorCarta.As) quantidadeAses++;
            }

            while (total > 21 && quantidadeAses > 0)
            {
                total -= 10;
                quantidadeAses--;
            }

            return total;
        }

        public bool TemBlackjack() => _mao.Count == 2 && CalcularPontos() == 21;
        public bool Estourou() => CalcularPontos() > 21;

       
    }
}