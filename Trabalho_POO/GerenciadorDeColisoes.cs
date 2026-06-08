using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Trabalho_POO
{
    public class GerenciadorDeColisoes
    {
        // ─── Delegates e Eventos ─────────────────────────────────
        public delegate void ColisaoHandler(string mensagem);

        public event ColisaoHandler OnAlienDestruido;
        public event ColisaoHandler OnVidaPerdida;
        public event ColisaoHandler OnDerrota;
        public event ColisaoHandler OnVitoria;

        // ─── Atributos ───────────────────────────────────────────
        private Form _form;

        // ─── Construtor ──────────────────────────────────────────
        public GerenciadorDeColisoes(Form form)
        {
            _form = form;
        }

        // ─── Verificação principal ───────────────────────────────
        public void Verificar(
            NaveJogador nave,
            List<Alien> aliens,
            List<Projetil> projetisJogador,
            List<Projetil> projetisAliens,
            int alturaTela)
        {
            VerificarProjetisJogadorXAliens(nave, aliens, projetisJogador);
            VerificarProjetisAliensXNave(nave, projetisAliens, alturaTela);
            VerificarAliensNaBordaInferior(aliens, alturaTela);
            VerificarVitoria(aliens);
        }

        // ─── Projétil do jogador × Aliens ────────────────────────
        private void VerificarProjetisJogadorXAliens(
            NaveJogador nave,
            List<Alien> aliens,
            List<Projetil> projetisJogador)
        {
            List<Projetil> projetisParaRemover = new List<Projetil>();
            List<Alien> aliensParaRemover = new List<Alien>();

            foreach (Projetil projetil in projetisJogador)
            {
                foreach (Alien alien in aliens)
                {
                    if (projetil.Bounds.IntersectsWith(alien.Bounds))
                    {
                        projetisParaRemover.Add(projetil);
                        aliensParaRemover.Add(alien);
                        OnAlienDestruido?.Invoke("Alien destruído!");
                    }
                }
            }

            // Remove depois de iterar — nunca modifique a lista dentro do foreach
            foreach (Projetil projetil in projetisParaRemover)
            {
                projetil.Remover(_form);
                projetisJogador.Remove(projetil);
            }

            foreach (Alien alien in aliensParaRemover)
            {
                alien.Destruir(_form);
                aliens.Remove(alien);
            }
        }

        // ─── Projétil dos aliens × Nave ──────────────────────────
        private void VerificarProjetisAliensXNave(
            NaveJogador nave,
            List<Projetil> projetisAliens,
            int alturaTela)
        {
            List<Projetil> projetisParaRemover = new List<Projetil>();

            foreach (Projetil projetil in projetisAliens)
            {
                // Saiu da tela — remove sem punir o jogador
                if (projetil.ForaDaTela(alturaTela))
                {
                    projetisParaRemover.Add(projetil);
                    continue;
                }

                // Acertou a nave
                if (projetil.Bounds.IntersectsWith(nave.Bounds))
                {
                    projetisParaRemover.Add(projetil);
                    nave.PerderVida();

                    if (!nave.EstaVivo)
                        OnDerrota?.Invoke("Game Over! Suas vidas acabaram.");
                    else
                        OnVidaPerdida?.Invoke($"Vida perdida! Vidas restantes: {nave.Vidas}");
                }
            }

            foreach (Projetil projetil in projetisParaRemover)
            {
                projetil.Remover(_form);
                projetisAliens.Remove(projetil);
            }
        }

        // ─── Alien alcançou a borda inferior ─────────────────────
        private void VerificarAliensNaBordaInferior(
            List<Alien> aliens,
            int alturaTela)
        {
            foreach (Alien alien in aliens)
            {
                if (alien.AlcancouBordaInferior(alturaTela))
                {
                    OnDerrota?.Invoke("Game Over! Os aliens chegaram até você!");
                    return;
                }
            }
        }

        // ─── Todos os aliens destruídos = vitória ─────────────────
        private void VerificarVitoria(List<Alien> aliens)
        {
            if (aliens.Count == 0)
                OnVitoria?.Invoke("Você venceu! Todos os aliens foram destruídos!");
        }
    }
}