using System;

namespace TrabalhoPOO
{
    internal class Carta
    {
        public ValorCarta Valor { get; private set; }
        public Naipe NaipeCarta { get; private set; }
        public int Peso { get; private set; }
        public string Path { get; private set; }

        public Carta(ValorCarta valor, Naipe naipe)
        {
            Valor = valor;
            NaipeCarta = naipe;
            AtribuirPeso();
            AtribuirPath();
        }

        private void AtribuirPeso()
        {
            switch (Valor)
            {
                case ValorCarta.As: Peso = 11; break;
                case ValorCarta.Valete:
                case ValorCarta.Dama:
                case ValorCarta.Rei: Peso = 10; break;
                default: Peso = (int)Valor + 1; break;
            }
        }

        private void AtribuirPath()
        {
            string naipeStr = "";
            switch (NaipeCarta)
            {
                case Naipe.Paus: naipeStr = "clubs"; break;
                case Naipe.Copas: naipeStr = "hearts"; break;
                case Naipe.Espadas: naipeStr = "spades"; break;
                case Naipe.Ouros: naipeStr = "diamonds"; break;
            }

            string valorStr = "";
            switch (Valor)
            {
                case ValorCarta.As: valorStr = "A"; break;
                case ValorCarta.Valete: valorStr = "J"; break;
                case ValorCarta.Dama: valorStr = "Q"; break;
                case ValorCarta.Rei: valorStr = "K"; break;
                default: valorStr = ((int)Valor + 1).ToString(); break;
            }

            Path = $"deck_1/{naipeStr}_{valorStr}.png";
        }

        public override string ToString() => $"{Valor} de {NaipeCarta}";
    }
}