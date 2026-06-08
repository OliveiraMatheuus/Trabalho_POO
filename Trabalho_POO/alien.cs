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

        // ─── Propriedades ────────────────────────────────────────
        public bool Ativo { get; private set; }

        // ─── Construtor ──────────────────────────────────────────
        public Alien(Form form, Image sprite, int x, int y, int larguraTela)
        {
            _velocidade = 2;
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

        // ─── IMovivel ────────────────────────────────────────────
        public void Mover()
        {
            PictureBox.Left += _velocidade * _direcaoHorizontal;
        }

        // Chamado pelo Jogo quando o grupo todo precisa inverter
        public void InverterDirecao()
        {
            _direcaoHorizontal *= -1;
        }

        // Chamado pelo Jogo para descer uma linha ao inverter
        public void Descer(int pixels = 20)
        {
            PictureBox.Top += pixels;
        }

        // ─── IAtira ──────────────────────────────────────────────
        public Projetil Atirar(Image spriteProjetil)
        {
            int projetilX = PictureBox.Left + (PictureBox.Width / 2) - 5;
            int projetilY = PictureBox.Bottom + 5;

            return new Projetil(projetilX, projetilY, DirecaoProjetil.Baixo, spriteProjetil);
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