using Microsoft.VisualStudio.TestTools.UnitTesting;
using Autofac;

namespace Node.Tests
{
    [TestClass]
    public class NodeTransformerTest
    {
        private static IContainer Container { get; set; }
        [TestMethod]
        public void ManyChildrenNodeTransform()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<NodeTransformer>().As<INodeTransformer>();
            builder.RegisterType<NodeDescriber>().As<INodeDescriber>();
            Container = builder.Build();

            using (var scope = Container.BeginLifetimeScope())
            {
                var nodeDescriber = scope.Resolve<INodeDescriber>();
                var nodeTransformer = scope.Resolve<INodeTransformer>();
                var testData = new ManyChildrenNode("root",
                                    new ManyChildrenNode("child1",
                                        new ManyChildrenNode("leaf1"),
                                        new ManyChildrenNode("child2",
                                            new ManyChildrenNode("leaf2"))));
                var result = nodeTransformer.Transform(testData);

                var expected = new SingleChildNode("root",
                                    new TwoChildrenNode("child1",
                                        new NoChildrenNode("leaf1"),
                                        new SingleChildNode("child2",
                                            new NoChildrenNode("leaf2"))));

                Assert.AreEqual(nodeDescriber.Describe(expected), nodeDescriber.Describe(result));
            }
        }
    }
}
