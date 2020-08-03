using System.Collections.Generic;
using tabuleiro;
using System;
using Xadrez_Console.Logs;

namespace xadrez
{
    class PartidaDeXadrez
    {

        public Tabuleiro tab { get; private set; }
        public int turno { get; private set; }
        public Cor jogadorAtual { get; private set; }
        public Peca vulneravelEnPassant { get; private set; }
        public bool terminada { get; private set; }
        private HashSet<Peca> pecas;
        private HashSet<Peca> capturadas;
        public bool xeque { get; private set; }
        

        public PartidaDeXadrez()
        {
            tab = new Tabuleiro(8, 8);
            turno = 1;
            jogadorAtual = Cor.Branca;
            terminada = false;
            xeque = false;
            vulneravelEnPassant = null;
            pecas = new HashSet<Peca>();
            capturadas = new HashSet<Peca>();
            colocarPecas();
        }
        /// <summary>
        /// Metodo responsavel por Executar um movimento dados os comandos do jogador
        /// </summary>
        /// <param name="origem">Cordenadas de origem da peca selecionada pelo jogador</param>
        /// <param name="destino">Cordenadas de destino escolhido pelo jogador</param>
        /// <returns>Retorna a peca capturada para adicionar na lista de pecas capturadas</returns>
        public Peca executaMovimento(Posicao origem, Posicao destino)
        {
            Peca p = tab.retirarPeca(origem);
            p.incrementarQteMovimentos();
            Peca pecaCapturada = tab.retirarPeca(destino);
            tab.colocarPeca(p, destino);
            if (pecaCapturada != null)
            {
                capturadas.Add(pecaCapturada);
            }

            // #jogadaEspecial Roque Pequeno
            if(p is Rei && destino.coluna == origem.coluna + 2)
            {
                Posicao origemT = new Posicao(origem.linha, origem.coluna + 3);
                Posicao destinoT = new Posicao(origem.linha, origem.coluna + 1);
                Peca T = tab.retirarPeca(origemT);
                T.incrementarQteMovimentos();
                tab.colocarPeca(T, destinoT);
            }

            // #jogadaEspecial EnPassant
            if (p is Peao && origem.coluna != destino.coluna && pecaCapturada == null)
            {
                Posicao posP;
                if (p.cor == Cor.Branca)

                    posP = new Posicao(destino.linha + 1, destino.coluna);

                else
                    posP = new Posicao(destino.linha - 1, destino.coluna);
                pecaCapturada = tab.retirarPeca(posP);
                capturadas.Add(pecaCapturada);
            }
            


            LogWriter.Info($"{jogadorAtual} fez uma jogada Peça:{p.Nome()} {origem} -> {destino}");
            return pecaCapturada;
        }

        /// <summary>
        /// Metodo responsavel por desfazer a ultima jogada, voltando o jogo ao estado anterior
        /// </summary>
        /// <param name="origem">Cordenadas de origem da peca selecionada pelo jogador</param>
        /// <param name="destino">Cordenadas de destino escolhido pelo jogador</param>
        /// <param name="pecaCapturada">Retorna a peca que foi capturada para ser retirada da lista de pecas capturadas</param>
        public void desfazMovimento(Posicao origem, Posicao destino, Peca pecaCapturada)
        {
            Peca p = tab.retirarPeca(destino);
            p.decrementarQteMovimentos();
            if (pecaCapturada != null)
            {
                tab.colocarPeca(pecaCapturada, destino);
                capturadas.Remove(pecaCapturada);
            }
            tab.colocarPeca(p, origem);

            // #jogadaEspecial Roque Pequeno
            if (p is Rei && destino.coluna == origem.coluna + 2)
            {
                Posicao origemT = new Posicao(origem.linha, origem.coluna + 3);
                Posicao destinoT = new Posicao(origem.linha, origem.coluna + 1);
                Peca T = tab.retirarPeca(destinoT);
                T.decrementarQteMovimentos();
                tab.colocarPeca(T, origemT);
            }

            // #jogadaEspecial Roque Grande
            if (p is Rei && destino.coluna == origem.coluna + 2)
            {
                Posicao origemT = new Posicao(origem.linha, origem.coluna -4);
                Posicao destinoT = new Posicao(origem.linha, origem.coluna - 1);
                Peca T = tab.retirarPeca(destinoT);
                T.decrementarQteMovimentos();
                tab.colocarPeca(T, origemT);
            }

            //#Jogada Especial En Passant
            if (p is Peao && origem.coluna != destino.coluna && pecaCapturada == vulneravelEnPassant)
            {
                Peca peao = tab.retirarPeca(destino);
                Posicao posP;
                if (p.cor == Cor.Branca)
                    posP = new Posicao(3, destino.coluna);
                else
                    posP = new Posicao(4, destino.coluna);
                tab.colocarPeca(peao, posP);
            }

        }

