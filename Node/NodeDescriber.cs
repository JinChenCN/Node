using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;

namespace Node
{
    public class NodeDescriber : INodeDescriber
    {
        INodeTransformer _nodeTransformer;

        public NodeDescriber (INodeTransformer nodeTransformer)
        {
            this._nodeTransformer = nodeTransformer;
        }
        public string Describe(Node node)
        {
            System.IO.StringWriter nodeWriter = new System.IO.StringWriter();
            IndentedTextWriter indentWriter = new IndentedTextWriter(nodeWriter, "    ");
            indentWriter.Indent = 0;
            node = _nodeTransformer.Transform(node);
            WriteNode(indentWriter, node, node.GetType() == typeof(NoChildrenNode)? true : false);
            return nodeWriter.ToString();
        }

        private void WriteNode (IndentedTextWriter indentWriter, Node node, bool isEnd)
        {
            Type nodeType = node.GetType();
            var typePartial = (nodeType.ToString().Split('.'))[1];
            if (nodeType == typeof(NoChildrenNode))
            {   
                if(isEnd)
                {
                    indentWriter.Write(@"new {0}(""{1}"")", typePartial, node.Name);
                } else
                {
                    indentWriter.WriteLine(@"new {0}(""{1}""),", typePartial, node.Name);
                }

            } else
            {
                indentWriter.WriteLine(@"new {0}(""{1}"",", typePartial, node.Name);
                indentWriter.Indent++;
                var @switch = new Dictionary<Type, Action>
                {
                    { typeof(SingleChildNode), () => WriteSingleChildNode(indentWriter, node) },
                    { typeof(TwoChildrenNode), () => WriteTwoChildrenNode(indentWriter, node) },
                    { typeof(ManyChildrenNode), () => WriteManyChildrenNode(indentWriter, node) },
                };
                @switch[nodeType]();
                indentWriter.Write(")");
            }

        }

        private void WriteSingleChildNode (IndentedTextWriter indentWriter, Node node)
        {
            Node childNode = ((SingleChildNode)node).Child;
            WriteNode(indentWriter, childNode, childNode.GetType() == typeof(NoChildrenNode) ? true : false);
        }

        private void WriteTwoChildrenNode(IndentedTextWriter indentWriter, Node node)
        {
            Node firstChild = ((TwoChildrenNode)node).FirstChild;
            Node secondChild = ((TwoChildrenNode)node).SecondChild;
            bool isFirstChildEnd = firstChild.GetType() == typeof(NoChildrenNode) ? true : false;
            bool isSecondChildEnd = secondChild.GetType() == typeof(NoChildrenNode)? true : false;
            WriteNode(indentWriter, firstChild, false);
            WriteNode(indentWriter, secondChild, isFirstChildEnd && isSecondChildEnd? true : false);
        }

        private void WriteManyChildrenNode(IndentedTextWriter indentWriter, Node node)
        {
            IEnumerable<Node> children = ((ManyChildrenNode)node).Children;
            var childrenList = children.ToList();
            bool isEnd = true;
            for (int i = 0; i < childrenList.Count; i++)
            {
                if (childrenList[i].GetType() != typeof(NoChildrenNode))
                    isEnd = false;
                
                if (i == children.ToList().Count - 1)
                {
                    WriteNode(indentWriter, childrenList[i], isEnd);
                    break;
                }
                WriteNode(indentWriter, childrenList[i], false);
            }
        }
    }
}