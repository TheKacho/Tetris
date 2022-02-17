using System;


namespace Tetris
{
    public class BlockQueue
    {
        private readonly Block[] blocks = new Block[]
        {
            new IBlock(),
            new JBlock(),
            new LBlock(),
            new OBlock(),
            new SBlock(),
            new TBlock(),
            new ZBlock()
        };

        private readonly Random random = new Random(); // queues up the next block at random

        public Block NextBlock { get; private set; } // with the UI, this will preview the next piece

        public BlockQueue() // blockqueue constructor initialized  next block with a random block
        {
            NextBlock = RandomBlock();
        }

        private Block RandomBlock()
        {
            return blocks[random.Next(blocks.Length)];
        }

        // this method returns next block and updates property

        public Block GetAndUpdate() // this will loop until it picks a new piece
        {
            Block block = NextBlock;

            do
            {
                NextBlock = RandomBlock();
            }
            while (block.Id == NextBlock.Id);

            return block;
        }
    }
}
