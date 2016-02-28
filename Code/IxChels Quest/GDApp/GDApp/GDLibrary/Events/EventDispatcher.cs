using System.Collections.Generic;
using Microsoft.Xna.Framework;
using GDApp;

namespace GDLibrary
{
    public class EventDispatcher : GameComponent
    {
        private static int initialSize = 25;
        private static List<EventData> list = new List<EventData>(initialSize);

        public EventDispatcher(Main game)
            : base(game)
        {
        }

        public static void Publish(EventData eventData)
        {
            list.Add(eventData);
        }

        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < list.Count; i++)
                Process(list[i]);

            list.Clear();

            base.Update(gameTime);
        }

        private void Process(EventData eventData)
        {
            //Switch - See https://msdn.microsoft.com/en-us/library/06tc147t.aspx
            switch(eventData.eventType)
            {
                case EventType.OnStart: 
                    OnStart(eventData);
                    break;

                //add a case to handle the On...() method for each type

                default:
                    break;
            }
        }

        //Add a delegate, event, and On..() method for each event type e.g. OnPlaySound, OnCameraReset etc
        public delegate void GameStartEventHandler(object sender);
        public event GameStartEventHandler GameStarted;
        public void OnStart(EventData eventData)
        {
            if (GameStarted != null)
                GameStarted(eventData.sender);
        }

    }
}
