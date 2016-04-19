using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GDLibrary
{
    public class CameraEventData : EventData
    {
        public string cameraID;
        public string cameraLayout;

        public CameraEventData(string id, object sender, EventType eventType, string cameraLayout, string cameraID)
            : base(id, sender, eventType)
        {
            this.cameraLayout = cameraLayout;
            this.cameraID = cameraID;
        }
    }
}
