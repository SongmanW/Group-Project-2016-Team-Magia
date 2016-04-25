using JigLibX.Collision;
using JigLibX.Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GDLibrary
{
    public class PawnTriangleMeshObject : TriangleMeshObject
    {
        #region Fields
        private List<IController> controllerList;
        private MaterialProperties materialProperties;
        #endregion

        #region Properties
        public List<IController> ControllerList
        {
            get
            {
                return this.controllerList;
            }
        }
        #endregion

        public PawnTriangleMeshObject(string id, ObjectType objectType, Transform3D transform, Effect effect,
            Texture2D texture, Model model, Color color, float alpha, MaterialProperties materialProperties)
            : base(id, objectType, transform, effect, texture, model, color, alpha, materialProperties)
        {
            this.materialProperties = materialProperties;
            
            //get the primitive mesh which forms the skin
            TriangleMesh triangleMesh = GetTriangleMesh(model, transform);

            //add the primitive mesh to the collision skin
            this.Body.CollisionSkin.AddPrimitive(triangleMesh, materialProperties);
        }

        private TriangleMesh GetTriangleMesh(Model model, Transform3D transform)
        {
            TriangleMesh triangleMesh = new TriangleMesh();
            List<Vector3> vertexList = new List<Vector3>();
            List<TriangleVertexIndices> indexList = new List<TriangleVertexIndices>();

            ExtractData(vertexList, indexList, model);

            for (int i = 0; i < vertexList.Count; i++)
            {
                vertexList[i] = Vector3.Transform(vertexList[i], transform.World);
            }

            // create the collision mesh
            triangleMesh.CreateMesh(vertexList, indexList, 1, 1.0f);

            return triangleMesh;
        }


        public void Add(IController controller)
        {
            if (this.controllerList == null)
                this.controllerList = new List<IController>();

            //contains? hash code and equals?
            this.controllerList.Add(controller);
        }

        public void Remove(string name)
        {
            for (int i = 0; i < this.controllerList.Count; i++)
            {
                if (this.controllerList[i].GetName().Equals(name))
                {
                    this.controllerList.RemoveAt(i);
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (this.controllerList != null)
            {
                foreach (IController controller in this.controllerList)
                    controller.Update(gameTime);
            }

            this.Body.CollisionSkin.RemoveAllPrimitives();

            //get the primitive mesh which forms the skin
            TriangleMesh triangleMesh = GetTriangleMesh(this.Model, this.Transform3D);

            //add the primitive mesh to the collision skin
            this.Body.CollisionSkin.AddPrimitive(triangleMesh, this.materialProperties);
        }
    }
}
