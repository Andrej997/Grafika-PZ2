﻿using System;
using System.Collections.Generic;
using PZ2.Model;

namespace PZ2.Controller
{
    public class BFSAlg
    {
        public static Func<AllPurpuseEntity, IEnumerable<AllPurpuseEntity>> ShortestPathFunction(Graph graph, AllPurpuseEntity start)
        {
            // recnik prethodnih, kljuc je child
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

            Func<AllPurpuseEntity, IEnumerable<AllPurpuseEntity>> shortestPath = v => {
                var path = new List<AllPurpuseEntity> { };

                // node koji se posmatra
                var current = v;
                while (!current.Equals(start)) // ide kroz recnik sve kod ne naidje na samog sebe
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

        public static Dictionary<AllPurpuseEntity, AllPurpuseEntity> BFS(Graph graph, AllPurpuseEntity start)
        {
            var visited = new Dictionary<AllPurpuseEntity, AllPurpuseEntity>();

            var queue = new Queue<AllPurpuseEntity>();
            queue.Enqueue(start);

            while (queue.Count > 0)
            {
                var vertex = queue.Dequeue();
                foreach (var neighbor in graph.AdjacencyList[vertex])
                {
                    if (visited.ContainsKey(neighbor))
                        continue;

                    visited[neighbor] = vertex;
                    queue.Enqueue(neighbor);
                }
            }

            return visited;
        }

        public static List<AllPurpuseEntity> Path(Dictionary<AllPurpuseEntity, AllPurpuseEntity> connectedNodes, AllPurpuseEntity start, AllPurpuseEntity v)
        {
            var path = new List<AllPurpuseEntity> { };

            // node koji se posmatra
            var current = v;
            while (!current.Equals(start)) // ide kroz recnik sve kod ne naidje na samog sebe
            {
                path.Add(current);
                try
                {
                    current = connectedNodes[current];
                }
                catch
                {
                    return null;
                }
            };

            path.Add(start);
            path.Reverse();

            return path;
        }

        public static HashSet<AllPurpuseEntity> VisitedNodes(Graph graph, AllPurpuseEntity start)
        {
            var visited = new HashSet<AllPurpuseEntity>();

            if (!graph.AdjacencyList.ContainsKey(start))
                return visited;

            var queue = new Queue<AllPurpuseEntity>();
            queue.Enqueue(start);

            while (queue.Count > 0)
            {
                var vertex = queue.Dequeue();

                if (visited.Contains(vertex))
                    continue;

                visited.Add(vertex);

                foreach (var neighbor in graph.AdjacencyList[vertex])
                    if (!visited.Contains(neighbor))
                        queue.Enqueue(neighbor);
            }

            return visited;
        }

    }
}
