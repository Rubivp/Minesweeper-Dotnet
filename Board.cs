namespace Minesweeper
{
    internal class Board
    {
        private char[,] minefield;
        private int mines;
        private int minefield_x;
        private int minefield_y;

        public Board(int height, int width, int mines)
        {            
            minefield_y = height;
            minefield_x = width;
            this.mines = mines;


            minefield = new char[minefield_y, minefield_x];

            

            for (int j = 0; j < minefield_y; j++)
            { 
                for (int k = 0; k < minefield_x; k++)
                {
                    minefield[j, k] = '.';
                }
            }

            
        }


        public void Mine_Gen(int cursor_y, int cursor_x)
        {
            Random random = new();
            int a;
            int b;

            int i = 0;
            while (i < mines)
            {
                a = random.Next(0, minefield_y);
                b = random.Next(0, minefield_x);
                if (minefield[a, b] != 'x' && a!=cursor_y && b!=cursor_x)
                { 
                    minefield[a, b] = 'x'; Num_Gen(a, b); i++;
                };
            }
        }

        private void Num_Gen(int y, int x)
        {
            for (int i = -1; i < 2; i++)
            {
                if (y + i < 0 || minefield_y <= y + i) { continue; }

                for (int j = -1; j < 2 ; j++)
                {
                    if (x + j < 0 || minefield_x <= x + j) { continue; }

                    if (minefield[y + i, x + j] == 'x') { continue;}

                    if (minefield[ y + i, x  + j ] == '.')
                    { minefield[ y  + i, x + j] = '1'; }
                    else { minefield[ y + i, x + j ]++; }
                    
                }
            }
        }

        public char GetBoardPos(int y, int x) { return minefield[y, x]; }
        public int GetMineCount() { return mines; }
        public int GetBoardX() { return minefield_x; }
        public int GetBoardY() { return minefield_y; }

    }
}
