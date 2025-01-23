using System.Collections;

namespace Minesweeper
{
    internal class Game
    {
        private char[,] playfield;
        private int cursor_x;
        private int cursor_y;
        private Board board;
        private Presets presets = new Presets();

        private int flags;
        private bool first_turn = true;
        private bool alive = true;

        public Game()
        {
            while (true)
            {
                Reset();
                SweepinTime();
            }
        }

        private void SweepinTime()
        {
            RenderPlayfield(true);
            while (alive == true)
            {
                PlayerInput();
                if (WinCheck() == true) { break; }
                RenderPlayfield(true);
            }
            RenderPlayfield(false);
            EndScreen();
        }

        private void PlayerInput()
        {
            ConsoleKey input;
            while (true)
            {
                //El bool es necesario para evitar mostrar el input en pantalla
                input = Console.ReadKey(true).Key;
                switch (input)
                {
                    case ConsoleKey.UpArrow:
                        if (cursor_y > 0) { cursor_y--; return; }
                        else { break; }

                    case ConsoleKey.LeftArrow:
                        if (cursor_x > 0) { cursor_x--; return; }
                        else { break; }

                    case ConsoleKey.DownArrow:
                        if (cursor_y < board.GetBoardY() - 1) { cursor_y++; return; }
                        else { break; }

                    case ConsoleKey.RightArrow:
                        if (cursor_x < board.GetBoardX() - 1) { cursor_x++; return; }
                        else { break; }

                    case ConsoleKey.F: //Quitar/Poner bandera
                        if (playfield[cursor_y, cursor_x] == '#')
                        {
                            playfield[cursor_y, cursor_x] = 'F';
                            flags--;
                            return;
                        }
                        else if (playfield[cursor_y, cursor_x] == 'F')
                        {
                            playfield[cursor_y, cursor_x] = '#';
                            flags++;
                            return;
                        }
                        else { break; }

                    case ConsoleKey.D: //revelar casilla
                        RemoveTile(cursor_y,cursor_x,playfield[cursor_y,cursor_x]);
                        return;

                    case ConsoleKey.Tab:
                        if (presets.PresetMenu() == true) { Reset(); return; }
                        else { return; }

                    default: break;
                }
            }
        }

        private void RemoveTile(int y, int x, char tile)
        {
            if (tile == 'F') {return;}
            
            if (first_turn == true)
            {
                board.Mine_Gen(y,x);
                first_turn = false;
            }

            if (tile == '#')
            {
                if (board.GetBoardPos(y,x) == 'x') {alive = false; ShowMines();}
                playfield [y,x] = board.GetBoardPos(y,x);
                if (playfield[y,x]=='.') {RemoveTile(y,x, '.');}
                return;
            }

            if (tile == '.')
            for (int i = -1; i < 2; i++)
            {
                if (y + i < 0 || board.GetBoardY() <= y + i) { continue; }

                for (int j = -1; j < 2; j++)
                {
                    if (x + j < 0 || board.GetBoardX() <= x + j) { continue; }

                    switch ( board.GetBoardPos(y+i,x+j) ){

                    case '.':
                        if (y == y+i && x == x+j || playfield[y+i,x+j] == board.GetBoardPos(y+i,x+j)) {continue;}
                        playfield [y+i, x+j] = board.GetBoardPos(y+i, x+j);
                        RemoveTile(y+i, x+j, playfield[y+i, x+j]);
                        break;

                    default:
                        playfield [y+i, x+j] = board.GetBoardPos(y+i, x+j);
                        break;
                    } 
                }
            }
        }
        private void EndScreen()
        {
            RenderPlayfield(false);
            if (alive == true)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Ganaste!");
            }
            else
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Perdiste...");
            }

            Console.Write("Quieres volver a jugar? [S/n]");
            Console.ResetColor();


            ConsoleKey input;
            while (true)
            {
                input = Console.ReadKey(false).Key;
                switch (input)
                {
                    case ConsoleKey.S:
                        return;

                    case ConsoleKey.N:
                        Environment.Exit(0); break;
                }
            }
        }

        private bool WinCheck()
        {
            int flag_matches = 0;
            
            for (int i = 0; i < board.GetBoardY(); i++)
            {
                for (int j = 0; j < board.GetBoardX(); j++)
                {
                    if (playfield[i, j] == 'F' && board.GetBoardPos(i, j) == 'x')
                    {
                        flag_matches++;
                    }
                    else if (playfield[i, j] == 'F') { return false; }
                    else if (playfield[i, j] == '#') { return false; }
                }
            }

            if (flag_matches == board.GetMineCount()) { return true; }
            else { return false; }
        }

        

        private void Reset()
        {
            board = new Board(presets.GetHeight(), presets.GetWidth(), presets.GetMines());

            playfield = new char[board.GetBoardY(), board.GetBoardX()];
            flags = board.GetMineCount();
            first_turn = true;
            alive = true;

            for (int i = 0; i < board.GetBoardY(); i++)
            {
                for (int j = 0; j < board.GetBoardX(); j++)
                {
                    playfield[i, j] = '#';
                }
            }

            cursor_y = board.GetBoardY() / 2;
            cursor_x = board.GetBoardX() / 2;
        }


        private void RenderPlayfield(bool showcontrols)
        {
            Console.Clear();
            if (flags < 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Banderas: {0}\n", flags);
                Console.ResetColor();
            }
            else { Console.WriteLine("Banderas: {0}\n", flags); }


            for (int i = 0; i < board.GetBoardY(); i++)
            {
                for (int j = 0; j < board.GetBoardX(); j++)
                {
                    if (cursor_y != i || cursor_x != j) { Colorize(playfield[i, j]); }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.Yellow;
                    }

                    Console.Write(playfield[i, j]);
                    Console.ResetColor();
                    Console.Write(" ");
                }
                Console.Write("\n");
            }


            if (showcontrols == true)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\nControles:\n" +
                "Flechas: Mover Cursor\n" +
                "D: Revelar Casilla\n" +
                "F: Colocar Bandera\n" +
                "Tab: Menu");
                Console.ResetColor();
                Console.WriteLine();
            }
        }

        // x -> Mina, F -> Bandera, . -> Espacio Vacio, # -> Casilla sin Revelar
        private static void Colorize(char input)
        {
            switch (input)
            {
                case '1':
                    Console.ForegroundColor = ConsoleColor.Cyan; break;
                case '2':
                    Console.ForegroundColor = ConsoleColor.Green; break;
                case '3':
                    Console.ForegroundColor = ConsoleColor.Red; break;
                case '4':
                    Console.ForegroundColor = ConsoleColor.Magenta; break;
                case '5':
                    Console.ForegroundColor = ConsoleColor.DarkRed; break;
                case '6':
                    Console.ForegroundColor = ConsoleColor.Blue; break;
                case '7':
                    Console.ForegroundColor = ConsoleColor.Gray; break;
                case 'F':
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White; break;
                case '.':
                    Console.ForegroundColor = ConsoleColor.DarkGray; break;
                case 'x':
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.Red; break;
                default:
                    Console.ForegroundColor = ConsoleColor.White; break;
            }
        }
        private void ShowMines()
        {
            for (int i = 0; i < board.GetBoardY(); i++)
            {
                for (int j = 0; j < board.GetBoardX(); j++)
                {
                    if (playfield[i, j] == 'F' && board.GetBoardPos(i, j) == 'x')
                    { continue; }
                    else if (board.GetBoardPos(i, j) == 'x')
                    { playfield[i, j] = board.GetBoardPos(i, j); }
                }
            }
        }
    }
}
