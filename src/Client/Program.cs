using Autofac;
using Microsoft.Extensions.Configuration;
using PrionBreak.Domain;
using System;
using System.IO;
using System.Linq;

namespace Client
{
    public class Program
    {
        public static IConfigurationRoot Configuration;
        private static IContainer _container;

        public static void Main(string[] args)
        {
            WireUpDI(WireUpSettings());

            using (var scope = _container.BeginLifetimeScope())
            {
                IPrisonProvider provider = scope.Resolve<IPrisonProvider>();
                var prisons = provider.Prisons().ToArray();

                int indexer = SelectPrison(prisons);

                var prison = prisons[indexer];

                var robot = scope.Resolve<IRobot>();
                robot.RobotMoved += (r, e) => Console.WriteLine($"{r.GetType().Name} moved to {e.Block}\n");

                var result = robot.Escape(prison);
                foreach (var prisonBlock in result)
                {
                    Console.WriteLine(prisonBlock);
                }
            }
            Console.ReadLine();
        }

        private static int SelectPrison(IPrison[] prisons)
        {
            for (var i = 0; i < prisons.Length; i++)
            {
                var prison = prisons[i];
                Console.WriteLine($"Prison: {i+1}.");
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
    }
}