using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniGIS
{
    public class Edge: Line
    {
        public double weight;
        public Point v1;
        public Point v2;
        public Graph Graph = null;
        public bool Passed = false;

        public Edge(Point p1, Point p2, double weight)
        {
            this.begin = p1.Position;
            this.end = p2.Position;
            v1 = p1;
            v2 = p2;
            this.weight = weight;
            objectType = MapObjectType.Edge;
        }

        public Edge(Point p1, Point p2)
        {
            this.begin = p1.Position;
            this.end = p2.Position;
            v1 = p1;
            v2 = p2;
            this.weight = GetWeight(begin, end);
            objectType = MapObjectType.Edge;
        }

        public double GetWeight(Vertex begin, Vertex end)
        {
            return Math.Sqrt(Math.Pow(begin.X - end.X, 2) + Math.Pow(begin.Y - end.Y, 2));
        }

        internal override void Draw(PaintEventArgs e)
        {
            Pen pen;
            if (Graph != null && Graph.search != null && Graph.search.pathE.Contains(this))
                pen = new Pen(Color.Green, 3);
            else if (Passed) pen = new Pen(Color.Red, 2);
            else pen = GetCurrentPen();
            var beginPoint = Layer.Map.MapToScreen(begin);
            var endPoint = Layer.Map.MapToScreen(end);
            e.Graphics.DrawLine(pen, beginPoint, endPoint);
            var point = Layer.Map.MapToScreen(new Vertex((v1.X + v2.X) / 2,(v1.Y + v2.Y) / 2));
            e.Graphics.DrawString(Math.Round(weight,2).ToString(), new Font("Arial", 10), Brushes.Black, point);
        }
    }
}
