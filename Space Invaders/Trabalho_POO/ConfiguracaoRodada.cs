using System;

namespace Trabalho_POO
{
    public class ConfiguracaoRodada
    {
        public const int RodadaMaxima = 5;

        public int Rodada { get; private set; }
        public int VelocidadeAlien { get; private set; }
        public int VelocidadeProjetil { get; private set; }
        public int IntervaloDisparoAlien { get; private set; }
        public int LinhasAlien { get; private set; }
        public int ColunasAlien { get; private set; }
        public int LarguraAlien { get; private set; }
        public int AlturaAlien { get; private set; }
        public int EspacoXAlien { get; private set; }
        public int EspacoYAlien { get; private set; }

        public bool JogoCompleto => Rodada > RodadaMaxima;

        public ConfiguracaoRodada()
        {
            Rodada = 1;
            VelocidadeAlien = 3;
            VelocidadeProjetil = 8;
            IntervaloDisparoAlien = 60;

            LinhasAlien = 3;
            ColunasAlien = 7;
            LarguraAlien = 35;
            AlturaAlien = 28;
            EspacoXAlien = 15;
            EspacoYAlien = 15;
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