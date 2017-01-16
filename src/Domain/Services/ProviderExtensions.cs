using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrisonBreak.Domain
{
    internal static class ProviderExtensions
    {
        private static readonly Func<char, BlockType> _mapState = (c) =>
        {
            BlockType result;
            switch (c)
            {
                case 'X': result = BlockType.Wall; break;
                case '_': result = BlockType.Clear; break;
                case 'C': result = BlockType.Cell; break;
                case 'E': result = BlockType.Exit; break;
                default: throw new ArgumentOutOfRangeException(nameof(c));
            }
            return result;
        };

        private static readonly Func<BlockType, char> _displayState = (s) =>
        {
            var result = '\0';
            switch (s)
            {
                case BlockType.Cell: result = 'C'; break;
                case BlockType.Wall: result = 'X'; break;
                case BlockType.Clear: result = '_'; break;
                case BlockType.Exit: result = 'E'; break;
                default: throw new ArgumentOutOfRangeException(nameof(s));
            }
            return result;
        };

        internal static int[,] LoadMaze(this string[] lines)
        {
            if (lines == null || lines.All(x => string.IsNullOrWhiteSpace(x.Trim()))) throw new ArgumentNullException(nameof(lines));
            var allowedChars = new[] { 'X', '_', 'C', 'E' };
            var sanitizedLines = new List<string>();
            foreach (var line in lines)
            {
                var sanitized = line;

                foreach (var c in line)
                {
                    if (!allowedChars.Contains(c)) sanitized = sanitized.Replace(c.ToString(), string.Empty);
                }
                sanitizedLines.Add(sanitized);
            }
            if (sanitizedLines == null || sanitizedLines.All(x => string.IsNullOrWhiteSpace(x.Trim()))) throw new ArgumentNullException(nameof(lines));



            var hLength = sanitizedLines.First().Length;
            var vLength = sanitizedLines.Count;

            var maze = new int[hLength, vLength];
            for (int x = 0; x < hLength; x++)
            {
                for (int y = 0; y < vLength; y++)
                {
                    maze[x, y] = (int)_mapState(sanitizedLines[y][x]);
                }
            }
            return maze;
        }


        internal static IEnumerable<string> Display(this int[,] maze, int verticalLength, int horizontalLenght)
        {
            for (int y = 0; y < verticalLength; y++)
            {
                var sb = new StringBuilder();
                for (int x = 0; x < horizontalLenght; x++)
                {
                    sb.Append(_displayState((BlockType)maze[x, y]));
                }
                yield return sb.ToString();
            }
        }
    }
}