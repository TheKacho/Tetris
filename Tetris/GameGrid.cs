namespace Tetris
{
    public class GameGrid
    {
        private readonly int [,] grid;

        public int Rows { get; }
        public int Columns { get; }

        public int this[int r, int c]
        {
            get => grid[r, c];
            set => grid[r, c] = value; // with these in place, can use indexing directly on
                                       // game grid object
        }

        public GameGrid(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            grid = new int[rows, columns];
        }

        // this will check if the given row/ and column is inside grid or not
        public bool IsInside(int r, int c)
        {
            return r >= 0 && c >= 0 && r < Rows && c < Columns;
                    // row must be greater than/equal 0 and less than number of rows
                    // colums is the same thing, with less than number of columns
        }

        // this method checks if the cell is empty or not

        public bool IsEmpty(int r, int c)
        {
            return IsInside(r, c) && grid[r, c] == 0;
            // this means that it must be inside the grid and the value
            // of that entry in array must be zero
        }

        // this method checks if any rwos are full
        public bool IsRowFull(int r)
        {
            for(int c = 0; c < Columns; c++)
            {
                if (grid[r, c] == 0)
                {
                    return false;
                }
            }

            return true;

        }

        //this checks if the row is empty
        public bool IsRowEmpty(int r)
        {
            for (int c = 0; c < Columns; c++)
            {
                if (grid [r, c] != 0)
                {
                    return false;
                }
            }
            return true; 
                       
        }

        // anytime the rows are cleared, any non full rows are moved down
        private void ClearRow(int r)
        {
            for (int c = 0; c < Columns; c++)
            {
                grid[r, c] = 0;
            }
        }

        // this moves the rows down by a certain amount each time a full row is cleared
        private void MoveRowDown(int r, int numRows)
        {
            for (int c = 0; c < Columns; c++)
            {
                grid[r + numRows, c] = grid[r, c];
                grid[r, c] = 0;

            }
        }

        // this will clear any full rows
        public int ClearFullRows()
        {
            int cleared = 0;

            for (int r = Rows-1; r >= 0; r--)
            {
                if (IsRowFull(r))
                {
                    ClearRow(r);
                    cleared++;
                }
                else if (cleared > 0)
                {
                    MoveRowDown(r, cleared);
                }
            }

            return cleared;
        }

    }
}
