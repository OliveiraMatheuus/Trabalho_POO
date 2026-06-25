using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace TrabalhoPOO
{
    public partial class Form1 : Form
    {
        private ControladorBlackjack _controlador;
        private GerenciadorDeAudio _audio;
        private Panel _pnlMenu;

        private const int CARD_W = 72;
        private const int CARD_H = 100;
        private const int CARD_GAP = 8;
        private readonly string _pastaResources;

        public Form1()
        {
            InitializeComponent();

            _pastaResources = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "Resources"));
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            _audio = new GerenciadorDeAudio();
            CriarMenuInicial();
        }

        private void CriarMenuInicial()
        {
            _pnlMenu = new Panel();
            _pnlMenu.Dock = DockStyle.Fill;

            if (this.BackgroundImage != null)
            {
                _pnlMenu.BackgroundImage = this.BackgroundImage;
                _pnlMenu.BackgroundImageLayout = ImageLayout.Stretch;
            }
            else
            {
                _pnlMenu.BackColor = Color.DarkGreen;
            }

            Label lblTitulo = new Label();
            lblTitulo.Text = "BLACKJACK 21";
            lblTitulo.Font = new Font("Arial", 36, FontStyle.Bold);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.BackColor = Color.Transparent;
            lblTitulo.AutoSize = true;
            lblTitulo.Location = new Point((this.ClientSize.Width - lblTitulo.PreferredWidth) / 2, 80);

            Button btnJogar = new Button();
            btnJogar.Text = "JOGAR";
            btnJogar.Size = new Size(200, 50);
            btnJogar.Font = new Font("Arial", 14, FontStyle.Bold);
            btnJogar.BackColor = Color.White;
            btnJogar.Location = new Point((this.ClientSize.Width - 200) / 2, 210);
            btnJogar.Click += (s, e) => {
                _pnlMenu.Visible = false;
                _audio.TocarMusicaFundo();
                IniciarJogo();
            };

            Button btnSair = new Button();
            btnSair.Text = "SAIR";
            btnSair.Size = new Size(200, 50);
            btnSair.Font = new Font("Arial", 14, FontStyle.Bold);
            btnSair.BackColor = Color.White;
            btnSair.Location = new Point((this.ClientSize.Width - 200) / 2, 280);
            btnSair.Click += (s, e) => { Application.Exit(); };

            _pnlMenu.Controls.Add(lblTitulo);
            _pnlMenu.Controls.Add(btnJogar);
            _pnlMenu.Controls.Add(btnSair);
            this.Controls.Add(_pnlMenu);
            _pnlMenu.BringToFront();
        }

        private void IniciarJogo()
        {
            _audio.PararEfeitos();

            _controlador = new ControladorBlackjack();
            _controlador.IniciarNovaRodada();

            _audio.TocarCarta();
            lblResultado.Text = "";
            UpdateControlesInterface(true);

            AtualizarTela(revelarBanca: false);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (_controlador == null || !_controlador.JogoAtivo) return;

            _audio.TocarCarta();
            ResultadoRodada resultado = _controlador.ComprarCartaJogador();

            AtualizarTela(revelarBanca: false);

            if (resultado == ResultadoRodada.JogadorEstourou)
                ProcessarFimDeJogo(resultado);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (_controlador == null || !_controlador.JogoAtivo) return;

            ResultadoRodada resultado = _controlador.PararEAnalisarVencedor();
            AtualizarTela(revelarBanca: true);
            ProcessarFimDeJogo(resultado);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            IniciarJogo();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Evento vazio, mantido para o Visual Studio não dar erro de compilação
        }

        private void ProcessarFimDeJogo(ResultadoRodada resultado)
        {
            UpdateControlesInterface(false);

            switch (resultado)
            {
                case ResultadoRodada.JogadorEstourou:
                    lblResultado.Text = "Você estourou! Banca vence.";
                    lblResultado.ForeColor = Color.Red;
                    _audio.TocarDerrota();
                    break;
                case ResultadoRodada.BancaEstourou:
                case ResultadoRodada.JogadorVence:
                    lblResultado.Text = "Você vence!";
                    lblResultado.ForeColor = Color.LightGreen;
                    _audio.TocarVitoria();
                    break;
                case ResultadoRodada.Empate:
                    lblResultado.Text = "Empate! O jogo ficou igualado.";
                    lblResultado.ForeColor = Color.Yellow;
                    _audio.TocarEmpate();
                    break;
                case ResultadoRodada.BancaVence:
                    lblResultado.Text = "Banca vence!";
                    lblResultado.ForeColor = Color.Red;
                    _audio.TocarDerrota();
                    break;
            }
        }

        private void AtualizarTela(bool revelarBanca)
        {
            DesenharCartas(pnlJogador, _controlador.JogadorAtual.Mao, revelar: true);
            DesenharCartas(pnlBanca, _controlador.BancaAtual.Mao, revelar: revelarBanca);

            lblPontosJogador.Text = $"Jogador: {_controlador.JogadorAtual.CalcularPontos()} pts";

            if (revelarBanca)
                lblPontosBanca.Text = $"Banca: {_controlador.BancaAtual.CalcularPontos()} pts";
            else
                lblPontosBanca.Text = "Banca: ?";
        }

        private void DesenharCartas(Panel painel, IReadOnlyList<Carta> mao, bool revelar)
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
                {
                    string nomeArquivo = Path.GetFileName(mao[i].Path);
                    string caminhoCompleto = Path.Combine(_pastaResources, nomeArquivo);

                    if (!File.Exists(caminhoCompleto))
                        caminhoCompleto = Path.Combine(_pastaResources, mao[i].Path);

                    if (File.Exists(caminhoCompleto))
                        pb.Image = Image.FromFile(caminhoCompleto);
                    else
                        pb.BackColor = Color.White;
                }
                else
                {
                    string caminhoVerso = Path.Combine(_pastaResources, "back_dark.png");
                    if (File.Exists(caminhoVerso))
                        pb.Image = Image.FromFile(caminhoVerso);
                    else
                        pb.BackColor = Color.DarkBlue;
                }

                painel.Controls.Add(pb);
            }
        }

        private void UpdateControlesInterface(bool jogoAtivo)
        {
            if (btnComprar != null) btnComprar.Enabled = jogoAtivo;
            if (btnParar != null) btnParar.Enabled = jogoAtivo;
        }
    }
}