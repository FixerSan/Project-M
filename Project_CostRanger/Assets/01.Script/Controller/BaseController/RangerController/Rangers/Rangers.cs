using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ranger 
{
    public Define.Specialty[] specialty;
    public RangerControllerData data; 
}

namespace Rangers
{
    public class TestRanger : Ranger
    {
        public TestRanger(RangerControllerData _data)
        {
            data = _data;
        }
    }
}