using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Trabalho_POO
{
    public delegate void EntidadeDestruidaHandler();

    internal class NaveJogador : EntidadeJogo, Destruivel
    {
        public event EntidadeDestruidaHandler Destruida;

        // --- CONSTRUTOR ---
        public NaveJogador(PictureBox spriteNave)
        {
            // Atribuindo valores às propriedades herdadas de EntidadeJogo

            Sprite = spriteNave;
            Velocidade = 1;
            Vidas = 3;
        }

        public void ReceberDano()
        {
            Vidas--;

            
            if (Vidas <= 0)
            {
                Destruir();
            }
        }

        public void Destruir()
        {
            
            Destruida?.Invoke();
            Sprite.Dispose();
        }

       

        public override void Mover(A)
        {
            
        }


        public void MoverEsquerda()
        {
            
            if (Sprite.Left > 0)
            {
                Sprite.Left -= Velocidade;
            }
        }

        public void MoverDireita(int larguraTela)
        {
            
            if (Sprite.Right < larguraTela)
            {
                Sprite.Left += Velocidade;
            }
        }
    }
}