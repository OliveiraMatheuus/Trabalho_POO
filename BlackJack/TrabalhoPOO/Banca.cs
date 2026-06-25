using System;

namespace TrabalhoPOO
{
    internal class Banca : Jogador
    {
        // Regra oficial: banca compra até atingir 17 ou mais, independente do jogador.
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