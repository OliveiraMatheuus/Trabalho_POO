using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;


namespace Trabalho_POO
{
    public class Jogo
    {
        // ─── Delegates e Eventos ─────────────────────────────────
        public delegate void EventoJogo(string mensagem);
        public event EventoJogo OnVidaPerdida;
        public event EventoJogo OnAlienDestruido;
        public event EventoJogo OnJogoEncerrado;

        // ─── Entidades ───────────────────────────────────────────
        private NaveJogador _nave;
        private List<Alien> _aliens;
        private List<Projetil> _projetisJogador;
        private List<Projetil> _projetisAliens;

        // ─── Gerenciador ─────────────────────────────────────────
        private GerenciadorDeColisoes _gerenciadorDeColisoes;

        // ─── Referências ─────────────────────────────────────────
        private Form _form;
        private Image _spriteNave;
        private Image _spriteAlien;
        private Image _spriteProjetilJogador;
        private Image _spriteProjetilAlien;

        // ─── Controle do loop ────────────────────────────────────
        private Thread _threadJogo;
        private bool _rodando;
        private bool _jogoEncerrado;

        // ─── Controle de input ───────────────────────────────────
        private bool _moverEsquerda;
        private bool _moverDireita;
        private bool _atirar;

        // ─── Controle de disparo do jogador ──────────────────────
        private int _intervaloDisparo;
        private int _contadorDisparo;

        // ─── Controle de disparo dos aliens ──────────────────────
        private Random _random;
        private int _intervaloDisparoAlien;
        private int _contadorDisparoAlien;
        

        // ─── Controle de movimento dos aliens ────────────────────
        private int _contadorMovimentoAlien;
        private int _intervaloMovimentoAlien;

        private ConfiguracaoRodada _config;
        private int _rodadaAtual;

        // ─── Construtor ──────────────────────────────────────────
        public Jogo(Form form, Image spriteNave, Image spriteAlien,
            Image spriteProjetilJogador, Image spriteProjetilAlien)
        {
            _form = form;
            _spriteNave = spriteNave;
            _spriteAlien = spriteAlien;
            _spriteProjetilJogador = spriteProjetilJogador;
            _spriteProjetilAlien = spriteProjetilAlien;

            _aliens = new List<Alien>();
            _projetisJogador = new List<Projetil>();
            _projetisAliens = new List<Projetil>();
            _random = new Random();

            _config = new ConfiguracaoRodada();
            _rodadaAtual = 1;

            _intervaloDisparo = 20;  // ticks entre disparos do jogador
            _contadorDisparo = 0;
            _intervaloDisparoAlien = 60;  // ticks entre disparos dos aliens
            _contadorDisparoAlien = 0;
            _intervaloDisparoAlien = _config.IntervaloDisparoAlien;
            _intervaloMovimentoAlien = _config.IntervaloMovimento;

            _gerenciadorDeColisoes = new GerenciadorDeColisoes(form);

            // Inscreve nos eventos do gerenciador
            _gerenciadorDeColisoes.OnAlienDestruido += msg =>
                OnAlienDestruido?.Invoke(msg);

            _gerenciadorDeColisoes.OnVidaPerdida += msg =>
                OnVidaPerdida?.Invoke(msg);

            _gerenciadorDeColisoes.OnDerrota += msg => EncerrarJogo(msg);
            // Jogo.cs — substituir OnVitoria no construtor
            _gerenciadorDeColisoes.OnVitoria += msg =>
            {
                // ✅ Não encerra — avança para próxima rodada
                _form.Invoke((Action)(() => AvancarRodada()));
            };
        }

        // ─── Iniciar ─────────────────────────────────────────────
        public void Iniciar()
        {
            _nave = new NaveJogador(
                _form,
                _spriteNave,
                _form.ClientSize.Width
            );

            CriarAliens();

            _rodando = true;
            _jogoEncerrado = false;

            _threadJogo = new Thread(LoopJogo);
            _threadJogo.IsBackground = true;
            _threadJogo.Start();
        }

        // ─── Criação dos aliens em matriz ────────────────────────
        // Jogo.cs — CriarAliens() atualizado
        private void CriarAliens()
        {
            int linhas = 3;
            int colunas = 5;
            int largura = 50;
            int altura = 40;
            int espacoX = 20;
            int espacoY = 20;
            int offsetX = 80;
            int offsetY = 50;

            for (int linha = 0; linha < linhas; linha++)
            {
                for (int coluna = 0; coluna < colunas; coluna++)
                {
                    int x = offsetX + coluna * (largura + espacoX);
                    int y = offsetY + linha * (altura + espacoY);

                    Alien alien = new Alien(
                        _form,
                        _spriteAlien,
                        x, y,
                        _form.ClientSize.Width,
                        _config.VelocidadeAlien  // ← velocidade da rodada atual
                    );

                    _aliens.Add(alien);
                }
            }
        }

        // Jogo.cs — novo método AvancarRodada()
        // Jogo.cs — adicionar evento

