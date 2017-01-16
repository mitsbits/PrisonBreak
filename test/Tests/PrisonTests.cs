using PrisonBreak.Domain;
using Shouldly;
using System.Linq;
using Xunit;

namespace PrisonBreak.Tests
{
    public class PrisonTests
    {
        private IPrison _prison = null;

        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 0)]
        [InlineData(2, 2)]
        [InlineData(3, 2)]
        public void a_prison_should_allow_block_discovery(int x, int y)
        {
            _prison = new Prison(Common.mediumMazeLines.LoadMaze());
            var block = _prison.Discover(x, y);
            block.ShouldNotBeNull();
            block.X.ShouldBeGreaterThanOrEqualTo(0);
            block.Y.ShouldBeGreaterThanOrEqualTo(0);
            block.Flavor.ShouldBeOfType(typeof(BlockType));
        }

        [Theory]
        [InlineData(-15, -800)]
        [InlineData(99, 99)]
        [InlineData(42, -42)]
        [InlineData(741852, 369852)]
        public void a_prison_should_return_wall_when_parameters_are_out_of_range_on_block_discovery(int x, int y)
        {
            _prison = new Prison(Common.mediumMazeLines.LoadMaze());
            var block = _prison.Discover(x, y);
            block.ShouldNotBeNull();
            block.X.ShouldBe(-1);
            block.Y.ShouldBe(-1);
            block.Flavor.ShouldBe(BlockType.Wall);
        }

        [Fact]
        public void a_prison_should_contain_a_cell()
        {
            _prison = new Prison(Common.mediumMazeLines.LoadMaze());
            var block = _prison.Cell;
            block.X.ShouldBeGreaterThanOrEqualTo(0);
            block.Y.ShouldBeGreaterThanOrEqualTo(0);
            block.Flavor.ShouldBe(BlockType.Cell);
        }

        [Fact]
        public void a_prison_should_return_a_string_representation_of_the_maze()
        {
            _prison = new Prison(Common.mediumMazeLines.LoadMaze());
            _prison.StringRepresentation().Count().ShouldBeGreaterThan(0);
            foreach (var line in _prison.StringRepresentation())
            {
                line.Length.ShouldBeGreaterThan(0);
            }
        }
    }
}