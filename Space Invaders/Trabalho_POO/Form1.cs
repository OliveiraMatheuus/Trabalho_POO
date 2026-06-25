using System;
using System.Drawing;
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
        private Label _lblRecorde;
        private int _placar;

        private Image _spriteNave, _spriteAlien, _spriteUFO;
        private Image _spriteProjetilJogador, _spriteProjetilAlien, _spriteBarreira;

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
            _spriteUFO = Properties.Resources.ufo; // adicione "ufo.png" nos Resources
            _spriteProjetilJogador = Properties.Resources.tiro_nave;
            _spriteProjetilAlien = Properties.Resources.tiro_alien;
            _spriteBarreira = Properties.Resources.space_bar;
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
            _lblRecorde = new Label
            {
                Text = $"Recorde: {Jogo.LerRecorde()}",
                ForeColor = Color.Orange,
                BackColor = Color.Transparent,
                Font = new Font("Arial", 11, FontStyle.Bold),
                Location = new Point(10, 38),
                AutoSize = true,
                Visible = false
            };

            this.Controls.Add(_lblVidas);
            this.Controls.Add(_lblPlacar);
            this.Controls.Add(_lblRodada);
            this.Controls.Add(_lblRecorde);
        }

        private void MostrarMenu()
        {
            int recorde = Jogo.LerRecorde();

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
                (this.ClientSize.Width - titulo.PreferredWidth) / 2, 200);

            var lblRecordeMenu = new Label
            {
                Text = recorde > 0 ? $"Recorde: {recorde}" : "Sem recorde ainda",
                ForeColor = Color.Orange,
                BackColor = Color.Transparent,
                Font = new Font("Courier New", 13, FontStyle.Bold),
                AutoSize = true
            };
            lblRecordeMenu.Location = new Point(
                (this.ClientSize.Width - lblRecordeMenu.PreferredWidth) / 2, 265);

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
            btnJogar.Location = new Point((this.ClientSize.Width - 180) / 2, 320);
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
            btnSair.Location = new Point((this.ClientSize.Width - 180) / 2, 390);
            btnSair.Click += (s, ev) => this.Close();

            _menuPanel.Controls.Add(titulo);
            _menuPanel.Controls.Add(lblRecordeMenu);
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
            _lblRecorde.Text = $"Recorde: {Jogo.LerRecorde()}";

            _lblVidas.Visible = true;
            _lblPlacar.Visible = true;
            _lblRodada.Visible = true;
            _lblRecorde.Visible = true;
            _lblVidas.BringToFront();
            _lblPlacar.BringToFront();
            _lblRodada.BringToFront();
            _lblRecorde.BringToFront();

            _jogo = new Jogo(
                this, _canvas,
                _spriteNave, _spriteAlien, _spriteUFO,
                _spriteProjetilJogador, _spriteProjetilAlien,
                _spriteBarreira,
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

            // UFO vale pontos bônus (valor vem do evento como string)
            _jogo.OnUFODestruido += msg =>
            {
                if (int.TryParse(msg, out int bonus))
                    _placar += bonus;
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
                // ── Correção: Join no lugar de Thread.Sleep fixo ──────────────
                _jogo.Parar();
                _jogo.Dispose();

                // Salva recorde antes de mostrar resultado
                Jogo.SalvarRecordeSeNecessario(_placar);
                int recorde = Jogo.LerRecorde();

                var r = MessageBox.Show(
                    $"{msg}\n\nPlacar: {_placar}\nRecorde: {recorde}\n\nJogar novamente?",
                    "Fim de Jogo", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (r == DialogResult.Yes)
                {
                    _lblVidas.Visible = false;
                    _lblPlacar.Visible = false;
                    _lblRodada.Visible = false;
                    _lblRecorde.Visible = false;
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

        // ── Correção: Thread.Sleep(50) substituído por Join com timeout ───────
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (_jogo != null)
            {
                _jogo.Parar();
                _jogo.Dispose();
            }
            base.OnFormClosing(e);
        }
    }
}