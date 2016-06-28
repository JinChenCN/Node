using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node
{
    public abstract class Node
    {
        public string Name { get; }
        protected Node(string name)
        {
            Name = name;
        }

    }

    public class NoChildrenNode : Node
    {
        public NoChildrenNode(string name) : base(name)
        {

        }
    }

    public class SingleChildNode : Node
    {
        public Node Child { get; }

        public SingleChildNode(string name, Node child) : base(name)
        {
            Child = child;
        }
    }

    public class TwoChildrenNode : Node

    {
        public Node FirstChild { get; }
        public Node SecondChild { get; }
        public TwoChildrenNode(string name, Node first, Node second) : base(name)
        {
            FirstChild = first;
            SecondChild = second;
        }

    }

    public class ManyChildrenNode : Node
    {
        public IEnumerable<Node> Children { get; }
        public ManyChildrenNode(string name, params Node[] children) : base(name)
        {

        }
    }

}