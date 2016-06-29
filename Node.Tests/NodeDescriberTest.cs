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
        public void ThreeLevelDescribe()
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

            WriteLevel(indentwriter, outPutDic);
            Assert.AreEqual(basetextwriter.ToString(), result);
        }

        #region helper
        private void WriteLevel(IndentedTextWriter indentWriter, Dictionary<int, List<string>> outPutDic)
        {
            for (var i = 0; i<outPutDic.Count; i++)
            {
                List<string> outPutList = outPutDic[i];
                if (i != outPutDic.Count - 1)
                {
                    foreach(string s in outPutList)
                    {
                        indentWriter.WriteLine(s);
                    }
                }
                else
                {
                    for (var j = 0; j < outPutList.Count; j++)
                    {
                        if(j != outPutList.Count - 1)
                        {
                            indentWriter.WriteLine(outPutList[j]);
                        } else
                        {
                            indentWriter.Write(outPutList[j]);
                        }
                    }
                }

                indentWriter.Indent++;
            }
        }
        #endregion


    }
}
