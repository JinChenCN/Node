using System.Threading.Tasks;

namespace Node
{
    interface INodeWriter
    {
        Task WriteToFileAsync(Node node, string filePath);
    }
}
