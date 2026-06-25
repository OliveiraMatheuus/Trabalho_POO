using System;
using System.Drawing;

namespace Trabalho_POO
{
    /// <summary>
    /// Nave-mãe que cruza o topo da tela horizontalmente em intervalos aleatórios.
    /// Concede pontos bônus ao ser destruída.
    /// Demonstra herança de EntidadeJogo + IMovivel + IDestruivel.
    /// </summary>
    public class NaveUFO : EntidadeJogo, IMovivel, IDestruivel
    {
        private int _velocidade;
        private int _larguraTela;

        public bool Ativo { get; private set; }
        public int PontosBonus { get; }

        public NaveUFO(Image sprite, int larguraTela, bool vaiParaDireita = true)
        {
            Sprite = sprite;
            Largura = 50;
            Altura = 25;
            _larguraTela = larguraTela;
            _velocidade = vaiParaDireita ? 3 : -3;
            X = vaiParaDireita ? -Largura : larguraTela;
            Y = 20;
            Ativo = true;
            PontosBonus = 500;
        }

        public void Mover() { X += _velocidade; }
        public void Destruir() { Ativo = false; }

        public bool SaiuDaTela() => X + Largura < 0 || X > _larguraTela;

        public override void Desenhar(Graphics g)
        {
            if (!Ativo) return;

            if (Sprite != null)
            {
                g.DrawImage(Sprite, X, Y, Largura, Altura);
            }
            else
            {
                // Fallback visual sem sprite
                g.FillEllipse(Brushes.Magenta, X, Y, Largura, Altura);
                g.DrawEllipse(Pens.White, X, Y, Largura, Altura);
                g.FillEllipse(Brushes.Cyan,
                    X + Largura / 4, Y - Altura / 4,
                    Largura / 2, Altura / 2);
            }
        }
    }
}