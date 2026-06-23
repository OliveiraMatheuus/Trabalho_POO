using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Trabalho_POO
{
    public partial class Form1 : Form
    {
        private Jogo _jogo;
        private PictureBox _canvas;
        private Panel _menuPanel;

        private Label _lblVidas;
        private Label _lblPlacar;
        private Label _lblRodada;
        private int _placar;

        private Image _spriteNave;
        private Image _spriteAlien;
        private Image _spriteProjetilJogador;
        private Image _spriteProjetilAlien;
        private Image _spriteBarreira; // <- Declarada aqui

        private bool _esquerda, _direita, _atirar;

        public Form1()
        {
            InitializeComponent();
            this.Text = "Space Invaders";
            this.ClientSize = new Size(600, 700);
            this.BackColor = Color.Black;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.KeyPreview = true;
            this.KeyDown += FormJogo_KeyDown;
            this.KeyUp += FormJogo_KeyUp;

            CarregarSprites();
            CriarCanvas();
            CriarHUD();
            MostrarMenu();
        }

        private void CarregarSprites()
        {
            _spriteNave = Properties.Resources.Nave;
            _spriteAlien = Properties.Resources.alien_removebg_preview;
            _spriteProjetilJogador = Properties.Resources.tiro_nave;
            _spriteProjetilAlien = Properties.Resources.tiro_alien;
            _spriteBarreira = Properties.Resources.space_bar; // <- Carregada aqui (o hífen vira underline)
        }

        private void CriarCanvas()
        {
            _canvas = new PictureBox
            {
                Size = this.ClientSize,
                Location = new Point(0, 0),
                BackColor = Color.Black
            };
            this.Controls.Add(_canvas);
            _canvas.SendToBack();
        }

        private void CriarHUD()
        {
            _lblVidas = new Label
            {
                Text = "Vidas: 3",
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Font = new Font("Arial", 14, FontStyle.Bold),
                Location = new Point(10, 10),
                AutoSize = true,
                Visible = false
            };
            _lblPlacar = new Label
            {
                Text = "Placar: 0",
                ForeColor = Color.Yellow,
                BackColor = Color.Transparent,
                Font = new Font("Arial", 14, FontStyle.Bold),
                Location = new Point(this.ClientSize.Width - 150, 10),
                AutoSize = true,
                Visible = false
            };
            _lblRodada = new Label
            {
                Text = "Rodada: 1",
                ForeColor = Color.Cyan,
                BackColor = Color.Transparent,
                Font = new Font("Arial", 14, FontStyle.Bold),
                Location = new Point(this.ClientSize.Width / 2 - 55, 10),
                AutoSize = true,
                Visible = false
            };
            this.Controls.Add(_lblVidas);
            this.Controls.Add(_lblPlacar);
            this.Controls.Add(_lblRodada);
        }

        private void MostrarMenu()
        {
            _menuPanel = new Panel
            {
                Size = this.ClientSize,
                Location = new Point(0, 0),
                BackColor = Color.Transparent
            };

            var titulo = new Label
            {
                Text = "SPACE INVADERS",
                ForeColor = Color.Lime,
                BackColor = Color.Transparent,
                Font = new Font("Courier New", 26, FontStyle.Bold),
                AutoSize = true
            };
            titulo.Location = new Point(
                (this.ClientSize.Width - titulo.PreferredWidth) / 2, 240);

            var btnJogar = new Button
            {
                Text = "JOGAR",
                ForeColor = Color.White,
                BackColor = Color.FromArgb(40, 40, 100),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Courier New", 13, FontStyle.Bold),
                Size = new Size(180, 50)
            };
            btnJogar.FlatAppearance.BorderColor = Color.Cyan;
            btnJogar.Location = new Point((this.ClientSize.Width - 180) / 2, 340);
            btnJogar.Click += (s, ev) => IniciarJogo();

            var btnSair = new Button
            {
                Text = "SAIR",
                ForeColor = Color.White,
                BackColor = Color.FromArgb(100, 20, 20),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Courier New", 13, FontStyle.Bold),
                Size = new Size(180, 50)
            };
            btnSair.FlatAppearance.BorderColor = Color.Red;
            btnSair.Location = new Point((this.ClientSize.Width - 180) / 2, 410);
            btnSair.Click += (s, ev) => this.Close();

            _menuPanel.Controls.Add(titulo);
            _menuPanel.Controls.Add(btnJogar);
            _menuPanel.Controls.Add(btnSair);

            this.Controls.Add(_menuPanel);
            _menuPanel.BringToFront();
        }

        private void IniciarJogo()
        {
            if (_menuPanel != null)
            {
                _menuPanel.Visible = false;
                this.Controls.Remove(_menuPanel);
                _menuPanel.Dispose();
                _menuPanel = null;
            }

            _placar = 0;
            _lblVidas.Text = "Vidas: 3";
            _lblPlacar.Text = "Placar: 0";
            _lblRodada.Text = "Rodada: 1";
            _lblVidas.Visible = true;
            _lblPlacar.Visible = true;
            _lblRodada.Visible = true;
            _lblVidas.BringToFront();
            _lblPlacar.BringToFront();
            _lblRodada.BringToFront();

            _jogo = new Jogo(
                this, _canvas,
                _spriteNave, _spriteAlien,
                _spriteProjetilJogador, _spriteProjetilAlien,
                _spriteBarreira, // <- Passa a barreira pro jogo aqui
                Properties.Resources.fundo
            );

            _jogo.OnVidaPerdida += msg =>
            {
                _lblVidas.Text = $"Vidas: {msg}";
                _lblVidas.BringToFront();
            };

            _jogo.OnAlienDestruido += msg =>
            {
                _placar += 100;
                _lblPlacar.Text = $"Placar: {_placar}";
                _lblPlacar.BringToFront();
            };

            _jogo.OnRodadaAvancou += msg =>
            {
                _lblRodada.Text = msg;
                _lblRodada.BringToFront();
            };

            _jogo.OnJogoEncerrado += msg =>
            {
                _jogo.Dispose();

                var r = MessageBox.Show(
                    $"{msg}\nPlacar: {_placar}\n\nJogar novamente?",
                    "Fim de Jogo", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (r == DialogResult.Yes)
                {
                    _lblVidas.Visible = false;
                    _lblPlacar.Visible = false;
                    _lblRodada.Visible = false;
                    MostrarMenu();
                }
                else
                {
                    this.Close();
                }
            };

            _jogo.Iniciar();
        }

        private void FormJogo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.A) _esquerda = true;
            if (e.KeyCode == Keys.Right || e.KeyCode == Keys.D) _direita = true;
            if (e.KeyCode == Keys.Space) _atirar = true;
            _jogo?.SetarInput(_esquerda, _direita, _atirar);
        }

        private void FormJogo_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.A) _esquerda = false;
            if (e.KeyCode == Keys.Right || e.KeyCode == Keys.D) _direita = false;
            if (e.KeyCode == Keys.Space) _atirar = false;
            _jogo?.SetarInput(_esquerda, _direita, _atirar);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _jogo?.Parar();
            _jogo?.Dispose();
            Thread.Sleep(50);
            base.OnFormClosing(e);
        }
    }
}