        public event EventoJogo OnRodadaAvancou;
        private void AvancarRodada()
        {
            _config.Avancar();
            _rodadaAtual++;

            // Atualiza intervalos com os novos valores
            _intervaloDisparoAlien = _config.IntervaloDisparoAlien;
            _intervaloMovimentoAlien = _config.IntervaloMovimento;
            _contadorDisparoAlien = 0;
            _contadorMovimentoAlien = 0;

            // Avisa o Form para atualizar o HUD
            OnRodadaAvancou?.Invoke($"Rodada {_rodadaAtual}");

            // Recria os aliens com nova velocidade
            CriarAliens();
        }

        // ─── Loop principal (roda em thread separada) ─────────────
        // Jogo.cs — LoopJogo() corrigido
        private void LoopJogo()
        {
            while (_rodando)
            {
                // ✅ Verifica se o Form ainda existe antes de Invoke
                if (_form.IsDisposed || !_form.IsHandleCreated)
                {
                    _rodando = false;
                    break;
                }

                try
                {
                    _form.Invoke((Action)(() =>
                    {
                        if (_form.IsDisposed) return;

                        ProcessarInput();
                        MoverProjetis();
                        MoverAliens();
                        AliensAtirar();
                        _gerenciadorDeColisoes.Verificar(
                            _nave,
                            _aliens,
                            _projetisJogador,
                            _projetisAliens,
                            _form.ClientSize.Height
                        );
                    }));
                }
                catch (ObjectDisposedException)
                {
                    // Form foi fechado durante o Invoke — encerra a thread silenciosamente
                    _rodando = false;
                    break;
                }

                Thread.Sleep(16);
            }
        }

        // ─── Input ───────────────────────────────────────────────
        public void SetarInput(bool esquerda, bool direita, bool atirar)
        {
            _moverEsquerda = esquerda;
            _moverDireita = direita;
            _atirar = atirar;
        }

        private void ProcessarInput()
        {
            if (_jogoEncerrado) return;

            if (_moverEsquerda) _nave.MoverEsquerda();
            if (_moverDireita) _nave.MoverDireita();

            // Controla cadência do disparo
            if (_atirar && _contadorDisparo <= 0)
            {
                // Jogo.cs — ProcessarInput() 
                Projetil p = _nave.Atirar(_spriteProjetilJogador, _config.VelocidadeProjetil);
                _form.Controls.Add(p.PictureBox);
                p.PictureBox.BringToFront();
                _projetisJogador.Add(p);
                _contadorDisparo = _intervaloDisparo;
            }

            if (_contadorDisparo > 0)
                _contadorDisparo--;
        }

        // ─── Movimento dos projéteis ──────────────────────────────
        private void MoverProjetis()
        {
            foreach (IMovivel p in _projetisJogador) p.Mover();
            foreach (IMovivel p in _projetisAliens) p.Mover();
        }

        // ─── Movimento dos aliens em grupo ────────────────────────
        // Jogo.cs — MoverAliens() corrigido
        // Jogo.cs — MoverAliens() reescrito

        
        private void MoverAliens()
        {
            if (_aliens.Count == 0) return;

            _contadorMovimentoAlien++;
            if (_contadorMovimentoAlien < _intervaloMovimentoAlien) return;
            _contadorMovimentoAlien = 0;

            // ✅ Simula o próximo passo ANTES de mover
            bool tocouBorda = false;

            foreach (Alien alien in _aliens)
            {
                int proximoX = alien.X + alien.VelocidadeAtual;

                if (proximoX <= 0 || proximoX + 50 >= _form.ClientSize.Width)
                {
                    tocouBorda = true;
                    break;
                }
            }

            if (tocouBorda)
            {
                foreach (Alien alien in _aliens)
                {
                    alien.InverterDirecao();
                    alien.Descer(15);
                }
            }
            else
            {
                foreach (Alien alien in _aliens)
                    alien.Mover();
            }
        }

        // ─── Disparo aleatório dos aliens ────────────────────────
        private void AliensAtirar()
        {
            if (_aliens.Count == 0) return;

            _contadorDisparoAlien++;
            if (_contadorDisparoAlien < _intervaloDisparoAlien) return;
            _contadorDisparoAlien = 0;

            // Escolhe um alien aleatório para atirar
            int indice = _random.Next(_aliens.Count);
            Alien atirador = _aliens[indice];

            // Jogo.cs — AliensAtirar()
            Projetil p = atirador.Atirar(_spriteProjetilAlien, _config.VelocidadeProjetil);
            _form.Controls.Add(p.PictureBox);
            p.PictureBox.BringToFront();
            _projetisAliens.Add(p);
        }

        // ─── Encerrar ────────────────────────────────────────────
        private void EncerrarJogo(string mensagem)
        {
            _jogoEncerrado = true;
            _rodando = false;
            OnJogoEncerrado?.Invoke(mensagem);
        }

        public void Parar()
        {
            _rodando = false;
        }
    }
}