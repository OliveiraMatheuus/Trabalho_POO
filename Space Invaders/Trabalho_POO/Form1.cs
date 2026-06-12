using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Trabalho_POO
{
    public partial class Form1 : Form
    {
        // ─── Atributos ───────────────────────────────────────────
        private Jogo _jogo;
        private Label _lblVidas;
        private Label _lblPlacar;
        private int _placar;
        // FormJogo.cs — adicionar label de rodada
        private Label _lblRodada;

        // ─── Sprites ─────────────────────────────────────────────
        private Image _spriteNave;
        private Image _spriteAlien;
        private Image _spriteProjetilJogador;
        private Image _spriteProjetilAlien;

        // ─── Controle de teclas ───────────────────────────────────
        private bool _esquerda;
        private bool _direita;
        private bool _atirar;


        // FormJogo.cs — adicionar atributo
        private PictureBox _fundoAnimado;

        // ─── Construtor ──────────────────────────────────────────
        public Form1()
        {
            InitializeComponent();
            ConfigurarForm();
            CarregarSprites();
            CriarHUD();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            CriarFundo();   // ← primeiro
            IniciarJogo();  // ← depois
        }

        // FormJogo.cs — novo método
        private void CriarFundo()
        {
            _fundoAnimado = new PictureBox
            {
                Image = Image.FromFile("fundo.gif"),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Size = this.ClientSize,
                Location = new Point(0, 0)
            };

            this.Controls.Add(_fundoAnimado);
            _fundoAnimado.SendToBack(); // ✅ fica atrás de tudo
        }

        // ─── Configuração do Form ────────────────────────────────
        private void ConfigurarForm()
        {
            this.Text = "Space Invaders";
            this.ClientSize = new Size(600, 700);
            this.BackColor = Color.Black;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.KeyPreview = true; // Form recebe teclas antes dos controles

            // Eventos de teclado
            this.KeyDown += FormJogo_KeyDown;
            this.KeyUp += FormJogo_KeyUp;
        }

        // ─── Carregamento das sprites ─────────────────────────────
        private void CarregarSprites()
        {
            // Os arquivos devem estar em Resources/ com
            // Build Action = "Embedded Resource" ou "Content - Copy Always"
            _spriteNave = Image.FromFile("hh.png");
            _spriteAlien = Image.FromFile("alien-removebg-preview.png");
            _spriteProjetilJogador = Image.FromFile("tiro_nave.png");
            _spriteProjetilAlien = Image.FromFile("tiro_alien.png");
        }

        // ─── HUD (vidas e placar) ────────────────────────────────
        private void CriarHUD()
        {
            _lblVidas = new Label
            {
                Text = "Vidas: 3",
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Font = new Font("Arial", 14, FontStyle.Bold),
                Location = new Point(10, 10),
                AutoSize = true
            };

            _lblPlacar = new Label
            {
                Text = "Placar: 0",
                ForeColor = Color.Yellow,
                BackColor = Color.Transparent,
                Font = new Font("Arial", 14, FontStyle.Bold),
                AutoSize = true
            };

            // FormJogo.cs — CriarHUD() adicionar
            _lblRodada = new Label
            {
                Text = "Rodada: 1",
                ForeColor = Color.Cyan,
                BackColor = Color.Transparent,
                Font = new Font("Arial", 14, FontStyle.Bold),
                Location = new Point(this.ClientSize.Width / 2 - 50, 10),
                AutoSize = true
            };

            this.Controls.Add(_lblRodada);
            _lblRodada.BringToFront();

            // Alinha o placar à direita
            _lblPlacar.Location = new Point(
                this.ClientSize.Width - 150, 10
            );

            this.Controls.Add(_lblVidas);
            this.Controls.Add(_lblPlacar);
            _lblVidas.BringToFront();
            _lblPlacar.BringToFront();
        }
        // FormJogo.cs — novo método
        private void AtualizarRodada(string mensagem)
        {
            this.Invoke((Action)(() =>
            {
                _lblRodada.Text = mensagem;
                _lblRodada.BringToFront();
            }));
        }

        // ─── Iniciar jogo ─────────────────────────────────────────
        private void IniciarJogo()
        {
            _placar = 0;

            _jogo = new Jogo(
                this,
                _spriteNave,
                _spriteAlien,
                _spriteProjetilJogador,
                _spriteProjetilAlien
            );

            // Inscreve nos eventos do Jogo
            _jogo.OnVidaPerdida += AtualizarVidas;
            _jogo.OnAlienDestruido += AtualizarPlacar;
            _jogo.OnJogoEncerrado += MostrarResultado;
            // FormJogo.cs — inscrever no novo evento dentro de IniciarJogo()
            _jogo.OnRodadaAvancou += AtualizarRodada;

            _jogo.Iniciar();
        }

        // ─── Eventos do Jogo ──────────────────────────────────────
        // FormJogo.cs — AtualizarVidas()
        private void AtualizarVidas(string mensagem)
        {
            this.Invoke((Action)(() =>
            {
                _lblVidas.Text = $"Vidas: {mensagem}";
                _lblVidas.BringToFront();
            }));
        }

        private void AtualizarPlacar(string mensagem)
        {
            this.Invoke((Action)(() =>
            {
                _placar += 100;
                _lblPlacar.Text = $"Placar: {_placar}";
                _lblPlacar.BringToFront();
            }));
        }

        private void MostrarResultado(string mensagem)
        {
            this.Invoke((Action)(() =>
            {
                _jogo.Parar();

                DialogResult resultado = MessageBox.Show(
                    $"{mensagem}\nPlacar final: {_placar}\n\nDeseja jogar novamente?",
                    "Fim de Jogo",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information
                );

                if (resultado == DialogResult.Yes)
                    ReiniciarJogo();
                else
                    this.Close();
            }));
        }

        // ─── Reiniciar ───────────────────────────────────────────
        private void ReiniciarJogo()
        {
            // Remove todos os controles exceto os labels do HUD
            for (int i = this.Controls.Count - 1; i >= 0; i--)
            {
                Control c = this.Controls[i];
                if (c != _lblVidas && c != _lblPlacar)
                    this.Controls.RemoveAt(i);
            }

            IniciarJogo();
        }

        // ─── Input do teclado ─────────────────────────────────────
        private void FormJogo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.A) _esquerda = true;
            if (e.KeyCode == Keys.Right || e.KeyCode == Keys.D) _direita = true;
            if (e.KeyCode == Keys.Space) _atirar = true;

            _jogo.SetarInput(_esquerda, _direita, _atirar);
        }

        private void FormJogo_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.A) _esquerda = false;
            if (e.KeyCode == Keys.Right || e.KeyCode == Keys.D) _direita = false;
            if (e.KeyCode == Keys.Space) _atirar = false;

            _jogo.SetarInput(_esquerda, _direita, _atirar);
        }

        // ─── Fechar com segurança ────────────────────────────────
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _jogo?.Parar();
            Thread.Sleep(50);
            base.OnFormClosing(e);
        }
    }
}