using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniGIS
{
    public class Graph : MapObject
    {
        public List<Point> points = new List<Point>();
        public List<Edge> edges = new List<Edge>();
        private Bounds bounds;
        private double Inf = 999999999;
        public Search search=null;

        public Graph()
        {
            objectType = MapObjectType.Graph;
        }

        public Graph(Graph graph)
        {
            points = new List<Point>(graph.points);
            edges = new List<Edge>(graph.edges);
            objectType = MapObjectType.Graph;
        }

        public void AddPoint(Point point)
        {
            points.Add(point);
            point.Layer = Layer;
            point.Graph = this;
        }

        public void AddEdge(Edge edge)
        {
            edges.Add(edge);
            edge.Layer = Layer;
            edge.Graph = this;
        }

        internal List<Point> Neighbors(Point current)
        {
            var neighbors = new List<Point>();
            foreach (var edge in edges)
            {
                if (edge.v1 == current)
                    if (!neighbors.Contains(edge.v2))
                        neighbors.Add(edge.v2);
                if (edge.v2 == current)
                    if (!neighbors.Contains(edge.v1))
                        neighbors.Add(edge.v1);
            }
            return neighbors;
        }

        internal double Cost(Point current, Point next)
        {
            foreach (var edge in edges)
            {
                if ((edge.v1 == current && edge.v2 == next) || (edge.v2 == current && edge.v1 == next))
                {
                    edge.v1.Passed = true;
                    edge.v2.Passed = true;
                    edge.Passed = true;
                    return edge.weight;
                }
            }
            return Inf;
        }

        public void AddEdge(Point p1, Point p2)
        {
            var edge = new Edge(p1, p2);
            edges.Add(edge);
            edge.Layer = Layer;
            edge.Graph = this;
        }

        public void AddEdge(Point p1, Point p2, double weight)
        {
            var edge = new Edge(p1, p2, weight);
            edges.Add(edge);
            edge.Layer = Layer;
            edge.Graph = this;
        }

        internal override void Draw(PaintEventArgs e)
        {
            foreach (var edge in edges)
            {
                edge.Draw(e);
            }
            foreach (var point in points)
            {
                point.Draw(e);
            }
            if (search != null)
            {
                int x=0;
                int y=0;
                foreach (var edge in edges)
                {
                    if (edge.Passed) x++;
                }
                foreach (var point in points)
                {
                    if (point.Passed) y++;
                }
                e.Graphics.DrawString("Пройдено вершин: " + y, new Font("Arial", 10), Brushes.Black, Layer.Map.MapToScreen(new Vertex(Bounds.Xmin, Bounds.Ymin-8)));
                e.Graphics.DrawString("Пройдено рёбер: " + x, new Font("Arial", 10), Brushes.Black, Layer.Map.MapToScreen(new Vertex(Bounds.Xmin, Bounds.Ymin-16)));
                e.Graphics.DrawString("Длина пути: " + search.cost, new Font("Arial", 10), Brushes.Black, Layer.Map.MapToScreen(new Vertex(Bounds.Xmin, Bounds.Ymin - 24)));
            }
        }

        protected override Bounds GetBounds()
        {
            return bounds = CalcBounds();
        }

        public Bounds CalcBounds()
        {
            bounds = new Bounds();
            foreach (var obj in points)
            {
                bounds += obj.Bounds;
            }
            foreach (var obj in edges)
            {
                bounds += obj.Bounds;
            }
            return bounds;
        }

        public override double Area()
        {
            return 0;
        }

        internal override bool IsIntersectsWithQuad(Vertex searchPoint, double d)
        {
            for(int i = points.Count - 1; i >= 0; i--)
                {
                if (points[i].IsIntersectsWithQuad(searchPoint, d))
                {
                    return true;
                }
            }
            for (int i = edges.Count - 1; i >= 0; i--)
            {
                if (edges[i].IsIntersectsWithQuad(searchPoint, d))
                {
                    return true;
                }
            }
            return false;
        }
        
        public Layer Layer
        {
            get { return layer; }
            set
            {
                if (layer != null)                  //объект может находиться только в одном слое,
                    layer.RemoveObject(this);       //поэтому удаляем его из предидущего слоя,
                layer = value;                      //а затем добавляем в новый
                foreach (var obj in points)
                {
                    obj.Layer = Layer;
                }
                foreach (var obj in edges)
                {
                    obj.Layer = Layer;
                }
            }
        }
    }
}
