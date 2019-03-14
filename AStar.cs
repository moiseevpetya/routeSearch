using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniGIS
{
    public class AStar:Search
    {
        public AStar(Graph graph, Point A, Point Z)
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
                        double priority = newCost + Heuristic(next, goal);
                        frontier.Enqueue(next, priority);
                        cameFrom[next] = current;
                    }
                }
            }
            GetPath();
        }

        static public double Heuristic(Point begin, Point end)
        {
            return Math.Sqrt(Math.Pow(begin.X - end.X, 2) + Math.Pow(begin.Y - end.Y, 2));//евклидово расстояние
            //return Math.Pow(begin.X - end.X, 2) + Math.Pow(begin.Y - end.Y, 2);
            //return begin.X - end.X + begin.Y - end.Y;//манхетенское расстояние
        }
    }
}
