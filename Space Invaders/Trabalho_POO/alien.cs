using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Trabalho_POO
{
    public class Alien : EntidadeJogo, IAtira, IMovivel
    {
        // ─── Atributos ───────────────────────────────────────────
        private int _velocidade;
        private int _direcaoHorizontal; // +1 direita | -1 esquerda
        private int _larguraTela;
        public int VelocidadeAtual => _velocidade * _direcaoHorizontal;

        // ─── Propriedades ────────────────────────────────────────
        public bool Ativo { get; private set; }

        // ─── Construtor ──────────────────────────────────────────
        // Alien.cs — construtor atualizado
        public Alien(Form form, Image sprite, int x, int y, int larguraTela, int velocidade)
        {
            _velocidade = velocidade; // ← vem da ConfiguracaoRodada
            _direcaoHorizontal = 1;
            _larguraTela = larguraTela;
            Ativo = true;

            PictureBox = new PictureBox
            {
                Image = sprite,
                SizeMode = PictureBoxSizeMode.StretchImage,
                Size = new Size(50, 40),
                Left = x,
                Top = y,
                BackColor = Color.Transparent
            };

            form.Controls.Add(PictureBox);
            PictureBox.BringToFront();
        }

        // ─── IMovivel ───────────────────────────────────────────
        public void Mover()
        {
            int novoX = PictureBox.Left + (_velocidade * _direcaoHorizontal);

            // Garante que nunca ultrapasse a borda
            if (novoX < 0)
                novoX = 0;
            else if (novoX + PictureBox.Width > _larguraTela)
                novoX = _larguraTela - PictureBox.Width;

            PictureBox.Left = novoX;
        }

        // Chamado pelo Jogo quando o grupo todo precisa inverter
        public void InverterDirecao()
        {
            _direcaoHorizontal *= -1;
        }

        // Chamado pelo Jogo para descer uma linha ao inverter
        public void Descer(int pixels)
        {
            PictureBox.Top += pixels;
        }

        // ─── IAtira ──────────────────────────────────────────────
        // Alien.cs — Atirar atualizado
        public Projetil Atirar(Image spriteProjetil, int velocidade)
        {
            int projetilX = PictureBox.Left + (PictureBox.Width / 2) - 5;
            int projetilY = PictureBox.Bottom + 5;

            return new Projetil(projetilX, projetilY, DirecaoProjetil.Baixo, spriteProjetil, velocidade);
        }

        // ─── Destruição ──────────────────────────────────────────
        public void Destruir(Form form)
        {
            Ativo = false;
            Remover(form);
        }

        // ─── Alcançou a borda inferior? ──────────────────────────
        public bool AlcancouBordaInferior(int alturaTela)
        {
            return PictureBox.Bottom >= alturaTela;
        }

        // ─── Alcançou as bordas laterais? ────────────────────────
        public bool AlcancouBordaDireita()
        {
            return PictureBox.Right >= _larguraTela;
        }

        public bool AlcancouBordaEsquerda()
        {
            return PictureBox.Left <= 0;
        }
    }
}