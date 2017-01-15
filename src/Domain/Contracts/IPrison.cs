using System.Collections.Generic;

namespace PrionBreak.Domain
{
    public interface IPrison
    {
        PrisonBlock Discover(int x, int y);

        PrisonBlock Cell { get; }

        IEnumerable<string> StringRepresentation();
    }
}