using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// ConfiguracaoRodada.cs
namespace Trabalho_POO
{
    public class ConfiguracaoRodada
    {
        // ─── Propriedades ────────────────────────────────────────
        public int Rodada { get; private set; }
        public int VelocidadeAlien { get; private set; }
        public int VelocidadeProjetil { get; private set; }
        public int IntervaloDisparoAlien { get; private set; }
        public int IntervaloMovimento { get; private set; }

        // ─── Construtor ──────────────────────────────────────────
        public ConfiguracaoRodada()
        {
            Rodada = 1;
            VelocidadeAlien = 2;
            VelocidadeProjetil = 8;
            IntervaloDisparoAlien = 60;
            IntervaloMovimento = 8;
        }

        // ─── Avança para a próxima rodada ────────────────────────
        public void Avancar()
        {
            Rodada++;

            // Aumenta progressivamente — com limite mínimo para não ficar impossível
            VelocidadeAlien = System.Math.Min(VelocidadeAlien + 1, 8);
            VelocidadeProjetil = System.Math.Min(VelocidadeProjetil + 2, 18);
            IntervaloDisparoAlien = System.Math.Max(IntervaloDisparoAlien - 8, 20);
            IntervaloMovimento = System.Math.Max(IntervaloMovimento - 1, 2);
        }
    }
}