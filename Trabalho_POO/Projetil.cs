using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Trabalho_POO
{
    // ─── Enum de direção ─────────────────────────────────────────
    public enum DirecaoProjetil
    {
        Cima,   // projétil do jogador
        Baixo   // projétil dos aliens
    }

    public class Projetil : EntidadeJogo, IMovivel
    {
        // ─── Atributos ───────────────────────────────────────────
        private int _velocidade;
        private DirecaoProjetil _direcao;

        // ─── Propriedade ─────────────────────────────────────────
        public DirecaoProjetil Direcao => _direcao;

        // ─── Construtor ──────────────────────────────────────────
        public Projetil(int x, int y, DirecaoProjetil direcao, Image sprite)
        {
            _velocidade = 8;
            _direcao = direcao;

            PictureBox = new PictureBox
            {
                Image = sprite,
                SizeMode = PictureBoxSizeMode.StretchImage,
                Size = new Size(10, 20),
                Left = x,
                Top = y,
                BackColor = Color.Transparent
            };
        }

        // ─── IMovivel ────────────────────────────────────────────
        public void Mover()
        {
            if (_direcao == DirecaoProjetil.Cima)
                PictureBox.Top -= _velocidade;
            else
                PictureBox.Top += _velocidade;
        }

        // ─── Saiu da tela? ───────────────────────────────────────
        public bool ForaDaTela(int alturaTela)
        {
            return PictureBox.Bottom < 0 || PictureBox.Top > alturaTela;
        }
    }
}