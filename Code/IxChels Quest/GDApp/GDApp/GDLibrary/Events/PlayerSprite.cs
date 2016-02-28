using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GDLibrary
{
    public class PlayerSprite
    {
        //1 - allow anonymous methods to register for event
        public delegate void PlayerRestartEventHandler(object sender, 
            int health);
        //2 - flag indicating the event has taken place
        public event PlayerRestartEventHandler PlayerRestarted;
        //3 - method called by generating code
        public void OnPlayerRestart(int health)
        {
            if (PlayerRestarted != null)
                PlayerRestarted(this, health);
        }
        //4 - call the event - it occured i.e. when we reach a point in 2D
        public void ItHappened()
        {
            OnPlayerRestart(100);
        }


        #region Tidy
        public string name;
        private bool gender;
        //properties?
        public PlayerSprite(string name, bool gender)
        {
            this.name = name;
            this.gender = gender;
        }
        #endregion
    }
}
