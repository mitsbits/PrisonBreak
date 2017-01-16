using Autofac;
using Microsoft.Extensions.Configuration;
using PrisonBreak.Domain;
using System;
using System.IO;
using System.Linq;

namespace Client
{
    public class Program
    {
        private static IConfigurationRoot Configuration;
        private static IContainer _container;

        public static void Main(string[] args)
        {
            WireUpDI(WireUpSettings());

            using (var scope = _container.BeginLifetimeScope())
            {
                Console.WriteLine("A list of prison maps follows.\nSelect a map please.");

                IPrisonProvider provider = scope.Resolve<IPrisonProvider>();
                var prisons = provider.Prisons().ToArray();

                int indexer = SelectPrison(prisons);

                var prison = prisons[indexer];

                Console.WriteLine("The robots will find the way out.\nPress any key to start.");

                Console.ReadLine();

                Console.WriteLine("Robots on the move...\n");

                var robot = scope.Resolve<IRobot>();
                robot.RobotMoved += (r, e) => Console.WriteLine($"{r.GetType().Name} moved to {e.Block}");

                try
                {
                    var result = robot.Escape(prison);

                    Console.WriteLine("\nThe solution is:\n");

                    foreach (var prisonBlock in result)
                    {
                        Console.WriteLine(prisonBlock);
                    }
                }
                catch (NoSolutionException e)
                {
                    Console.WriteLine(e);
                }
            }
            Console.ReadLine();
        }

        private static int SelectPrison(IPrison[] prisons)
        {
            for (var i = 0; i < prisons.Length; i++)
            {
                var prison = prisons[i];
                Console.WriteLine($"Prison: {i + 1}.\n");
                foreach (var line in prison.StringRepresentation())
                {
                    Console.WriteLine(line);
                }
                Console.WriteLine();
            }
            Console.WriteLine($"Please select a maze from 1 to {prisons.Length}");
            var input = Console.ReadLine();
            var selection = 1;
            int.TryParse(input, out selection);
            return selection - 1;
        }

        #region bootstrap

        private static void WireUpDI(ClientSettings settings)
        {
            var builder = new ContainerBuilder();

            builder.RegisterInstance(settings);

            builder.RegisterType<FolderPrisonProvider>()
                .As<IPrisonProvider>()
                .WithParameter("path", settings.GetPrisonsFolder())
                .InstancePerLifetimeScope();

            builder.RegisterType<MultiRobot>()
                .As<IRobot>()
                .WithParameter("robots",
                    new Robot[] { new DefaultRobot(), new ClockwiseRobot(), new CounterClockwiseRobot() })
                .InstancePerLifetimeScope();

            _container = builder.Build();
        }

        private static ClientSettings WireUpSettings()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            Configuration = builder.Build();

            var settings = new ClientSettings();
            Configuration.Bind(settings);
            return settings;
        }

        #endregion bootstrap
    }
}