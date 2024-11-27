using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
namespace ASP_NET_WEEK3_Homework_Roguelike.View
{
    public static class ConsoleHelper
    {
        public static void PrintColored(string text, ConsoleColor color, bool newLine = true)
        {
            ForegroundColor = color;
            if (newLine)
                WriteLine(text);
            else
                Write(text);
            ResetColor();
        }
    }
}
