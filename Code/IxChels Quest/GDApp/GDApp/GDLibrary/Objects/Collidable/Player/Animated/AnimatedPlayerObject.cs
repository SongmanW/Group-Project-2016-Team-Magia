using GDApp;
using JigLibX.Collision;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SkinnedModel;
using System;

namespace GDLibrary
{
    public class AnimatedPlayerObject : PlayerObject
    {
        #region Variables
        private AnimationPlayer animationPlayer;
        private SkinningData skinningData;
        private string takeName;
        #endregion

        #region Properties
        public AnimationPlayer AnimationPlayer
        {
            get
            {
                return animationPlayer;
            }
        }
        #endregion


        public AnimatedPlayerObject(string id, ObjectType objectType, Transform3D transform,
            Effect effect, Texture2D texture, Model model, Color color, float alpha,
            Keys[] moveKeys, float radius, float height, float accelerationRate, float decelerationRate, 
            string takeName, Vector3 translationOffset)
            : base(id, objectType, transform, effect, texture, model, color, 
            alpha, moveKeys, radius, height, accelerationRate, decelerationRate, translationOffset)
        {
            //set initial animation played when player instanciated
            this.takeName = takeName;

            //load animation player with initial take e.g. idle
            SetAnimation(model, takeName);
        }

        public void SetAnimation(Model model, string takeName)
        {
            // Look up our custom skinning information.
            skinningData = model.Tag as SkinningData;

            if (skinningData == null)
                throw new InvalidOperationException
                    ("This model does not contain a SkinningData tag.");

            // Create an animation player, and start decoding an animation clip.
            animationPlayer = new AnimationPlayer(skinningData);

            //set initial clip
            SetClip(takeName);
        }


        public override void Update(GameTime gameTime)
        {
            //update player to return bone transforms for the appropriate frame in the animation
            animationPlayer.Update(gameTime.ElapsedGameTime, true, Matrix.Identity);

            base.Update(gameTime);
        }

        //call to change animation clip during gameplay
        public void SetClip(string takeName)
        {
            animationPlayer.StartClip(skinningData.AnimationClips[takeName]);
        }
    }
}
