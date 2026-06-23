using System;
using System.Drawing;
using System.Windows.Forms;

namespace Trabalho_POO
{
    public class NaveJogador : EntidadeJogo, IAtira
    {
        private int _vidas;
        private int _velocidade;
        private int _larguraTela;

        public int Vidas => _vidas;
        public bool EstaVivo => _vidas > 0;

        public NaveJogador(Image sprite, int larguraTela, int alturaTela)
        {
            Sprite = sprite;
            Largura = 60;
            Altura = 40;
            _vidas = 3;
            _velocidade = 6;
            _larguraTela = larguraTela;
            X = (larguraTela / 2) - 30;
            Y = alturaTela - 80;
        }

        public void MoverEsquerda()
        {
            if (X - _velocidade >= 0) X -= _velocidade;
        }

        public void MoverDireita()
        {
            if (X + Largura + _velocidade <= _larguraTela) X += _velocidade;
        }

        public Projetil Atirar(Image spriteProjetil, int velocidade)
        {
            return new Projetil(X + Largura / 2 - 5, Y - 20,
                DirecaoProjetil.Cima, spriteProjetil, velocidade);
        }

        public void PerderVida() { _vidas--; }
    }
}