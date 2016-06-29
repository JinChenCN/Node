using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node
{
    public class NodeTransformer : INodeTransformer
    {
        public string Transform(Node node)
        {
            System.IO.StringWriter nodeWriter = new System.IO.StringWriter();
            IndentedTextWriter indentWriter = new IndentedTextWriter(nodeWriter, "    ");
            indentWriter.Indent = 0;
            var childrenNum = GetChildrenNum(node);
            if (childrenNum == 0)
            {
                WriteNode(indentWriter, new NoChildrenNode(node.Name), true);
            } else
            {
                WriteNode(indentWriter, node, false);
            }
            return nodeWriter.ToString();
        }

        private int GetChildrenNum(Node node)
        {
            var childrenNum = 0;
            var nodeType = node.GetType();
            var @switch = new Dictionary<Type, Action>
                {
                    { typeof(SingleChildNode), () => childrenNum = ((SingleChildNode)node).Child != null? 1 : 0 },
                    { typeof(TwoChildrenNode), () => childrenNum = GetChildrenNumOfTwoChildrenNode((TwoChildrenNode)node) },
                    { typeof(ManyChildrenNode), () => childrenNum = GetChildrenNumOfManyChildrenNode((ManyChildrenNode)node) },
                };
            @switch[nodeType]();
            return childrenNum;
        }

        private int GetChildrenNumOfTwoChildrenNode(TwoChildrenNode node)
        {
            var childrenNum = 2;
            Node firstChild = node.FirstChild;
            Node secondChild = node.SecondChild;
            if (firstChild == null)
            {
                childrenNum--;
            }
            if (secondChild == null)
            {
                childrenNum--;
            }
            return childrenNum;
        }

        private int GetChildrenNumOfManyChildrenNode(ManyChildrenNode node)
        {
            var children = node.Children;
            if (children == null)
            {
                return 0;
            }
            return children.ToList().Count;
        }

        private void WriteNode(IndentedTextWriter indentWriter, Node node, bool isEnd)
        {
            Type nodeType = node.GetType();
            var typePartial = (nodeType.ToString().Split('.'))[1];
            if (nodeType == typeof(NoChildrenNode))
            {
                if (isEnd)
                {
                    indentWriter.Write(@"new {0}(""{1}"")", typePartial, node.Name);
                }
                else
                {
                    indentWriter.WriteLine(@"new {0}(""{1}""),", typePartial, node.Name);
                }

            }
            else
            {
                if (nodeType == typeof(TwoChildrenNode))
                {
                    if (GetChildrenNum(node) == 2)
                    {
                        indentWriter.WriteLine(@"new {0}(""{1}"",", typePartial, node.Name);
                        indentWriter.Indent++;
                        WriteTwoChildrenNode(indentWriter, node);
                        indentWriter.Write(")");
                        return;
                    } else if (((TwoChildrenNode)node).FirstChild == null)
                    {
                        node = new SingleChildNode(node.Name, ((TwoChildrenNode)node).SecondChild);
                    } else
                    {
                        node = new SingleChildNode(node.Name, ((TwoChildrenNode)node).FirstChild);
                    }
                }

                if (nodeType == typeof(ManyChildrenNode))
                {
                    var children = ((ManyChildrenNode)node).Children;
                    if (GetChildrenNum(node) > 2)
                    {
                        indentWriter.WriteLine(@"new {0}(""{1}"",", typePartial, node.Name);
                        indentWriter.Indent++;
                        WriteTwoChildrenNode(indentWriter, node);
                        indentWriter.Write(")");
                        return;
                    }else if(GetChildrenNum(node) == 2)
                    {
                        node = new TwoChildrenNode(node.Name, children.ToList()[0], children.ToList()[1]);
                    }
                    else if (GetChildrenNum(node) == 1)
                    {
                        node = new SingleChildNode(node.Name, children.ToList()[0]);
                    }
                }

                nodeType = node.GetType();
                typePartial = (nodeType.ToString().Split('.'))[1];
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

        private void WriteSingleChildNode(IndentedTextWriter indentWriter, Node node)
        {
            Node childNode = ((SingleChildNode)node).Child;
            CheckNoChildNodeThenWrite(indentWriter, childNode, true);           
        }

        private void WriteTwoChildrenNode(IndentedTextWriter indentWriter, Node node)
        {
            Node firstChild = ((TwoChildrenNode)node).FirstChild;
            Node secondChild = ((TwoChildrenNode)node).SecondChild;
            CheckNoChildNodeThenWrite(indentWriter, firstChild, false);
            CheckNoChildNodeThenWrite(indentWriter, secondChild, true);
            //bool isFirstChildEnd = firstChild.GetType() == typeof(NoChildrenNode) ? true : false;
            //bool isSecondChildEnd = secondChild.GetType() == typeof(NoChildrenNode) ? true : false;
            //WriteNode(indentWriter, firstChild, false);
            //WriteNode(indentWriter, secondChild, isFirstChildEnd && isSecondChildEnd ? true : false);
        }

        private void WriteManyChildrenNode(IndentedTextWriter indentWriter, Node node)
        {
            IEnumerable<Node> children = ((ManyChildrenNode)node).Children;
            var childrenList = children.ToList();
            bool isEnd = true;
            for (int i = 0; i < childrenList.Count; i++)
            {
                if (GetChildrenNum(childrenList[i]) != 0)
                    isEnd = false;

                if (i == children.ToList().Count - 1)
                {
                    CheckNoChildNodeThenWrite(indentWriter, childrenList[i], isEnd);
                    //WriteNode(indentWriter, childrenList[i], isEnd);
                    break;
                }
                CheckNoChildNodeThenWrite(indentWriter, childrenList[i], false);
                //WriteNode(indentWriter, childrenList[i], false);
            }
        }

        private void CheckNoChildNodeThenWrite(IndentedTextWriter indentWriter, Node node, bool isLastNode)
        {
            var childrenNum = GetChildrenNum(node);
            if (childrenNum == 0)
            {
                WriteNode(indentWriter, new NoChildrenNode(node.Name), isLastNode? true : false);
            }
            else
            {
                WriteNode(indentWriter, node, false);
            }
        }
    }
}
