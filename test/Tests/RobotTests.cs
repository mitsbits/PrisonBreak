using PrisonBreak.Domain;
using Shouldly;
using Xunit;

namespace PrisonBreak.Tests
{
    public class RobotTests
    {
        private Robot _robot;

        [Fact]
        public void robot_raises_event_when_moving_to_block()
        {
            var raised = false;
            _robot = new DefaultRobot();
            _robot.RobotMoved += (r, e) => raised = true;
            IPrison prison = new Prison(Common.smallMazeLines.LoadMaze());
            _robot.Current = prison.Cell;
            raised.ShouldBeTrue();
            raised = false;
            _robot.TryMove(prison, Direction.East); //we know west is clear
            raised.ShouldBeTrue();
        }

        [Fact]
        public void a_robot_can_not_move_to_wall()
        {
            var raised = false;
            _robot = new DefaultRobot();
            _robot.RobotMoved += (r, e) => raised = true;
            IPrison prison = new Prison(Common.smallMazeLines.LoadMaze());
            _robot.Current = prison.Cell;
            raised.ShouldBeTrue();
            raised = false;
            var pos = PrisonBlock.Create(_robot.Current.X, _robot.Current.X, _robot.Current.Flavor);
            _robot.TryMove(prison, Direction.West); //we know east is wall
            raised.ShouldBeFalse();
            pos.ShouldBe(_robot.Current);
        }

        [Fact]
        public void a_robot_escapes_when_it_finds_an_exit()
        {
            var raised = false;
            _robot = new DefaultRobot();
            _robot.RobotMoved += (r, e) => raised = true;
            var cell = PrisonBlock.Create(1, 1, BlockType.Cell);
            _robot.Current = cell;
            raised.ShouldBeTrue();
            _robot.Escaped.ShouldBeFalse();
            raised = false;
            var exit = PrisonBlock.Create(1, 1, BlockType.Exit);
            _robot.Current = exit;
            raised.ShouldBeTrue();
            _robot.Escaped.ShouldBeTrue();
        }

        [Fact]
        public void a_robot_moves_to_escape_and_returns_path()
        {
            _robot = new DefaultRobot();
            IPrison prison = new Prison(Common.largeMazeLines.LoadMaze());
            var solution = _robot.Escape(prison);
            solution.Length.ShouldBeGreaterThan(1);
            solution[0].Flavor.ShouldBe(BlockType.Cell);
            for (var i = 1; i < solution.Length - 1; i++)
            {
                solution[i].Flavor.ShouldBe(BlockType.Clear);
            }
            solution[solution.Length - 1].Flavor.ShouldBe(BlockType.Exit);
            _robot.Escaped.ShouldBeTrue();
        }

        [Fact]
        public void a_robot_throws_when_there_is_no_escape()
        {
            _robot = new DefaultRobot();
            IPrison prison = new Prison(Common.noSolutionMazeLines.LoadMaze());
            Should.Throw<NoSolutionException>(() =>
            {
                var solution = _robot.Escape(prison);
            });
        }
    }
}