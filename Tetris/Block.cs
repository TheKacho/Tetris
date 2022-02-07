using System.Collections.Generic;

namespace Tetris
{
    public abstract class Block
    {
        protected abstract Position[][] Tiles { get; } // start offset decides where the block spawns in the grid
        protected abstract Position StartOffset { get; } // this also decides where the block spawns

        public  abstract int Id { get; }

        // this will store current rotation state and current offset
        private int rotationState;
        private Position offset;

        public Block() // cpnstructor taht sets offset equal to start offset
        {
            offset = new Position(StartOffset.Row, StartOffset.Column); // factoring in current roation and offset
        }

        public IEnumerable<Position> TilePositions() // method loops over tile positions in current rotation state
        {
            foreach (Position p in Tiles[rotationState])
            {
                yield return new Position(p.Row +offset.Row, p.Column + offset.Column); //this adds the row offset and column offset

            }
        }

        public void RotateCW() // rotates the piece 90 degrees/clockwise
        {
            rotationState = (rotationState + 1) % Tiles.Length;
        }
        public void RotateCCW() // this rotates it counterclockwise
        {
            if (rotationState == 0)
            {
                rotationState = Tiles.Length - 1;
            }
            else
            {
                rotationState--;
            }
        }
    }
}
