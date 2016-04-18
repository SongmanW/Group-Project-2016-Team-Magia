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
        public PawnTriangleMeshObject(string id, ObjectType objectType, Transform3D transform,
            Texture2D texture, Model model, Color color, float alpha, MaterialProperties materialProperties)
            : base(id, objectType, transform, texture, model, color, alpha, materialProperties)
        {
            //get the primitive mesh which forms the skin
            TriangleMesh triangleMesh = GetTriangleMesh(model, transform);

            //add the primitive mesh to the collision skin
            this.Body.CollisionSkin.AddPrimitive(triangleMesh, materialProperties);
        }
    }
}
