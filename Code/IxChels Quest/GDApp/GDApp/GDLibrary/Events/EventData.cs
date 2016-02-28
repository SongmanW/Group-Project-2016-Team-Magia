
namespace GDLibrary
{
    public class EventData
    {
        public EventType eventType;
        public object sender;
        public string id;

        public EventData(string id,
            object sender, EventType eventType)
        {
            this.id = id;
            this.sender = sender;
            this.eventType = eventType;
        }
    }
}
