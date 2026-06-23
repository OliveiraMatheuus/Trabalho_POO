using System;

namespace TrabalhoPOO
{
    internal class Banca : Jogador
    {
        public void ExecutarJogada(Baralho baralho, int pontosDoJogador)
        {
            
            if (pontosDoJogador > 21) return;

            while (CalcularPontos() <= 21)
            {
                int pontos = CalcularPontos();

                if (pontos >= 17 && pontos >= pontosDoJogador) break;

                ReceberCarta(baralho.ComprarCarta());
            }
        }
    }
}
