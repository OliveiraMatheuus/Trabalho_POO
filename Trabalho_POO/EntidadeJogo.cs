using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Trabalho_POO
{
    internal abstract class EntidadeJogo
    {
        public PictureBox Sprite { get; protected set; }
        public int Velocidade { get; protected set; }
        public int Vidas { get; set; }
        public abstract void Mover();
    }

    public interface Destruivel
    {
        void ReceberDano();
        void Destruir();
    }
}
