using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Linq;
using Serilog;
using Serilog.Core;

namespace Xadrez_Console.Logs
{
    public static class LogWriter
    {
        private static Logger _logger;

        private static Logger Logger
        {
            get
            {
                _logger = new LoggerConfiguration()
                .WriteTo.File("XadrezLog.txt")
                .CreateLogger();

                return _logger;
            }
        }

        public static void Info(string info)
        {
            Logger.Information(info);
        }

        public static void Error(string erro)
        {
            Logger.Error(erro);
        }

        public static void Error(string erro, Exception ex)
        {
            Logger.Error(ex, erro);
        }

        internal static void FatalError(string message, Exception e)
        {
            Logger.Fatal(e, message);
        }
    }
}
