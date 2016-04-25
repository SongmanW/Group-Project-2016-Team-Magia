using GDApp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GDLibrary
{
    public class UIMouseObject : UITextureObject
    {
        #region Variables
        #endregion

        //temp vars
        private ModelObject lastPickedModelObject;

        #region Properties
        #endregion

        public UIMouseObject(string id, ObjectType objectType, Transform2D transform,
        Color color, SpriteEffects spriteEffects, float layerDepth, Texture2D texture, bool isVisible)
            : this(id, objectType, transform, color, spriteEffects, layerDepth, texture, 
                new Rectangle(0, 0, texture.Width, texture.Height),
                    new Vector2(texture.Width / 2.0f, texture.Height / 2.0f), isVisible)
        {

        }

        public UIMouseObject(string id, ObjectType objectType, Transform2D transform,
            Color color, SpriteEffects spriteEffects, float layerDepth, Texture2D texture, Rectangle sourceRectangle, Vector2 origin, bool isVisible)
            : base(id, objectType, transform, color, spriteEffects, layerDepth, texture, sourceRectangle, origin, isVisible)
        {

        }

        public override void Draw(GameTime gameTime)
        {
            this.Transform2D.Translation = game.MouseManager.Position;
            game.SpriteBatch.Draw(this.Texture, this.Transform2D.Translation, //bug - 22/4/16
                this.SourceRectangle, this.Color, this.Transform2D.Rotation, this.Origin, 
                    this.Transform2D.Scale, this.SpriteEffects, this.LayerDepth);
        }

        public override void Update(GameTime gameTime)
        {
            DoMousePick(gameTime);
            base.Update(gameTime);
        }

        public virtual void DoMousePick(GameTime gameTime)
        {
            if (game.CameraManager.ActiveCamera != null)
            {
                Vector3 pos, normal;

                Actor pickedActor = game.MouseManager.GetPickedObject(
                    game.CameraManager.ActiveCamera, 
                    GameData.FirstPersonStartPickDistance /*5 == how far from 1st Person collidable to start testing for collisions - should always exceed capsule collision skin radius*/, 
                    1000, out pos, out normal);

                //am i picking something collidable that isnt the ground?
                if ((pickedActor != null) && (pickedActor.ObjectType != GDLibrary.ObjectType.CollidableGround))
                {
                    ModelObject nextPickedModelObject = pickedActor as ModelObject;

                    //make mouse icon change color on collision
                    this.Color = Color.Red;
                    this.SourceRectangle = new Microsoft.Xna.Framework.Rectangle(128, 0, 128, 128);
                    this.Transform2D.Rotation += 0.1f * gameTime.ElapsedGameTime.Milliseconds;

                    //we could publish an event - remove object, play a sound etc

                    //picking a new object?
                    if (nextPickedModelObject != lastPickedModelObject)
                    {
                        //set the last back to original color
                        if (this.lastPickedModelObject != null)
                        {
                            this.lastPickedModelObject.Color = this.lastPickedModelObject.OriginalColor;
                            this.lastPickedModelObject.Alpha = this.lastPickedModelObject.OriginalAlpha;
                        }

                        //set next to picked color
                        if (nextPickedModelObject != null)
                        {
                            nextPickedModelObject.Color = Color.Red;
                            nextPickedModelObject.Alpha = 0.5f;
                        }
                    }

                    this.lastPickedModelObject = nextPickedModelObject;
                }
                else
                {
                    //if no collision then set mouse icon back to white etc
                    this.Color = Color.White;
                    this.SourceRectangle = new Microsoft.Xna.Framework.Rectangle(0, 0, 128, 128);
                    this.Transform2D.Rotation = 0;
                }
            }
        }
    }
}
