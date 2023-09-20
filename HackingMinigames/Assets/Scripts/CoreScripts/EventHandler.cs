
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class EventHandler : MonoBehaviour
{
    private void Update()
    {
        checkStrokes();
    }

    public static void AddTrigger(EventTrigger eventTrigger, EventTriggerType eventType, UnityAction call)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = eventType;
        entry.callback.AddListener((eventData) => call.Invoke());
        eventTrigger.triggers.Add(entry);
    }

    private void checkStrokes()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("escape pressed");
            SceneNavigator.ToggleSettings();
        }
    }
}
