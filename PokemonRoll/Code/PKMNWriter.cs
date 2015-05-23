using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonRoll
{
    class PKMNWriter : TextWriter
    {
        private TextWriter originalOut;

        public PKMNWriter()
        {
            originalOut = Console.Out;
        }

        public override Encoding Encoding
        {
            get { return new System.Text.ASCIIEncoding(); }
        }

        public override void WriteLine()
        {
            Console.ResetColor();
            originalOut.WriteLine();
        }

        public override void WriteLine(string message)
        {
            Write(message);
            Console.ResetColor();
            originalOut.WriteLine();
        }

        public void setConsoleColor(char color,bool foreground)
        {
            ConsoleColor colorto = ConsoleColor.White;
            switch (color)
            {
                case '0':
                    colorto = ConsoleColor.Black;
                    break;
                case '1':
                    colorto = ConsoleColor.Blue;
                    break;
                case '2':
                    colorto = ConsoleColor.Green;
                    break;
                case '3':
                    colorto = ConsoleColor.Cyan;
                    break;
                case '4':
                    colorto = ConsoleColor.Red;
                    break;
                case '5':
                    colorto = ConsoleColor.Magenta;
                    break;
                case '6':
                    colorto = ConsoleColor.Yellow;
                    break;
                case '7':
                    colorto = ConsoleColor.White;
                    break;
                case '8':
                    colorto = ConsoleColor.Gray;
                    break;
                case '9':
                    colorto = ConsoleColor.DarkBlue;
                    break;
                case 'a':
                    colorto = ConsoleColor.DarkGreen;
                    break;
                case 'b':
                    colorto = ConsoleColor.DarkCyan;
                    break;
                case 'c':
                    colorto = ConsoleColor.DarkRed;
                    break;
                case 'd':
                    colorto = ConsoleColor.DarkMagenta;
                    break;
                case 'e':
                    colorto = ConsoleColor.DarkYellow;
                    break;
                case 'f':
                    colorto = ConsoleColor.DarkGray;
                    break;
            }

            if(foreground)
            {
                Console.ForegroundColor = colorto;
            }
            else
            {
                Console.BackgroundColor = colorto;
            }
        }

        public override void Write(string message)
        {
            char charicter = ' ';
            for (int i = 0; i < message.Length; i++)
            {
                charicter = message[i];
                if (charicter == '~')
                {
                    setConsoleColor(message[i + 1], true);
                    i++;
                }
                else if (charicter == ';')
                {
                    setConsoleColor(message[i + 1], false);
                    i++;
                }
                else
                {
                    originalOut.Write(message[i]);
                }
            }
        }
    }
}
