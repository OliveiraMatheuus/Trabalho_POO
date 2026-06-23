using System;
using System.Drawing;
using System.Windows.Forms;

namespace TrabalhoPOO
{
    public partial class Form1 : Form
    {
        private Baralho _baralho;
        private Jogador _jogador;
        private Banca _banca;
        private bool _jogoAtivo;

        // tamanho de cada carta na tela
        private const int CARD_W = 72;
        private const int CARD_H = 100;
        private const int CARD_GAP = 8;

        public Form1()
        {
            InitializeComponent();
            IniciarJogo();
        }

        private void IniciarJogo()
        {
            _baralho = new Baralho();
            _jogador = new Jogador();
            _banca = new Banca();
            _jogoAtivo = true;

            // cada um começa com 2 cartas
            _jogador.ReceberCarta(_baralho.ComprarCarta());
            _jogador.ReceberCarta(_baralho.ComprarCarta());
            _banca.ReceberCarta(_baralho.ComprarCarta());
            _banca.ReceberCarta(_baralho.ComprarCarta());

            lblResultado.Text = "";
            btnComprar.Enabled = true;
            btnParar.Enabled = true;

            AtualizarTela();
        }

        private void AtualizarTela(bool revelarBanca = false)
        {
            DesenharCartas(pnlJogador, _jogador.Mao, revelar: true);

            DesenharCartas(pnlBanca, _banca.Mao, revelar: revelarBanca);

            // atualiza pontuações
            lblPontosJogador.Text = $"Jogador: {_jogador.CalcularPontos()} pts";

            if (revelarBanca)
                lblPontosBanca.Text = $"Banca: {_banca.CalcularPontos()} pts";
            else
                lblPontosBanca.Text = "Banca: ?";
        }

        private void DesenharCartas(Panel painel, System.Collections.Generic.List<Carta> mao, bool revelar)
        {
            painel.Controls.Clear();

            for (int i = 0; i < mao.Count; i++)
            {
                PictureBox pb = new PictureBox();
                pb.Size = new Size(CARD_W, CARD_H);
                pb.Location = new Point(10 + i * (CARD_W + CARD_GAP), 15);
                pb.SizeMode = PictureBoxSizeMode.StretchImage;
                pb.BorderStyle = BorderStyle.FixedSingle;

                bool mostrar = revelar || (painel == pnlBanca && i == 0);

                if (mostrar)
                    pb.Image = Image.FromFile(mao[i].Path);
                else
                    pb.BackColor = Color.DarkBlue; // carta virada para baixo

                painel.Controls.Add(pb);
            }
        }

        private void EncerrarRodada()
        {
            _jogoAtivo = false;
            btnComprar.Enabled = false;
            btnParar.Enabled = false;

            int ptsJogador = _jogador.CalcularPontos();

            _banca.ExecutarJogada(_baralho, ptsJogador);

            // revela todas as cartas da banca
            AtualizarTela(revelarBanca: true);

            int ptsBanca = _banca.CalcularPontos();

            if (ptsJogador > 21)
            {
                lblResultado.Text = "Você estourou! Banca vence.";
                lblResultado.ForeColor = Color.Red;
            }
            else if (ptsBanca > 21)
            {
                lblResultado.Text = "Banca estourou! Você vence!";
                lblResultado.ForeColor = Color.LightGreen;
            }
            else if (ptsJogador > ptsBanca)
            {
                lblResultado.Text = "Você vence!";
                lblResultado.ForeColor = Color.LightGreen;
            }
            else
            {
                lblResultado.Text = "Banca vence!";
                lblResultado.ForeColor = Color.Red;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!_jogoAtivo) return;

            _jogador.ReceberCarta(_baralho.ComprarCarta());
            AtualizarTela();

            if (_jogador.CalcularPontos() > 21)
                EncerrarRodada();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!_jogoAtivo) return;
            EncerrarRodada();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            IniciarJogo();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
