using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.CodeDom.Compiler;

namespace Node.Tests
{
    [TestClass]
    public class NodeTest
    {
        [TestMethod]
        public void TestDescribe()
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
            indentwriter.Indent = 0;
            WriteLevel(indentwriter, 0, 3);
            Assert.AreEqual(basetextwriter.ToString(), result);
        }

        #region helper
        private void WriteLevel(IndentedTextWriter indentWriter, int level, int totalLevels)
        {
            switch (level)
            {
                case 0:
                    indentWriter.WriteLine(@"new SingleChildNode(""root"",");
                    break;
                case 1:
                    indentWriter.WriteLine(@"new TwoChildrenNode(""child1"",");
                    break;
                case 2:
                    indentWriter.WriteLine(@"new NoChildrenNode(""leaf1""),");
                    indentWriter.WriteLine(@"new SingleChildNode(""child2"",");

                    break;
                case 3:
                    indentWriter.WriteLine(@"new NoChildrenNode(""leaf2""))))");
                    break;
            }

            // If not yet at the highest recursion level, call this output method for the next level of indentation.
            if (level < totalLevels)
            {
                // Increase the indentation count for the next level of indented output.
                indentWriter.Indent++;

                // Call the WriteLevel method to write test output for the next level of indentation.
                WriteLevel(indentWriter, level + 1, totalLevels);

                // Restores the indentation count for this level after the recursive branch method has returned.
                indentWriter.Indent--;
            }

        }
        #endregion


    }
}
