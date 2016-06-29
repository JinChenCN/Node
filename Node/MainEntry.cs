using Autofac;
using System;
using System.IO;

namespace Node
{
    class MainEntry
    {
        private static IContainer Container { get; set; }
        public static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<NodeTransformer>().As<INodeTransformer>();
            builder.RegisterType<NodeDescriber>().As<INodeDescriber>();
            builder.RegisterType<NodeWriter>().As<INodeWriter>();
            Container = builder.Build();
            var filePath = args[0];
            Test(filePath);
        }

        private static async void Test(string filePath)
        {
            using(var scope = Container.BeginLifetimeScope())
            {
                var testData = new ManyChildrenNode("root",
                    new ManyChildrenNode("child1",
                        new ManyChildrenNode("leaf1"),
                        new ManyChildrenNode("child2",
                            new ManyChildrenNode("leaf2"))));
                var nodeWriter = scope.Resolve<INodeWriter>();
                await nodeWriter.WriteToFileAsync(testData, filePath);
                var result = File.ReadAllText(filePath);
                Console.WriteLine(result);
                Console.ReadKey();
            }
        }
    }
}
