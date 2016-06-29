using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Tests
{
    internal class Utility
    {
        public static void WriteLevel(IndentedTextWriter indentWriter, Dictionary<int, List<string>> outPutDic)
        {
            for (var i = 0; i < outPutDic.Count; i++)
            {
                List<string> outPutList = outPutDic[i];
                if (i != outPutDic.Count - 1)
                {
                    foreach (string s in outPutList)
                    {
                        indentWriter.WriteLine(s);
                    }
                }
                else
                {
                    for (var j = 0; j < outPutList.Count; j++)
                    {
                        if (j != outPutList.Count - 1)
                        {
                            indentWriter.WriteLine(outPutList[j]);
                        }
                        else
                        {
                            indentWriter.Write(outPutList[j]);
                        }
                    }
                }

                indentWriter.Indent++;
            }
        }
    }
}