        /// <summary>
        /// metodo que executa a jogada passada pelo jogador e solicita confirmacoes de xeque e xequemate
        /// </summary>
        /// <param name="origem"></param>
        /// <param name="destino"></param>
        public void realizaJogada(Posicao origem, Posicao destino)
        {
            Peca pecaCapturada = executaMovimento(origem, destino);

            if (estaEmXeque(jogadorAtual))
            {
                desfazMovimento(origem, destino, pecaCapturada);
                throw new TabuleiroException("Você não pode se colocar em xeque!");
            }

            Peca p = tab.peca(destino);

            //#jogadaEspecial Promocao
            if ((p.cor == Cor.Branca && destino.linha == 0) || (p.cor == Cor.Amarela && destino.linha == 7))
            {
                bool pergunta = false;
                while (!pergunta)
                {
                    Console.WriteLine("Deseja transformar seu Peao em qual Peca? D/C/T/B");
                    char resposta = char.Parse(Console.ReadLine());
                    if (resposta == 'd' || resposta == 'D')
                    {
                        p = tab.retirarPeca(destino);
                        pecas.Remove(p);
                        Peca dama = new Dama(tab, p.cor);
                        tab.colocarPeca(dama, destino);
                        pecas.Add(dama);
                        pergunta = true;
                    }
                    else if (resposta == 'c' || resposta == 'C')
                    {
                        p = tab.retirarPeca(destino);
                        pecas.Remove(p);
                        Peca cavalo = new Cavalo(tab, p.cor);
                        tab.colocarPeca(cavalo, destino);
                        pecas.Add(cavalo);
                        pergunta = true;
                    }
                    else if (resposta == 't' || resposta == 'T')
                    {
                        p = tab.retirarPeca(destino);
                        pecas.Remove(p);
                        Peca torre = new Torre(tab, p.cor);
                        tab.colocarPeca(torre, destino);
                        pecas.Add(torre);
                        pergunta = true;
                    }
                    else if (resposta == 'b' || resposta == 'B')
                    {
                        p = tab.retirarPeca(destino);
                        pecas.Remove(p);
                        Peca bispo = new Bispo(tab, p.cor);
                        tab.colocarPeca(bispo, destino);
                        pecas.Add(bispo);
                        pergunta = true;
                    }
                    else
                        Console.WriteLine("Opcao invalida");
                }
            }

            if (estaEmXeque(adversaria(jogadorAtual)))
            {
                xeque = true;
            }
            else
            {
                xeque = false;
            }



            if (testeXequemate(adversaria(jogadorAtual)))
            {
                terminada = true;
            }
            else
            {
                turno++;
                mudaJogador();
            }
            

            //#JogadaEspecial En Passant
            if (p is Peao && destino.linha == origem.linha - 2 || destino.linha == origem.linha + 2)
            {
                vulneravelEnPassant = p;
            }
            else
                vulneravelEnPassant = null;
        }

