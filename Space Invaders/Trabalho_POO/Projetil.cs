using System;
using System.Drawing;

namespace Trabalho_POO
{
    public enum DirecaoProjetil { Cima, Baixo }

    public class Projetil : EntidadeJogo, IMovivel
    {
        private int _velocidade;
        private DirecaoProjetil _direcao;

        public Projetil(int x, int y, DirecaoProjetil direcao, Image sprite, int velocidade)
        {
            X = x; Y = y;
            Largura = 10; Altura = 30;
            Sprite = sprite;
            _velocidade = velocidade;
            _direcao = direcao;
        }

        public void Mover()
        {
            if (_direcao == DirecaoProjetil.Cima) Y -= _velocidade;
            else Y += _velocidade;
        }

        public bool ForaDaTela(int alturaTela) => Y + Altura < 0 || Y > alturaTela;
    }
}