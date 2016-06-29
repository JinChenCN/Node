using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace Node.Tests
{
    [TestClass]
    public class NodeDescriberTest
    {
        [TestMethod]
        public void DefaultTreeDescribe()
        {
            INodeDescriber nodedescriber = new NodeDescriber();
            var testdata = new SingleChildNode("root",
                new TwoChildrenNode("child1",
                    new NoChildrenNode("leaf1"),
                    new SingleChildNode("child2",
                        new NoChildrenNode("leaf2"))));
            var result = nodedescriber.Describe(testdata);

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
