using System;
using System.Collections.Generic;

namespace Trabalho_POO
{
    public class GerenciadorDeColisoes
    {
        public delegate void ColisaoHandler(string mensagem);
        public event ColisaoHandler OnAlienDestruido;
        public event ColisaoHandler OnUFODestruido;
        public event ColisaoHandler OnVidaPerdida;
        public event ColisaoHandler OnDerrota;
        public event ColisaoHandler OnVitoria;

        private readonly GerenciadorDeAudio _audio;

        public GerenciadorDeColisoes(GerenciadorDeAudio audio)
        {
            _audio = audio;
        }

        public void Verificar(
            NaveJogador nave,
            List<Alien> aliens,
            List<Barreira> barreiras,
            List<Projetil> projetisJogador,
            List<Projetil> projetisAliens,
            NaveUFO ufo,
            int alturaTela)
        {
            VerificarProjetisJogadorXAliens(aliens, projetisJogador);
            VerificarProjetisJogadorXUFO(ufo, projetisJogador);
            VerificarProjetisAliensXNave(nave, projetisAliens, alturaTela);
            VerificarColisaoBarreiras(barreiras, projetisJogador, projetisAliens);
            LimparProjetisForaDaTela(projetisJogador, projetisAliens, alturaTela);
            VerificarAliensNaBordaInferior(aliens, alturaTela);
            VerificarVitoria(aliens);
        }

        private void LimparProjetisForaDaTela(
            List<Projetil> projetisJogador,
            List<Projetil> projetisAliens,
            int alturaTela)
        {
            projetisJogador.RemoveAll(p => p.ForaDaTela(alturaTela));
            projetisAliens.RemoveAll(p => p.ForaDaTela(alturaTela));
        }

       
        private void VerificarColisaoBarreiras(
            List<Barreira> barreiras,
            List<Projetil> projetisJogador,
            List<Projetil> projetisAliens)
        {
            var projetisJRemover = new HashSet<Projetil>();
            var projetisARemover = new HashSet<Projetil>();
            var barreirasRemover = new List<Barreira>();

            foreach (var b in barreiras)
            {
                foreach (var p in projetisJogador)
                    if (!projetisJRemover.Contains(p) && p.Bounds.IntersectsWith(b.Bounds))
                    {
                        projetisJRemover.Add(p);
                        b.ReceberDano();
                    }

                foreach (var p in projetisAliens)
                    if (!projetisARemover.Contains(p) && p.Bounds.IntersectsWith(b.Bounds))
                    {
                        projetisARemover.Add(p);
                        b.ReceberDano();
                    }

                if (!b.Ativa) barreirasRemover.Add(b);
            }

            projetisJogador.RemoveAll(p => projetisJRemover.Contains(p));
            projetisAliens.RemoveAll(p => projetisARemover.Contains(p));
            barreiras.RemoveAll(b => barreirasRemover.Contains(b));
        }

        private void VerificarProjetisJogadorXAliens(
            List<Alien> aliens,
            List<Projetil> projetisJogador)
        {
            var projetisRemover = new HashSet<Projetil>();
            var aliensRemover = new HashSet<Alien>();

            foreach (var p in projetisJogador)
                foreach (var a in aliens)
                    if (!aliensRemover.Contains(a) && p.Bounds.IntersectsWith(a.Bounds))
                    {
                        projetisRemover.Add(p);
                        aliensRemover.Add(a);
                        OnAlienDestruido?.Invoke("Alien destruído!");
                        _audio?.TocarExplosaoAlien();
                    }

            projetisJogador.RemoveAll(p => projetisRemover.Contains(p));
            foreach (var a in aliensRemover) { a.Destruir(); aliens.Remove(a); }
        }

        private void VerificarProjetisJogadorXUFO(
            NaveUFO ufo,
            List<Projetil> projetisJogador)
        {
            if (ufo == null || !ufo.Ativo) return;

            var remover = new List<Projetil>();
            foreach (var p in projetisJogador)
                if (p.Bounds.IntersectsWith(ufo.Bounds))
                {
                    remover.Add(p);
                    ufo.Destruir();
                    OnUFODestruido?.Invoke(ufo.PontosBonus.ToString());
                    _audio?.TocarExplosaoAlien();
                    break;
                }

            projetisJogador.RemoveAll(p => remover.Contains(p));
        }

        private void VerificarProjetisAliensXNave(
            NaveJogador nave,
            List<Projetil> projetisAliens,
            int alturaTela)
        {
            var remover = new List<Projetil>();

            foreach (var p in projetisAliens)
            {
                if (p.ForaDaTela(alturaTela)) { remover.Add(p); continue; }

                // Respeita o período de invencibilidade pós-dano
                if (!nave.EstaInvencivel && p.Bounds.IntersectsWith(nave.Bounds))
                {
                    remover.Add(p);
                    nave.PerderVida();
                    if (!nave.EstaVivo) OnDerrota?.Invoke("Game Over! Suas vidas acabaram.");
                    else OnVidaPerdida?.Invoke(nave.Vidas.ToString());
                }
            }

            projetisAliens.RemoveAll(p => remover.Contains(p));
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