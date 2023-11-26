using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Specialty : MonoBehaviour
{
    public int specialtyCount;
    public int specialLevel;
    public SpecialtyData data;

    public void AddCount()
    {
        specialtyCount++;
        CheckCount();
    }

    public abstract void Effect();
    public abstract void CheckCount();
}
