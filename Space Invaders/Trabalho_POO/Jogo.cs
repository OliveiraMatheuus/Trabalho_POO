using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Trabalho_POO
{
    public class Jogo : IDisposable
    {
        public delegate void EventoJogo(string mensagem);
        public event EventoJogo OnVidaPerdida;
        public event EventoJogo OnAlienDestruido;
        public event EventoJogo OnUFODestruido;
        public event EventoJogo OnJogoEncerrado;
        public event EventoJogo OnRodadaAvancou;

        private NaveJogador _nave;
        private NaveUFO _ufo;
        private List<Alien> _aliens = new List<Alien>();
        private List<Projetil> _projetisJogador = new List<Projetil>();
        private List<Projetil> _projetisAliens = new List<Projetil>();
        private List<Barreira> _barreiras = new List<Barreira>();

        private GerenciadorDeColisoes _gerenciador;
        private ConfiguracaoRodada _config;
        private GerenciadorDeAudio _audio;

        private Form _form;
        private PictureBox _canvas;
        private Image _spriteNave, _spriteAlien, _spriteUFO;
        private Image _spriteProjetilJogador, _spriteProjetilAlien, _spriteBarreira;
        private Image _imagemFundo;

        private Thread _threadJogo;
        private bool _rodando;
        private bool _jogoEncerrado;
        private bool _moverEsquerda, _moverDireita, _atirar;

        private int _contadorDisparo = 0;
        private int _intervaloDisparo = 20;
        private int _contadorDisparoAlien = 0;
        private int _intervaloDisparoAlien;

        private int _contadorUFO = 0;
        private int _intervaloUFO = 600;
        private readonly Random _random = new Random();

        private int _rodadaAtual = 1;
        private string _mensagemEncerramento = null;

        private PaintEventHandler _paintHandler;
        private EventHandler _gifFrameHandler;

        private readonly object _lockGrafico = new object();

        private static readonly string ArquivoRecorde = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "recorde.txt");

        public Jogo(Form form, PictureBox canvas,
            Image spriteNave, Image spriteAlien, Image spriteUFO,
            Image spriteProjetilJogador, Image spriteProjetilAlien,
            Image spriteBarreira, Image imagemFundo)
        {
            _form = form;
            _canvas = canvas;
            _spriteNave = spriteNave;
            _spriteAlien = spriteAlien;
            _spriteUFO = spriteUFO;
            _spriteProjetilJogador = spriteProjetilJogador;
            _spriteProjetilAlien = spriteProjetilAlien;
            _spriteBarreira = spriteBarreira;
            _imagemFundo = imagemFundo;

            _config = new ConfiguracaoRodada();
            _intervaloDisparoAlien = _config.IntervaloDisparoAlien;

            _audio = new GerenciadorDeAudio();
            _gerenciador = new GerenciadorDeColisoes(_audio);

            _gerenciador.OnAlienDestruido += msg => _form.BeginInvoke((Action)(() => OnAlienDestruido?.Invoke(msg)));
            _gerenciador.OnUFODestruido += msg => _form.BeginInvoke((Action)(() => OnUFODestruido?.Invoke(msg)));
            _gerenciador.OnVidaPerdida += msg => _form.BeginInvoke((Action)(() => OnVidaPerdida?.Invoke(msg)));
            _gerenciador.OnDerrota += msg => _form.BeginInvoke((Action)(() => EncerrarJogo(msg)));

            _gerenciador.OnVitoria += msg =>
            {
                if (!_form.IsDisposed)
                    _form.BeginInvoke((Action)AvancarRodada);
            };

            _paintHandler = (s, e) => Renderizar(e.Graphics);
            _canvas.Paint += _paintHandler;

            if (_imagemFundo != null)
            {
                _gifFrameHandler = (s, e) =>
                {
                    if (!_canvas.IsDisposed) _canvas.Invalidate();
                };
                ImageAnimator.Animate(_imagemFundo, _gifFrameHandler);
            }

            _audio.TocarMusicaFundo();
            ReiniciarIntervaloUFO();
        }

        public void Iniciar()
        {
            _nave = new NaveJogador(_spriteNave, _canvas.Width, _canvas.Height);
            CriarAliens();
            CriarBarreiras();

            _rodando = true;
            _jogoEncerrado = false;

            _threadJogo = new Thread(LoopJogo) { IsBackground = true };
            _threadJogo.Start();
        }

        public void Parar()
        {
            _jogoEncerrado = true;
            _rodando = false;
        }

        public void Dispose()
        {
            _rodando = false;

            if (_imagemFundo != null && _gifFrameHandler != null)
                ImageAnimator.StopAnimate(_imagemFundo, _gifFrameHandler);

            if (_canvas != null && !_canvas.IsDisposed)
                _canvas.Paint -= _paintHandler;

            lock (_lockGrafico)
            {
                _aliens.Clear();
                _projetisJogador.Clear();
                _projetisAliens.Clear();
                _barreiras.Clear();
            }
            _audio?.Dispose();
        }

        private void CriarAliens()
        {
            _aliens.Clear();
            int offsetX = 40, offsetY = 60;

            for (int l = 0; l < _config.LinhasAlien; l++)
            {
                for (int c = 0; c < _config.ColunasAlien; c++)
                {
                    _aliens.Add(new Alien(
                        _spriteAlien,
                        offsetX + c * (_config.LarguraAlien + _config.EspacoXAlien),
                        offsetY + l * (_config.AlturaAlien + _config.EspacoYAlien),
                        _canvas.Width,
                        _config.VelocidadeAlien));
                }
            }
        }

        private void CriarBarreiras()
        {
            _barreiras.Clear();
            int y = _canvas.Height - 150;
            int larguraBarreira = 80;
            int margem = 60;

            _barreiras.Add(new Barreira(_spriteBarreira, margem, y));
            _barreiras.Add(new Barreira(_spriteBarreira, (_canvas.Width / 2) - (larguraBarreira / 2), y));
            _barreiras.Add(new Barreira(_spriteBarreira, _canvas.Width - margem - larguraBarreira, y));
        }

        private void AvancarRodada()
        {
            lock (_lockGrafico)
            {
                _config.Avancar();

                if (_config.JogoCompleto)
                {
                    EncerrarJogo("Você venceu o jogo! Parabéns!");
                    return;
                }

                _rodadaAtual++;
                _intervaloDisparoAlien = _config.IntervaloDisparoAlien;
                _contadorDisparoAlien = 0;
                _projetisJogador.Clear();
                _projetisAliens.Clear();
                _ufo = null;
                ReiniciarIntervaloUFO();

                OnRodadaAvancou?.Invoke($"Rodada {_rodadaAtual}");
                CriarAliens();
                CriarBarreiras();
            }
        }

        private void LoopJogo()
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();

            while (_rodando)
            {
                if (_form.IsDisposed) break;

                long inicioFrame = sw.ElapsedMilliseconds;

                try
                {
                    lock (_lockGrafico)
                    {
                        if (!_jogoEncerrado)
                        {
                            ProcessarInput();
                            _nave.Atualizar();
                            MoverProjetis();
                            MoverAliens();
                            AtualizarUFO();
                            AliensAtirar();

                            _gerenciador.Verificar(
                                _nave, _aliens, _barreiras,
                                _projetisJogador, _projetisAliens,
                                _ufo, _canvas.Height);
                        }
                    }

                    if (!_form.IsDisposed && !_jogoEncerrado)
                    {
                        _form.Invoke((Action)(() => _canvas.Invalidate()));
                    }
                }
                catch (ObjectDisposedException) { break; }
                catch (Exception ex) { Console.WriteLine("Erro no Game Loop: " + ex.Message); break; }

                if (_mensagemEncerramento != null)
                {
                    string msg = _mensagemEncerramento;
                    _mensagemEncerramento = null;
                    if (!_form.IsDisposed)
                        _form.BeginInvoke((Action)(() => OnJogoEncerrado?.Invoke(msg)));
                    break;
                }

                long tempoGasto = sw.ElapsedMilliseconds - inicioFrame;
                int tempoEspera = (int)(16 - tempoGasto);
                if (tempoEspera > 0)
                {
                    Thread.Sleep(tempoEspera);
                }
            }
        }

        private void Renderizar(Graphics g)
        {
            if (_imagemFundo != null)
            {
                ImageAnimator.UpdateFrames(_imagemFundo);
                g.DrawImage(_imagemFundo, 0, 0, _canvas.Width, _canvas.Height);
            }
            else
            {
                g.Clear(Color.Black);
            }

            lock (_lockGrafico)
            {
                _ufo?.Desenhar(g);
                _nave?.Desenhar(g);
                foreach (var a in _aliens) a.Desenhar(g);
                foreach (var p in _projetisJogador) p.Desenhar(g);
                foreach (var p in _projetisAliens) p.Desenhar(g);
                foreach (var b in _barreiras) b.Desenhar(g);
            }
        }

        public void SetarInput(bool esquerda, bool direita, bool atirar)
        {
            _moverEsquerda = esquerda;
            _moverDireita = direita;
            _atirar = atirar;
        }

        private void ProcessarInput()
        {
            if (_moverEsquerda) _nave.MoverEsquerda();
            if (_moverDireita) _nave.MoverDireita();

            if (_atirar && _contadorDisparo <= 0)
            {
                _projetisJogador.Add(
                    _nave.Atirar(_spriteProjetilJogador, _config.VelocidadeProjetil));
                _contadorDisparo = _intervaloDisparo;
                _audio?.TocarTiroJogador();
            }
            if (_contadorDisparo > 0) _contadorDisparo--;
        }

        private void MoverProjetis()
        {
            foreach (var p in _projetisJogador) p.Mover();
            foreach (var p in _projetisAliens) p.Mover();
        }

        private void MoverAliens()
        {
            if (_aliens.Count == 0) return;

            bool tocouBorda = false;
            foreach (var a in _aliens)
            {
                int proxX = a.X + a.VelocidadeAtual;
                if (proxX <= 0 || proxX + a.Largura >= _canvas.Width)
                { tocouBorda = true; break; }
            }

            if (tocouBorda)
                foreach (var a in _aliens) { a.InverterDirecao(); a.Descer(15); }
            else
                foreach (var a in _aliens) a.Mover();
        }

        private void AliensAtirar()
        {
            if (_aliens.Count == 0) return;
            if (++_contadorDisparoAlien < _intervaloDisparoAlien) return;
            _contadorDisparoAlien = 0;

            var atirador = _aliens[_random.Next(_aliens.Count)];
            _projetisAliens.Add(
                atirador.Atirar(_spriteProjetilAlien, _config.VelocidadeProjetil));
            _audio?.TocarTiroAlien();
        }

        private void ReiniciarIntervaloUFO()
        {
            _intervaloUFO = _random.Next(500, 1100);
            _contadorUFO = 0;
        }

        private void AtualizarUFO()
        {
            if (_ufo != null && _ufo.Ativo)
            {
                _ufo.Mover();
                if (_ufo.SaiuDaTela())
                {
                    _ufo = null;
                    ReiniciarIntervaloUFO();
                }
                return;
            }

            if (_ufo != null && !_ufo.Ativo)
            {
                _ufo = null;
                ReiniciarIntervaloUFO();
                return;
            }

            if (++_contadorUFO >= _intervaloUFO)
            {
                bool vaiParaDireita = _random.Next(2) == 0;
                _ufo = new NaveUFO(_spriteUFO, _canvas.Width, vaiParaDireita);
            }
        }

        private void EncerrarJogo(string msg)
        {
            if (_jogoEncerrado) return;
            _jogoEncerrado = true;
            _rodando = false;
            _mensagemEncerramento = msg;
        }

        public static int LerRecorde()
        {
            if (!File.Exists(ArquivoRecorde)) return 0;
            return int.TryParse(File.ReadAllText(ArquivoRecorde).Trim(), out int v) ? v : 0;
        }

        public static void SalvarRecordeSeNecessario(int placarAtual)
        {
            if (placarAtual > LerRecorde())
                File.WriteAllText(ArquivoRecorde, placarAtual.ToString());
        }
    }
}