using System;
using System.Collections.Generic;
using tabuleiro;
using xadrez;

namespace Xadrez_Console
{
    /// <summary>
    ///Classe responsavel por instanciar e atualizar os dados no console 
    /// </summary>
    static class Tela
    {
        /// <summary>
        /// Metodo responsavel por imprimir o jogo inteiro, caso esteja em jogo, chama o metodo de imprimir o tabuleiro, caso nao finaliza o jogo
        /// </summary>
        /// <param name="partida"></param>
        public static void imprimirPartida(PartidaDeXadrez partida)
        {
            imprimirTabuleiro(partida.tab);
            Console.WriteLine();
            imprimirPecasCapturadas(partida);
            Console.WriteLine();
            Console.WriteLine("Turno: " + partida.turno);
            if (!partida.terminada)
            {
                Console.WriteLine("Aguardando jogada: " + partida.jogadorAtual);
                if (partida.xeque)
                {
                    Console.WriteLine("XEQUE!");
                }
            }
            else
            {
                Console.WriteLine("XEQUEMATE!");
                Console.WriteLine("Vencedor: " + partida.jogadorAtual);
            }
        }
        /// <summary>
        /// Metodo responsavel por imprimir as pecas capturadas, junto ao metodo imprimirconjuno
        /// </summary>
        /// <param name="partida"></param>
        public static void imprimirPecasCapturadas(PartidaDeXadrez partida)
        {
            Console.WriteLine("Peças capturadas:");
            Console.Write("Brancas: ");
            imprimirConjunto(partida.pecasCapturadas(Cor.Branca));
            Console.WriteLine();
            Console.Write("Amarelas: ");
            ConsoleColor aux = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            imprimirConjunto(partida.pecasCapturadas(Cor.Amarela));
            Console.ForegroundColor = aux;
            Console.WriteLine();
        }
        /// <summary>
        /// Metodo responsavel por imprimir o conjunto de pecas capturadas
        /// </summary>
        /// <param name="conjunto"></param>
        public static void imprimirConjunto(HashSet<Peca> conjunto)
        {
            Console.Write("[");
            foreach (Peca x in conjunto)
            {
                Console.Write(x + " ");
            }
            Console.Write("]");
        }

        /// <summary>
        /// Metodo responsavel por imprimir o tabuleiro antes da jogada
        /// </summary>
        /// <param name="tab"></param>
        public static void imprimirTabuleiro(Tabuleiro tab)
        {

            for (int i = 0; i < tab.linhas; i++)
            {
                ConsoleColor aux = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write(8 - i + " ");
                Console.ForegroundColor = aux;
                for (int j = 0; j < tab.colunas; j++)
                {
                    imprimirPeca(tab.peca(i, j));
                }
                Console.WriteLine();
            }
            ConsoleColor aux2 = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("  A B C D E F G H");
            Console.ForegroundColor = aux2;
        }

        /// <summary>
        /// Metodo responsavel por imprimir o tabuleiro durante a jogada
        /// </summary>
        /// <param name="tab"></param>
        /// <param name="posicoePossiveis"></param>
        public static void imprimirTabuleiro(Tabuleiro tab, bool[,] posicoePossiveis)
        {

            ConsoleColor fundoOriginal = Console.BackgroundColor;
            ConsoleColor fundoAlterado = ConsoleColor.DarkGray;

            for (int i = 0; i < tab.linhas; i++)
            {
                ConsoleColor aux = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write(8 - i + " ");
                Console.ForegroundColor = aux;
                for (int j = 0; j < tab.colunas; j++)
                {
                    if (posicoePossiveis[i, j])
                    {
                        Console.BackgroundColor = fundoAlterado;
                    }
                    else
                    {
                        Console.BackgroundColor = fundoOriginal;
                    }
                    imprimirPeca(tab.peca(i, j));
                    Console.BackgroundColor = fundoOriginal;
                }
                Console.WriteLine();
            }
            ConsoleColor aux2 = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("  A B C D E F G H");
            Console.BackgroundColor = fundoOriginal;
            Console.ForegroundColor = aux2;
        }

        /// <summary>
        /// Metodo responsavel por identificar o comando do jogador
        /// </summary>
        /// <returns></returns>
        public static PosicaoXadrez lerPosicaoXadrez()
        {
            string s = Console.ReadLine();
            string s1 = s.ToLower();
            char coluna = s1[0];
            int linha = int.Parse(s1[1] + "");
            

            return new PosicaoXadrez(coluna, linha);
        }

        /// <summary>
        /// Metodo responsavel por imprimir as pecas no tabuleiro
        /// </summary>
        /// <param name="peca"></param>
        public static void imprimirPeca(Peca peca)
        {

            if (peca == null)
            {
                ConsoleColor aux = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("- ");
                Console.ForegroundColor = aux;
            }
            else
            {
                if (peca.cor == Cor.Branca)
                {
                    Console.Write(peca);
                }
                else
                {
                    ConsoleColor aux = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(peca);
                    Console.ForegroundColor = aux;
                }
                Console.Write(" ");
            }
        }
    }
}
