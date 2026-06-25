using System;
using System.Drawing;

namespace Trabalho_POO
{
    // Alien agora implementa IDestruivel explicitamente
    public class Alien : EntidadeJogo, IAtira, IMovivel, IDestruivel
    {
        private int _velocidade;
        private int _direcaoHorizontal;
        private int _larguraTela;

        public bool Ativo { get; private set; }
        public int VelocidadeAtual => _velocidade * _direcaoHorizontal;

        public Alien(Image sprite, int x, int y, int larguraTela, int velocidade)
        {
            Sprite = sprite;
            Largura = 35;
            Altura = 28;
            X = x; Y = y;
            _velocidade = velocidade;
            _direcaoHorizontal = 1;
            _larguraTela = larguraTela;
            Ativo = true;
        }

        public void Mover()
        {
            int novoX = X + (_velocidade * _direcaoHorizontal);
            if (novoX < 0) novoX = 0;
            else if (novoX + Largura > _larguraTela) novoX = _larguraTela - Largura;
            X = novoX;
        }

        public void InverterDirecao() { _direcaoHorizontal *= -1; }
        public void Descer(int pixels) { Y += pixels; }

        public Projetil Atirar(Image spriteProjetil, int velocidade)
        {
            return new Projetil(X + Largura / 2 - 5, Y + Altura + 5,
                DirecaoProjetil.Baixo, spriteProjetil, velocidade);
        }

        public void Destruir() { Ativo = false; }

        public bool AlcancouBordaInferior(int alturaTela) => Y + Altura >= alturaTela;
        public bool AlcancouBordaDireita() => X + Largura >= _larguraTela;
        public bool AlcancouBordaEsquerda() => X <= 0;
    }
}