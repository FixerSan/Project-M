using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class EventManager 
{
    // �̺�Ʈ �׼� ���� ����
    public Action<VoidEventType> OnVoidEvent;
    public Action<IntEventType, int> OnIntEvent;

    public void AddVoidEvent(Action<VoidEventType> _callback)
    {
        OnVoidEvent -= _callback;
        OnVoidEvent += _callback;
    }

    public void RemoveVoidEvent(Action<VoidEventType> _callback)
    {
        OnVoidEvent -= _callback;
    }
}
