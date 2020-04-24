using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ2.Model
{
    public class Graph
    {
        public Graph() { }

        public Graph(IEnumerable<AllPurpuseEntity> vertices, IEnumerable<LineEntity> edges)
        {
            foreach (var vertex in vertices)
                AddVertex(vertex);

            foreach (var edge in edges)
                AddEdge(edge);
        }

        public Dictionary<AllPurpuseEntity, HashSet<AllPurpuseEntity>> AdjacencyList { get; } 
            = new Dictionary<AllPurpuseEntity, HashSet<AllPurpuseEntity>>();

        public void AddVertex(AllPurpuseEntity vertex)
        {
            if (vertex != null)
                AdjacencyList[vertex] = new HashSet<AllPurpuseEntity>();
        }

        public void AddEdge(LineEntity edge)
        {
            foreach (var node in AdjacencyList.Keys)
            {
                if (node != null) // ako nije prazno polje matrice
                {
                    if (node.Entity.Id == edge.FirstEnd) // ako im se poklapaju
                    {
                        foreach (var node2 in AdjacencyList.Keys)
                        {
                            if (node2 != null)
                            {
                                if (node2.Entity.Id == edge.SecondEnd)
                                {
                                    AdjacencyList[node].Add(node2);
                                    AdjacencyList[node2].Add(node);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
