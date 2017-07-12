using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;

namespace UMLcreator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            SetupLogger();
            Trace.WriteLine("\t Hello World!");
        }

        private static void SetupLogger() {
            Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
            Trace.AutoFlush = true;
            Trace.IndentSize = 4;
        }

    }
}
