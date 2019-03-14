using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniGIS
{
    internal class Dijkstra:Search
    {
        public Dijkstra(Graph graph, Point A, Point Z)
        {
            if (!graph.points.Contains(A)) return;
            if (!graph.points.Contains(Z)) return;

            var frontier = new PriorityQueue<Point>();
            start = A;
            goal = Z;
            this.graph = graph;

            frontier.Enqueue(start, 0);
            cameFrom[start] = start;
            costSoFar[start] = 0;

            while (frontier.Count > 0)
            {
                var current = frontier.Dequeue();

                if (current.Equals(goal))
                {
                    break;
                }

                foreach (var next in graph.Neighbors(current))
                {
                    double newCost = costSoFar[current]
                        + graph.Cost(current, next);
                    if (!costSoFar.ContainsKey(next)
                        || newCost < costSoFar[next])
                    {
                        costSoFar[next] = newCost;
                        double priority = newCost;
                        frontier.Enqueue(next, priority);
                        cameFrom[next] = current;
                    }
                }
            }
            GetPath();
        }
    }
}
