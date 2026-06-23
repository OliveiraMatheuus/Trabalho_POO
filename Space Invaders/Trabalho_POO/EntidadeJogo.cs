using System;
using System.Drawing;
using System.Windows.Forms;

namespace Trabalho_POO
{
    public interface IMovivel { void Mover(); }
    public interface IAtira { Projetil Atirar(Image spriteProjetil, int velocidade); }

    public abstract class EntidadeJogo
    {
        public int X { get; protected set; }
        public int Y { get; protected set; }
        public int Largura { get; protected set; }
        public int Altura { get; protected set; }
        public Image Sprite { get; protected set; }
        public Rectangle Bounds => new Rectangle(X, Y, Largura, Altura);

        public virtual void Desenhar(Graphics g)
        {
            if (Sprite != null)
                g.DrawImage(Sprite, X, Y, Largura, Altura);
        }
    }
}