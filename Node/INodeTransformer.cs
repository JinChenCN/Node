using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node
{
    interface INodeTransformer
    {
        string Transform(Node node);
    }
}
