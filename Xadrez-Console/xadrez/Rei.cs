using System;
using System.Collections.Generic;
using System.Text;
using tabuleiro;

namespace xadrez
{
    class Rei : Peca
    {
        public Rei (Tabuleiro tab, Cor cor) : base(cor, tab)
        {
        }
        public override string ToString()
        {
            return "R";
        }

        private bool podeMover(Posicao pos)
        {
            Peca p = tab.peca(pos);
            return p == null || p.cor != cor;
        }
        public override bool[,] movimentosPossiveis()
        {
            bool[,] mat = new bool[tab.linhas, tab.colunas];
            Posicao pos = new Posicao(0, 0);


            //posicao de cima
            pos.definirValores(posicao.linha - 1, posicao.coluna);
            if (tab.posicaoValida(pos) && podeMover(pos))
                mat[pos.linha, pos.coluna] = true;

            //posicao de baixo
            pos.definirValores(posicao.linha + 1, posicao.coluna);
            if (tab.posicaoValida(pos) && podeMover(pos))
                mat[pos.linha, pos.coluna] = true;

            //posicao da direita
            pos.definirValores(posicao.linha, posicao.coluna + 1);
            if (tab.posicaoValida(pos) && podeMover(pos))
                mat[pos.linha, pos.coluna] = true;

            //posicao da esquerda
            pos.definirValores(posicao.linha, posicao.coluna -1);
            if (tab.posicaoValida(pos) && podeMover(pos))
                mat[pos.linha, pos.coluna] = true;

            //posicao do nordeste
            pos.definirValores(posicao.linha + 1, posicao.coluna + 1);
            if (tab.posicaoValida(pos) && podeMover(pos))
                mat[pos.linha, pos.coluna] = true;

            //posicao do sudeste
            pos.definirValores(posicao.linha + 1, posicao.coluna - 1);
            if (tab.posicaoValida(pos) && podeMover(pos))
                mat[pos.linha, pos.coluna] = true;

            //posicao do noroeste
            pos.definirValores(posicao.linha - 1, posicao.coluna +1);
            if (tab.posicaoValida(pos) && podeMover(pos))
                mat[pos.linha, pos.coluna] = true;

            //posicao do noroeste
            pos.definirValores(posicao.linha - 1, posicao.coluna + 1);
            if (tab.posicaoValida(pos) && podeMover(pos))
                mat[pos.linha, pos.coluna] = true;

            //posicao do sudoeste
            pos.definirValores(posicao.linha - 1, posicao.coluna - 1);
            if (tab.posicaoValida(pos) && podeMover(pos))
                mat[pos.linha, pos.coluna] = true;

            return mat;
        }
    }
}
