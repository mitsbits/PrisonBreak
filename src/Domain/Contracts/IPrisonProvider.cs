using System.Collections.Generic;

namespace PrionBreak.Domain
{
    public interface IPrisonProvider
    {
        IEnumerable<IPrison> Prisons();
    }
}