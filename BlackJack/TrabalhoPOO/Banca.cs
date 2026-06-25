using System;

namespace TrabalhoPOO
{
    internal class Banca : Jogador
    {
        public void ExecutarJogada(Baralho baralho)
        {
            while (CalcularPontos() < 17)
            {
                Carta carta = baralho.ComprarCarta();
                if (carta == null) break;
                ReceberCarta(carta);
            }
        }
    }
}