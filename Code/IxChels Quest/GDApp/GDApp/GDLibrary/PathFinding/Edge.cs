using Microsoft.Xna.Framework;

namespace GDLibrary
{
    /// <summary>
    /// A path that links two nodes the cost is based on straight-line distance multiplied by cost (normally 1)
    /// We can use cost to make some links more expensive
    /// </summary>
    public class Edge
    {
        private string id;
        private Node start, end;
        private float distance, cost;

        public string ID
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }
        public Node Start
        {
            get
            {
                return start;
            }
        }
        public Node End
        {
            get
            {
                return end;
            }
        }
        public float Distance
        {
            get
            {
                return distance;
            }
        }

        public float Cost
        {
            get
            {
                return cost;
            }
            set
            {
                cost = value * distance;
            }
        }

        public Edge(string id, Node start, Node end, float cost)
        {
            //unique id so we can delete by id later if necessary
            this.id = id;

            //set edge start and end points
            this.start = start;
            this.end = end;
            
            //straight-line distance between two points
            this.distance = Vector3.Distance(start.Position, end.Position);

            //cost of using the link is distance by some multiplier
            this.cost = distance * cost;
        }

    }
}
