using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PrisonBreak.Domain
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
}