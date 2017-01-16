using System;
using System.Collections.Generic;
using System.Linq;

namespace PrisonBreak.Domain
{
    public delegate void RobotMovedEventHandler(object sender, RobotMovedEventArgs e);

    public class RobotMovedEventArgs : EventArgs
    {
        public RobotMovedEventArgs(PrisonBlock block)
        {
            Block = block;
        }

        public PrisonBlock Block { get; }
    }

    public abstract class Robot : IRobot
    {
        private readonly ICollection<PrisonBlock> _visited = new HashSet<PrisonBlock>();
        private readonly Stack<Direction> _moves = new Stack<Direction>();
        private readonly List<PrisonBlock> _solution = new List<PrisonBlock>();
        private readonly Direction[] _rotation;

        private bool _retreat;

        protected Robot(Direction[] rotation)
        {
            _rotation = Guard(rotation);
            _current = PrisonBlock.Empty();
        }

        #region IRobot

        public event RobotMovedEventHandler RobotMoved;

        public virtual PrisonBlock[] Escape(IPrison prison)
        {
            Current = prison.Cell;
            while (!Escaped)
            {
                if (IsDeadEnd(prison))
                {
                    _solution.Remove(Current);
                    if (_moves.Count == 0) throw new NoSolutionException(prison);
                    var stepBack = _moves.Pop();
                    TryMove(prison, UTurn(stepBack));
                }
            }
            return _solution.ToArray();
        }

        #endregion IRobot

        protected virtual void OnRobotMoved(RobotMovedEventArgs e)
        {
            RobotMovedEventHandler handler = RobotMoved;
            handler?.Invoke(this, e);
        }

        private PrisonBlock _current;

        internal PrisonBlock Current
        {
            get { return _current; }
            set
            {
                if (value.Flavor != BlockType.Wall)
                {
                    if (!_retreat) { _visited.Add(value); _solution.Add(value); }
                    _current = value;
                    OnRobotMoved(new RobotMovedEventArgs(_current));
                }
            }
        }

        internal bool Escaped => Current.Flavor == BlockType.Exit;

        internal bool TryMove(IPrison prison, Direction direction)
        {
            int destX;
            int destY;

            destX = (direction == Direction.East)
                ? Current.X + 1 : (direction == Direction.West)
                ? Current.X - 1 : Current.X;
            destY = (direction == Direction.South)
                ? Current.Y + 1 : (direction == Direction.North)
                ? Current.Y - 1 : Current.Y;

            if (_retreat)
            {
                return TryGoBack(prison, destX, destY);
            }
            if (!_visited.Any(b => b.X == destX && b.Y == destY))
            {
                return TryGoForward(prison, direction, destX, destY);
            }
            return false;
        }

        private bool IsDeadEnd(IPrison prison)
        {
            var result = !TryMove(prison, _rotation[0]) && !TryMove(prison, _rotation[1]) &&
                         !TryMove(prison, _rotation[2]) && !TryMove(prison, _rotation[3]);
            _retreat = result;
            return result;
        }

        private static Direction UTurn(Direction direction)
        {
            Direction result;
            switch (direction)
            {
                case Direction.East: result = Direction.West; break;
                case Direction.West: result = Direction.East; break;
                case Direction.North: result = Direction.South; break;
                case Direction.South: result = Direction.North; break;
                default: throw new ArgumentOutOfRangeException(nameof(direction));
            }
            return result;
        }

        private bool TryGoForward(IPrison prison, Direction direction, int destX, int destY)
        {
            var block = prison.Discover(destX, destY);
            if (block.Flavor == BlockType.Wall) return false;
            _moves.Push(direction);
            Current = block;
            return true;
        }

        private bool TryGoBack(IPrison prison, int destX, int destY)
        {
            var block = prison.Discover(destX, destY);
            if (block.Flavor == BlockType.Wall) return false;
            Current = block;
            _retreat = false;
            return true;
        }

        private static Direction[] Guard(Direction[] rotation)
        {
            if (rotation == null || !rotation.Any() || rotation.Distinct().Count() != 4)
                rotation = Enum.GetValues(typeof(Direction)).Cast<Direction>().ToArray();
            return rotation;
        }
    }
}