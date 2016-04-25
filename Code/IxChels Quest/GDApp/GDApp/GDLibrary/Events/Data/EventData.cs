
namespace GDLibrary
{
    public class EventData
    {
        public EventType eventType;
        public EventCategoryType eventCategoryType;
        public object sender;
        public string id;

        public EventData(string id,
            object sender, EventType eventType)
        {
            this.id = id;
            this.sender = sender;
            this.eventType = eventType;
            this.eventCategoryType = EventCategoryType.Own;
        }

        public EventData(string id,
            object sender, EventType eventType, EventCategoryType eventCategoryType)
        {
            this.id = id;
            this.sender = sender;
            this.eventType = eventType;
            this.eventCategoryType = eventCategoryType;
        }
    }
}
