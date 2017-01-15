using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrionBreak.Domain
{
    public class DefaultRobot : Robot
    {
        public DefaultRobot() : base(null)
        {
        }
    }

    public class ClockwiseRobot : Robot
    {
        public ClockwiseRobot() : base(new[] { Direction.East, Direction.South, Direction.West, Direction.North })
        {
        }
    }

    public class CounterClockwiseRobot : Robot
    {
        public CounterClockwiseRobot() : base(new[] { Direction.West, Direction.South, Direction.East, Direction.West })
        {
        }
    }

    public class MultiRobot : IRobot
    {
        private readonly IEnumerable<Robot> _internals;

        public MultiRobot(IEnumerable<Robot> robots)
        {
            if (robots == null || !robots.Any()) throw new ArgumentNullException(nameof(robots));
            _internals = robots;
            foreach (var robot in _internals)
            {
                robot.RobotMoved += OnRobotMoved;
            }
        }

        public event RobotMovedEventHandler RobotMoved;

        public PrisonBlock[] Escape(IPrison prison)
        {
            var result = new List<PrisonBlock[]>();
            Parallel.ForEach(_internals, new ParallelOptions() { MaxDegreeOfParallelism = 16 }, (robot) =>
              {
                  try
                  {
                      result.Add(robot.Escape(prison));
                  }
                  catch (Exception ex) when (ex is NoSolutionException)
                  {
                      //TODO:log
                  }
              });

            if (!result.Any()) throw new NoSolutionException(prison);
            return result.OrderBy(x => x.Length).FirstOrDefault();
        }

        protected virtual void OnRobotMoved(object sender, RobotMovedEventArgs e)
        {
            RobotMovedEventHandler handler = RobotMoved;
            handler?.Invoke(sender, e);
        }
    }
}