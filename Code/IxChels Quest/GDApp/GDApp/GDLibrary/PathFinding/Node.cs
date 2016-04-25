using Microsoft.Xna.Framework;


namespace GDLibrary
{
    public class Node 
    {
        private string id;
        private Vector3 position;     
        
        #region PROPERTIES
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

        public Vector3 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }
        #endregion

        public Node(string id, Vector3 position)
        {
            this.id = id; 
            this.position = position;
        }
    }
}
