using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node
{
    class MainEntry
    {
        public static void Main()
        {
            INodeDescriber nodeDescriber = new NodeDescriber();
            var testData = new SingleChildNode("root",
                            new TwoChildrenNode("child1",
                            new NoChildrenNode("leaf1"),
                            new SingleChildNode("child2",
                                new NoChildrenNode("leaf2"))));

            var result = nodeDescriber.Describe(testData);
            Console.Write(result);
        }
    }
}
