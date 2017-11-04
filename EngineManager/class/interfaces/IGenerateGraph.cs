using Microsoft.Glee.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineManager
{
    public interface IGenerateGraph
    {
        void DrawGraph(Graph g, Node pn);
        void DrawRelationalGraph(Graph g, Node pn);

    }
}
