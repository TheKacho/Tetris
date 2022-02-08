﻿namespace Tetris
{
    public class IBlock : Block // this is for the long tetris piece (the I block)
    {
        private readonly Position[][] tiles = new Position[][]
        {
            new Position[] { new(1,0), new(1, 1), new(1, 2), new(1, 3) }, // state 0
            new Position[] { new(0, 2), new(1, 2), new(2, 2), new(3, 2) }, // state 1
            new Position[] { new(2,0), new(2, 1), new(2, 2), new(2, 3) }, // state 2
            new Position[] { new(9, 1), new(1, 1), new(2, 1), new(3, 1) } // state 3
        };

        public override int Id => 1;


        // this will spawn in the middle of the top row
        protected override Position StartOffset => new Position(-1, 3);

        protected override Position[][] Tiles => tiles;
    }
}
