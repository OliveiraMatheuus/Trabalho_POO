using System;
using System.IO;
using System.Threading;
using WMPLib;

namespace TrabalhoPOO
{
    public class GerenciadorDeAudio
    {
        private WindowsMediaPlayer _musicaFundo;
        private WindowsMediaPlayer _somCarta;
        private WindowsMediaPlayer _somVitoria;
        private WindowsMediaPlayer _somDerrota;
        private WindowsMediaPlayer _somEmpate; 

        private readonly string _pastaAudio;
        private readonly SynchronizationContext _contexto;

        public GerenciadorDeAudio()
        {
            _contexto = SynchronizationContext.Current ?? new SynchronizationContext();

            _pastaAudio = Path.GetFullPath(
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                    "..", "..", "Resources"));

            InicializarPlayers();
        }

        private void InicializarPlayers()
        {
            try
            {
                _musicaFundo = CriarPlayer("musica_fundo.mp3", loop: true, volume: 10);
                _somCarta = CriarPlayer("comprar_carta.mp3", loop: false, volume: 30);
                _somVitoria = CriarPlayer("vitoria.mp3", loop: false, volume: 40);
                _somDerrota = CriarPlayer("derrota.mp3", loop: false, volume: 20);
                _somEmpate = CriarPlayer("empate.mp3", loop: false, volume: 40); 
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

        public void PararEfeitos()
        {
            _contexto.Post(_ =>
            {
                try
                {
                    _somCarta?.controls.stop();
                    _somVitoria?.controls.stop();
                    _somDerrota?.controls.stop();
                    _somEmpate?.controls.stop();
                }
                catch { }
            }, null);
        }

        public void TocarMusicaFundo() => Tocar(_musicaFundo);
        public void TocarCarta() => Tocar(_somCarta);
        public void TocarVitoria() => Tocar(_somVitoria);
        public void TocarDerrota() => Tocar(_somDerrota);
        public void TocarEmpate() => Tocar(_somEmpate);
    }
}