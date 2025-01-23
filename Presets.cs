using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper
{
    internal class Presets
    {
        int width = 8;
        int height = 8;
        int mines = 10;

        public bool PresetMenu()
        {
            ConsoleKey? input;


            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.WriteLine("// Opciones //\n\n" +
            "----------- Dificultades -----------\n\n"+
            " [1] - Principiante (8x8, 10 minas)\n" +
            " [2] - Intermedio (16x16, 40 minas)\n" +
            " [3] - Experto (30x16, 99 minas)\n" +
            "\n------------------------------------\n\n"+
            "[tab] - Volver Atras\n" +
            "[esc] - Cerrar Juego\n");

            Console.ResetColor();

            while (true)
            {
                input = Console.ReadKey(true).Key;

                switch (input)
                {
                    case ConsoleKey.Tab:
                        return false;
                    
                    case ConsoleKey.Escape:
                        Environment.Exit(0);
                        break;

                    case ConsoleKey.D1:
                        width = 8;
                        height = 8;
                        mines = 10;
                        return true;

                    case ConsoleKey.D2:
                        width = 16;
                        height = 16;
                        mines = 40;
                        return true;

                    case ConsoleKey.D3:
                        width = 32;
                        height = 16;
                        mines = 90;
                        return true;

                    default: break;
                }
            }
        }

        public int GetWidth() { return width; }
        public int GetHeight() { return height; }
        public int GetMines() { return mines; }
    }
}
