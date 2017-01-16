using System;
using System.Text;

namespace PrisonBreak.Domain
{
    public class NoSolutionException : Exception
    {
        public NoSolutionException(IPrison prison) : base(SetExceptionMessage(prison)) { }

        private static string SetExceptionMessage(IPrison prison)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Prison Exit can not be reached!");
            foreach (string line in prison.StringRepresentation())
            {
                sb.AppendLine(line);
            }
            return sb.ToString();
        }
    }
}