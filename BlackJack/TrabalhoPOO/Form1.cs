using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace TrabalhoPOO
{
    public partial class Form1 : Form
    {
        private Baralho _baralho;
        private Jogador _jogador;
        private Banca _banca;
        private bool _jogoAtivo;

        private GerenciadorDeAudio _audio;
        private Panel _pnlMenu;

        private const int CARD_W = 72;
        private const int CARD_H = 100;
        private const int CARD_GAP = 8;

        private readonly string _pastaResources;

        public Form1()
        {
            InitializeComponent();

            _pastaResources = Path.GetFullPath(
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "Resources"));

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

            lblTitulo.Location = new Point(
                (this.ClientSize.Width - lblTitulo.PreferredWidth) / 2, 80);

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

            btnSair.Click += (s, e) => {
                Application.Exit(); 
            };

            _pnlMenu.Controls.Add(lblTitulo);
            _pnlMenu.Controls.Add(btnJogar);
            _pnlMenu.Controls.Add(btnSair); 
            this.Controls.Add(_pnlMenu);
            _pnlMenu.BringToFront();
        }

        private void IniciarJogo()
        {
            
            _audio.PararEfeitos();

            _baralho = new Baralho();
            _jogador = new Jogador();
            _banca = new Banca();
            _jogoAtivo = true;

            _audio.TocarCarta();

            _jogador.ReceberCarta(_baralho.ComprarCarta());
            _jogador.ReceberCarta(_baralho.ComprarCarta());
            _banca.ReceberCarta(_baralho.ComprarCarta());
            _banca.ReceberCarta(_baralho.ComprarCarta());

            lblResultado.Text = "";
            UpdateControlesInterface(true);

            AtualizarTela();
        }

        private void UpdateControlesInterface(bool jogoAtivo)
        {
            btnComprar.Enabled = jogoAtivo;
            btnParar.Enabled = jogoAtivo;
        }

        private void PatternMuteEfeitosAnteriores()
        {
            _audio.PararEfeitos();
        }

        private void AtualizarTela(bool revelarBanca = false)
        {
            DesenharCartas(pnlJogador, _jogador.Mao, revelar: true);
            DesenharCartas(pnlBanca, _banca.Mao, revelar: revelarBanca);

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
                {
                    string nomeArquivo = Path.GetFileName(mao[i].Path);
                    string caminhoCompleto = Path.Combine(_pastaResources, nomeArquivo);

                    if (!File.Exists(caminhoCompleto))
                    {
                        caminhoCompleto = Path.Combine(_pastaResources, mao[i].Path);
                    }

                    if (File.Exists(caminhoCompleto))
                    {
                        pb.Image = Image.FromFile(caminhoCompleto);
                    }
                    else
                    {
                        pb.BackColor = Color.White;
                    }
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

        private void EncerrarRodada()
        {
            _jogoAtivo = false;
            UpdateControlesInterface(false);

            int ptsJogador = _jogador.CalcularPontos();
            _banca.ExecutarJogada(_baralho, ptsJogador);

            AtualizarTela(revelarBanca: true);
            int ptsBanca = _banca.CalcularPontos();

            if (ptsJogador > 21)
            {
                lblResultado.Text = "Você estourou! Banca vence.";
                lblResultado.ForeColor = Color.Red;
                _audio.TocarDerrota();
            }
            else if (ptsBanca > 21)
            {
                lblResultado.Text = "Banca estourou! Você vence!";
                lblResultado.ForeColor = Color.LightGreen;
                _audio.TocarVitoria();
            }
            else if (ptsJogador > ptsBanca)
            {
                lblResultado.Text = "Você vence!";
                lblResultado.ForeColor = Color.LightGreen;
                _audio.TocarVitoria();
            }
            else if (ptsJogador == ptsBanca)
            {
                lblResultado.Text = "Empate! O jogo ficou igualado.";
                lblResultado.ForeColor = Color.Yellow;
                _audio.TocarEmpate();
            }
            else
            {
                lblResultado.Text = "Banca vence!";
                lblResultado.ForeColor = Color.Red;
                _audio.TocarDerrota();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!_jogoAtivo) return;

            _audio.TocarCarta();

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