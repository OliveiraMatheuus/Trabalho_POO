using System;
using System.Collections.Generic;

namespace Trabalho_POO
{
    public class GerenciadorDeColisoes
    {
        public delegate void ColisaoHandler(string mensagem);
        public event ColisaoHandler OnAlienDestruido;
        public event ColisaoHandler OnVidaPerdida;
        public event ColisaoHandler OnDerrota;
        public event ColisaoHandler OnVitoria;
        private GerenciadorDeAudio _audio;

        public GerenciadorDeColisoes(GerenciadorDeAudio audio)
        {
            _audio = audio;
        }

        public void Verificar(
            NaveJogador nave,
            List<Alien> aliens,
            List<Barreira> barreiras, // <- Nova lista de barreiras aqui
            List<Projetil> projetisJogador,
            List<Projetil> projetisAliens,
            int alturaTela)
        {
            VerificarProjetisJogadorXAliens(aliens, projetisJogador);
            VerificarProjetisAliensXNave(nave, projetisAliens, alturaTela);
            VerificarColisaoBarreiras(barreiras, projetisJogador, projetisAliens); // <- Verificação nova
            VerificarAliensNaBordaInferior(aliens, alturaTela);
            VerificarVitoria(aliens);
        }

        private void VerificarColisaoBarreiras(List<Barreira> barreiras, List<Projetil> projetisJogador, List<Projetil> projetisAliens)
        {
            var projetisRemover = new List<Projetil>();
            var barreirasRemover = new List<Barreira>();

            foreach (var b in barreiras)
            {
                // Verifica colisão com tiros do jogador
                foreach (var p in projetisJogador)
                {
                    if (p.Bounds.IntersectsWith(b.Bounds))
                    {
                        projetisRemover.Add(p);
                        b.ReceberDano();
                    }
                }
                foreach (var p in projetisRemover) projetisJogador.Remove(p);
                projetisRemover.Clear();

                // Verifica colisão com tiros dos aliens
                foreach (var p in projetisAliens)
                {
                    if (p.Bounds.IntersectsWith(b.Bounds))
                    {
                        projetisRemover.Add(p);
                        b.ReceberDano();
                    }
                }
                foreach (var p in projetisRemover) projetisAliens.Remove(p);
                projetisRemover.Clear();

                // Se a barreira ficou sem vida, marca para remover
                if (!b.Ativa) barreirasRemover.Add(b);
            }

            foreach (var b in barreirasRemover) barreiras.Remove(b);
        }

        private void VerificarProjetisJogadorXAliens(List<Alien> aliens, List<Projetil> projetisJogador)
        {
            var projetisRemover = new List<Projetil>();
            var aliensRemover = new List<Alien>();

            foreach (var p in projetisJogador)
                foreach (var a in aliens)
                    if (p.Bounds.IntersectsWith(a.Bounds))
                    {
                        projetisRemover.Add(p);
                        aliensRemover.Add(a);
                        OnAlienDestruido?.Invoke("Alien destruído!");
                        _audio?.TocarExplosaoAlien();
                    }

            foreach (var p in projetisRemover) projetisJogador.Remove(p);
            foreach (var a in aliensRemover) { a.Destruir(); aliens.Remove(a); }
        }

        private void VerificarProjetisAliensXNave(NaveJogador nave, List<Projetil> projetisAliens, int alturaTela)
        {
            var remover = new List<Projetil>();

            foreach (var p in projetisAliens)
            {
                if (p.ForaDaTela(alturaTela)) { remover.Add(p); continue; }
                if (p.Bounds.IntersectsWith(nave.Bounds))
                {
                    remover.Add(p);
                    nave.PerderVida();
                    if (!nave.EstaVivo) OnDerrota?.Invoke("Game Over! Suas vidas acabaram.");
                    else OnVidaPerdida?.Invoke(nave.Vidas.ToString());
                }
            }

            foreach (var p in remover) projetisAliens.Remove(p);
        }

        private void VerificarAliensNaBordaInferior(List<Alien> aliens, int alturaTela)
        {
            foreach (var a in aliens)
                if (a.AlcancouBordaInferior(alturaTela))
                {
                    OnDerrota?.Invoke("Game Over! Os aliens chegaram até você!");
                    return;
                }
        }

        private void VerificarVitoria(List<Alien> aliens)
        {
            if (aliens.Count == 0) OnVitoria?.Invoke("Vitória!");
        }
    }
}