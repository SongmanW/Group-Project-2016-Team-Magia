using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GDLibrary
{
    public class RotationEventData : EventData
    {
        public EventType eventType;
        public object sender;
        public string id;
        public bool clockwise;

        public RotationEventData(string id,
            object sender, EventType eventType, bool clockwise)
            : base(id, sender, eventType)
        {
            this.id = id;
            this.sender = sender;
            this.eventType = eventType;
            this.clockwise = clockwise;
        }
    }
}
