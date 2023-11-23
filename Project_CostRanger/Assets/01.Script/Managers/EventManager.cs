using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class EventManager 
{
    // 이벤트 액션 변수 선언
    public Action<VoidEventType> OnVoidEvent;
    public Action<IntEventType, int> OnIntEvent;

    public Dictionary<VoidEventType, Action> voidEvents;

    public EventManager() 
    {
        voidEvents = new Dictionary<VoidEventType, Action>();
    }   

    public void AddVoidEvent(VoidEventType _type, Action _eventAction)
    {
        if (voidEvents.TryGetValue(_type, out Action eventAction))
        {
            eventAction -= _eventAction;
            eventAction += _eventAction;
        }

        else
        {
            voidEvents.Add(_type, new Action(_eventAction));
        }
    }

    public void InvokeVoidEvent(VoidEventType _type)
    {
        if (voidEvents.TryGetValue(_type, out Action eventAction))
            eventAction.Invoke();
    }

    public void RemoveVoidEvent(VoidEventType _type, Action _eventAction)
    {
        if (voidEvents.TryGetValue(_type, out Action eventAction))
        {
            eventAction -= _eventAction;
            if(eventAction == null)
                voidEvents.Remove(_type);
        }
    }

}
