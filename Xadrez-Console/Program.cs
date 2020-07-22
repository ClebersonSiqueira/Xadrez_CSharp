﻿using System;
using tabuleiro;
using xadrez;

namespace Xadrez_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Tabuleiro tab = new Tabuleiro(8, 8);

                tab.colocarPeca(new Torre(tab, Cor.Amarela), new Posicao(2, 3));
                tab.colocarPeca(new Rei(tab, Cor.Amarela), new Posicao(6, 2));
                tab.colocarPeca(new Torre(tab, Cor.Branca), new Posicao(1, 3));
                tab.colocarPeca(new Rei(tab, Cor.Branca), new Posicao(5, 2));



                Tela.imprimirTabuleiro(tab);





                
            }
            catch (TabuleiroException e){
                Console.WriteLine(e.Message);
            }
            Console.ReadLine();
        }
    }
}