        /// <summary>
        /// Metodo para validar se tem peca disponivel na posicao de origem
        /// </summary>
        /// <param name="pos">posicao passada pelo jogador</param>
        public void validarPosicaoDeOrigem(Posicao pos)
        {
            if (tab.peca(pos) == null)
            {
                throw new TabuleiroException("Não existe peça na posição de origem escolhida!");
            }
            if (jogadorAtual != tab.peca(pos).cor)
            {
                throw new TabuleiroException("A peça de origem escolhida não é sua!");
            }
            if (!tab.peca(pos).existeMovimentosPossiveis())
            {
                throw new TabuleiroException("Não há movimentos possíveis para a peça de origem escolhida!");
            }
            
                
        }
        /// <summary>
        /// Metodo responsavel por definir se é possivel mover a peca para a casa selecionada
        /// </summary>
        /// <param name="origem">posicao de origem passada pelo jogador</param>
        /// <param name="destino">posicao de destino passada pelo jogador</param>
        public void validarPosicaoDeDestino(Posicao origem, Posicao destino)
        {
            if (!tab.peca(origem).movimentoPossivel(destino))
            {
                throw new TabuleiroException("Posição de destino inválida!");
            }
        }

        /// <summary>
        /// Metodo que muda a vez do jogador
        /// </summary>
        private void mudaJogador()
        {
            if (jogadorAtual == Cor.Branca)
            {
                jogadorAtual = Cor.Amarela;
            }
            else
            {
                jogadorAtual = Cor.Branca;
            }
        }

        /// <summary>
        /// Metodo que define todas as pecas capturadas
        /// </summary>
        /// <param name="cor"></param>
        /// <returns></returns>
        public HashSet<Peca> pecasCapturadas(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca x in capturadas)
            {
                if (x.cor == cor)
                {
                    aux.Add(x);
                }
            }
            return aux;
        }

