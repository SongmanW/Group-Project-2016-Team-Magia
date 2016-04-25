using System.Collections.Generic;

namespace GDLibrary
{
    //used by the EventDispatcher to compare to events in the HashSet - remember that HashSets allow us to quickly prevent something from being added to a list/stack twice
    public class EventDataEqualityComparer : IEqualityComparer<EventData>
    {

        public bool Equals(EventData x, EventData y)
        {
            return x.Equals(y);
        }

        public int GetHashCode(EventData obj)
        {
            return obj.GetHashCode();
        }
    }
}
