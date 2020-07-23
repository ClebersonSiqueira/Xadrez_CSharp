﻿using System;
using System.Collections.Generic;
using System.Text;
using tabuleiro;
using xadrez;

namespace Xadrez_Console
{
    class Tela
    {
        public static void imprimirPartida(PartidaDeXadrez partida)
        {
            Tela.imprimirTabuleiro(partida.tab);
            Console.WriteLine();
            imprimirPecasCapturadas(partida);
            Console.WriteLine();
            Console.WriteLine("Turno: " + partida.turno);
            Console.WriteLine("Aguardando jogada do jogador " + partida.jogadorAtual);
            if (partida.xeque)
            {
                Console.WriteLine("Xeque");
            }
        }

        public static void imprimirPecasCapturadas(PartidaDeXadrez partida)
        {
            Console.WriteLine("Pecas capturadas: ");
            Console.Write("Brancas: ");
            imprimirConjunto(partida.pecasCapturadas(Cor.Branca));
            Console.WriteLine();
            ConsoleColor aux = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Amarelas: ");
            imprimirConjunto(partida.pecasCapturadas(Cor.Amarela));
            Console.ForegroundColor = aux;
            Console.WriteLine();
        }

        public static void imprimirConjunto(HashSet<Peca> conunto)
        {
            Console.Write("[");
            foreach (Peca x in conunto)
                Console.Write(x + " ");
            Console.Write("]");
        }

        public static void imprimirTabuleiro (Tabuleiro tab)
        {
            for (int i = 0; i < tab.linhas; i++)
            {
                ConsoleColor aux = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write(8 - i + " ");
                Console.ForegroundColor = aux;
                for (int j = 0; j < tab.colunas; j++)
                {
                        Tela.imprimirPeca(tab.peca(i, j));
                }
                Console.WriteLine();
            }
            ConsoleColor aux2 = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("  A B C D E F G H");
            Console.ForegroundColor = aux2;
        }

        public static void imprimirTabuleiro(Tabuleiro tab, bool[,] posicoesPossiveis)
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
                    if (posicoesPossiveis[i, j])
                        Console.BackgroundColor = fundoAlterado;
                    else
                        Console.BackgroundColor = fundoOriginal;
                    Tela.imprimirPeca(tab.peca(i, j));
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

        public static PosicaoXadrez lerPosicaoXadrez()
        {
            string s = Console.ReadLine();
            char coluna = s[0];
            int linha = int.Parse(s[1] + "");
            return new PosicaoXadrez(coluna, linha);
        }
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
                    Console.Write(peca);
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