        /// <summary>
        /// Metodo que define todas as pecas em jogo
        /// </summary>
        /// <param name="cor"></param>
        /// <returns></returns>
        public HashSet<Peca> pecasEmJogo(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca x in pecas)
            {
                if (x.cor == cor)
                {
                    aux.Add(x);
                }
            }
            aux.ExceptWith(pecasCapturadas(cor));
            return aux;
        }

        /// <summary>
        /// Metodo para identificar quem é o adversario do jogador atual
        /// </summary>
        /// <param name="cor"></param>
        /// <returns></returns>
        private Cor adversaria(Cor cor)
        {
            if (cor == Cor.Branca)
            {
                return Cor.Amarela;
            }
            else
            {
                return Cor.Branca;
            }
        }

        /// <summary>
        /// Metodo que localiza a peca da classe Rei
        /// </summary>
        /// <param name="cor"></param>
        /// <returns></returns>
        private Peca rei(Cor cor)
        {
            foreach (Peca x in pecasEmJogo(cor))
            {
                if (x is Rei)
                {
                    return x;
                }
            }
            return null;
        }

        /// <summary>
        /// Metodo que identifica se o jogador esta em Xeque
        /// </summary>
        /// <param name="cor"></param>
        /// <returns></returns>
        public bool estaEmXeque(Cor cor)
        {
            Peca R = rei(cor);
            if (R == null)
            {
                throw new TabuleiroException("Não tem rei da cor " + cor + " no tabuleiro!");
            }
            foreach (Peca x in pecasEmJogo(adversaria(cor)))
            {
                bool[,] mat = x.movimentosPossiveis();
                if (mat[R.posicao.linha, R.posicao.coluna])
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Metodo que verifica que o jogador esta em XequeMat
        /// </summary>
        /// <param name="cor"></param>
        /// <returns></returns>
        public bool testeXequemate(Cor cor)
        {
            if (!estaEmXeque(cor))
            {
                return false;
            }
            foreach (Peca x in pecasEmJogo(cor))
            {
                bool[,] mat = x.movimentosPossiveis();
                for (int i = 0; i < tab.linhas; i++)
                {
                    for (int j = 0; j < tab.colunas; j++)
                    {
                        if (mat[i, j])
                        {
                            Posicao origem = x.posicao;
                            Posicao destino = new Posicao(i, j);
                            Peca pecaCapturada = executaMovimento(origem, destino);
                            bool testeXeque = estaEmXeque(cor);
                            desfazMovimento(origem, destino, pecaCapturada);
                            if (!testeXeque)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Metodo que coloca uma nova peca no tabuleiro
        /// </summary>
        /// <param name="coluna"></param>
        /// <param name="linha"></param>
        /// <param name="peca"></param>
        public void colocarNovaPeca(char coluna, int linha, Peca peca)
        {
            tab.colocarPeca(peca, new PosicaoXadrez(coluna, linha).toPosicao());
            pecas.Add(peca);
        }


        /// <summary>
        /// Metodo utilizado para definir quais pecas e quais as pocisoes iniciais delas no tabuleiro
        /// </summary>
        private void colocarPecas()
        {
           
            colocarNovaPeca('a', 1, new Torre(tab, Cor.Branca));
            colocarNovaPeca('b', 1, new Cavalo(tab, Cor.Branca));
            colocarNovaPeca('c', 1, new Bispo(tab, Cor.Branca));
            colocarNovaPeca('d', 1, new Dama(tab, Cor.Branca));
            colocarNovaPeca('e', 1, new Rei(tab, Cor.Branca, this));
            colocarNovaPeca('f', 1, new Bispo(tab, Cor.Branca));
            colocarNovaPeca('g', 1, new Cavalo(tab, Cor.Branca));
            colocarNovaPeca('h', 1, new Torre(tab, Cor.Branca));
            colocarNovaPeca('a', 2, new Peao(tab, Cor.Branca, this));
            colocarNovaPeca('b', 2, new Peao(tab, Cor.Branca, this));
            colocarNovaPeca('c', 2, new Peao(tab, Cor.Branca, this));
            colocarNovaPeca('d', 2, new Peao(tab, Cor.Branca, this));
            colocarNovaPeca('e', 2, new Peao(tab, Cor.Branca, this));
            colocarNovaPeca('f', 2, new Peao(tab, Cor.Branca, this));
            colocarNovaPeca('g', 2, new Peao(tab, Cor.Branca, this));
            colocarNovaPeca('h', 2, new Peao(tab, Cor.Branca, this));

            colocarNovaPeca('a', 8, new Torre(tab, Cor.Amarela));
            colocarNovaPeca('b', 8, new Cavalo(tab, Cor.Amarela));
            colocarNovaPeca('c', 8, new Bispo(tab, Cor.Amarela));
            colocarNovaPeca('d', 8, new Dama(tab, Cor.Amarela));
            colocarNovaPeca('e', 8, new Rei(tab, Cor.Amarela, this));
            colocarNovaPeca('f', 8, new Bispo(tab, Cor.Amarela));
            colocarNovaPeca('g', 8, new Cavalo(tab, Cor.Amarela));
            colocarNovaPeca('h', 8, new Torre(tab, Cor.Amarela));
            colocarNovaPeca('a', 7, new Peao(tab, Cor.Amarela, this));
            colocarNovaPeca('b', 7, new Peao(tab, Cor.Amarela, this));
            colocarNovaPeca('c', 7, new Peao(tab, Cor.Amarela, this));
            colocarNovaPeca('d', 7, new Peao(tab, Cor.Amarela, this));
            colocarNovaPeca('e', 7, new Peao(tab, Cor.Amarela, this));
            colocarNovaPeca('f', 7, new Peao(tab, Cor.Amarela, this));
            colocarNovaPeca('g', 7, new Peao(tab, Cor.Amarela, this));
            colocarNovaPeca('h', 7, new Peao(tab, Cor.Amarela, this));
        }
    }
}
