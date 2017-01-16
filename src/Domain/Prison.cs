using System;
using System.Collections.Generic;
using System.Linq;

namespace PrisonBreak.Domain
{
    public class Prison : IPrison
    {
        private readonly int[,] _maze;
        private readonly int _horizontalLenght;
        private readonly int _verticalLenght;

        public Prison(int[,] maze)
        {
            if (maze == null) throw new ArgumentNullException(nameof(maze));
            _maze = maze;
            _horizontalLenght = _maze.GetLength(0);
            _verticalLenght = _maze.GetLength(1);
        }

        #region IPrison

        public PrisonBlock Discover(int x, int y)
        {
            return OutOfRange(x, y) ? PrisonBlock.Empty() : Block(x, y);
        }

        public PrisonBlock Cell
        {
            get
            {
                var hit = Enumerable.Range(0, _horizontalLenght)
                 .SelectMany(v => Enumerable.Range(0, _verticalLenght)
                 .Select(h => new { v, h, t = _maze[v, h] })).Single(b => b.t == (int)BlockType.Cell);
                return PrisonBlock.Create(hit.v, hit.h, BlockType.Cell);
            }
        }

        public IEnumerable<string> StringRepresentation()
        {
            return _maze.Display(_verticalLenght, _horizontalLenght);
        }

        #endregion IPrison

        private bool OutOfRange(int x, int y)
        {
            return (x < 0 || x > _horizontalLenght - 1) || (y < 0 || y > _verticalLenght - 1);
        }

        private PrisonBlock Block(int x, int y)
        {
            var hit = Enumerable.Range(0, _horizontalLenght)
                  .SelectMany(v => Enumerable.Range(0, _verticalLenght)
                  .Select(h => new { v, h, t = _maze[v, h] })).SingleOrDefault(b => b.v == x && b.h == y);
            return (hit != null) ? PrisonBlock.Create(hit.v, hit.h, (BlockType)hit.t) : PrisonBlock.Empty();
        }
    }
}