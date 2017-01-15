using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PrionBreak.Domain
{
    internal class FilePrisonProvider : IPrisonProvider
    {
        private readonly IPrison _prison;

        public FilePrisonProvider(string path)
        {
            if (!File.Exists(path)) throw new ArgumentException(path);
            var lines = File.ReadAllLines(path);
            _prison = new Prison(lines.ToArray().LoadMaze());
        }

        public IEnumerable<IPrison> Prisons()
        {
            return new[] { _prison };
        }
    }

    public class FolderPrisonProvider : IPrisonProvider
    {
        private readonly List<IPrison> _prisons;

        public FolderPrisonProvider(string path)
        {
            if (!Directory.Exists(path)) throw new ArgumentException(path);
            var files = Directory.GetFiles(path, "*.txt");
            if (!files.Any()) throw new ArgumentException(path);
            _prisons = new List<IPrison>();
            foreach (var file in files)
            {
                var lines = File.ReadAllLines(file);
                _prisons.Add(new Prison(lines.ToArray().LoadMaze()));
            }
        }

        public IEnumerable<IPrison> Prisons()
        {
            return _prisons;
        }
    }

    internal static class ProviderExtensions
    {
        private static Func<char, BlockType> _map = (c) =>
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

        public static int[,] LoadMaze(this string[] lines)
        {
            if (lines == null || lines.All(x => string.IsNullOrWhiteSpace(x.Trim()))) throw new ArgumentNullException(nameof(lines));
            var hLength = lines.First().Length;
            var vLength = lines.Length;

            var maze = new int[hLength, vLength];
            for (int x = 0; x < hLength; x++)
            {
                for (int y = 0; y < vLength; y++)
                {
                    maze[x, y] = (int)_map(lines[y][x]);
                }
            }
            return maze;
        }
    }
}