using Microsoft.Extensions.PlatformAbstractions;
using System.IO;

namespace Client
{
    public class ClientSettings
    {
        public string PrisonsDirectory { get; set; }

        public string GetPrisonsFolder()
        {
            return string.IsNullOrWhiteSpace(PrisonsDirectory) || !Directory.Exists(PrisonsDirectory)
                ? Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "prisons")
                : PrisonsDirectory;
        }
    }
}