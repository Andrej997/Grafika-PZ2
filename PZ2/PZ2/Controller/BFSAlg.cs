using System;
using System.Collections.Generic;
using PZ2.Model;

namespace PZ2.Controller
{
    public class BFSAlg
    {
        public static Func<AllPurpuseEntity, IEnumerable<AllPurpuseEntity>> ShortestPathFunction(Graph graph, AllPurpuseEntity start)
        {
            var previous = new Dictionary<AllPurpuseEntity, AllPurpuseEntity>();

            var queue = new Queue<AllPurpuseEntity>();
            queue.Enqueue(start);

            while (queue.Count > 0)
            {
                var vertex = queue.Dequeue();
                foreach (var neighbor in graph.AdjacencyList[vertex])
                {
                    if (previous.ContainsKey(neighbor))
                        continue;

                    previous[neighbor] = vertex;
                    queue.Enqueue(neighbor);
                }
            }

            int stop = 1;

            Func<AllPurpuseEntity, IEnumerable<AllPurpuseEntity>> shortestPath = v => {
                var path = new List<AllPurpuseEntity> { };

                var current = v;
                while (!current.Equals(start))
                {
                    path.Add(current);
                    try
                    {
                        current = previous[current];
                    }
                    catch
                    {
                        return null;
                    }
                };

                path.Add(start);
                path.Reverse();

                return path;
            };

            return shortestPath;
        }
    }
}
