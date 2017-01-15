namespace PrionBreak.Domain
{
    public struct PrisonBlock
    {
        private PrisonBlock(int x, int y, BlockType flavor)
        {
            X = x;
            Y = y;
            Flavor = flavor;
        }

        public int X { get; }
        public int Y { get; }
        public BlockType Flavor { get; }

        internal static PrisonBlock Empty()
        {
            return new PrisonBlock(-1, -1, BlockType.Wall);
        }

        internal static PrisonBlock Create(int x, int y, BlockType type = BlockType.Wall)
        {
            return new PrisonBlock(x, y, type);
        }

        public override string ToString()
        {
            return $"[{X},{Y}]:{Flavor}";
        }
    }
}