using System.Collections.Generic;
using Microsoft.Xna.Framework;
using GDApp;

namespace GDLibrary
{
    public class EventDispatcher : GameComponent
    {
        private static Stack<EventData> stack;
        private static HashSet<EventData> uniqueSet;


        public delegate void MainMenuEventHandler(EventData eventData);
        public delegate void UIMenuEventHandler(EventData eventData);
        public delegate void VideoEventHandler(EventData eventData);
        public delegate void SoundEventHandler(EventData eventData);
        public delegate void TextRenderEventHandler(EventData eventData);
        public delegate void ZoneEventHandler(EventData eventData);
        public delegate void CameraEventHandler(EventData eventData);
        public delegate void PlayerEventHandler(EventData eventData);
        public delegate void NonPlayerEventHandler(EventData eventData);
        public delegate void PickupEventHandler(EventData eventData);

        //normally at least one event for each category type
        public event MainMenuEventHandler MainMenuChanged;
        public event UIMenuEventHandler UIMenuChanged;
        public event VideoEventHandler VideoChanged;
        public event SoundEventHandler SoundChanged;
        public event TextRenderEventHandler TextRenderChanged;
        public event ZoneEventHandler ZoneChanged;
        public event PlayerEventHandler PlayerChanged;
        public event NonPlayerEventHandler NonPlayerChanged;
        public event PickupEventHandler PickupChanged;

        public EventDispatcher(Main game, int initialSize)
            : base(game)
        {
            stack = new Stack<EventData>(initialSize);
            uniqueSet = new HashSet<EventData>(new EventDataEqualityComparer());
        }

        public static void Publish(EventData eventData)
        {
            //this prevents the same event being added multiple times within a single update e.g. 10x bell ring sounds
            if (!uniqueSet.Contains(eventData))
                stack.Push(eventData);
        }

        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < stack.Count; i++)
                Process(stack.Pop());

            stack.Clear();
            uniqueSet.Clear();

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

                case EventType.OnRotationStart:
                    OnRotationStart(eventData);
                    break;

                case EventType.OnRotationEnd:
                    OnRotationEnd(eventData);
                    break;

                case EventType.OnCameraChanged:
                    OnCameraChange(eventData);
                    break;

                //add a case to handle the On...() method for each type

                default:
                    switch (eventData.eventCategoryType)
                    {
                        case EventCategoryType.MainMenu:
                            OnMainMenu(eventData);
                            break;
                    }
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

        public delegate void RotationStartEventHandler(EventData data);
        public event RotationStartEventHandler RotationStarted;
        public void OnRotationStart(EventData eventData)
        {
            if (RotationStarted != null)
                RotationStarted(eventData);
        }

        public delegate void RotationEndEventHandler(EventData data);
        public event RotationEndEventHandler RotationEnd;
        public void OnRotationEnd(EventData eventData)
        {
            if (RotationEnd != null)
                RotationEnd(eventData);
        }

        public delegate void CameraChangedEventHandler(EventData data);
        public event CameraChangedEventHandler CameraChange;
        public void OnCameraChange(EventData eventData)
        {
            if (CameraChange != null)
                CameraChange(eventData);
        }

        //called when a menu event needs to be generated e.g. click exit
        protected virtual void OnMainMenu(EventData eventData)
        {
            if (MainMenuChanged != null)
                MainMenuChanged(eventData);
        }
    }
}
