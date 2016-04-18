using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GDLibrary
{
    public class OffsetController : Controller
    {
        private Vector3 maxOffset;
        private Vector3 offset;
        private Vector3 originalTranslation;
        private bool bSet = false;

        public bool isSet
        {
            get
            {
                return bSet;
            }
        }

        public OffsetController(string name, Actor parentActor, bool bEnabled, Vector3 maxOffset)
            : base(name, parentActor, bEnabled)
        {
            this.maxOffset = maxOffset;
            this.originalTranslation = ParentActor.Transform3D.Translation;
        }

        public override void Update(GameTime gameTime)
        {
            if (bSet)
            {
                ParentActor.Transform3D.Translation = this.originalTranslation + this.offset;
                if(this.ParentActor.ObjectType == ObjectType.Door)
                    ((PawnCollidableObject)this.ParentActor).Body.MoveTo(this.ParentActor.Transform3D.Translation, this.ParentActor.Transform3D.Orientation);
                if (this.ParentActor.ObjectType == ObjectType.Rotation)
                    ((PawnCollidableObject)this.ParentActor).Body.MoveTo(this.ParentActor.Transform3D.Translation + Vector3.UnitY * 20, this.ParentActor.Transform3D.Orientation);
                if (offset.Length() < maxOffset.Length())
                {
                    this.offset += maxOffset / 100 * gameTime.ElapsedGameTime.Milliseconds;
                }
            }
            else
            {
                if (gameTime.ElapsedGameTime.Milliseconds != 0)
                {
                    ParentActor.Transform3D.Translation = this.originalTranslation + this.offset;
                    this.offset /= 100 / gameTime.ElapsedGameTime.Milliseconds;
                }
            }
        }

        public void Set()
        {
            bSet = true;
        }

        public void Unset()
        {
            bSet = false;
        }
    }
}
