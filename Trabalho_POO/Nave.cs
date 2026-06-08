using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Trabalho_POO
{
    public class NaveJogador : EntidadeJogo, IAtira
    {
        // ─── Atributos ───────────────────────────────────────────
        private int _vidas;
        private int _velocidade;
        private int _larguraTela;

        // ─── Propriedades ────────────────────────────────────────
        public int Vidas => _vidas;
        public bool EstaVivo => _vidas > 0;

        // ─── Construtor ──────────────────────────────────────────
        public NaveJogador(Form form, Image sprite, int larguraTela)
        {
            _vidas = 3;
            _velocidade = 6;
            _larguraTela = larguraTela;

            PictureBox = new PictureBox
            {
                Image = sprite,
                SizeMode = PictureBoxSizeMode.StretchImage,
                Size = new Size(60, 40),
                Left = (larguraTela / 2) - 30,
                Top = form.ClientSize.Height - 80,
                BackColor = Color.Transparent
            };

            form.Controls.Add(PictureBox);
            PictureBox.BringToFront();
        }

        // ─── Movimentação ────────────────────────────────────────
        public void MoverEsquerda()
        {
            if (PictureBox.Left - _velocidade >= 0)
                PictureBox.Left -= _velocidade;
        }

        public void MoverDireita()
        {
            if (PictureBox.Left + PictureBox.Width + _velocidade <= _larguraTela)
                PictureBox.Left += _velocidade;
        }

        // ─── IAtira ──────────────────────────────────────────────
        // NaveJogador.cs — Atirar atualizado
        public Projetil Atirar(Image spriteProjetil, int velocidade)
        {
            int projetilX = PictureBox.Left + (PictureBox.Width / 2) - 5;
            int projetilY = PictureBox.Top - 20;

            return new Projetil(projetilX, projetilY, DirecaoProjetil.Cima, spriteProjetil, velocidade);
        }

        // ─── Vida ────────────────────────────────────────────────
        public void PerderVida()
        {
            _vidas--;
        }
    }
}