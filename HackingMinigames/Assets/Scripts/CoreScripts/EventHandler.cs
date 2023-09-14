
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class EventHandler
{
    public static void AddTrigger(EventTrigger eventTrigger, EventTriggerType eventType, UnityAction call)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = eventType;
        entry.callback.AddListener((eventData) => call.Invoke());
        eventTrigger.triggers.Add(entry);
    }
}
