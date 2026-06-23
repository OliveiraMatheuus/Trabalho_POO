using System.Drawing;

namespace Trabalho_POO
{
    public class Barreira : EntidadeJogo
    {
        public int Vida { get; private set; }
        public bool Ativa => Vida > 0;

        public Barreira(Image sprite, int x, int y)
        {
            
            Sprite = sprite;
            X = x;
            Y = y;
            Largura = 80;
            Altura = 20;
            Vida = 10;
        }

        public void ReceberDano()
        {
            Vida--;
        }

        
        public override void Desenhar(Graphics g)
        {
            Brush corAtual;

            
            if (Vida > 6)
                corAtual = Brushes.LimeGreen; 
            else if (Vida > 3)
                corAtual = Brushes.Yellow;    
            else
                corAtual = Brushes.Red;       

            g.FillRectangle(corAtual, Bounds);
            g.DrawRectangle(Pens.Black, Bounds);
        }
   
    }
}