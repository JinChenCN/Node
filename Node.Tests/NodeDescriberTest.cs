using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using Autofac;

namespace Node.Tests
{
    [TestClass]
    public class NodeDescriberTest
    {
        private static IContainer Container { get; set; }
        [TestMethod]
        public void DefaultTreeDescribe()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<NodeTransformer>().As<INodeTransformer>();
            builder.RegisterType<NodeDescriber>().As<INodeDescriber>();
            Container = builder.Build();

            using (var scope = Container.BeginLifetimeScope())
            {
                var nodeDescriber = scope.Resolve<INodeDescriber>();
                var nodeTransformer = scope.Resolve<INodeTransformer>();
                var testdata = new SingleChildNode("root",
                new TwoChildrenNode("child1",
                    new NoChildrenNode("leaf1"),
                    new SingleChildNode("child2",
                        new NoChildrenNode("leaf2"))));
                var result = nodeDescriber.Describe(testdata);

                System.IO.StringWriter basetextwriter = new System.IO.StringWriter();
                IndentedTextWriter indentwriter = new IndentedTextWriter(basetextwriter, "    ");

                Dictionary<int, List<string>> outPutDic = new Dictionary<int, List<string>>();
                outPutDic.Add(0, new List<string>(new string[] { @"new SingleChildNode(""root""," }));
                outPutDic.Add(1, new List<string>(new string[] { @"new TwoChildrenNode(""child1""," }));
                outPutDic.Add(2, new List<string>(new string[] { @"new NoChildrenNode(""leaf1""),", @"new SingleChildNode(""child2""," }));
                outPutDic.Add(3, new List<string>(new string[] { @"new NoChildrenNode(""leaf2""))))" }));

                Utility.WriteLevel(indentwriter, outPutDic);
                Assert.AreEqual(basetextwriter.ToString(), result);
            }
        }
    }
}
