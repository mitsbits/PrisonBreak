using PrisonBreak.Domain;
using Shouldly;
using System;
using Xunit;

namespace PrisonBreak.Tests
{
    public class PrisonProviderExtensionsTests
    {
        private readonly Func<string[]> smallMazeLines = () => new[] { "C__", "__X", "E_X" };

        private readonly Func<string[]> mediumMazeLines = () => new[]   {
            "_X__X_X_X", "CXX______", "____XX__X", "XX__X_X__",
            "XXXXXEX__", "X______X_", "__XXXXX__", "________X"
        };

        private readonly Func<string[]> largeMazeLines = () => new[]
        {
            "X_XXXXXXXXX__", "__X___E______", "_XXXXXXXXXXX_",
            "____X___X____", "XXXXXXX______", "_____________", "_XXXX________",
            "X_CX__XXXXXX_", "____XXXXX____", "_XX________X_",
        };

        public PrisonProviderExtensionsTests()
        {
        }

        [Fact]
        public void check_that_lines_produce_correct_dimentions()
        {
            var smallMaze = smallMazeLines.Invoke().LoadMaze();
            smallMaze.GetLength(0).ShouldBe(3);
            smallMaze.GetLength(1).ShouldBe(3);

            var mediumMaze = mediumMazeLines.Invoke().LoadMaze();
            mediumMaze.GetLength(0).ShouldBe(9);
            mediumMaze.GetLength(1).ShouldBe(8);

            var largeMaze = largeMazeLines.Invoke().LoadMaze();
            largeMaze.GetLength(0).ShouldBe(13);
            largeMaze.GetLength(1).ShouldBe(10);
        }

        [Fact]
        public void empty_lines_maze_input_should_throw_argument_null_exception()
        {
            Should.Throw<ArgumentNullException>(() =>
            {
                string[] val = null;
                val.LoadMaze();
            });
            Should.Throw<ArgumentNullException>(() =>
            {
                var val = new string[0];
                val.LoadMaze();
            });

            Should.Throw<ArgumentNullException>(() =>
            {
                var val = new string[] { "", "", "", "" };
                val.LoadMaze();
            });

            Should.Throw<ArgumentNullException>(() =>
            {
                var val = new[] { "dfswdfsdf", ";odl.vnwe09034953", "sdvvdslm;ls-034t", "fdsafsdffffswf" };
                val.LoadMaze();
            });
        }
    }
}