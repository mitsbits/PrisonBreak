using PrisonBreak.Domain;
using Shouldly;
using System;
using System.Linq;
using Xunit;

namespace PrisonBreak.Tests
{
    public class PrisonProviderTests
    {
        //this folder should exist and containing text files with mazes :)
        private const string ValidProviderFolder = @"K:\Users\mitsbits\Source\Repos\PrisonBreak\src\Client\prisons";

        [Theory]
        [InlineData("")]
        [InlineData(@"!&%\")]
        [InlineData(@"Y:\thispathdoesnotexists\")]
        public void providers_should_throw_on_invalid_path(string path)
        {
            Should.Throw<ArgumentException>(() =>
            {
                var provider = new FolderPrisonProvider(path);
            });
        }

        [Fact]
        public void provider_should_provide_prisons()
        {
            IPrisonProvider provider = new FolderPrisonProvider(ValidProviderFolder);
            provider.Prisons().Count().ShouldBeGreaterThan(0);
            provider.Prisons().First().Cell.Flavor.ShouldBe(BlockType.Cell);
        }
    }

    public class PrisonProviderExtensionsTests
    {
        [Fact]
        public void string_is_correctly_translated_to_prison_block_type()
        {
            var maze = Common.mazeSymbols.LoadMaze();
            maze[0, 0].ShouldBe((int)BlockType.Wall);
            maze[1, 0].ShouldBe((int)BlockType.Clear);
            maze[0, 1].ShouldBe((int)BlockType.Cell);
            maze[1, 1].ShouldBe((int)BlockType.Exit);
        }

        [Fact]
        public void check_that_lines_produce_correct_dimention_sizes()
        {
            var smallMaze = Common.smallMazeLines.LoadMaze();
            smallMaze.GetLength(0).ShouldBe(3);
            smallMaze.GetLength(1).ShouldBe(3);

            var mediumMaze = Common.mediumMazeLines.LoadMaze();
            mediumMaze.GetLength(0).ShouldBe(9);
            mediumMaze.GetLength(1).ShouldBe(8);

            var largeMaze = Common.largeMazeLines.LoadMaze();
            largeMaze.GetLength(0).ShouldBe(13);
            largeMaze.GetLength(1).ShouldBe(10);
        }

        [Fact]
        public void empty_lines_from_maze_input_should_throw_argument_null_exception()
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
                var val = new[] { "", "", "", "" };
                val.LoadMaze();
            });

            Should.Throw<ArgumentNullException>(() =>
            {
                var val = new[] { "dfswdfsdf", ";odl.vnwe09034953", "sd!@$^5664dslm;ls-034t", "fdsafsdffffswf" };
                val.LoadMaze();
            });

            Should.Throw<ArgumentNullException>(() =>
            {
                var val = new[] { "x-", "0!" };
                val.LoadMaze();
            });
        }

        [Fact]
        public void create_a_maze_from_lines_and_check_that_dispaly_is_in_sync_with_source()
        {
            IPrison prison = new Prison(Common.smallMazeLines.LoadMaze());
            var outcome = prison.StringRepresentation();
            outcome.Count().ShouldBe(Common.smallMazeLines.Length);
            for (int i = 0; i < Common.smallMazeLines.Length - 1; i++)
            {
                outcome.ToArray()[i].ShouldBe(Common.smallMazeLines[i]);
            }
        }
    }
}