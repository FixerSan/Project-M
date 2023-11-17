using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TestScript : MonoBehaviour, IDragHandler
{
    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("드래그 되긴 함");

    }
}
