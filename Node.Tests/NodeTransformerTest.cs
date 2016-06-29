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
            var testData = new ManyChildrenNode("root",
                                new ManyChildrenNode("child1",
                                    new ManyChildrenNode("leaf1"),
                                    new ManyChildrenNode("child2",
                                        new ManyChildrenNode("leaf2"))));
            var result = nodeTransformer.Transform(testData);

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
