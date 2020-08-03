using System;
using tabuleiro;
using xadrez;
using Xadrez_Console.Logs;

namespace Xadrez_Console
{
    /// <summary>
    /// Classe responsavel por iniciar e instanciar os recursos iniciais do programa
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {

            try
            {
                AppDomain currentDomain = AppDomain.CurrentDomain;
                currentDomain.UnhandledException += new UnhandledExceptionEventHandler(MyHandler);

                PartidaDeXadrez partida = new PartidaDeXadrez();

                while (!partida.terminada)
                {

                    try
                    {
                        Console.Clear();
                        Tela.imprimirPartida(partida);

                        Console.WriteLine();
                        Console.Write("Origem: ");
                        Posicao origem = Tela.lerPosicaoXadrez().toPosicao();
                        partida.validarPosicaoDeOrigem(origem);
                        

                        bool[,] posicoesPossiveis = partida.tab.peca(origem).movimentosPossiveis();

                        Console.Clear();
                        Tela.imprimirTabuleiro(partida.tab, posicoesPossiveis);

                        Console.WriteLine();
                        Console.Write("Destino: ");
                        Posicao destino = Tela.lerPosicaoXadrez().toPosicao();
                        partida.validarPosicaoDeDestino(origem, destino);

                        partida.realizaJogada(origem, destino);
                    }
                    catch (TabuleiroException e)
                    {
                        LogWriter.Error(e.Message, e);
                        Console.WriteLine(e.Message);
                        Console.ReadLine();
                    }
                    catch (IndexOutOfRangeException e)
                    {
                        LogWriter.Error(e.Message, e);
                        Console.WriteLine("Posição inválida!");
                        Console.ReadLine();
                    }
                    catch (Exception e)
                    {
                        LogWriter.Error(e.Message, e);
                        Console.WriteLine(e.Message);
                        Console.ReadLine();
                    }
                }
                Console.Clear();
                Tela.imprimirPartida(partida);
            }
            catch (TabuleiroException e)
            {
                Console.WriteLine(e.Message);
                LogWriter.Error(e.Message, e);
                Console.ReadLine();
            }
            catch (IndexOutOfRangeException e)
            {
                LogWriter.Error(e.Message, e);
                Console.WriteLine("Posição inválida!");
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                LogWriter.Error(e.Message, e);
                Console.ReadLine();
            }

            Console.ReadLine();
        }

        static void MyHandler(object sender, UnhandledExceptionEventArgs args)
        {
            Exception e = (Exception)args.ExceptionObject;
            LogWriter.FatalError(e.Message, e);
            Console.WriteLine("MyHandler caught : " + e.Message);
            Console.WriteLine("Runtime terminating: {0}", args.IsTerminating);
        }
    }
}
