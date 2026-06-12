using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Trabalho_POO
{
    // ─── Interfaces ──────────────────────────────────────────────

    public interface IMovivel
    {
        void Mover();
    }

    // EntidadeJogo.cs — IAtira atualizada
    public interface IAtira
    {
        Projetil Atirar(Image spriteProjetil, int velocidade);
    }
    // ─── Classe Abstrata ─────────────────────────────────────────

    public abstract class EntidadeJogo
    {
        public PictureBox PictureBox { get; protected set; }

        public Rectangle Bounds => PictureBox.Bounds;
        public int X => PictureBox.Left;
        public int Y => PictureBox.Top;

        public virtual void Remover(Form form)
        {
            form.Controls.Remove(PictureBox);
            PictureBox.Dispose();
        }
    }
}