using System;

namespace Trabalho_POO
{
    public class ConfiguracaoRodada
    {
        public const int RodadaMaxima = 5; // vitória total após esta rodada

        public int Rodada { get; private set; }
        public int VelocidadeAlien { get; private set; }
        public int VelocidadeProjetil { get; private set; }
        public int IntervaloDisparoAlien { get; private set; }
        public int IntervaloMovimento { get; private set; }
        public bool JogoCompleto => Rodada > RodadaMaxima;

        public ConfiguracaoRodada()
        {
            Rodada = 1;
            VelocidadeAlien = 3;
            VelocidadeProjetil = 8;
            IntervaloDisparoAlien = 60;
            IntervaloMovimento = 1;
        }

        public void Avancar()
        {
            Rodada++;
            VelocidadeAlien = Math.Min(VelocidadeAlien + 1, 5);
            VelocidadeProjetil = Math.Min(VelocidadeProjetil + 2, 18);
            IntervaloDisparoAlien = Math.Max(IntervaloDisparoAlien - 8, 20);
        }
    }
}