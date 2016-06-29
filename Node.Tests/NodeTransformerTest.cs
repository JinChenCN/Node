using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace Node.Tests
{
    [TestClass]
    public class NodeTransformerTest
    {
        [TestMethod]
        public void ManyChildrenNodeTransform()
        {
            INodeTransformer nodeTransformer = new NodeTransformer();
            INodeDescriber nodeDescriber = new NodeDescriber();
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
