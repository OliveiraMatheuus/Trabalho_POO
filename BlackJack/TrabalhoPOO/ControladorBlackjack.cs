namespace TrabalhoPOO
{
    public enum ResultadoRodada
    {
        EmAndamento,
        JogadorEstourou,
        BancaEstourou,
        JogadorVence,
        BancaVence,
        Empate
    }

    internal class ControladorBlackjack
    {
        public Baralho BaralhoJogo { get; private set; }
        public Jogador JogadorAtual { get; private set; }
        public Banca BancaAtual { get; private set; }
        public bool JogoAtivo { get; private set; }

        public ControladorBlackjack()
        {
            BaralhoJogo = new Baralho();
            JogadorAtual = new Jogador();
            BancaAtual = new Banca();
        }

        public void IniciarNovaRodada()
        {
            JogoAtivo = true;
            JogadorAtual.LimparMao();
            BancaAtual.LimparMao();
            BaralhoJogo = new Baralho(); 

            JogadorAtual.ReceberCarta(BaralhoJogo.ComprarCarta());
            JogadorAtual.ReceberCarta(BaralhoJogo.ComprarCarta());
            BancaAtual.ReceberCarta(BaralhoJogo.ComprarCarta());
            BancaAtual.ReceberCarta(BaralhoJogo.ComprarCarta());
        }

        public ResultadoRodada ComprarCartaJogador()
        {
            if (!JogoAtivo) return ResultadoRodada.EmAndamento;

            JogadorAtual.ReceberCarta(BaralhoJogo.ComprarCarta());

            if (JogadorAtual.Estourou())
            {
                JogoAtivo = false;
                return ResultadoRodada.JogadorEstourou;
            }

            return ResultadoRodada.EmAndamento;
        }

        public ResultadoRodada PararEAnalisarVencedor()
        {
            JogoAtivo = false;
            BancaAtual.ExecutarJogada(BaralhoJogo);

            int ptsJogador = JogadorAtual.CalcularPontos();
            int ptsBanca = BancaAtual.CalcularPontos();

            if (BancaAtual.Estourou()) return ResultadoRodada.BancaEstourou;
            if (ptsJogador > ptsBanca) return ResultadoRodada.JogadorVence;
            if (ptsJogador == ptsBanca) return ResultadoRodada.Empate;

            return ResultadoRodada.BancaVence;
        }
    }
}