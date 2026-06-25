using System;
using System.IO;
using System.Threading;
using WMPLib;
using System.Windows.Forms;

namespace Trabalho_POO
{
    public class GerenciadorDeAudio : IDisposable
    {
        private WindowsMediaPlayer _musicaFundo;
        private WindowsMediaPlayer _somTiroJogador;
        private WindowsMediaPlayer _somTiroAlien;
        private WindowsMediaPlayer _somExplosao;

        private readonly string _pastaAudio;
        private readonly SynchronizationContext _contexto;

        public GerenciadorDeAudio()
        {
            _contexto = SynchronizationContext.Current ?? new SynchronizationContext();

            _pastaAudio = Path.Combine(Application.StartupPath, "Resources");

            if (!Directory.Exists(_pastaAudio))
            {
                _pastaAudio = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "Resources"));
            }

            InicializarPlayers();
        }

        private void InicializarPlayers()
        {
            try
            {
                _musicaFundo = CriarPlayer("01. Knuckles Goes Ratchet (Chunk Lee Mix).mp3", loop: true, volume: 4);
                _somTiroJogador = CriarPlayer("piu.mp3", loop: false, volume: 30);
                _somTiroAlien = CriarPlayer("piu_in.mp3", loop: false, volume: 30);
                _somExplosao = CriarPlayer("big_boom-big-boom-202678.mp3", loop: false, volume: 1);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao carregar áudios: " + ex.Message);
            }
        }

        private WindowsMediaPlayer CriarPlayer(string arquivo, bool loop, int volume)
        {
            string caminho = Path.Combine(_pastaAudio, arquivo);

            if (!File.Exists(caminho)) return null;

            var player = new WindowsMediaPlayer();
            player.settings.autoStart = false;
            player.URL = caminho;
            player.settings.volume = volume;
            player.settings.setMode("loop", loop);
            return player;
        }

        private void Tocar(WindowsMediaPlayer player)
        {
            if (player == null) return;

            _contexto.Post(_ =>
            {
                try
                {
                    player.controls.stop();
                    player.controls.play();
                }
                catch { }
            }, null);
        }

        public void TocarMusicaFundo() => Tocar(_musicaFundo);

        public void PararMusicaFundo()
        {
            _contexto.Post(_ =>
            {
                try { _musicaFundo?.controls.stop(); } catch { }
            }, null);
        }

        public void TocarTiroJogador() => Tocar(_somTiroJogador);
        public void TocarTiroAlien() => Tocar(_somTiroAlien);
        public void TocarExplosaoAlien() => Tocar(_somExplosao);

        public void Dispose()
        {
            _contexto.Post(_ =>
            {
                try { _musicaFundo?.controls.stop(); } catch { }
                try { _somTiroJogador?.controls.stop(); } catch { }
                try { _somTiroAlien?.controls.stop(); } catch { }
                try { _somExplosao?.controls.stop(); } catch { }
            }, null);
        }
    }
}