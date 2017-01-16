using System.Collections.Generic;

namespace PrisonBreak.Domain
{
    public interface IPrisonProvider
    {
        IEnumerable<IPrison> Prisons();
    }
}