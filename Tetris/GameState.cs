namespace Tetris
{
    public class GameState
    {
        private Block currentBlock;

        public Block CurrentBlock
        {
            get => currentBlock;
            private set
            {
                currentBlock = value;
                currentBlock.Reset(); // reset method called to set start position and rotation 

                for (int i = 0; i < 2; i++) // this will spawn the blocks two rows down away from the hidden rows
                {
                    currentBlock.Move(1, 0);

                    if (!BlockFits())
                    {
                        currentBlock.Move(-1, 0);
                    }
                }
            }
        }

        public GameGrid GameGrid { get; }
        public BlockQueue BlockQueue { get; }

        public bool GameOver { get; private set; } // displays the Game Over screen

        public int Score { get; private set; } // displays score

        public Block HeldBlock { get; private set; } // lets players put a block on hold

        public bool CanHold { get; private set; }

        public GameState()
        {
            GameGrid = new GameGrid(22, 10); // the game grid space of 22 rows, 10 columns
            BlockQueue = new BlockQueue();
            CurrentBlock = BlockQueue.GetAndUpdate();
            CanHold = true;
            
        }

        // next method is important , checks if current block is in a legal position or not

        private bool BlockFits()
        {
            foreach (Position p in CurrentBlock.TilePositions())
            {
                if (!GameGrid.IsEmpty(p.Row, p.Column)) // if any piece goes outside the game grid
                    // or overlaps another piece, then it returns false
                {
                    return false;
                }
            }
            return true;
        }

        public void HoldBlock()
        {
            if (!CanHold) // if it can't hold a block
            {
                return;
            }

            if (HeldBlock == null) // if there's no block on hold, the held block is the current block on active
            {
                HeldBlock = CurrentBlock;
                CurrentBlock = BlockQueue.GetAndUpdate(); // if the block is held, then the queue updates on next block to be current
            }
            else // if there is already a block on hold, then it swaps held block with the current block
            {
                Block tmp = CurrentBlock;
                CurrentBlock = HeldBlock;
                HeldBlock = tmp;
            }

            CanHold = false; // this will prevent players from spamming hold
        }

        // this method is where the current block rotates clockwise but only if
        // its possible to do so from where it is

        public void RotateBlockCW()
        {
            CurrentBlock.RotateCW();

            if (!BlockFits())
            {
                CurrentBlock.RotateCCW(); // if the piece lands in an illegal position, the piece will rotate back
            }
        }

        public void RotateBlockCCW()
        {
            CurrentBlock.RotateCCW();

            if (!BlockFits())
            {
                CurrentBlock.RotateCW();
            }
        }

        public void MoveBlockLeft()
        {
            CurrentBlock.Move(0, -1);

            if (!BlockFits())
            {
                CurrentBlock.Move(0, 1);
            }
        }

        public void MoveBlockRight()
        {
            CurrentBlock.Move(0, 1);

            if (!BlockFits())
            {
                CurrentBlock.Move(0, -1);
            }
        }

        private bool IsGameOver() // if either the hidden rows on the top are not empty, the game is over
        {
            return !(GameGrid.IsRowEmpty(0) && GameGrid.IsRowEmpty(1));
        }

        private void PlaceBlock()
        {
            foreach (Position p in CurrentBlock.TilePositions())
            {
                GameGrid[p.Row, p.Column] = CurrentBlock.Id;
            }

            Score += GameGrid.ClearFullRows(); // score is tracked by how many rows are cleared

            if (IsGameOver())
            {
                GameOver = true;
            }

            else
            {
                CurrentBlock = BlockQueue.GetAndUpdate();
                CanHold = true;
            }
        }

        public void MoveBlockDown() // this checks if the block that is placed can't be moved
        {
            CurrentBlock.Move(1, 0);

            if (!BlockFits())
            {
                CurrentBlock.Move(-1, 0);
                PlaceBlock();
            }
        }

        private int TileDropDistance(Position p) // with this method, players can find out where the piece can drop
        {
            int drop = 0;

            while (GameGrid.IsEmpty(p.Row + drop + 1, p.Column))
            {
                drop++;
            }

            return drop;
        }

        public int BlockDropDistance() // this invokes for every tile in current block and takes the minimum
        {
            int drop = GameGrid.Rows;

            foreach (Position p in CurrentBlock.TilePositions())
            {
                drop = System.Math.Min(drop, TileDropDistance(p));
            }

            return drop;
        }

        public void DropBlock() // this moves current block as many rows as possible, placing in grid
        {
            CurrentBlock.Move(BlockDropDistance(), 0);
            PlaceBlock();
        }
    }
}
