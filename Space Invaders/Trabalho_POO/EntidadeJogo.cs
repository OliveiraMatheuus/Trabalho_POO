using System;
using System.Drawing;
using System.Windows.Forms;

namespace Trabalho_POO
{
    public interface IMovivel { void Mover(); }
    public interface IAtira { Projetil Atirar(Image spriteProjetil, int velocidade); }

    /// <summary>
    /// Garante que qualquer entidade exponha seus limites de colisão
    /// de forma uniforme, sem depender do tipo concreto.
    /// </summary>
    public interface ICollidable { Rectangle Bounds { get; } }

    /// <summary>
    /// Entidades que podem ser destruídas expõem esse contrato.
    /// Permite que o GerenciadorDeColisoes opere sem conhecer os tipos concretos.
    /// </summary>
    public interface IDestruivel
    {
        bool Ativo { get; }
        void Destruir();
    }

    public abstract class EntidadeJogo : ICollidable
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